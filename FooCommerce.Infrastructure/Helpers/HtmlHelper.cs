using System.Collections;
using System.Reflection;
using System.Text;
using System.Text.Encodings.Web;
using System.Web;

using FooCommerce.Application.Helpers;
using FooCommerce.Domain.Interfaces;

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FooCommerce.Infrastructure.Helpers;

public static class HtmlHelper
{
    public static ModelExpression GetPropertyModelExpression(this ModelExpression model, string propertyName)
    {
        return new ModelExpression($"{model.Name}.{propertyName}", model.ModelExplorer.GetExplorerForProperty(propertyName));
    }

    public static void ApplyNewBindProperty(this ModelMetadata metadata, ref TagHelperOutput output)
    {
        var propName = metadata.PropertyName;

        var containerType = metadata.ContainerType;
        var propInfo = containerType
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .FirstOrDefault(x => x.Name.Equals(propName, StringComparison.CurrentCulture));

        var bindPropertyAttribute = propInfo?.GetCustomAttribute<BindPropertyAttribute>();
        if (bindPropertyAttribute == null)
            return;

        if (!string.IsNullOrEmpty(bindPropertyAttribute.Name))
            output.Attributes.Insert(0, new TagHelperAttribute("name", bindPropertyAttribute.Name));
    }

    private static int FindEndTagIndex(string html, int startPoint, string tagName, int possibleSimilar = 0)
    {
        if (string.IsNullOrEmpty(html))
            return -1;

        var rest = html[startPoint..];
        if (string.IsNullOrEmpty(rest))
            return -1;

        if (!rest.StartsWith("<"))
        {
            var possibleNextStartTagIndex = html.IndexOf("<", startPoint);
            if (possibleNextStartTagIndex == -1)
                return -1;

            return FindEndTagIndex(html, possibleNextStartTagIndex, tagName, possibleSimilar);
        }

        var possibleEndTagIndex = html.IndexOf($"</{tagName}>", startPoint);
        if (possibleEndTagIndex < 0)
            return -1;

        var possibleEndTagRange = html.IndexOf(">", possibleEndTagIndex);
        if (possibleEndTagRange == -1)
            return -1;

        var testingPossibleEndTag = html.Substring(possibleEndTagIndex, possibleEndTagRange - possibleEndTagIndex + 1);
        var possibleSimilarStartIndex = html.IndexOf($"<{tagName}", startPoint);
        if (possibleSimilarStartIndex == -1 || possibleEndTagIndex < possibleSimilarStartIndex)
        {
            if (possibleSimilar <= 0)
                return possibleEndTagIndex;

            return FindEndTagIndex(html, ++possibleEndTagIndex, tagName, --possibleSimilar);
        }

        possibleSimilar++;
        var possibleSimilarEndIndex = html.IndexOf(">", possibleSimilarStartIndex) + 1;
        if (possibleSimilarEndIndex == -1)
            return -1;

        // only for development
        // var testingSimilar = rest[possibleSimilarStartIndex..^(html.Length - possibleSimilarEndIndex - 1)];

        if (!string.IsNullOrEmpty(rest))
            return FindEndTagIndex(html, possibleSimilarEndIndex, tagName, possibleSimilar);

        return -1;
    }

    private static bool ParseAsTagBuilder(string html, int startIndex, out TagBuilder tag, out int tagEndIndex)
    {
        tag = null;
        tagEndIndex = -1;
        if (string.IsNullOrEmpty(html))
            return false;

        html = html
            .Replace(Environment.NewLine, "")
            .Replace(@"\n", "")
            .Replace(@"\r\n", "")
            .Replace("</ ", "</")
            .Trim();

        if (string.IsNullOrEmpty(html))
            return false;

        var content = string.Empty;
        var tagIndexA = html.IndexOf("<", startIndex);
        var isComment = html[tagIndexA + 1] == '!';
        if (isComment)
        {
            const string commentFinisher = "—-->";
            tagIndexA = html.IndexOf(commentFinisher, startIndex) + commentFinisher.Length;
            if (tagIndexA >= html.Length)
                return false;

            // var mock = html[tagIndexA..]; // for development
            return ParseAsTagBuilder(html, tagIndexA, out tag, out tagEndIndex);
        }

        List<string> tagAttributes;
        if (tagIndexA > -1)
        {
            if (tagIndexA != startIndex)
                return ParseAsTagBuilder(html, tagIndexA, out tag, out tagEndIndex);

            // var tempHtml = html[tagIndexA..];

            var tagIndexZ = html.IndexOf(">", tagIndexA, StringComparison.Ordinal);
            if (tagIndexZ > -1)
            {
                var plainAttributes = html[(tagIndexA + 1)..tagIndexZ];
                var hasEndTag = html[tagIndexZ - 1] != '/';

                if (plainAttributes.EndsWith("/"))
                    plainAttributes = plainAttributes[..^1].Trim();

                tagAttributes = ParseAttributes(plainAttributes);
                if (tagAttributes == null || !tagAttributes.Any())
                    throw new NullReferenceException($"Unable to find valid tag.");

                var tagName = tagAttributes[0];
                if (string.IsNullOrEmpty(tagName))
                    throw new NullReferenceException($"Unable to find name of the tag.");

                //tagName = tagName.StartsWith("/") ? tagName[1..] : tagName;
                tagAttributes = tagAttributes.Skip(1).ToList();
                tag = new TagBuilder(tagName)
                {
                    TagRenderMode = !hasEndTag
                        ? TagRenderMode.SelfClosing
                        : TagRenderMode.Normal
                };

                if (hasEndTag)
                {
                    tagEndIndex = FindEndTagIndex(html, tagIndexZ + 1, tagName);
                    if (tagEndIndex == -1)
                        throw new NullReferenceException($"Unable to find matching end tag for <{tagName}>");

                    content = html[(tagIndexZ + 1)..tagEndIndex];
                    if (!string.IsNullOrEmpty(content))
                        tag.InnerHtml.AppendHtml(content);
                }
                else
                {
                    tagEndIndex = tagIndexZ;
                }
            }
            else
            {
                throw new NullReferenceException($"Unable to find a valid start tag.");
            }
        }
        else
        {
            return false;
        }

        if (tagAttributes.Any())
        {
            foreach (var attribute in tagAttributes)
            {
                var parts = attribute.Split("=");
                if (parts.Length <= 1)
                    throw new Exception("Unable to find matching value for attribute.");

                var key = parts[0];
                var value = string.Join("=", parts.Skip(1));
                value = value.Replace("\"", "").Replace("'", "");
                tag.Attributes.Add(key, value);
            }
        }

        return true;
    }

    private static List<string> ParseAttributes(string str)
    {
        var charCounter = 0;
        var word = string.Empty;
        var doubleQuotation = false;
        var tagAttributes = new List<string>();
        while (charCounter < str.Length)
        {
            var @char = str[charCounter];
            if (@char == '=' && str[charCounter + 1] == '\"')
            {
                doubleQuotation = true;
            }
            else if (doubleQuotation && @char == '\"' && str[charCounter - 1] != '=')
            {
                doubleQuotation = false;
                word += @char;
                tagAttributes.Add(word);
                word = string.Empty;
                charCounter++;
                continue;
            }
            else if (@char == ' ' && !doubleQuotation)
            {
                if (!string.IsNullOrEmpty(word))
                    tagAttributes.Add(word);

                word = string.Empty;
                charCounter++;
                continue;
            }
            else if (@charCounter == str.Length - 1)
            {
                word += @char;
                tagAttributes.Add(word);
                break;
            }

            word += @char;
            charCounter++;
        }

        return tagAttributes;
    }

    private static void ParseAsTagBuilders(string html, int startIndex, out List<TagBuilder> output)
    {
        output = new List<TagBuilder>();
        var canContinue = ParseAsTagBuilder(html, startIndex, out var tag, out var tagEndIndex);

        if (tag != null)
            output.Add(tag);

        if (!canContinue)
            return;

        startIndex = tagEndIndex + 1;
        if (startIndex >= html.Length || html.IndexOf("<", startIndex) == -1)
            return;

        ParseAsTagBuilders(html, startIndex, out var tempOutput);
        if (tempOutput?.Any() == true)
            output.AddRange(tempOutput);
    }

    /// <summary>
    /// Parses only html TAGs into a <see cref="List{T}"/> of <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="html">A <see cref="string"/> that contains html.</param>
    /// <returns>A <see cref="List{T}"/> that contains parsed tag builders of given html.</returns>
    public static List<TagBuilder> ParseAsTagBuilders(string html)
    {
        ParseAsTagBuilders(html, 0, out var output);
        return output;
    }

    /// <summary>
    /// Parses only html TAGs into a <see cref="List{T}"/> of <see cref="TagBuilder"/>.
    /// </summary>
    /// <param name="html">A <see cref="IHtmlContent"/> that contains html.</param>
    /// <returns>A <see cref="List{T}"/> that contains parsed tag builders of given html.</returns>
    public static List<TagBuilder> ParseAsTagBuilders(this IHtmlContent html)
    {
        ParseAsTagBuilders(html.GetString(), 0, out var output);
        return output;
    }

    /// <summary>
    /// Returns a generated instance of <see cref="TagBuilder"/> by given html string.
    /// </summary>
    /// <param name="tag">An specific html string that representing an html tag.</param>
    /// <returns>An <see cref="TagBuilder"/> object.</returns>
    public static TagBuilder GetTagBuilder(this IHtmlContent tag)
    {
        var contentToStr = tag.GetString();
        var decoded = HttpUtility.HtmlDecode(contentToStr);
        var result = ParseAsTagBuilder(decoded);
        return result;
    }

    /// <summary>
    /// Returns a collection of generated instance of <see cref="ITagBuilder"/> by given html string.
    /// </summary>
    /// <param name="tag">An specific html string that representing an list of html tags.</param>
    /// <returns>An list of <see cref="TagBuilder"/> object.</returns>
    public static List<TagBuilder> GetTagBuilders(this IHtmlContent tag)
    {
        var contentToStr = tag.GetString();
        var decoded = HttpUtility.HtmlDecode(contentToStr);
        var result = ParseAsTagBuilders(decoded);
        return result;
    }

    /// <summary>
    /// Returns a generated instance of <see cref="ITagBuilder"/> by given html string.
    /// </summary>
    /// <param name="tag">An specific html string that representing an html tag.</param>
    /// <returns>An <see cref="ITagBuilder"/> object.</returns>
    public static TagBuilder GetTagBuilder(this Func<string, IHtmlContent> tag)
    {
        if (tag == null)
            throw new ArgumentNullException(nameof(tag));

        var contentToStr = tag.Invoke("").GetString();
        var decoded = HttpUtility.HtmlDecode(contentToStr);
        var result = ParseAsTagBuilder(decoded);
        return result;
    }

    /// <summary>
    /// Returns a rendered html string of given <see cref="IHtmlContent"/> value.
    /// </summary>
    /// <param name="content">A <see cref="IHtmlContent"/> value that representing html string.</param>
    /// <returns>A <see cref="string"/> value.</returns>
    public static string GetString(this IHtmlContent content)
    {
        if (content == null)
            return null;

        using var writer = new StringWriter();
        content.WriteTo(writer, HtmlEncoder.Default);
        var text = writer.ToString();
        return string.IsNullOrEmpty(text) ? null : text;
    }

    /// <summary>
    /// Converts <see cref="TagBuilder.InnerHtml"/> contents to a list of <see cref="TagBuilder"/>;
    /// </summary>
    /// <param name="tag"></param>
    /// <exception cref="ArgumentNullException"></exception>
    /// <returns></returns>
    public static List<TagBuilder> GetInnerHtmlNodes(this TagBuilder tag)
    {
        if (tag == null)
            throw new ArgumentNullException(nameof(tag));

        var content = tag.InnerHtml.GetString();
        return string.IsNullOrEmpty(content) ? null : ParseAsTagBuilders(content);
    }

    /// <summary>
    /// Returns an <see cref="TagBuilder"/> object from given html tag.
    /// </summary>
    /// <param name="html">A <see cref="string"/> value that representing an html tag.</param>
    /// <returns>A <see cref="TagBuilder"/> object.</returns>
    public static TagBuilder ParseAsTagBuilder(string html)
    {
        if (html == null)
            return null;

        ParseAsTagBuilder(html, 0, out var tag, out _);
        return tag;
    }

    private static (TagHelperContext, TagHelperOutput) InitCore<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var tagType = tagHelper.GetType();
        if (tagType == null)
            throw new NullReferenceException(nameof(tagType));

        var targetAttrs = tagType.GetCustomAttributes<HtmlTargetElementAttribute>().ToList();
        if (targetAttrs?.Any() != true)
            throw new NullReferenceException(nameof(targetAttrs));

        var tagName = targetAttrs[0].Tag;
        var attributes = tagHelper.GetType()
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Select(x => new TagHelperAttribute(x.Name, x.GetValue(tagHelper)))
            .ToList();

        var items = new Dictionary<object, object>();
        var uniqueIdentifier = Guid.NewGuid().ToString();
        var tagHelperAttributeList = new TagHelperAttributeList(attributes);
        var context = new TagHelperContext(tagHelperAttributeList, items, uniqueIdentifier);
        var output = new TagHelperOutput(
            tagName,
            new TagHelperAttributeList(),
            (useCachedResult, encoder) =>
                Task.Factory.StartNew<TagHelperContent>(
                    () =>
                    {
                        var instance = new DefaultTagHelperContent();

                        if (!string.IsNullOrEmpty(unencodedContent))
                            instance.SetContent(unencodedContent);

                        return instance;
                    }));

        return new ValueTuple<TagHelperContext, TagHelperOutput>(context, output);
    }

    private static TagHelperContent RenderContent(this TagHelperOutput output)
    {
        var content = output.Content.GetContent();
        var postContent = output.PostContent.GetContent();
        var preContent = output.PreContent.GetContent();

        return null;
    }

    private static TagBuilder RenderCore(this TagHelperOutput output)
    {
        var createdTag = new TagBuilder(output.TagName);
        createdTag.MergeAttributes(output.Attributes.ToDictionary(c => c.Name, c => c.Value));
        return createdTag;
    }

    /// <summary>
    /// Replaces an html string contents with given dictionary of tags.
    /// </summary>
    /// <param name="htmlDecodedText">A <see cref="string"/> value that representing non-encoded html string.</param>
    /// <param name="tags">A <see cref="Dictionary{TKey,TValue}"/> that representing tags to be replaces with given <see cref="htmlDecodedText"/>.</param>
    /// <returns>A <see cref="HtmlString"/> object.</returns>
    public static HtmlString ReplaceHtmlByTagName(this string htmlDecodedText, Dictionary<string, Func<string, IHtmlContent>> tags)
    {
        if (string.IsNullOrEmpty(htmlDecodedText))
            return new HtmlString(null);

        if (tags?.Any() != true)
            return new HtmlString(htmlDecodedText);

        htmlDecodedText = htmlDecodedText
            .Replace("\r\n", "")
            .Replace("</ ", "</");

        foreach (var tagGroup in tags.Select(x => new { Id = x.Key, Tag = x.Value.GetTagBuilder() }))
        {
            try
            {
                var tagDicName = tagGroup.Id;
                var tag = tagGroup.Tag;

                var temp = htmlDecodedText.TryReplaceCore(tag, tagDicName, out var tempHtmlDecodedText);
                if (!temp)
                    continue;

                htmlDecodedText = tempHtmlDecodedText;
            }
            catch
            {
            }
        }

        return new HtmlString(htmlDecodedText);
    }

    private static bool TryReplaceCore(this string html, TagBuilder tag, string tagName, out string editedHtmlText)
    {
        editedHtmlText = html;
        var tagStart = $"<{tagName}>";
        var tagEnd = $"</{tagName}>";
        var attributes = string.Join(" ", tag.Attributes.Select(x => $"{x.Key}='{x.Value}'"));

        var innerHtmlStartIndex = html.IndexOf(tagStart) + tagStart.Length;
        var innerHtmlEndIndex = html.IndexOf(tagEnd, innerHtmlStartIndex);

        if (innerHtmlStartIndex == -1 || innerHtmlEndIndex == -1)
            return false;

        var innerHtml = html[innerHtmlStartIndex..innerHtmlEndIndex];

        var stringBuilder = new StringBuilder(html);
        var finalInnerHtml = tag.HasInnerHtml
            ? tag.InnerHtml.GetString().Trim()
            : innerHtml?.Trim();

        if (!string.IsNullOrEmpty(finalInnerHtml))
        {
            if (innerHtmlStartIndex >= 0 && !string.IsNullOrEmpty(innerHtml))
                stringBuilder.Remove(innerHtmlStartIndex, innerHtml.Length);

            stringBuilder.Insert(innerHtmlStartIndex, finalInnerHtml);
            html = stringBuilder.ToString();
        }

        html = html.Replace(tagStart, "<" + tag.TagName + " " + attributes + ">");
        html = html.Replace(tagEnd, $"</{tag.TagName}>");
        editedHtmlText = html;
        return true;
    }

    public static HtmlString ReplaceHtml(this string html, params Func<string, IHtmlContent>[] tags)
    {
        if (string.IsNullOrEmpty(html))
            return new HtmlString(null);

        if (tags?.Any() != true)
            return new HtmlString(html);

        html = html
            .Replace("\r\n", "")
            .Replace("</ ", "</");

        for (var i = 0; i < tags.Length; i++)
        {
            var tag = tags[i].GetTagBuilder();

            var temp = html.TryReplaceCore(tag, i.ToString(), out var tempHtmlDecodedText);
            if (!temp)
                continue;

            html = tempHtmlDecodedText;
        }

        return new HtmlString(html);
    }

    public static HtmlString Replace(this IHtmlHelper _, string htmlDecodedText, params Func<string, IHtmlContent>[] tags)
    {
        return ReplaceHtml(htmlDecodedText, tags);
    }

    public static HtmlString ReplaceByTagName(this IHtmlHelper _, string htmlDecodedText, Dictionary<string, Func<string, IHtmlContent>> tags)
    {
        return ReplaceHtmlByTagName(htmlDecodedText, tags);
    }

    public static (TagHelperContext, TagHelperOutput) Init<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var (context, output) = tagHelper.InitCore(unencodedContent);
        tagHelper.Process(context, output);
        return new ValueTuple<TagHelperContext, TagHelperOutput>(context, output);
    }

    public static async Task<(TagHelperContext, TagHelperOutput)> InitAsync<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var (context, output) = tagHelper.InitCore(unencodedContent);
        await tagHelper.ProcessAsync(context, output);
        return new ValueTuple<TagHelperContext, TagHelperOutput>(context, output);
    }

    public static TagHelperOutput GetOutput<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var (context, output) = tagHelper.Init(unencodedContent);
        return output;
    }

    public static async Task<TagHelperOutput> GetOutputAsync<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var (context, output) = await tagHelper.InitAsync(unencodedContent).ConfigureAwait(false);
        return output;
    }

    public static TagBuilder GetTagBuilder<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var tagHelperOutput = tagHelper.GetOutput(unencodedContent);
        var tagBuilder = tagHelperOutput.GetTagBuilder();
        return tagBuilder;
    }

    public static async Task<TagBuilder> GetTagBuilderAsync<THelper>(this THelper tagHelper, string unencodedContent = null) where THelper : TagHelper
    {
        var tagHelperOutput = await tagHelper.GetOutputAsync(unencodedContent);
        var tagBuilder = tagHelperOutput.GetTagBuilder();
        return tagBuilder;
    }

    public static List<SelectListItem> GetSelectListItemFromMetadata(this TagHelperOutput output, ModelExpression @for, ILocalizer localizer)
    {
        var items = new List<SelectListItem>();
        if (@for == null)
            return items;

        var metadata = @for.Metadata;
        // var containerType = metadata.ContainerType;
        var forType = metadata.ModelType;

        // var propName = metadata.Name;
        var value = @for.Model;

        var underlyingType = forType.GetUnderlyingType();
        if (Nullable.GetUnderlyingType(forType) != null)
        {
            var emptyItem = new SelectListItem(localizer["Choose"], "");
            items.Add(emptyItem);
        }

        if (underlyingType.IsEnum)
        {
            var enums = EnumReflections.ToArray(underlyingType);
            if (!enums.Any())
                return items;

            foreach (var enumKey in enums)
            {
                var @enum = Enum.Parse(underlyingType, enumKey.ToString()) as Enum;
                var enumName = @enum.ToString();

                var selected = false;
                if (value != null)
                {
                    if (value is IEnumerable list)
                    {
                        foreach (var obj in list)
                        {
                            selected = !string.IsNullOrEmpty(obj.ToString()) && obj.ToString().Equals(enumName, StringComparison.InvariantCultureIgnoreCase);
                            if (selected)
                                break;
                        }
                    }
                    else
                    {
                        selected = !string.IsNullOrEmpty(value.ToString()) && value.ToString().Equals(enumName, StringComparison.InvariantCultureIgnoreCase);
                    }
                }
                var item = new SelectListItem
                {
                    Value = enumKey.ToString(),
                    Text = localizer[@enum],
                    Selected = selected,
                };
                items.Add(item);
            }
        }
        else
        {
            if (underlyingType != typeof(bool))
                return items;

            var trueItem = new SelectListItem
            {
                Value = true.ToString(),
                Text = localizer["Yes"] ?? "Yes",
                Selected = value is bool propBool && propBool
            };
            items.Add(trueItem);
            var falseItem = new SelectListItem
            {
                Value = false.ToString(),
                Text = localizer["No"] ?? "No",
                Selected = value is bool propBool2 && !propBool2
            };
            items.Add(falseItem);
        }

        return items;
    }

    internal static void SetAttribute(this AttributeDictionary attributeDictionary, string key, string value)
    {
        if (attributeDictionary.ContainsKey(key))
            attributeDictionary.Remove(key);

        if (!attributeDictionary.ContainsKey(key))
            attributeDictionary.Add(key, value);
    }

    internal static void RemoveAttribute(this TagHelperAttributeList attributeList, string name)
    {
        attributeList.SetAttribute(name, "");
        var minifierAttr = attributeList.FirstOrDefault(x => x.Name == name);
        if (minifierAttr != null)
            attributeList.Remove(minifierAttr);
    }

    public static void MoveAttributesTo(this TagHelperOutput output, TagBuilder tagBuilder)
    {
        var attributes = output.Attributes;
        if (attributes == null || !attributes.Any())
            return;

        var removingClasses = new List<string>();
        var removingAttributes = new List<TagHelperAttribute>();
        foreach (var attribute in attributes)
        {
            if (attribute.Name.Equals("disabled", StringComparison.InvariantCultureIgnoreCase))
                continue;

            if (attribute.Name.Equals("class", StringComparison.InvariantCultureIgnoreCase))
            {
                if (attribute.Value != null && !string.IsNullOrEmpty(attribute.Value.ToString()))
                {
                    var classes = attribute.Value.ToString().Split(" ").ToList();
                    if (classes?.Any() == true)
                    {
                        foreach (var @class in classes)
                        {
                            tagBuilder.AddCssClass(@class);
                            removingClasses.Add(@class);
                        }
                    }
                }

                continue;
            }

            if (tagBuilder.Attributes.ContainsKey(attribute.Name))
                tagBuilder.Attributes.Remove(tagBuilder.Attributes[attribute.Name]);

            tagBuilder.Attributes.SetAttribute(attribute.Name, attribute.Value.ToString());
            removingAttributes.Add(attribute);
        }

        if (removingAttributes.Any())
            removingAttributes.ForEach(attribute => output.Attributes.Remove(attribute));

        if (removingClasses.Any())
            removingClasses.ForEach(@class => output.RemoveClass(@class, HtmlEncoder.Default));
    }
}
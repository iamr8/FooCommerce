using System.Text;

namespace FooCommerce.NotificationAPI.Models.Communications;

public record NotificationEmailModel
{
    public IReadOnlyDictionary<string, string> Values { get; init; }
    public string Html { get; init; }

    public NotificationEmailModel() { }
    private NotificationEmailModel(string html, IDictionary<string, string> values)
    {
        Values = (IReadOnlyDictionary<string, string>)values;
        Html = html;
    }

    /// <summary>
    /// Creates an instance of <see cref="NotificationEmailModel"/>.
    /// </summary>
    /// <param name="html"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static NotificationEmailModel GetInstance(string html, IDictionary<string, string> values)
    {
        if (html == null) throw new ArgumentNullException(nameof(html));
        if (values == null) throw new ArgumentNullException(nameof(values));

        var htmlBuilder = new StringBuilder(html);
        foreach (var (key, value) in values)
        {
            htmlBuilder.Replace(key, value);
        }

        return new NotificationEmailModel(htmlBuilder.ToString(), values);
    }
}
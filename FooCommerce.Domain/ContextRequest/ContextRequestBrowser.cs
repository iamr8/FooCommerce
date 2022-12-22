namespace FooCommerce.Domain.ContextRequest;

public record ContextRequestBrowser
{
    public ContextRequestBrowser(string name, string version)
    {
        Name = name;
        Version = version;
    }

    public ContextRequestBrowser()
    {
    }

    public string Name { get; init; }
    public string Version { get; init; }

    public override string ToString()
    {
        return $"{Name} v{Version}";
    }

    public static explicit operator ContextRequestBrowser(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return null;

        var parts = s.Split(" v");
        return new ContextRequestBrowser(parts[0], parts[1]);
    }

    public static implicit operator string(ContextRequestBrowser browser)
    {
        return browser.ToString();
    }
}
namespace FooCommerce.Domain.ContextRequest;

public record ContextRequestDevice
{
    public ContextRequestDevice()
    {
    }

    public ContextRequestDevice(string type)
    {
        Type = type;
    }

    public string Type { get; init; }

    public override string ToString()
    {
        return Type;
    }

    public static explicit operator ContextRequestDevice(string s)
    {
        if (string.IsNullOrWhiteSpace(s))
            return null;

        return new ContextRequestDevice(s);
    }

    public static implicit operator string(ContextRequestDevice device)
    {
        return device.ToString();
    }
}
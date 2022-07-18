namespace FooCommerce.Domain.DbProvider.Interfaces
{
    public interface IEntityProductBarcode
    {
        string Barcode { get; set; }
        int BarcodeType { get; set; }
    }
}
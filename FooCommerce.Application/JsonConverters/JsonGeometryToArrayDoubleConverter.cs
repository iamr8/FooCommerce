using NetTopologySuite.Geometries;

using Newtonsoft.Json;

namespace FooCommerce.Application.JsonConverters;

public class JsonGeometryToArrayDoubleConverter : JsonConverter<Geometry>
{
    public override void WriteJson(JsonWriter writer, Geometry value, JsonSerializer serializer)
    {
        if (value == null)
            return;

        var coordinates = new double[2];
        coordinates[0] = value.Coordinate.X;
        coordinates[1] = value.Coordinate.Y;
        writer.WriteValue(coordinates);
    }

    public override Geometry ReadJson(JsonReader reader, Type objectType, Geometry existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        var coordinates = (double[])reader.Value;
        if (coordinates == null)
            return null;

        var geometry = new Point(coordinates[0], coordinates[1]) { SRID = 4326 };
        return geometry;
    }
}
using System.Xml;
using System.Xml.Serialization;

namespace CatFactory.SqlServer.Tests.Helpers;

public static class XmlSerializerHelper
{
    public static string Serialize<T>(T obj)
    {
        var serializer = new XmlSerializer(obj.GetType());
        using var writer = new StringWriter();
        serializer.Serialize(writer, obj);

        return writer.ToString();
    }

    public static T Deserialze<T>(string source)
    {
        using var stream = new FileStream(source, FileMode.Open);

        using var reader = XmlReader.Create(stream);

        var serializer = new XmlSerializer(typeof(T));
        return (T)serializer.Deserialize(reader);
    }
}

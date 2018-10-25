using Newtonsoft.Json;
using System;
using System.Net;

namespace MinChain
{
    public class IPEndPointConverter : JsonConverter
    {
        public override void WriteJson(
            JsonWriter writer, object value, JsonSerializer serializer)
        {
            var ipEndPoint = value as IPEndPoint;
            if (ipEndPoint.IsNull())
                writer.WriteNull();
            else
                writer.WriteValue(
                    $"{ipEndPoint.Address}:{ipEndPoint.Port}");
        }

        public override object ReadJson(
            JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;

            var hexString = reader.Value.ToString();
            var array = hexString.Split(':');

            return new IPEndPoint(
                Resolve(array[0]),
                int.Parse(array[1]));
        }

        static IPAddress Resolve(string hostNameOrAddress)
        {
            return
                IPAddress.TryParse(hostNameOrAddress, out var addr) ? addr :
                Dns.GetHostAddresses(hostNameOrAddress)[0];
        }

        public override bool CanConvert(Type objectType) =>
            objectType == typeof(IPEndPoint);
    }
}

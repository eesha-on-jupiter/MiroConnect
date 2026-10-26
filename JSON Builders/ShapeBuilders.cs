using System.Globalization;
using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class ShapeBuilders
    {
        public static string BuildShapeStyle(string fillColor, string strokeColor, double strokeWidth, int fontSize)
        {
            var obj = new JObject();
            if (!string.IsNullOrWhiteSpace(fillColor))   obj["fillColor"]   = fillColor;
            if (!string.IsNullOrWhiteSpace(strokeColor)) obj["borderColor"] = strokeColor;
            if (strokeWidth > 0) obj["borderWidth"] = strokeWidth.ToString(CultureInfo.InvariantCulture);
            if (fontSize > 0)    obj["fontSize"]    = fontSize.ToString(CultureInfo.InvariantCulture);
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string BuildShapeBody(string shape, string content, string geometryJson, string styleJson, string parentJson)
        {
            var body = new JObject();

            var data = new JObject();
            if (!string.IsNullOrWhiteSpace(shape))   data["shape"]   = shape;
            if (!string.IsNullOrWhiteSpace(content)) data["content"] = content;
            body["data"] = data;

            SharedBuilders.ApplyGeometry(body, geometryJson);
            if (!string.IsNullOrWhiteSpace(styleJson)) body["style"] = JObject.Parse(styleJson);
            SharedBuilders.ApplyParent(body, parentJson);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

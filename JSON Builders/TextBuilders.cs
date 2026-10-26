using System.Globalization;
using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class TextBuilders
    {
        public static string BuildTextStyle(string fillColor, string fontFamily, int fontSize, string textAlign)
        {
            var obj = new JObject();
            if (!string.IsNullOrWhiteSpace(fillColor))  obj["fillColor"]  = fillColor;
            if (!string.IsNullOrWhiteSpace(fontFamily)) obj["fontFamily"] = fontFamily;
            if (fontSize > 0) obj["fontSize"] = fontSize.ToString(CultureInfo.InvariantCulture);
            if (!string.IsNullOrWhiteSpace(textAlign)) obj["textAlign"] = textAlign;
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string BuildTextBody(string content, string geometryJson, string styleJson, string parentJson)
        {
            var body = new JObject();

            var data = new JObject();
            if (!string.IsNullOrWhiteSpace(content)) data["content"] = content;
            body["data"] = data;

            SharedBuilders.ApplyGeometryWidthOnly(body, geometryJson);
            if (!string.IsNullOrWhiteSpace(styleJson)) body["style"] = JObject.Parse(styleJson);
            SharedBuilders.ApplyParent(body, parentJson);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

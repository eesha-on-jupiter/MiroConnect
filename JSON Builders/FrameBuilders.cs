using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class FrameBuilders
    {
        public static string BuildFrameStyle(string fillColor, string borderColor)
        {
            var obj = new JObject();
            if (!string.IsNullOrWhiteSpace(fillColor))   obj["fillColor"]   = fillColor;
            if (!string.IsNullOrWhiteSpace(borderColor)) obj["borderColor"] = borderColor;
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string BuildFrameBody(string title, string geometryJson, string styleJson)
        {
            var body = new JObject();

            var data = new JObject { ["type"] = "freeform", ["format"] = "custom" };
            if (!string.IsNullOrWhiteSpace(title)) data["title"] = title;
            body["data"] = data;

            SharedBuilders.ApplyGeometry(body, geometryJson);
            if (!string.IsNullOrWhiteSpace(styleJson)) body["style"] = JObject.Parse(styleJson);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

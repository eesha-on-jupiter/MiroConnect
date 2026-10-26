using System.Globalization;
using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class StickyNoteBuilders
    {
        public static string BuildStickyNoteStyle(string fillColor, string textAlign, int fontSize)
        {
            var obj = new JObject();
            if (!string.IsNullOrWhiteSpace(fillColor)) obj["fillColor"] = fillColor;
            if (!string.IsNullOrWhiteSpace(textAlign))  obj["textAlign"]  = textAlign;
            if (fontSize > 0) obj["fontSize"] = fontSize.ToString(CultureInfo.InvariantCulture);
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string BuildStickyNoteBody(string content, string geometryJson, string styleJson, string parentJson)
        {
            var body = new JObject();

            var data = new JObject();
            if (!string.IsNullOrWhiteSpace(content)) data["content"] = content;
            body["data"] = data;

            // Sticky notes only accept geometry.width + position.
            if (!string.IsNullOrWhiteSpace(geometryJson))
            {
                var g = JObject.Parse(geometryJson);
                body["geometry"] = new JObject { ["width"] = g["width"] };
                body["position"] = new JObject
                {
                    ["x"]      = g["x"],
                    ["y"]      = g["y"],
                    ["origin"] = "center"
                };
            }

            if (!string.IsNullOrWhiteSpace(styleJson)) body["style"] = JObject.Parse(styleJson);
            SharedBuilders.ApplyParent(body, parentJson);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

using System.Globalization;
using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class ConnectorBuilders
    {
        public static string BuildConnectorStyle(string lineStyle, string strokeColor, double strokeWidth)
        {
            var obj = new JObject();
            // dash patterns go to strokeStyle; routing values are moved to the body's "shape" by BuildConnectorBody
            if (!string.IsNullOrWhiteSpace(lineStyle))
            {
                var value = lineStyle.Trim().ToLowerInvariant();
                if (value == "normal" || value == "dashed" || value == "dotted") obj["strokeStyle"] = value;
                else obj["lineStyle"] = lineStyle;
            }
            if (!string.IsNullOrWhiteSpace(strokeColor)) obj["strokeColor"] = strokeColor;
            if (strokeWidth > 0) obj["strokeWidth"] = strokeWidth.ToString(CultureInfo.InvariantCulture);
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string BuildConnectorBody(string startItemId, string endItemId, string styleJson)
        {
            var body = new JObject
            {
                ["startItem"] = new JObject { ["id"] = startItemId },
                ["endItem"]   = new JObject { ["id"] = endItemId }
            };
            if (!string.IsNullOrWhiteSpace(styleJson))
            {
                var style = JObject.Parse(styleJson);

                // routing is Miro's top-level connector "shape", not a style field
                var lineStyle = style["lineStyle"];
                if (lineStyle != null)
                {
                    style.Remove("lineStyle");
                    body["shape"] = lineStyle;
                }

                if (style.Count > 0) body["style"] = style;
            }
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class CardBuilders
    {
        public static string BuildCardStyle(string cardColor)
        {
            var obj = new JObject();
            if (!string.IsNullOrWhiteSpace(cardColor)) obj["cardTheme"] = cardColor;
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        public static string BuildCardBody(string title, string description, string dueDate, string geometryJson, string styleJson, string parentJson)
        {
            var body = new JObject();

            var data = new JObject();
            if (!string.IsNullOrWhiteSpace(title))       data["title"]       = title;
            if (!string.IsNullOrWhiteSpace(description)) data["description"] = description;
            if (!string.IsNullOrWhiteSpace(dueDate))     data["dueDate"]     = dueDate;
            body["data"] = data;

            SharedBuilders.ApplyGeometry(body, geometryJson);
            if (!string.IsNullOrWhiteSpace(styleJson)) body["style"] = JObject.Parse(styleJson);
            SharedBuilders.ApplyParent(body, parentJson);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

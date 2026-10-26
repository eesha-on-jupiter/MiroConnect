using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class TagBuilders
    {
        public static string BuildTagBody(string title, string fillColor)
        {
            var obj = new JObject();
            if (!string.IsNullOrWhiteSpace(title))     obj["title"]     = title;
            if (!string.IsNullOrWhiteSpace(fillColor)) obj["fillColor"] = fillColor;
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

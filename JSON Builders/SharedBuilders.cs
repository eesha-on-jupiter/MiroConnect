using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    /// <summary>
    /// Geometry, parent, and color helpers shared by all item builder files.
    /// </summary>
    public static class SharedBuilders
    {
        // Produces internal geometry blob: { x, y, width, height, rotation }
        public static string BuildGeometry(double x, double y, double width, double height, double rotation)
        {
            var obj = new JObject
            {
                ["x"]        = x,
                ["y"]        = y,
                ["width"]    = width,
                ["height"]   = height,
                ["rotation"] = rotation
            };
            return obj.ToString(Newtonsoft.Json.Formatting.None);
        }

        // Produces parent blob: { id: frameId }
        public static string BuildParent(string frameId)
        {
            return new JObject { ["id"] = frameId }
                .ToString(Newtonsoft.Json.Formatting.None);
        }

        // Splits internal geometry blob into Miro position + geometry sub-objects (full width + height).
        public static void ApplyGeometry(JObject body, string geometryJson)
        {
            if (string.IsNullOrWhiteSpace(geometryJson)) return;
            var g = JObject.Parse(geometryJson);

            body["position"] = new JObject
            {
                ["x"]      = g["x"],
                ["y"]      = g["y"],
                ["origin"] = "center"
            };

            var geom = new JObject { ["width"] = g["width"], ["height"] = g["height"] };
            if (g["rotation"] != null && (double)g["rotation"] != 0)
                geom["rotation"] = g["rotation"];
            body["geometry"] = geom;
        }

        // Width-only variant for items with fixed aspect ratios (image, text).
        public static void ApplyGeometryWidthOnly(JObject body, string geometryJson)
        {
            if (string.IsNullOrWhiteSpace(geometryJson)) return;
            var g = JObject.Parse(geometryJson);

            body["position"] = new JObject
            {
                ["x"]      = g["x"],
                ["y"]      = g["y"],
                ["origin"] = "center"
            };

            body["geometry"] = new JObject { ["width"] = g["width"] };
        }

        // Attaches parent object to body if parentJson is provided.
        public static void ApplyParent(JObject body, string parentJson)
        {
            if (string.IsNullOrWhiteSpace(parentJson)) return;
            body["parent"] = JObject.Parse(parentJson);
        }
    }
}

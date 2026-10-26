using System;
using Newtonsoft.Json.Linq;

namespace MiroConnect.JsonBuilders
{
    public static class ImageBuilders
    {
        // image — url
        public static string BuildImageBody(string imageUrl, string geometryJson, string parentJson)
        {
            var body = new JObject();

            var data = new JObject();
            if (!string.IsNullOrWhiteSpace(imageUrl)) data["url"] = imageUrl;
            body["data"] = data;

            SharedBuilders.ApplyGeometryWidthOnly(body, geometryJson);
            SharedBuilders.ApplyParent(body, parentJson);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }

        // image — binary upload sentinel (file path or bitmap)
        // CreateItemComponent detects _imageUpload and switches to multipart POST.
        public static string BuildImageUploadSentinel(byte[] imageBytes, string mimeType, string geometryJson, string parentJson)
        {
            var body = new JObject();
            body["_imageUpload"] = true;
            body["_mime"]        = mimeType ?? "image/png";

            SharedBuilders.ApplyGeometryWidthOnly(body, geometryJson);
            SharedBuilders.ApplyParent(body, parentJson);

            body["_data"] = Convert.ToBase64String(imageBytes);
            return body.ToString(Newtonsoft.Json.Formatting.None);
        }
    }
}

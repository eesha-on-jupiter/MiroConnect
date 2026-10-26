using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MiroConnect.Utilities
{
    /// <summary>
    /// Thin HTTP wrapper for the Miro REST API.
    /// All components share this static client — one instance per process.
    /// </summary>
    public static class MiroClient
    {
        public const string BaseUrl = "https://api.miro.com/v2";

        private static readonly HttpClient _http = new HttpClient();

        public static string Get(string token, string path)
        {
            return Send(token, HttpMethod.Get, path, null);
        }

        public static string Post(string token, string path, string jsonBody)
        {
            return Send(token, HttpMethod.Post, path, jsonBody);
        }

        public static string Delete(string token, string path)
        {
            return Send(token, HttpMethod.Delete, path, null);
        }

        public static string Patch(string token, string path, string jsonBody)
        {
            return Send(token, new HttpMethod("PATCH"), path, jsonBody);
        }

        public static string PostImageUpload(string token, string path, byte[] imageBytes, string mimeType, string metadataJson)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, BaseUrl + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var multipart = new MultipartFormDataContent();

            var imageContent = new ByteArrayContent(imageBytes);
            imageContent.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            string fileExt = mimeType == "image/jpeg" ? "jpg"
                           : mimeType == "image/svg+xml" ? "svg"
                           : "png";
            multipart.Add(imageContent, "resource", $"image.{fileExt}");

            if (!string.IsNullOrWhiteSpace(metadataJson))
                multipart.Add(new StringContent(metadataJson, Encoding.UTF8, "application/json"), "metadata");

            request.Content = multipart;

            var response = _http.SendAsync(request).GetAwaiter().GetResult();
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return body;
        }

        private static string Send(string token, HttpMethod method, string path, string jsonBody)
        {
            var request = new HttpRequestMessage(method, BaseUrl + path);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (jsonBody != null)
                request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

            var response = _http.SendAsync(request).GetAwaiter().GetResult();
            var body = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"HTTP {(int)response.StatusCode} {response.ReasonPhrase}: {body}");

            return body;
        }
    }
}

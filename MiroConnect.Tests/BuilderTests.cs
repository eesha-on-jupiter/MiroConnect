using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json.Linq;
using MiroConnect.JsonBuilders;
using Xunit;

namespace MiroConnect.Tests
{
    // Proves each static builder emits the request-body shape the Miro REST API v2 expects.
    // Pure functions in, JSON out: no API, no Grasshopper.
    public class BuilderTests
    {
        private static readonly string Geo = SharedBuilders.BuildGeometry(10, -20, 300, 150, 0);
        private static readonly string GeoRotated = SharedBuilders.BuildGeometry(0, 0, 200, 100, 45);
        private static readonly string Parent = SharedBuilders.BuildParent("frame-1");

        // ── Builder catalogue: every builder, null/empty inputs, must not throw and must emit compact JSON ──

        public static IEnumerable<object[]> AllBuilders()
        {
            yield return Case("Geometry", () => SharedBuilders.BuildGeometry(0, 0, 0, 0, 0), "x", "y", "width", "height", "rotation");
            yield return Case("Parent", () => SharedBuilders.BuildParent(null), "id");
            yield return Case("StickyNoteStyle", () => StickyNoteBuilders.BuildStickyNoteStyle(null, null, 0));
            yield return Case("StickyNoteBody", () => StickyNoteBuilders.BuildStickyNoteBody(null, null, null, null), "data");
            yield return Case("ShapeStyle", () => ShapeBuilders.BuildShapeStyle(null, null, 0, 0));
            yield return Case("ShapeBody", () => ShapeBuilders.BuildShapeBody(null, null, null, null, null), "data");
            yield return Case("TextStyle", () => TextBuilders.BuildTextStyle(null, null, 0, null));
            yield return Case("TextBody", () => TextBuilders.BuildTextBody(null, null, null, null), "data");
            yield return Case("CardStyle", () => CardBuilders.BuildCardStyle(null));
            yield return Case("CardBody", () => CardBuilders.BuildCardBody(null, null, null, null, null, null), "data");
            yield return Case("FrameStyle", () => FrameBuilders.BuildFrameStyle(null, null));
            yield return Case("FrameBody", () => FrameBuilders.BuildFrameBody(null, null, null), "data");
            yield return Case("ImageBody", () => ImageBuilders.BuildImageBody(null, null, null), "data");
            yield return Case("ImageUpload", () => ImageBuilders.BuildImageUploadSentinel(new byte[0], null, null, null), "_imageUpload", "_mime", "_data");
            yield return Case("ConnectorStyle", () => ConnectorBuilders.BuildConnectorStyle(null, null, 0));
            yield return Case("ConnectorBody", () => ConnectorBuilders.BuildConnectorBody(null, null, null), "startItem", "endItem");
            yield return Case("TagBody", () => TagBuilders.BuildTagBody(null, null));
        }

        private static object[] Case(string name, Func<string> invoke, params string[] requiredKeys) =>
            new object[] { name, invoke, requiredKeys };

        [Theory]
        [MemberData(nameof(AllBuilders))]
        public void Builder_EmptyInputs_EmitsCompactJsonWithRequiredKeys(string name, Func<string> invoke, string[] requiredKeys)
        {
            var json = invoke();
            Assert.False(json.Contains("\n"), $"{name}: expected Formatting.None");
            var o = JObject.Parse(json);
            foreach (var key in requiredKeys)
                Assert.True(o.ContainsKey(key), $"{name}: missing key {key}");
        }

        // ── Geometry + parent ──

        [Fact]
        public void Geometry_InternalBlob_HasAllFields()
        {
            var o = JObject.Parse(SharedBuilders.BuildGeometry(1.5, -2, 300, 150, 30));
            Assert.Equal(1.5, (double)o["x"]);
            Assert.Equal(-2, (double)o["y"]);
            Assert.Equal(300, (double)o["width"]);
            Assert.Equal(150, (double)o["height"]);
            Assert.Equal(30, (double)o["rotation"]);
        }

        [Fact]
        public void Parent_IsIdObject()
        {
            Assert.Equal("{\"id\":\"frame-1\"}", Parent);
        }

        [Fact]
        public void ApplyGeometry_SplitsIntoCenterPositionAndGeometry_OmitsZeroRotation()
        {
            var body = new JObject();
            SharedBuilders.ApplyGeometry(body, Geo);
            Assert.Equal("center", (string)body["position"]["origin"]);
            Assert.Equal(10, (double)body["position"]["x"]);
            Assert.Equal(-20, (double)body["position"]["y"]);
            Assert.Equal(300, (double)body["geometry"]["width"]);
            Assert.Equal(150, (double)body["geometry"]["height"]);
            Assert.Null(body["geometry"]["rotation"]);
        }

        [Fact]
        public void ApplyGeometry_NonZeroRotation_IsSent()
        {
            var body = new JObject();
            SharedBuilders.ApplyGeometry(body, GeoRotated);
            Assert.Equal(45, (double)body["geometry"]["rotation"]);
        }

        [Fact]
        public void ApplyGeometryWidthOnly_SendsWidthOnly()
        {
            var body = new JObject();
            SharedBuilders.ApplyGeometryWidthOnly(body, GeoRotated);
            var geom = (JObject)body["geometry"];
            Assert.Single(geom.Properties());
            Assert.Equal(200, (double)geom["width"]);
        }

        // ── Sticky note ──

        [Fact]
        public void StickyNoteBody_Shape()
        {
            var style = StickyNoteBuilders.BuildStickyNoteStyle("light_yellow", "center", 14);
            var o = JObject.Parse(StickyNoteBuilders.BuildStickyNoteBody("Hello", Geo, style, Parent));
            Assert.Equal("Hello", (string)o["data"]["content"]);
            Assert.Equal("center", (string)o["position"]["origin"]);
            Assert.Equal(300, (double)o["geometry"]["width"]);
            Assert.Null(o["geometry"]["height"]);
            Assert.Equal("light_yellow", (string)o["style"]["fillColor"]);
            Assert.Equal("frame-1", (string)o["parent"]["id"]);
        }

        [Fact]
        public void StickyNoteStyle_NumbersAreStrings_EmptyValuesOmitted()
        {
            var o = JObject.Parse(StickyNoteBuilders.BuildStickyNoteStyle("", "left", 18));
            Assert.Null(o["fillColor"]);
            Assert.Equal(JTokenType.String, o["fontSize"].Type);
            Assert.Equal("18", (string)o["fontSize"]);
        }

        // ── Shape ──

        [Fact]
        public void ShapeBody_Shape()
        {
            var style = ShapeBuilders.BuildShapeStyle("#ffffff", "#1a1a1a", 2, 14);
            var o = JObject.Parse(ShapeBuilders.BuildShapeBody("circle", "A", Geo, style, null));
            Assert.Equal("circle", (string)o["data"]["shape"]);
            Assert.Equal("A", (string)o["data"]["content"]);
            Assert.Equal(150, (double)o["geometry"]["height"]);
            Assert.Equal("#1a1a1a", (string)o["style"]["borderColor"]);
            Assert.Equal("2", (string)o["style"]["borderWidth"]);
            Assert.Null(o["parent"]);
        }

        [Fact]
        public void ShapeStyle_StrokeMapsToBorder_ZeroWidthOmitted()
        {
            var o = JObject.Parse(ShapeBuilders.BuildShapeStyle(null, "#000000", 0, 0));
            Assert.Equal("#000000", (string)o["borderColor"]);
            Assert.Null(o["borderWidth"]);
            Assert.Null(o["fontSize"]);
            Assert.Null(o["strokeColor"]);
        }

        [Fact]
        public void ShapeStyle_DecimalWidth_IsCultureInvariant()
        {
            var previous = Thread.CurrentThread.CurrentCulture;
            try
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo("de-DE");
                var o = JObject.Parse(ShapeBuilders.BuildShapeStyle(null, null, 2.5, 0));
                Assert.Equal("2.5", (string)o["borderWidth"]);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        // ── Text ──

        [Fact]
        public void TextBody_Shape_WidthOnly()
        {
            var style = TextBuilders.BuildTextStyle("transparent", "arial", 14, "center");
            var o = JObject.Parse(TextBuilders.BuildTextBody("Title", Geo, style, Parent));
            Assert.Equal("Title", (string)o["data"]["content"]);
            Assert.Null(o["geometry"]["height"]);
            Assert.Equal("arial", (string)o["style"]["fontFamily"]);
            Assert.Equal("14", (string)o["style"]["fontSize"]);
            Assert.Equal("frame-1", (string)o["parent"]["id"]);
        }

        // ── Card ──

        [Fact]
        public void CardBody_Shape_EmptyFieldsOmitted()
        {
            var style = CardBuilders.BuildCardStyle("#2d9bf0");
            var json = CardBuilders.BuildCardBody("Task", "", "2026-10-01T00:00:00Z", Geo, style, null);
            var o = JObject.Parse(json);
            Assert.Equal("Task", (string)o["data"]["title"]);
            Assert.Null(o["data"]["description"]);
            // due date passes through verbatim (JObject.Parse would re-format it as a DateTime)
            Assert.Contains("\"dueDate\":\"2026-10-01T00:00:00Z\"", json);
            Assert.Equal("#2d9bf0", (string)o["style"]["cardTheme"]);
            Assert.Equal(150, (double)o["geometry"]["height"]);
        }

        [Fact]
        public void CardStyle_EmptyColor_IsEmptyObject()
        {
            Assert.Equal("{}", CardBuilders.BuildCardStyle(""));
        }

        // ── Frame ──

        [Fact]
        public void FrameBody_IsFreeformCustom_NoParent()
        {
            var style = FrameBuilders.BuildFrameStyle("#ffffff", null);
            var o = JObject.Parse(FrameBuilders.BuildFrameBody("Sprint", Geo, style));
            Assert.Equal("freeform", (string)o["data"]["type"]);
            Assert.Equal("custom", (string)o["data"]["format"]);
            Assert.Equal("Sprint", (string)o["data"]["title"]);
            Assert.Equal("#ffffff", (string)o["style"]["fillColor"]);
            Assert.Null(o["style"]["borderColor"]);
            Assert.Null(o["parent"]);
        }

        // ── Image ──

        [Fact]
        public void ImageBody_Url_WidthOnly()
        {
            var o = JObject.Parse(ImageBuilders.BuildImageBody("https://example.com/a.png", Geo, Parent));
            Assert.Equal("https://example.com/a.png", (string)o["data"]["url"]);
            Assert.Equal(300, (double)o["geometry"]["width"]);
            Assert.Null(o["geometry"]["height"]);
            Assert.Equal("frame-1", (string)o["parent"]["id"]);
        }

        [Fact]
        public void ImageUploadSentinel_CarriesBase64AndDefaultMime()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            var o = JObject.Parse(ImageBuilders.BuildImageUploadSentinel(bytes, null, Geo, null));
            Assert.True((bool)o["_imageUpload"]);
            Assert.Equal("image/png", (string)o["_mime"]);
            Assert.Equal(Convert.ToBase64String(bytes), (string)o["_data"]);
            Assert.Equal("center", (string)o["position"]["origin"]);
            Assert.Null(o["data"]);
        }

        // ── Connector ──

        [Fact]
        public void ConnectorBody_Shape()
        {
            var o = JObject.Parse(ConnectorBuilders.BuildConnectorBody("a", "b", null));
            Assert.Equal("a", (string)o["startItem"]["id"]);
            Assert.Equal("b", (string)o["endItem"]["id"]);
            Assert.Null(o["style"]);
            Assert.Null(o["position"]);
        }

        [Theory]
        [InlineData("curved")]
        [InlineData("elbowed")]
        [InlineData("straight")]
        public void Connector_RoutingValue_BecomesTopLevelShape(string routing)
        {
            var style = ConnectorBuilders.BuildConnectorStyle(routing, "#1a1a1a", 2);
            var o = JObject.Parse(ConnectorBuilders.BuildConnectorBody("a", "b", style));
            Assert.Equal(routing, (string)o["shape"]);
            Assert.Null(o["style"]["lineStyle"]);
            Assert.Equal("#1a1a1a", (string)o["style"]["strokeColor"]);
            Assert.Equal("2", (string)o["style"]["strokeWidth"]);
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("dashed")]
        [InlineData("dotted")]
        public void Connector_DashValue_BecomesStrokeStyle(string dash)
        {
            var style = ConnectorBuilders.BuildConnectorStyle(dash, null, 0);
            var o = JObject.Parse(ConnectorBuilders.BuildConnectorBody("a", "b", style));
            Assert.Equal(dash, (string)o["style"]["strokeStyle"]);
            Assert.Null(o["shape"]);
        }

        [Fact]
        public void Connector_RoutingOnlyStyle_DropsEmptyStyleObject()
        {
            var style = ConnectorBuilders.BuildConnectorStyle("straight", null, 0);
            var o = JObject.Parse(ConnectorBuilders.BuildConnectorBody("a", "b", style));
            Assert.Equal("straight", (string)o["shape"]);
            Assert.Null(o["style"]);
        }

        // ── Tag ──

        [Fact]
        public void TagBody_Shape()
        {
            var o = JObject.Parse(TagBuilders.BuildTagBody("Priority", "red"));
            Assert.Equal("Priority", (string)o["title"]);
            Assert.Equal("red", (string)o["fillColor"]);
        }

        [Fact]
        public void TagBody_NoColor_OmitsFillColor()
        {
            var o = JObject.Parse(TagBuilders.BuildTagBody("Priority", null));
            Assert.Null(o["fillColor"]);
        }
    }
}

# Image JSON

Builds Miro image request bodies from lists of public URLs, local file paths or Bitmaps.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Image URL | Text (tree) | No | - | Public image URLs. |
| File Path | Text (tree) | No | - | Local image file paths. Takes priority over URL. |
| Bitmap | Generic (tree) | No | - | `System.Drawing.Bitmap` objects. Takes priority over File Path and URL. |
| Position JSON | Text (tree) | No | - | Position JSONs from Position or Grid. |
| Parent JSON | Text (tree) | No | - | Parent JSONs from [Parent](../Geometry/ParentComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item JSON | Text (list) | Image request bodies for Create Item. |

## Output

No API call. Count is the longest of `Image URL`, `File Path`, `Bitmap` and `Position JSON`. Per index, the first available source wins (Bitmap, then File Path, then URL):

- **URL:** `{"data":{"url":"..."},"position":{...,"origin":"center"},"geometry":{"width":...},"parent":{...}}`
- **File Path or Bitmap:** an internal upload payload `{"_imageUpload":true,"_mime":"image/png","position":{...},"geometry":{...},"parent":{...},"_data":"<base64>"}` that Create Item turns into a multipart upload.

## Notes

- Only `width` is sent; Miro keeps the image's aspect ratio.
- Bitmaps are encoded as PNG. File MIME type comes from the extension: `.jpg`/`.jpeg`, `.gif`, `.webp`, `.bmp`, otherwise PNG.
- A missing file gives an error and a `null` at that index.
- Upload payloads carry the whole image as base64, so large images make large strings on the canvas.
- Send with [Create Item](../../HTTP/Items/CreateItemComponent.md) and `Item Type` = `image`.

## Related

- [Create Item](../../HTTP/Items/CreateItemComponent.md)
- [Parent](../Geometry/ParentComponent.md)

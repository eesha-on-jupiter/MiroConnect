# Text JSON

Builds Miro text item request bodies from lists of content, positions, style and parent.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Content | Text (tree) | Yes | - | Text contents, one per item. |
| Position JSON | Text (tree) | Yes | - | Position JSONs from Position, Grid or Offset. |
| Style JSON | Text (tree) | No | - | Style JSON from [Text Style](../Style/TextStyleComponent.md). |
| Parent JSON | Text (tree) | No | - | Parent JSONs from [Parent](../Geometry/ParentComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item JSON | Text (list) | Text item request bodies for Create Item. |

## Output

Builds `{"data":{"content":"..."},"position":{"x":0,"y":0,"origin":"center"},"geometry":{"width":200},"style":{...},"parent":{"id":"..."}}`. No API call. Count is the longer of `Content` and `Position JSON`.

## Notes

- Only `width` is sent; height and rotation from the Position are ignored.
- Send with [Create Item](../../HTTP/Items/CreateItemComponent.md) and `Item Type` = `text`.

## Related

- [Text Style](../Style/TextStyleComponent.md) and [Font Family](../../Presets/FontFamilyPreset.md)
- [Offset](../Geometry/OffsetComponent.md)

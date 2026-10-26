# Shape JSON

Builds Miro shape request bodies from lists of shape types, content, positions, style and parent.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Shape Type | Text (tree) | Yes | - | Shape type strings, e.g. `rectangle`, `circle`. Use the [Shape Type](../../Presets/ShapeTypePreset.md) preset. |
| Content | Text (tree) | No | - | Text inside each shape. |
| Position JSON | Text (tree) | Yes | - | Position JSONs from Position or Grid. |
| Style JSON | Text (tree) | No | - | Style JSON from [Shape Style](../Style/ShapeStyleComponent.md). |
| Parent JSON | Text (tree) | No | - | Parent JSONs from [Parent](../Geometry/ParentComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item JSON | Text (list) | Shape request bodies for Create Item. |

## Output

Builds `{"data":{"shape":"rectangle","content":"..."},"position":{"x":0,"y":0,"origin":"center"},"geometry":{"width":200,"height":200},"style":{...},"parent":{"id":"..."}}`. No API call. Count is the number of `Position JSON` entries.

## Notes

- Full geometry is sent: width, height, and rotation when it is non-zero.
- One shape per position: add positions (e.g. raise Grid `Count`) to get more shapes.
- Send with [Create Item](../../HTTP/Items/CreateItemComponent.md) and `Item Type` = `shape`.

## Related

- [Shape Style](../Style/ShapeStyleComponent.md) and [Shape Type](../../Presets/ShapeTypePreset.md)
- [Connector JSON](ConnectorJsonComponent.md): join the created shapes.

# Sticky Note JSON

Builds Miro sticky note request bodies from lists of content, positions, style and parent.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Content | Text (tree) | Yes | - | Sticky note text, one per note. |
| Position JSON | Text (tree) | Yes | - | Position JSONs from Position or Grid. |
| Style JSON | Text (tree) | No | - | Style JSON from [Sticky Note Style](../Style/StickyNoteStyleComponent.md). |
| Parent JSON | Text (tree) | No | - | Parent JSONs from [Parent](../Geometry/ParentComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item JSON | Text (list) | Sticky note request bodies for Create Item. |

## Output

Builds `{"data":{"content":"..."},"geometry":{"width":200},"position":{"x":0,"y":0,"origin":"center"},"style":{...},"parent":{"id":"..."}}`. No API call. Count is the longer of `Content` and `Position JSON`.

## Notes

- Only `width` is sent: Miro sizes sticky note height itself, so the Position's height and rotation are ignored.
- Send with [Create Item](../../HTTP/Items/CreateItemComponent.md) and `Item Type` = `sticky_note`.

## Related

- [Sticky Note Style](../Style/StickyNoteStyleComponent.md) and [Sticky Note Color](../../Presets/StickyNoteColorPreset.md)
- [Create Item](../../HTTP/Items/CreateItemComponent.md)

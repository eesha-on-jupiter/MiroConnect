# Frame JSON

Builds Miro frame request bodies from lists of titles, positions and style.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Title | Text (tree) | Yes | - | Frame titles. |
| Position JSON | Text (tree) | Yes | - | Position JSONs from Position or Grid. |
| Style JSON | Text (tree) | No | - | Style JSON from [Frame Style](../Style/FrameStyleComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item JSON | Text (list) | Frame request bodies for Create Item. |

## Output

Builds `{"data":{"type":"freeform","format":"custom","title":"..."},"position":{"x":0,"y":0,"origin":"center"},"geometry":{"width":200,"height":200},"style":{...}}`. No API call. Count is the longer of `Title` and `Position JSON`.

## Notes

- Frames are always `freeform` with a `custom` format.
- No `Parent JSON` input: frames are placed directly on the board.
- Send with [Create Item](../../HTTP/Items/CreateItemComponent.md) and `Item Type` = `frame`, then wire its `Item IDs` into [Parent](../Geometry/ParentComponent.md) to fill the frame.

## Related

- [Frame Style](../Style/FrameStyleComponent.md)
- [Parent](../Geometry/ParentComponent.md)

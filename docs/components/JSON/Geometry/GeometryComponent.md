# Position

Builds one Position JSON from a center point, size and rotation.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| X | Number | No | 0 | Center X position on the board canvas. |
| Y | Number | No | 0 | Center Y position on the board canvas (positive is down). |
| Width | Number | No | 200 | Item width in pixels. |
| Height | Number | No | 200 | Item height in pixels. |
| Rotation | Number | No | 0 | Rotation in degrees (clockwise). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Position JSON | Text | Position JSON for the Items JSON components. |

## Output

Builds the plugin's internal blob `{"x":0,"y":0,"width":200,"height":200,"rotation":0}`. No API call. Each Items JSON component splits it into Miro's `position` (with `"origin":"center"`) and `geometry` objects.

## Notes

- Miro's origin `(0, 0)` is the board center; X grows right, Y grows down.
- Not every item type uses every field: sticky notes, text and images keep only `width` (Miro sizes the height), and rotation is only sent for shapes, cards and frames when it is non-zero.
- The source file is `GeometryComponent.cs`, but the component is named **Position** on the ribbon.

## Related

- [Grid](GridComponent.md): many positions at once.
- [Offset](OffsetComponent.md): shift this position.
- [Items JSON tab](../Items/index.md): every builder's `Position JSON` input.

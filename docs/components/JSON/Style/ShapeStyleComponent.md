# Shape Style

Builds a Miro shape style object.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Fill Color | Text | No | `#ffffff` | Fill color as hex. |
| Stroke Color | Text | No | `#1a1a1a` | Border color as hex. |
| Stroke Width | Number | No | 2 | Border width in pixels. |
| Font Size | Integer | No | 14 | Font size in points. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Style JSON | Text | Shape style JSON for Shape JSON. |

## Output

Builds `{"fillColor":"#ffffff","borderColor":"#1a1a1a","borderWidth":"2","fontSize":"14"}`. No API call. `Stroke Color` maps to Miro's `borderColor` and `Stroke Width` to `borderWidth`. Empty values and widths or sizes of 0 or less are left out.

## Notes

- There is no font family input on this component.

## Related

- [Shape JSON](../Items/ShapeJsonComponent.md)
- [Shape Type](../../Presets/ShapeTypePreset.md)

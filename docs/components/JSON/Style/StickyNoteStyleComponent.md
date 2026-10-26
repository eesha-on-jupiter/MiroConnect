# Sticky Note Style

Builds a Miro sticky note style object.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Fill Color | Text | No | `light_yellow` | Named fill color, e.g. `light_yellow`, `cyan`, `pink`. Use the [Sticky Note Color](../../Presets/StickyNoteColorPreset.md) preset. |
| Text Align | Text | No | `center` | Horizontal alignment: `left`, `center` or `right`. |
| Font Size | Integer | No | 14 | Font size in points. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Style JSON | Text | Sticky note style JSON for Sticky Note JSON. |

## Output

Builds `{"fillColor":"light_yellow","textAlign":"center","fontSize":"14"}`. No API call. Empty values and a font size of 0 or less are left out.

## Notes

- Sticky notes take named colors only, not hex.

## Related

- [Sticky Note JSON](../Items/StickyNoteJsonComponent.md)
- [Sticky Note Color](../../Presets/StickyNoteColorPreset.md)

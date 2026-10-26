# Text Style

Builds a Miro text item style object.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Fill Color | Text | No | `transparent` | Background fill as hex, or `transparent`. |
| Font Family | Text | No | `arial` | Font family, e.g. `arial`, `open_sans`. Use the [Font Family](../../Presets/FontFamilyPreset.md) preset. |
| Font Size | Integer | No | 14 | Font size in points. |
| Text Align | Text | No | `center` | Horizontal alignment: `left`, `center` or `right`. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Style JSON | Text | Text style JSON for Text JSON. |

## Output

Builds `{"fillColor":"transparent","fontFamily":"arial","fontSize":"14","textAlign":"center"}`. No API call. Empty values and a font size of 0 or less are left out.

## Related

- [Text JSON](../Items/TextJsonComponent.md)
- [Font Family](../../Presets/FontFamilyPreset.md)

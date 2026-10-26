# Font Family

A value-list preset of Miro font families, for [Text Style](../JSON/Style/TextStyleComponent.md)'s `Font Family` input.

## Inputs

None.

## Outputs

One unnamed value-list output: the selected entry's value as text.

## Values

| Display Name | Value |
|---|---|
| Arial | `arial` |
| Open Sans | `open_sans` |
| Roboto | `roboto` |
| PT Sans | `pt_sans` |
| PT Serif | `pt_serif` |
| Roboto Mono | `roboto_mono` |
| Caveat | `caveat` |
| Permanent Marker | `permanent_marker` |
| Georgia | `georgia` |
| Times New Roman | `times_new_roman` |

## Feeds into

- [Text Style](../JSON/Style/TextStyleComponent.md) `Font Family`.

## Notes

- All values are from Miro's `fontFamily` enum; Miro accepts more families than the ten listed here, and any of them can be typed into a Panel instead.
- [Shape Style](../JSON/Style/ShapeStyleComponent.md) has no font family input.

## Related

- [Text JSON](../JSON/Items/TextJsonComponent.md)

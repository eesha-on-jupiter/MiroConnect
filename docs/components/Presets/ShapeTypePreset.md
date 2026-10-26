# Shape Type

A value-list preset of Miro shape types, for [Shape JSON](../JSON/Items/ShapeJsonComponent.md)'s `Shape Type` input.

## Inputs

None.

## Outputs

One unnamed value-list output: the selected entry's value as text.

## Values

| Display Name | Value |
|---|---|
| Rectangle | `rectangle` |
| Round Rectangle | `round_rectangle` |
| Circle | `circle` |
| Triangle | `triangle` |
| Rhombus | `rhombus` |
| Parallelogram | `parallelogram` |
| Trapezoid | `trapezoid` |
| Pentagon | `pentagon` |
| Hexagon | `hexagon` |
| Octagon | `octagon` |
| Star | `star` |
| Arrow Left | `arrow_left` |
| Arrow Right | `arrow_right` |
| Arrow Up Down | `arrow_up_down` |
| Cross | `cross` |
| Cloud | `cloud` |
| Can | `can` |
| Message Bubble | `message_bubble` |

## Feeds into

- [Shape JSON](../JSON/Items/ShapeJsonComponent.md) `Shape Type`.

## Notes

- Values are Miro's `data.shape` strings; anything else is rejected by the API.

## Related

- [Shape Style](../JSON/Style/ShapeStyleComponent.md)

# Connector Style

Builds a Miro connector style object.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Line Style | Text | No | `curved` | Line routing (`curved`, `elbowed`, `straight`) or a dash pattern (`normal`, `dashed`, `dotted`) from the [Stroke Style](../../Presets/StrokeStylePreset.md) preset. |
| Stroke Color | Text | No | `#1a1a1a` | Stroke color as hex. |
| Stroke Width | Number | No | 2 | Stroke width in pixels. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Style JSON | Text | Connector style JSON for Connector JSON. |

## Output

Builds `{"lineStyle":"curved","strokeColor":"#1a1a1a","strokeWidth":"2"}`, or `{"strokeStyle":"dashed",...}` for a dash pattern. No API call. Empty values and a width of 0 or less are left out; numbers are written as invariant-culture strings (`2.5`, never `2,5`).

## Notes

- A routing value is carried as `lineStyle` in the Style JSON; [Connector JSON](../Items/ConnectorJsonComponent.md) moves it to the request body's top-level `shape` field, which is where Miro reads routing.
- A dash pattern goes to `style.strokeStyle`. One input carries one or the other: routing and dash pattern cannot both be set on the same connector in this release.

## Related

- [Connector JSON](../Items/ConnectorJsonComponent.md)
- [Create Connector](../../HTTP/Connectors/CreateConnectorComponent.md)

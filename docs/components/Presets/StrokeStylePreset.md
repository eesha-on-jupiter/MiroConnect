# Stroke Style

A value-list preset of Miro connector dash patterns (solid, dashed, dotted), for [Connector Style](../JSON/Style/ConnectorStyleComponent.md)'s `Line Style` input.

## Inputs

None.

## Outputs

One unnamed value-list output: the selected entry's value as text.

## Values

| Display Name | Value |
|---|---|
| Solid | `normal` |
| Dashed | `dashed` |
| Dotted | `dotted` |

## Feeds into

- [Connector Style](../JSON/Style/ConnectorStyleComponent.md) `Line Style`: these three values are written to the connector's `style.strokeStyle`.

## Notes

- `Line Style` takes either a dash pattern from this preset or a routing value (`curved`, `elbowed`, `straight`), not both; with a dash pattern the connector uses Miro's default routing (`curved`).

## Related

- [Connector Style](../JSON/Style/ConnectorStyleComponent.md)
- [Connector JSON](../JSON/Items/ConnectorJsonComponent.md)

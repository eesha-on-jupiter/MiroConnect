# Card Style

Builds a Miro card style object.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Card Color | Text | No | `#2d9bf0` | Card theme color as hex. Use the [Card Color](../../Presets/CardColorPreset.md) preset. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Style JSON | Text | Card style JSON for Card JSON. |

## Output

Builds `{"cardTheme":"#2d9bf0"}`. No API call. An empty color gives `{}`.

## Related

- [Card JSON](../Items/CardJsonComponent.md)
- [Card Color](../../Presets/CardColorPreset.md)

# Frame Style

Builds a Miro frame style object.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Fill Color | Text | No | `#ffffff` | Frame background color as hex. |
| Border Color | Text | No | `#1a1a1a` | Frame border color as hex. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Style JSON | Text | Frame style JSON for Frame JSON. |

## Output

Builds `{"fillColor":"#ffffff","borderColor":"#1a1a1a"}`. No API call. Empty values are left out.

## Notes

- Leave `Border Color` empty to send `fillColor` only.

## Related

- [Frame JSON](../Items/FrameJsonComponent.md)

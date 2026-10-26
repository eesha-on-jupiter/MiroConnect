# Tag JSON

Builds Miro tag request bodies from lists of titles and fill colors.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Title | Text (tree) | Yes | - | Tag label texts. |
| Fill Color | Text (tree) | No | - | Named tag colors: `red`, `light_green`, `cyan`, `yellow`, `magenta`, `green`, `blue`, `gray`, `violet`, `dark_green`, `dark_blue`, `black`. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Tag JSON | Text (list) | Tag request bodies for Create Tag. |

## Output

Builds `{"title":"...","fillColor":"blue"}`. No API call. Count is the number of `Title` entries.

## Notes

- With no `Fill Color`, `fillColor` is left out and Miro uses its default (`red`).
- Miro's tag `fillColor` is a fixed set of names, not hex: a hex value is rejected by Create Tag.
- Send with [Create Tag](../../HTTP/Tags/CreateTagComponent.md).

## Related

- [Create Tag](../../HTTP/Tags/CreateTagComponent.md)
- [Attach Tag](../../HTTP/Tags/AttachTagComponent.md)

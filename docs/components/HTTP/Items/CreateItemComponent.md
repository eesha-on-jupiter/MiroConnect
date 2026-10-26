# Create Item

Creates one or more Miro board items from a list of item JSONs, one API call per item.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board to create the items on. The first value is used. |
| Item Type | Text (tree) | Yes | - | `sticky_note`, `shape`, `text`, `card`, `frame` or `image`. The first value is used. |
| Item JSON | Text (tree) | Yes | - | List or tree of request bodies from the matching Items JSON component. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item IDs | Text (list) | IDs of the newly created items, in input order. |
| Responses | Text (list) | Raw JSON responses from the Miro API, in input order. |

## API endpoint

`POST /v2/boards/{board_id}/{segment}` where `segment` comes from `Item Type`:

| Item Type | Path segment |
|---|---|
| `sticky_note` (or `sticky`) | `sticky_notes` |
| `shape` | `shapes` |
| `text` | `texts` |
| `card` | `cards` |
| `frame` | `frames` |
| `image` | `images` |

## Notes

- Button-triggered (**Create**): only fires on a click, never on recompute.
- All inputs are tree access, so the component runs once per click whatever the upstream data structure. Every branch of `Item JSON` is flattened into one ordered list.
- One `Item Type` per click: all JSONs must be of that type. Use a second Create Item for a second type.
- Image JSONs built from a file path or Bitmap are sent as a multipart upload (`resource` + `metadata` parts); URL images are posted as JSON. The wiring is the same either way.
- A failed item logs `Item N: <error>` and leaves `null` at its index, so IDs stay aligned with the input list.
- Each click creates new items; nothing is updated or replaced.

## Related

- [Items JSON tab](../../JSON/Items/index.md): builds the `Item JSON` input.
- [Delete Item](DeleteItemComponent.md): remove what this created.
- [Parent](../../JSON/Geometry/ParentComponent.md): feed a created frame's ID back in to place items inside it.

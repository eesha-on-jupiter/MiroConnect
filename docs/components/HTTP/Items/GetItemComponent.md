# Get Item

Retrieves a single Miro board item and returns its type, center position and size.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board ID | Text | Yes | - | ID of the board containing the item. |
| Item ID | Text | Yes | - | ID of the item to retrieve. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Type | Text | Item type, e.g. `sticky_note`, `shape`, `frame`. |
| X | Number | Item center X position. |
| Y | Number | Item center Y position. |
| Width | Number | Item width. |
| Height | Number | Item height. |
| Response | Text | Raw JSON response from the Miro API. |

## API endpoint

`GET /v2/boards/{board_id}/items/{item_id}`

## Notes

- Read-only, runs on every recompute.
- Any missing position or geometry field comes back as `0`.
- For an item inside a frame, check `Response` for its `parent` and how Miro reports its position.

## Related

- [Get All Items](GetAllItemsComponent.md): get item IDs to feed in.
- [Get Item Tags](../Tags/GetItemTagsComponent.md): the item's tags.

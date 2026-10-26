# Get All Items

Retrieves item IDs from a Miro board, with an optional item-type filter.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board ID | Text | Yes | - | ID of the board to query. |
| Type Filter | Text | No | - | Item type to filter by, e.g. `sticky_note`, `shape`, `frame`. Leave empty for all types. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Item IDs | Text (list) | IDs of the items returned. |

## API endpoint

`GET /v2/boards/{board_id}/items` (adds `?type={Type Filter}` when a filter is set)

## Notes

- Read-only, runs on every recompute.
- Reads a single response page and does not follow Miro's pagination cursor, so on a busy board it returns only the first page of items (Miro's default page size).
- Connectors are not board items and are not listed here.

## Related

- [Get Item](GetItemComponent.md): details for one ID.
- [Delete Item](DeleteItemComponent.md): wire `Item IDs` straight in to clear a board.

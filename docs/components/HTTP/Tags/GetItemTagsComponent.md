# Get Item Tags

Retrieves the IDs and titles of every tag attached to a Miro board item.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board ID | Text | Yes | - | ID of the board. |
| Item ID | Text | Yes | - | ID of the item to read tags from. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Tag IDs | Text (list) | IDs of all tags attached to the item. |
| Tag Titles | Text (list) | Titles of those tags, in the same order. |

## API endpoint

`GET /v2/boards/{board_id}/items/{item_id}/tags`

## Notes

- Read-only, runs on every recompute.
- Reads the `tags` array of the response, falling back to `data` if `tags` is absent.

## Related

- [Attach Tag](AttachTagComponent.md) / [Detach Tag](DetachTagComponent.md)
- [Get Item](../Items/GetItemComponent.md)

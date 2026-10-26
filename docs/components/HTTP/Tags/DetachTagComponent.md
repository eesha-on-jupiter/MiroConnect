# Detach Tag

Removes one Miro tag from one or more board items, one API call per item.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board. The first value is used. |
| Item IDs | Text (tree) | Yes | - | List or tree of item IDs to detach the tag from. |
| Tag ID | Text (tree) | Yes | - | ID of the tag to detach. The first value is used. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Responses | Text (list) | Raw responses from the Miro API, one per item. |

## API endpoint

`DELETE /v2/boards/{board_id}/items/{item_id}/tags/{tag_id}`

## Notes

- Button-triggered (**Detach**): only fires on a click.
- Removes the tag from the items only; the tag itself stays on the board.
- A failed detach logs `Item N: <error>` and leaves `null` at its index.

## Related

- [Attach Tag](AttachTagComponent.md)
- [Get Item Tags](GetItemTagsComponent.md)

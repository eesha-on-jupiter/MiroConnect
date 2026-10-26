# Attach Tag

Attaches one Miro tag to one or more board items, one API call per item.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board. The first value is used. |
| Item IDs | Text (tree) | Yes | - | List or tree of item IDs to attach the tag to. |
| Tag ID | Text (tree) | Yes | - | ID of the tag to attach. The first value is used. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Responses | Text (list) | Raw responses from the Miro API, one per item. |

## API endpoint

`POST /v2/boards/{board_id}/items/{item_id}/tags/{tag_id}` (no request body)

## Notes

- Button-triggered (**Attach**): only fires on a click.
- One tag per click; to attach several tags, use one Attach Tag per tag.
- Every branch of `Item IDs` is flattened into one list; empty entries are skipped.
- A failed attach logs `Item N: <error>` and leaves `null` at its index.

## Related

- [Create Tag](CreateTagComponent.md): source of the `Tag ID`.
- [Detach Tag](DetachTagComponent.md): the reverse.
- [Get Item Tags](GetItemTagsComponent.md): verify the result.

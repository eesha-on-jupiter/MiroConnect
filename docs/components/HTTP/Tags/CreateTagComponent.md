# Create Tag

Creates one or more tags on a Miro board from a list of tag JSONs, one API call per tag.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board to create the tags on. The first value is used. |
| Tag JSON | Text (tree) | Yes | - | List or tree of request bodies from [Tag JSON](../../JSON/Items/TagJsonComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Tag IDs | Text (list) | IDs of the newly created tags, in input order. |
| Responses | Text (list) | Raw JSON responses from the Miro API, in input order. |

## API endpoint

`POST /v2/boards/{board_id}/tags`

## Notes

- Button-triggered (**Create**): only fires on a click.
- Creating a tag does not attach it to anything; use [Attach Tag](AttachTagComponent.md) next.
- A failed tag logs `Item N: <error>` and leaves `null` at its index.

## Related

- [Tag JSON](../../JSON/Items/TagJsonComponent.md)
- [Attach Tag](AttachTagComponent.md)

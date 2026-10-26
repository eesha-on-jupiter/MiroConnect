# Delete Item

Deletes one or more Miro board items by ID, one API call per item.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board containing the items. The first value is used. |
| Item IDs | Text (tree) | Yes | - | List or tree of item IDs to delete. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Responses | Text (list) | Raw responses from the Miro API, one per ID (empty on success). |

## API endpoint

`DELETE /v2/boards/{board_id}/items/{item_id}`

## Notes

- Button-triggered (**Delete**): only fires on a click.
- Every branch of `Item IDs` is flattened into one list; empty entries are skipped.
- A failed delete logs `Item N: <error>` and leaves `null` at its index.
- Delete connectors first: see [Delete Connector](../Connectors/DeleteConnectorComponent.md).

## Related

- [Get All Items](GetAllItemsComponent.md): list IDs to delete.
- [Create Item](CreateItemComponent.md)

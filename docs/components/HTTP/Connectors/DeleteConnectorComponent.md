# Delete Connector

Deletes one or more Miro connectors by ID, one API call per connector.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board containing the connectors. The first value is used. |
| Connector IDs | Text (tree) | Yes | - | List or tree of connector IDs to delete. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Responses | Text (list) | Raw responses from the Miro API, one per ID (empty on success). |

## API endpoint

`DELETE /v2/boards/{board_id}/connectors/{connector_id}`

## Notes

- Button-triggered (**Delete**): only fires on a click.
- Every branch of `Connector IDs` is flattened into one list; empty entries are skipped.
- Delete connectors before the items they join, otherwise the calls can fail on connectors whose ends are already gone.

## Related

- [Create Connector](CreateConnectorComponent.md)
- [Delete Item](../Items/DeleteItemComponent.md)

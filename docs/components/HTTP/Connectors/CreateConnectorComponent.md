# Create Connector

Creates one or more Miro connectors from a list of connector JSONs, one API call per connector.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text (tree) | Yes | - | Miro bearer token. The first value is used. |
| Board ID | Text (tree) | Yes | - | ID of the board to create the connectors on. The first value is used. |
| Connector JSON | Text (tree) | Yes | - | List or tree of request bodies from [Connector JSON](../../JSON/Items/ConnectorJsonComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Connector IDs | Text (list) | IDs of the newly created connectors, in input order. |
| Responses | Text (list) | Raw JSON responses from the Miro API, in input order. |

## API endpoint

`POST /v2/boards/{board_id}/connectors`

## Notes

- Button-triggered (**Create**): only fires on a click.
- Every branch of `Connector JSON` is flattened into one ordered list.
- Both end items must already exist on the board, so create items first and feed their IDs into Connector JSON.
- A failed connector logs `Item N: <error>` and leaves `null` at its index.

## Related

- [Connector JSON](../../JSON/Items/ConnectorJsonComponent.md) and [Connector Style](../../JSON/Style/ConnectorStyleComponent.md)
- [Delete Connector](DeleteConnectorComponent.md)

# Get Connector

Retrieves a Miro connector and returns the IDs of the items it joins.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board ID | Text | Yes | - | ID of the board containing the connector. |
| Connector ID | Text | Yes | - | ID of the connector to retrieve. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Start Item ID | Text | ID of the item at the connector start. |
| End Item ID | Text | ID of the item at the connector end. |
| Response | Text | Raw JSON response from the Miro API. |

## API endpoint

`GET /v2/boards/{board_id}/connectors/{connector_id}`

## Notes

- Read-only, runs on every recompute.
- Line style and captions are in `Response`.

## Related

- [Create Connector](CreateConnectorComponent.md): source of connector IDs.
- [Get Item](../Items/GetItemComponent.md): look up either end.

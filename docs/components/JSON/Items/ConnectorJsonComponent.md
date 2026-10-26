# Connector JSON

Builds Miro connector request bodies from lists of start and end item IDs and a style.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Start Item ID | Text (tree) | Yes | - | Item IDs where connectors start. |
| End Item ID | Text (tree) | Yes | - | Item IDs where connectors end. |
| Style JSON | Text (tree) | No | - | Style JSON from [Connector Style](../Style/ConnectorStyleComponent.md). |

## Outputs

| Name | Type | Description |
|---|---|---|
| Connector JSON | Text (list) | Connector request bodies for Create Connector. |

## Output

Builds `{"startItem":{"id":"..."},"endItem":{"id":"..."},"shape":"curved","style":{...}}`. No API call. A `lineStyle` in the Style JSON becomes the top-level `shape`; the rest stays under `style`. Count is the longer of `Start Item ID` and `End Item ID`.

## Notes

- Connectors carry no position; they only reference item IDs.
- To chain items in order, feed `Item IDs` through a Sub List of `0..N-2` into `Start Item ID` and `1..N-1` into `End Item ID`.
- Send with [Create Connector](../../HTTP/Connectors/CreateConnectorComponent.md), not Create Item.

## Related

- [Connector Style](../Style/ConnectorStyleComponent.md)
- [Create Connector](../../HTTP/Connectors/CreateConnectorComponent.md)

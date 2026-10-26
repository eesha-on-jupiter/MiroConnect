# Parent

Builds Parent JSONs from frame IDs, so items are created inside those frames.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Frame IDs | Text (tree) | Yes | - | List or tree of frame item IDs. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Parent JSON | Text (list) | One `{"id":"<frame id>"}` per frame ID, for the Items JSON components. |

## Output

Builds `{"id":"<Frame ID>"}` for every non-empty ID, flattened across all branches. No API call. Item builders place it under the request body's `parent` key.

## Notes

- Create the frame first ([Frame JSON](../Items/FrameJsonComponent.md) into [Create Item](../../HTTP/Items/CreateItemComponent.md)) and wire its `Item IDs` here.
- A single Parent JSON is reused for every item in a list, so one frame ID is enough for a whole batch.
- No IDs gives a warning and no output.

## Related

- [Frame JSON](../Items/FrameJsonComponent.md)
- [Items JSON tab](../Items/index.md): every builder except Frame, Connector and Tag JSON has a `Parent JSON` input.

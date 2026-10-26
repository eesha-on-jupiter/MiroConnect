# Get Board

Retrieves the name, owning team and creation date of a Miro board.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board ID | Text | Yes | - | ID of the board to retrieve. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Name | Text | Board name. |
| Team | Text | Name of the team that owns the board. |
| Created At | Text | Board creation date (ISO 8601). |

## API endpoint

`GET /v2/boards/{board_id}`

## Notes

- Read-only, runs on every recompute.
- The raw response is not exposed; only the three fields above.

## Related

- [Find Board](FindBoardComponent.md): get a Board ID by name.
- [Get All Items](../Items/GetAllItemsComponent.md): see what is on the board.

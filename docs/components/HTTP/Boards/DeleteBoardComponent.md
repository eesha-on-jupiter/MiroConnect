# Delete Board

Deletes a Miro board by its ID.

## Inputs

| Name | Type | Required | Default | Description |
|---|---|---|---|---|
| Token | Text | Yes | - | Miro bearer token. |
| Board ID | Text | Yes | - | ID of the board to delete. |

## Outputs

| Name | Type | Description |
|---|---|---|
| Response | Text | Raw response from the Miro API (empty on success). |

## API endpoint

`DELETE /v2/boards/{board_id}`

## Notes

- Button-triggered (**Delete**): only fires on a click.
- Deletes the whole board and everything on it. Wire `Board ID` from its own Panel rather than a shared board param, so a stray click cannot remove the board you are working on.
- Needs the `boards:write` scope.

## Related

- [Create Board](CreateBoardComponent.md)
- [Delete Item](../Items/DeleteItemComponent.md): remove board content without deleting the board.

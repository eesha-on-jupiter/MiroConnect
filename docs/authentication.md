# Authentication

MiroConnect authenticates every request with a Miro access token sent as an `Authorization: Bearer <token>` header. Any valid Miro REST API v2 token works: the usual route is the access token Miro issues when you install your own app on a team. There is no OAuth flow inside Grasshopper; you paste the token once and wire it everywhere.

## 1. Get a token

1. Open [Miro developer settings](https://miro.com/app/settings/user-profile/apps) and create a new app (Miro asks you to pick a developer team).
2. On the app's page, tick the permission scopes you need (see step 2).
3. Click **Install app and get OAuth token**, choose the team whose boards you want to use, and confirm.
4. Copy the access token Miro shows. This is the string every component's `Token` input expects.

## 2. Scopes

| Scope | Needed for |
|---|---|
| `boards:read` | Miro Auth, Find Board, Get Board, Get Item, Get All Items, Get Connector, Get Item Tags |
| `boards:write` | Create Board, Delete Board, Create/Delete Item, Create/Delete Connector, Create/Attach/Detach Tag |

- The token only sees boards in the team it was installed on.
- If you add scopes later, reinstall the app to get a token that carries them.

## 3. Wiring the token in

- Put the token on one Panel and wire it into the `Token` input of every HTTP component.
- A **Param > Primitive > Text** param named `TOKEN` between the Panel and the components keeps the canvas tidy.
- Treat the token like a password: clear the Panel before sharing a `.gh` file.

## 4. Verifying it works

- Wire the token into [Miro Auth](components/Auth/MiroAuthComponent.md) (Auth tab).
- It calls `GET /v2/boards?limit=1` and its `Status` output reads `Connected` on success.
- On failure `Status` reads `Error: HTTP <code> ...` with Miro's message: `401` means a bad or expired token, `403` a missing scope.
- Next, wire the token into [Find Board](components/HTTP/Boards/FindBoardComponent.md) to list the boards it can see and pick a Board ID.

## API version

All requests go to `https://api.miro.com/v2`. The base URL is fixed in the plugin and not configurable from the canvas.

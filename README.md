# Simple-Blog
A simple blog using various languages and frameworks for testing and learning purposes.

## Server

| Controller          | Method    | Parameter | Response | Description |
|---------------------|-----------|-----------|----------|-------------|
| /page/               | Get       |           | html     | Display all pages. |
| /page/{id\|url}/     | Get       |           | html     | Display a page by id or display a page by url if id is not digital. |
| ---                 |           |           |          |             |
| /api/page/{?id,token}| Get       |           | json     | Return a page by id, or return all pages if id equal to all. |
| /api/page/{?token}.  | Post      | page data | json     | Add a new page. |
| /api/page/{?id,token}| Put       | page data | json     | Update a page by id. |
| /api/page/{?id,token}| Delete    |           | json     | Delete a page by id. |
| ---                 |           |           |          |             |
| /api/user/login/{?username,password}| Get |  | json     | Login with username and password, return JWT token. |
| /api/user/logout/{?username}| Get | username | json     | Logout.     |
| ---                 |           |           |          |             |
| /page/**            | Get       |           | html     | Display server-side rendering html page. |
| /api/**             | REST      |           | json     | Return json data. |
| /api/page/**        | Get       |           | json     | Can only be accessed by authorized user. |

## Client

User login, logout, page management, client-side rendering and fetching data from server-side API.

## Database:

| Table | Column |
|-------|--------|
| user  | id, username, password |
| page  | id, user_id, url, title, content, date_published, date_modified |

## Implementation

### Server

- [ ] C#
- [ ] Go
- [ ] Haskell
- [x] Java
- [ ] Lisp
- [x] PHP
- [x] Python
- [ ] Ruby
- [ ] Rust
- [ ] Typescript

### Client

- [ ] Electron
- [ ] NET MAUI
- [ ] Qwik
- [x] React
- [ ] Solid
- [ ] Svelte
- [ ] Tauri
- [ ] Unity
- [ ] WASM

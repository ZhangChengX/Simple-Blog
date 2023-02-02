# Simple-Blog
A simple blog using various languages and frameworks for testing and learning purposes.

## Server

| Controller        | Method    | Parameter | Response | Description |
|-------------------|-----------|-----------|----------|-------------|
| /page             | Get       |           | html     | Display all pages. |
| /page/[id\|url]   | Get       |           | html     | Display page by id or display page by url if id is not digital. |
| ---               |           |           |          |             |
| /api/page         | Get       | id, token | json     | Return a page by id, or return all pages if id equal to all. |
| /api/page         | Put       | data, token | json     | Add a new page. |
| /api/page         | Update    | data, token | json     | Update a page by id. |
| /api/page         | Delete    | id, token | json     | Delete a page by id. |
| ---               |           |           |          |             |
| /api/user/login   | Get       | username and password | json     | Login with username and password, return JWT token. |
| /api/user/logout  | Get       | username  | json     | Logout.     |

- /page/** &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Display server-side rendering html page.
- /api/** &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; &nbsp; Return json data.
- /api/page/** &nbsp; Can only be accessed by authorized user.

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
- [ ] PHP
- [x] Python
- [ ] Ruby
- [ ] Rust
- [ ] Typescript

### Client

- [ ] C#
- [ ] Electron
- [ ] Qwik
- [x] React
- [ ] Solid
- [ ] Svelte
- [ ] Tauri
- [ ] Unity

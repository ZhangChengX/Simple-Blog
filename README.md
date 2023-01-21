# Simple-Blog
A simple blog using various languages and frameworks for testing and learning purposes.

## Server

| Controller        | Method    | Parameter | Response | Description |
|-------------------|-----------|-----------|----------|-------------|
| /page             | Get       |           | html     | Display all pages. |
| /page/[id\|url]   | Get       |           | html     | Display page by id or display page by url if id is not digital. |
| ---               |           |           |          |             |
| /api/page         | Get       | id        | json     | Return a page by id, or return all pages if id equal to all. |
| /api/page         | Put       | data      | json     | Add a new page. |
| /api/page         | Update    | id        | json     | Update a page by id. |
| /api/page         | Delete    | id        | json     | Delete a page by id. |
| ---               |           |           |          |             |
| /api/user         | Get       |           | json     | Return username if the current user is authorized, otherwise return anonymous. |
| /api/user/login   | Get       | username and password | json     | Login with username and password. |
| /api/user/logout  | Get       |           | json     | Logout.     |

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
- [x] Java
- [ ] Python
- [ ] Typescript
- [ ] Ruby
- [ ] Rust
- [ ] Go

### Client

- [ ] React
- [ ] Solid
- [ ] Svelte
- [ ] Qwik

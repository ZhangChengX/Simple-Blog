# Simple-Blog
A simple blog implemented using various languages and frameworks for testing and learning purposes.

## Server

| Controller           | Method | Parameter | Response | Description |
|----------------------|--------|-----------|----------|-------------|
| /page/**             | Get    |           | html     | Display server-side rendering html page. |
| /page/               | Get    |           | html     | Display all pages. |
| /page/{id\|url}/     | Get    |           | html     | Display a page by id or display a page by url if id is not digital. |
| ---                  |        |           |          |             |
| /api/page/**         | REST   |           | json     | Can only be accessed by authorized user. |
| /api/page/{?id,token}| Get    |           | json     | Return a page by id, or return all pages if id equal to all. |
| /api/page/{?token}.  | Post   | json      | json     | Add a new page. |
| /api/page/{?id,token}| Put    | json      | json     | Update a page by id. |
| /api/page/{?id,token}| Delete |           | json     | Delete a page by id. |
| ---                  |        |           |          |             |
| /api/user/login/{?username,password}| Get |  | json     | Login with username and password, return JWT token. |
| /api/user/logout/{?token}| Get |          | json     | Logout.     |

## Client

User login, logout, page management, client-side rendering and fetching data from server-side API.

## Database:

| Table | Column |
|-------|--------|
| user  | id, username, password |
| page  | id, user_id, url, title, content, date_published, date_modified |

## Implementation

### Languages

- [x] C#
- [ ] C++
- [ ] Dart
- [ ] Elixir
- [x] Go
- [ ] Haskell
- [x] Java
- [ ] Kotlin
- [ ] Lisp
- [ ] Pascal
- [x] PHP
- [x] Python
- [ ] Ruby
- [ ] Rust
- [ ] Swift
- [ ] Typescript

### Frameworks

- [ ] Electron
- [ ] Flutter
- [ ] JavaFX
- [ ] Jetpack Compose
- [x] MAUI
- [ ] Qt
- [ ] Qwik
- [x] React
- [ ] Solid
- [ ] Svelte
- [ ] SwiftUI
- [ ] Tauri
- [ ] Unity
- [ ] WASM

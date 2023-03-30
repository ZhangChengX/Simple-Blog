const serverUrl = "http://localhost"
const serverPort = "8080"
const api = {
    "userLogin": serverUrl + ":" + serverPort + "/api/user/login/",
    "userLogout": serverUrl + ":" + serverPort + "/api/user/logout/",
    "page": serverUrl + ":" + serverPort + "/api/page/",
}
export { api }
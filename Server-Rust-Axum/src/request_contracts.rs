use serde::Deserialize;

#[derive(Deserialize)]
pub struct UserRequest {
    pub username: String,
    pub password: String,
}

#[derive(Deserialize)]
pub struct TokenRequest {
    pub token: Option<String>,
}

#[derive(Deserialize)]
pub struct PageRequest {
    // pub id: Option<u32>,
    pub url: Option<String>,
    pub title: Option<String>,
    pub content: Option<String>,
    pub user_id: Option<u32>,
    pub date_published: Option<i64>,
    pub date_modified: Option<i64>,
}

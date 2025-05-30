use serde::{Deserialize, Serialize};
use sqlx::{prelude::FromRow, Error, Pool, Sqlite};

#[derive(FromRow, Serialize, Deserialize)]
pub struct User {
    id: u32,
    username: String,
    password: String,
}

impl User {
    // fn new(id: u32, username: String, password: String) -> Self {
    //     Self {
    //         id,
    //         username,
    //         password,
    //     }
    // }

    pub fn get_id(&self) -> u32 {
        self.id
    }

    pub fn get_username(&self) -> String {
        self.username.to_owned()
    }
}

pub async fn get_by_id(pool: &Pool<Sqlite>, id: u32) -> Result<User, Error> {
    let sql = format!("SELECT * FROM user WHERE id = {}", id);
    let result = sqlx::query_as::<_, User>(&sql).fetch_one(pool).await;
    result
}

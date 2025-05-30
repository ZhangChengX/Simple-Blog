use serde::{Deserialize, Serialize};
use sqlx::{prelude::FromRow, sqlite::SqliteQueryResult, Error, Pool, Sqlite};

#[derive(FromRow, Serialize, Deserialize)]
pub struct Page {
    id: u32,
    url: String,
    title: String,
    content: String,
    date_published: i64,
    date_modified: i64,
    user_id: u32,
}

impl Page {
    pub fn new(
        url: String,
        title: String,
        content: String,
        date_published: i64,
        date_modified: i64,
        user_id: u32,
    ) -> Self {
        Self {
            id: 0,
            url,
            title,
            content,
            date_published,
            date_modified,
            user_id,
        }
    }

    pub fn new_with_id(
        id: u32,
        url: String,
        title: String,
        content: String,
        date_published: i64,
        date_modified: i64,
        user_id: u32,
    ) -> Self {
        Self {
            id,
            url,
            title,
            content,
            date_published,
            date_modified,
            user_id,
        }
    }

    pub fn get_author_id(&self) -> u32 {
        self.user_id
    }
}

pub async fn get_all(pool: &Pool<Sqlite>) -> Result<Vec<Page>, Error> {
    let sql = "SELECT * FROM page".to_owned();
    let result = sqlx::query_as::<_, Page>(&sql).fetch_all(pool).await;
    result
}

pub async fn get_by_id_or_url(pool: &Pool<Sqlite>, id: String) -> Result<Page, Error> {
    let result = id.parse::<u32>();
    match result {
        Ok(id) => get_by_id(pool, id).await,
        Err(_) => get_by_url(pool, id).await,
    }
}

pub async fn get_by_id(pool: &Pool<Sqlite>, id: u32) -> Result<Page, Error> {
    let sql = format!("SELECT * FROM page WHERE id = {}", id);
    let result = sqlx::query_as::<_, Page>(&sql).fetch_one(pool).await;
    result
}

pub async fn get_by_url(pool: &Pool<Sqlite>, url: String) -> Result<Page, Error> {
    let sql = format!("SELECT * FROM page WHERE url = '{}'", url);
    let result = sqlx::query_as::<_, Page>(&sql).fetch_one(pool).await;
    result
}

pub async fn post(pool: &Pool<Sqlite>, page: Page) -> Result<SqliteQueryResult, Error> {
    let sql = format!("INSERT INTO page (url, title, content, date_published, date_modified, user_id) VALUES ('{}', '{}', '{}', {}, {}, {})",
            page.url,
            page.title,
            page.content,
            page.date_published,
            page.date_modified,
            page.user_id,
    );
    let result = sqlx::query(&sql).execute(pool).await;
    result
}

pub async fn put(pool: &Pool<Sqlite>, page: Page) -> Result<SqliteQueryResult, Error> {
    let sql = format!("UPDATE page SET url='{}', title='{}', content='{}', date_published={}, date_modified={}, user_id={} WHERE id={}", 
            page.url,
            page.title,
            page.content,
            page.date_published,
            page.date_modified,
            page.user_id,
            page.id,
    );
    let result = sqlx::query(&sql).execute(pool).await;
    result
}

pub async fn delete(pool: &Pool<Sqlite>, id: u32) -> Result<SqliteQueryResult, Error> {
    let sql = format!("DELETE FROM page WHERE id={}", id);
    let result = sqlx::query(&sql).execute(pool).await;
    result
}

use axum::{routing::get, Router};
use axum_extra::routing::RouterExt;
use chrono::DateTime;
use minijinja::Environment;
use sqlx::{sqlite, Pool, Sqlite};
use std::sync::Arc;
use tower_http::cors::{Any, CorsLayer};
use tower_http::services::fs::{ServeDir, ServeFile};
use tower_http::trace::TraceLayer;

mod entities;
mod handlers;
mod request_contracts;
mod response_contracts;
mod token;
use handlers::{page, user};

#[tokio::main]
async fn main() {
    // Load .env
    dotenv::dotenv().ok();

    let addr = std::env::var("IP_ADDR").expect("IP_ADDR not found");
    let db_source = std::env::var("DB_SOURCE").expect("DB_SOURCE not found");

    let db_pool = db(&db_source).await;
    let template_env = template();
    let state = Arc::new(AppState {
        template_env,
        db_pool,
    });

    let cors_layer = CorsLayer::new()
        .allow_origin(Any)
        .allow_methods(Any)
        .allow_headers(Any);
    let trace_layer = TraceLayer::new_for_http();
    let app = router(state).layer(cors_layer).layer(trace_layer);
    let listener = tokio::net::TcpListener::bind(&addr).await.unwrap();

    println!("Current directory {:?}", std::env::current_dir().unwrap());
    println!("Listening on http://{} \nPress Control + C to exit", addr);
    axum::serve(listener, app.into_make_service())
        .await
        .unwrap();
}

struct AppState {
    template_env: Environment<'static>,
    db_pool: Pool<Sqlite>,
}

fn router(state: Arc<AppState>) -> Router {
    Router::new()
        .route_service("/", ServeFile::new("static/index.html"))
        .nest_service("/static", ServeDir::new("static/"))
        .route_with_tsr("/page", get(page::show_page))
        .route_with_tsr("/page/{id}", get(page::show_page))
        .route_with_tsr(
            "/api/page",
            get(page::get_page)
                .post(page::post_page)
                .put(page::put_page)
                .delete(page::delete_page),
        )
        .route_with_tsr("/api/user/login", get(user::login))
        .route_with_tsr("/api/user/logout", get(user::logout))
        .with_state(state)
}

fn template() -> Environment<'static> {
    let mut env = Environment::new();
    env.add_template("page_list", include_str!("./templates/list.html"))
        .unwrap();
    env.add_template("page_detail", include_str!("./templates/detail.html"))
        .unwrap();
    env.add_template("page_error", include_str!("./templates/error.html"))
        .unwrap();
    env.add_filter("timestamp_to_datetime", timestamp_to_datetime);
    env
}

async fn db(db_source: &str) -> Pool<Sqlite> {
    let opt = sqlite::SqliteConnectOptions::new()
        .filename(db_source)
        .create_if_missing(true);
    let pool = sqlite::SqlitePool::connect_with(opt).await.unwrap();
    // pool.execute("
    //   CREATE TABLE if not exists test (
    //     id INTEGER PRIMARY KEY AUTOINCREMENT,
    //     name TEST
    //   )
    // ").await.unwrap();
    pool
}

fn timestamp_to_datetime(timestamp: u64, is_millis: Option<bool>) -> String {
    let is_millis = is_millis.unwrap_or(true); // Set default value since Rust doesn't support default parameters in function signatures
    let (secs, nanos) = if is_millis {
        (
            (timestamp / 1000) as i64,
            (timestamp % 1000 * 1_000_000) as u32,
        )
    } else {
        (timestamp as i64, 0)
    };
    let datetime = DateTime::from_timestamp(secs, nanos).unwrap();
    datetime.format("%Y-%m-%d %H:%M:%S").to_string()
}

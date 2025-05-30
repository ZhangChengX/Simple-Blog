use axum::{
    extract::{Query, State},
    Json,
};
use serde_json::{json, Value};
use std::sync::Arc;

use crate::entities::user::User;
use crate::request_contracts::{TokenRequest, UserRequest};
use crate::response_contracts::Msg;
use crate::token::{generate_token, refresh_token};
use crate::AppState;

pub async fn login(
    State(state): State<Arc<AppState>>, 
    user_request: Query<UserRequest>,
) -> Json<Value> {
    if user_request.username.is_empty() || user_request.password.is_empty() {
        let msg = Msg {
            msg_type: "error",
            content: Value::from("Invalid Credentials provided."),
        };
        return Json(json!(msg));
    }

    let sql = format!(
        "SELECT * FROM user WHERE username = '{}' AND password = '{}'",
        user_request.username, user_request.password
    );
    let result = sqlx::query_as::<_, User>(&sql).fetch_one(&state.db_pool).await;
    match result {
        Ok(row) => {
            // Generate token
            let token = generate_token(row.get_id());
            let msg = Msg {
                msg_type: "success",
                content: Value::from(token),
            };
            Json(json!(msg))
        }
        Err(e) => {
            let msg = Msg {
                msg_type: "error",
                content: Value::from(format!("User not found. {}", e)),
            };
            Json(json!(msg))
        }
    }
}

pub async fn logout(token_request: Query<TokenRequest>) -> Json<Msg> {
    if token_request.token.is_none() {
        let msg = Msg {
            msg_type: "error",
            content: Value::from("Token is missing."),
        };
        Json(msg)
    } else {
        let token = token_request.token.as_ref().unwrap();
        if token.is_empty() {
            let msg = Msg {
                msg_type: "error",
                content: Value::from("Invalid token provided."),
            };
            Json(msg)
        } else {
            let msg = Msg {
                msg_type: "success",
                content: Value::from(refresh_token(token.to_owned(), 0)),
            };
            Json(msg)
        }
    }
}

use std::collections::HashMap;
use std::sync::Arc;

use axum::{
    extract::{Path, Query, State},
    response::Html,
    Json,
};
use chrono::Utc;
use minijinja::context;
use serde_json::{json, to_value, Value};

use crate::entities::page::{delete, get_all, get_by_id_or_url, post, put, Page};
use crate::entities::user::get_by_id as get_user_by_id;
use crate::request_contracts::{PageRequest, TokenRequest};
use crate::response_contracts::Msg;
use crate::token::verify_token;
use crate::AppState;

pub async fn show_page(
    // State(pool): State<Pool<Sqlite>>,
    State(state): State<Arc<AppState>>,
    id: Option<Path<String>>,
) -> Html<String> {
    let list_template = state.template_env.get_template("page_list").unwrap();
    let detail_template = state.template_env.get_template("page_detail").unwrap();
    let error_template = state.template_env.get_template("page_error").unwrap();

    match id {
        Some(Path(id)) => {
            let page = get_by_id_or_url(&state.db_pool, id).await;
            match page {
                Ok(row) => {
                    let author_name = get_user_by_id(&state.db_pool, row.get_author_id())
                        .await
                        .unwrap()
                        .get_username();
                    Html(
                        detail_template
                            .render(context! {
                                title => "Page Detail",
                                author_name => author_name,
                                page_detail => row
                            })
                            .unwrap(),
                    )
                }
                Err(e) => Html(
                    error_template
                        .render(context! {
                            title => "Error",
                            error => format!("Failed to fetch pages. {}", e)
                        })
                        .unwrap(),
                ),
            }
        }
        None => {
            let pages = get_all(&state.db_pool).await;
            match pages {
                Ok(rows) => Html(
                    list_template
                        .render(context! {
                            title => "Page List",
                            page_list => rows
                        })
                        .unwrap(),
                ),
                Err(e) => Html(
                    error_template
                        .render(context! {
                            title => "Error",
                            error => format!("Failed to fetch pages. {}", e)
                        })
                        .unwrap(),
                ),
            }
        }
    }
}

pub async fn get_page(
    State(state): State<Arc<AppState>>,
    token_request: Query<TokenRequest>,
    params: Query<HashMap<String, String>>,
) -> Json<Value> {
    if let Some(token) = &token_request.token {
        if !verify_token(token.to_owned()) {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Invalid token. Please login."),
            }));
        }
    } else {
        return Json(json!(Msg {
            msg_type: "error",
            content: Value::from("Missing token. Please login."),
        }));
    };

    let page_id = match params.get("id") {
        Some(id) => id.as_str(),
        None => {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Missing id parameter."),
            }))
        }
    };

    match page_id {
        "all" => {
            let pages = get_all(&state.db_pool).await;
            match pages {
                Ok(rows) => Json(json!(Msg {
                    msg_type: "success",
                    content: to_value(rows).unwrap(),
                })),
                Err(e) => Json(json!(Msg {
                    msg_type: "error",
                    content: Value::from(format!("Failed to fetch pages. {}", e)),
                })),
            }
        }
        page_id => {
            let page = get_by_id_or_url(&state.db_pool, page_id.to_string()).await;
            match page {
                Ok(row) => {
                    return Json(json!(Msg {
                        msg_type: "success",
                        content: to_value([row]).unwrap(),
                    }))
                }
                Err(e) => {
                    return Json(json!(Msg {
                        msg_type: "error",
                        content: Value::from(format!("Failed to fetch pages. {}", e)),
                    }))
                }
            }
        }
    }
}

pub async fn post_page(
    State(state): State<Arc<AppState>>,
    token_request: Query<TokenRequest>,
    page_request: Json<PageRequest>,
) -> Json<Value> {
    if let Some(token) = &token_request.token {
        if !verify_token(token.to_owned()) {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Invalid token. Please login."),
            }));
        }
    } else {
        return Json(json!(Msg {
            msg_type: "error",
            content: Value::from("Missing token. Please login."),
        }));
    };

    let page = Page::new(
        page_request.url.clone().unwrap_or_default(),
        page_request.title.clone().unwrap_or_default(),
        page_request.content.clone().unwrap_or_default(),
        page_request
            .date_published
            .unwrap_or(Utc::now().timestamp()),
        page_request.date_modified.unwrap_or(Utc::now().timestamp()),
        page_request.user_id.unwrap_or(0_u32),
    );

    match post(&state.db_pool, page).await {
        Ok(_) => Json(json!(Msg {
            msg_type: "success",
            content: Value::from("Page created successfully."),
        })),
        Err(e) => Json(json!(Msg {
            msg_type: "error",
            content: Value::from(format!("Failed to create page. {}", e)),
        })),
    }
}

pub async fn put_page(
    State(state): State<Arc<AppState>>,
    params: Query<HashMap<String, String>>,
    page_request: Json<PageRequest>,
) -> Json<Value> {
    let token = match params.get("token") {
        Some(token) => token.as_str(),
        None => {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Missing token. Please login."),
            }))
        }
    };
    if !verify_token(token.to_owned()) {
        return Json(json!(Msg {
            msg_type: "error",
            content: Value::from("Invalid token. Please login."),
        }));
    }

    let page_id = match params.get("id") {
        Some(id) => {
            let result = id.parse::<u32>();
            match result {
                Ok(id) => id,
                Err(_) => {
                    return Json(json!(Msg {
                        msg_type: "error",
                        content: Value::from("Invalid id parameter."),
                    }))
                }
            }
        }
        None => {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Missing id parameter."),
            }))
        }
    };
    let page = Page::new_with_id(
        page_id,
        page_request.url.clone().unwrap_or_default(),
        page_request.title.clone().unwrap_or_default(),
        page_request.content.clone().unwrap_or_default(),
        page_request
            .date_published
            .unwrap_or(Utc::now().timestamp()),
        page_request.date_modified.unwrap_or(Utc::now().timestamp()),
        page_request.user_id.unwrap_or(0_u32),
    );

    match put(&state.db_pool, page).await {
        Ok(_) => Json(json!(Msg {
            msg_type: "success",
            content: Value::from("Page updated successfully."),
        })),
        Err(e) => Json(json!(Msg {
            msg_type: "error",
            content: Value::from(format!("Failed to update page. {}", e)),
        })),
    }
}

pub async fn delete_page(
    State(state): State<Arc<AppState>>,
    params: Query<HashMap<String, String>>,
) -> Json<Value> {
    let token = match params.get("token") {
        Some(token) => token.as_str(),
        None => {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Missing token. Please login."),
            }))
        }
    };
    if !verify_token(token.to_owned()) {
        return Json(json!(Msg {
            msg_type: "error",
            content: Value::from("Invalid token. Please login."),
        }));
    }

    let page_id = match params.get("id") {
        Some(id) => {
            let result = id.parse::<u32>();
            match result {
                Ok(id) => id,
                Err(_) => {
                    return Json(json!(Msg {
                        msg_type: "error",
                        content: Value::from("Invalid id parameter."),
                    }))
                }
            }
        }
        None => {
            return Json(json!(Msg {
                msg_type: "error",
                content: Value::from("Missing id parameter."),
            }))
        }
    };

    match delete(&state.db_pool, page_id).await {
        Ok(_) => Json(json!(Msg {
            msg_type: "success",
            content: Value::from("Page deleted successfully."),
        })),
        Err(e) => Json(json!(Msg {
            msg_type: "error",
            content: Value::from(format!("Failed to delete page. {}", e)),
        })),
    }
}

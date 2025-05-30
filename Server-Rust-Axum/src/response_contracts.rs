use serde::{Deserialize, Serialize};
use serde_json::Value;

#[derive(Serialize, Deserialize)]
pub struct Msg {
    #[serde(rename = "type")] // Rename msg_type to type to avoid using keyword type
    pub msg_type: &'static str,
    pub content: Value,
}

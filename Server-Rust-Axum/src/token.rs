use jsonwebtoken::{decode, encode, DecodingKey, EncodingKey, Header, Validation};
use serde::{Deserialize, Serialize};

#[derive(Debug, Serialize, Deserialize)]
struct Claims {
    user_id: u32,
    // expiry: u64,
    // #[serde(rename = "expiry")]
    exp: u64,
}

pub fn generate_token(user_id: u32) -> String {
    let secret_key = std::env::var("SECRET_KEY").expect("SECRET_KEY not found");
    let timestamp = std::time::SystemTime::now()
        .duration_since(std::time::UNIX_EPOCH)
        .unwrap()
        .as_secs()
        + 60 * 3;
    let claims = Claims {
        user_id: user_id,
        // expiry: timestamp,
        exp: timestamp,
    };
    let token = encode(
        &Header::default(),
        &claims,
        &EncodingKey::from_secret(secret_key.as_ref()),
    )
    .unwrap();
    token
}

pub fn verify_token(token: String) -> bool {
    if token.is_empty() {
        return false;
    }
    let secret_key = std::env::var("SECRET_KEY").expect("SECRET_KEY not found");
    let current_timestamp = std::time::SystemTime::now()
        .duration_since(std::time::UNIX_EPOCH)
        .unwrap()
        .as_secs();
    match decode::<Claims>(
        &token,
        &DecodingKey::from_secret(secret_key.as_ref()),
        &Validation::default(),
    ) {
        Ok(token_data) => token_data.claims.exp > current_timestamp,
        Err(_) => false,
    }
}

pub fn refresh_token(token: String, expiry_time: u64) -> String {
    let secret_key = std::env::var("SECRET_KEY").expect("SECRET_KEY not found");

    let old_claims = decode::<Claims>(
        &token,
        &DecodingKey::from_secret(secret_key.as_ref()),
        &Validation::default(),
    )
    .unwrap();

    let new_claims = Claims {
        user_id: old_claims.claims.user_id,
        // expiry: expiry_time,
        exp: expiry_time,
    };
    let refreshed_token = encode(
        &Header::default(),
        &new_claims,
        &EncodingKey::from_secret(secret_key.as_ref()),
    )
    .unwrap();
    refreshed_token
}

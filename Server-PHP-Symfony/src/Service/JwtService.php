<?php
namespace App\Service;

use Firebase\JWT\JWT;
use Firebase\JWT\Key;

class JwtService {
    public function generate_token($user_id, $key): string {
        return JWT::encode([
            'user_id' => $user_id,
            'expiry' => time() + 60 * 3,
        ], $key, 'HS256');
    }

    public function verify_token($token, $key): bool {
        $decoded_token = JWT::decode($token, new Key($key, 'HS256'));
        $decoded_array = (array) $decoded_token;
        return $decoded_array['expiry'] > time();
    }

    public function refresh_token($token, $key, $expiry): string {
        $decoded_token = JWT::decode($token, new Key($key, 'HS256'));
        $decoded_array = (array) $decoded_token;
        $decoded_array['expiry'] = $expiry;
        $token = JWT::encode($decoded_array, $key, 'HS256');
        return $token;
    }
}
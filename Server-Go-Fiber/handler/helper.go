package handler

import (
	"os"
	"time"
	"errors"
	"github.com/golang-jwt/jwt/v5"
)

type Msg struct {
	Type string `json:"type"`
	Content interface{} `json:"content"`
}

func GenerateToken(userId int) (string) {
	claims := jwt.MapClaims{
		"user_id": userId,
		"expiry":  time.Now().Unix() + 60 * 3,
	}
	secretKey := os.Getenv("SECRET_KEY")
	token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
	tokenString, _ := token.SignedString([]byte(secretKey))
	return tokenString
}

func VerifyToken(tokenString string) (bool) {
	if tokenString == "" {
		return false
	}
	secretKey := os.Getenv("SECRET_KEY")
	token, _ := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, errors.New("Unexpected signing method: " + token.Header["alg"].(string))
		}
		return []byte(secretKey), nil
	})

	if claims, ok := token.Claims.(jwt.MapClaims); ok && token.Valid {
		return int64(claims["expiry"].(float64)) > time.Now().Unix()
	} else {
		return false
	}
}

func RefreshToken(tokenString string, expiryTime int64) (string) {
	secretKey := os.Getenv("SECRET_KEY")
	token, err := jwt.Parse(tokenString, func(token *jwt.Token) (interface{}, error) {
		if _, ok := token.Method.(*jwt.SigningMethodHMAC); !ok {
			return nil, errors.New("Unexpected signing method: " + token.Header["alg"].(string))
		}
		return []byte(secretKey), nil
	})

	if claims, ok := token.Claims.(jwt.MapClaims); ok && token.Valid {
		claims["expiry"] = expiryTime
		token := jwt.NewWithClaims(jwt.SigningMethodHS256, claims)
		tokenString, _ := token.SignedString([]byte(secretKey))
		return tokenString
	} else {
		return err.Error()
	}
}

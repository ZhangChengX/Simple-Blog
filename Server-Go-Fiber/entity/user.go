package entity

import (
    "os"
	"errors"
	"database/sql"
	_ "github.com/mattn/go-sqlite3"
)

type User struct {
	Id       int
	Username string
	Password string
}

func connect() (*sql.DB, error) {
    db, err := sql.Open("sqlite3", os.Getenv("DB_SOURCE"))
    if err != nil {
        return nil, err
    }
    return db, nil
}

func GetUser(username string, password string) (User, error) {
    db, err := connect()
    if err != nil {
        return User{}, err
    }
    defer db.Close()

    var user User
    err = db.QueryRow("SELECT * FROM user WHERE username = ? AND password = ?", username, password).Scan(&user.Id, &user.Username, &user.Password)
    if err != nil {
        if err == sql.ErrNoRows {
            return User{}, errors.New("User not found")
        } else {
            return User{}, err
        }
    }
    return user, nil
}

func GetUserById(id int) (User, error) {
    db, err := connect()
    if err != nil {
        return User{}, err
    }
    defer db.Close()

    var user User
    err = db.QueryRow("SELECT * FROM user WHERE id = ?", id).Scan(&user.Id, &user.Username, &user.Password)
    if err != nil {
        if err == sql.ErrNoRows {
            return User{}, errors.New("User not found")
        } else {
            return User{}, err
        }
    }
    return user, nil
}
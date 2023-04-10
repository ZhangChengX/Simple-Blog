package handler

import (
	"time"
	"github.com/gofiber/fiber/v2"
	"SimpleBlog/Server-Go-Fiber/entity"
)

func Login(c *fiber.Ctx) error {
	username := c.Query("username")
	password := c.Query("password")
	if username == "" || password == "" {
		return c.JSON(Msg{Type: "error", Content: "Invalid Credentials provided."})
	}
	
	user, err := entity.GetUser(username, password)
	if err != nil {
		// return c.JSON(Msg{Type: "error", Content: err.Error()})
		return c.JSON(Msg{Type: "error", Content: "Invalid Credentials provided."})
	}

	return c.JSON(Msg{Type: "success", Content: GenerateToken(user.Id)})
}

func Logout(c *fiber.Ctx) error {
	token := c.Query("token")
	if token == "" {
		return c.JSON(Msg{Type: "error", Content: "Invalid token provided."})
	}
    return c.JSON(Msg{Type: "success", Content: RefreshToken(token, time.Now().Unix() - 3600)})
}
package main

import (
	"github.com/gofiber/fiber/v2"
	"github.com/gofiber/fiber/v2/middleware/cors"
	"github.com/gofiber/template/html"
	"github.com/joho/godotenv"
	"SimpleBlog/Server-Go-Fiber/handler"
)

func main() {

	if err := godotenv.Load(); err != nil {
		panic("Error loading .env file")
	}
	engine := html.New("./view", ".tpl.html")
    app := fiber.New(fiber.Config{
        Views: engine,
    })
	app.Use(cors.New())

	app.Static("/", "./static") 

	app.Get("/page/:id?", handler.ShowPage)

	app.Get("/api/page/", handler.GetPage)
	app.Post("/api/page/", handler.PostPage)
	app.Put("/api/page/", handler.PutPage)
	app.Delete("/api/page/", handler.DeletePage)

	app.Get("/api/user/login/", handler.Login)
	app.Get("/api/user/logout/", handler.Logout)

    app.Listen(":8080")
}
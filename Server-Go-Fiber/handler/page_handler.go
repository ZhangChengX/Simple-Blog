package handler

import (
	"time"
	"strconv"
	"github.com/gofiber/fiber/v2"
	"SimpleBlog/Server-Go-Fiber/entity"
)

func TimestampToDate(timestamp *int) string {
	if timestamp == nil {
		return ""
	} else { 
		return time.Unix(int64(*timestamp), 0).Format("2006-01-02 15:04:05")
	}
}

func ShowPage(c *fiber.Ctx) error {
	id := c.Params("id")
	if id == "" {
		// Get all pages
		pages, err := entity.GetAll()
		if err != nil {
			return c.Render("error", fiber.Map{
				"title": "Error",
				"error": err.Error(),
			})
		} else {
			return c.Render("page_list", fiber.Map{
				"title": "Page List",
				"pageList": pages,
				"TimestampToDate": TimestampToDate, // Allow calling from template
			})
		}
	} else {
		// Get page by id or url
		page, err := entity.GetByIdOrUrl(id)
		if err != nil {
			return c.Render("error", fiber.Map{
				"title": "Error",
				"error": err.Error(),
			})
		} else {
			username := ""
			user, err := entity.GetUserById(page.UserId)
			if err == nil {
				username = user.Username
			}
			date := page.DateModified
			if page.DateModified == nil {
				date = page.DatePublished
			}
			return c.Render("page_detail", fiber.Map{
				"title": page.Title,
				"content": page.Content,
				"username": username,
				"date": TimestampToDate(date),
			})
		}
	}
}

func GetPage(c *fiber.Ctx) error {
	token := c.Query("token")
	if !VerifyToken(token) {
		return c.JSON(Msg{Type: "error", Content: "Invalid token. Please login."})
	}
	id := c.Query("id")
	if id == "all" {
		pages, err := entity.GetAll()
		if err != nil {
			return c.JSON(Msg{Type: "error", Content: err.Error()})
		} else {
			return c.JSON(Msg{Type: "success", Content: pages})
		}
	} else {
		page, err := entity.GetByIdOrUrl(id)
		if err != nil {
			return c.JSON(Msg{Type: "error", Content: err.Error()})
		} else {
			return c.JSON(Msg{Type: "success", Content: []entity.Page{page}})
		}
	}
}

func PostPage(c *fiber.Ctx) error {
	token := c.Query("token")
	if !VerifyToken(token) {
		return c.JSON(Msg{Type: "error", Content: "Invalid token. Please login."})
	}
	page := entity.Page{}
	if err := c.BodyParser(&page); err != nil {
		return c.JSON(Msg{Type: "error", Content: err.Error()})
	}
	err := entity.Post(page)
	if err != nil {
		return c.JSON(Msg{Type: "error", Content: err.Error()})
	} else {
		return c.JSON(Msg{Type: "success", Content: "Page added successfully."})
	}
}

func PutPage(c *fiber.Ctx) error {
	token := c.Query("token")
	if !VerifyToken(token) {
		return c.JSON(Msg{Type: "error", Content: "Invalid token. Please login."})
	}
	id := c.Query("id")
	page := entity.Page{}
	// c.App().Config().JSONDecoder(c.Body(), page)
	if err := c.BodyParser(&page); err != nil {
		return c.JSON(Msg{Type: "error", Content: err.Error()})
	}
	idInt, err := strconv.Atoi(id)
	if err != nil {
		return c.JSON(Msg{Type: "error", Content: "Invalid ID: " + id + " " + err.Error()})
	}
	page.Id = idInt
	err = entity.Put(page)
	if err != nil {
		return c.JSON(Msg{Type: "error", Content: err.Error()})
	} else {
		return c.JSON(Msg{Type: "success", Content: "Page updated successfully."})
	}
}

func DeletePage(c *fiber.Ctx) error {
	token := c.Query("token")
	if !VerifyToken(token) {
		return c.JSON(Msg{Type: "error", Content: "Invalid token. Please login."})
	}
	id := c.Query("id")
	idInt, err := strconv.Atoi(id)
	if err != nil {
		return c.JSON(Msg{Type: "error", Content: "Invalid ID: " + id + " " + err.Error()})
	}
	err = entity.Delete(idInt)
	if err != nil {
		return c.JSON(Msg{Type: "error", Content: err.Error()})
	} else {
		return c.JSON(Msg{Type: "success", Content: "Page deleted successfully."})
	}
}
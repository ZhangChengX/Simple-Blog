package entity

import (
	"errors"
	"strconv"
	"database/sql"
	_ "github.com/mattn/go-sqlite3"
)

type Page struct {
	Id     int  `json:"id"`
	Url  string  `json:"url"`
	Title  string  `json:"title"`
	Content string  `json:"content"`
	// DatePublished sql.NullInt64  `json:"date_published"`
	// DateModified sql.NullInt64  `json:"date_modified"`
	DatePublished *int  `json:"date_published"`
	DateModified *int  `json:"date_modified"` // Prevent: converting NULL to int is unsupported
	UserId  int `json:"user_id"`
}

func GetAll() ([]Page, error) {
	db, err := connect() // connect() declared in user.go
    if err != nil {
        return []Page{}, err
    }
    defer db.Close()

	rows, err := db.Query("SELECT * FROM page")
	if err != nil {
		return nil, err
	}
 
	pages := []Page{}
	for rows.Next() {
		var page Page
		if err := rows.Scan(&page.Id, &page.Url, &page.Title, &page.Content, &page.DatePublished, &page.DateModified, &page.UserId); err != nil {
			return nil, err
		}
		pages = append(pages, page)
	}
	return pages, nil
}

func GetByIdOrUrl(id string) (Page, error) {
	db, err := connect()
	if err != nil {
		return Page{}, err
	}
	defer db.Close()

	var page Page
	var row *sql.Row
	if i, err := strconv.Atoi(id); err == nil {
		// Get page by id
		row = db.QueryRow("SELECT * FROM page WHERE id = ?", i)
	} else {
		// Get page by url
		row = db.QueryRow("SELECT * FROM page WHERE url = ?", id)
	}
	if err := row.Scan(&page.Id, &page.Url, &page.Title, &page.Content, &page.DatePublished, &page.DateModified, &page.UserId); err != nil {
		if err == sql.ErrNoRows {
			return Page{}, errors.New("Page not found: " + id)
		} else {
			return Page{}, err
		}
	}
	return page, nil
}

func Post(page Page) (error) {
	db, err := connect()
	if err != nil {
		return err
	}
	defer db.Close()

    _, err = db.Exec("INSERT INTO page (url, title, content, date_published, date_modified, user_id) VALUES (?, ?, ?, ?, ?, ?)", 
										page.Url, page.Title, page.Content, page.DatePublished, page.DateModified, page.UserId)
    if err != nil {
        return err
    }
    // id, err := result.LastInsertId()
    // if err != nil {
    //     return err
    // }
    return nil
}

func Put(page Page) (error) {
	db, err := connect()
	if err != nil {
		return err
	}
	defer db.Close()

	// _, err = db.Exec("UPDATE page SET url = ?, title = ?, content = ?, date_published = ?, date_modified = ?, user_id = ? WHERE id = ?",
	// 									page.Url, page.Title, page.Content, page.DatePublished, page.DateModified, page.UserId, page.Id)
	sql := "UPDATE page SET "
	if page.Url != "" {
		sql += "url = '" + page.Url + "', "
	}
	if page.Title != "" {
		sql += "title = '" + page.Title + "', "
	}
	if page.Content != "" {
		sql += "content = '" + page.Content + "', "
	}
	if page.DatePublished != nil {
		sql += "date_published = " + strconv.Itoa(*page.DatePublished) + ", "
	}
	if page.DateModified != nil {
		sql += "date_modified = " + strconv.Itoa(*page.DateModified) + ", "
	}
	if page.UserId != 0 {
		sql += "user_id = " + strconv.Itoa(page.UserId) + ", "
	}
	sql = sql[:len(sql)-2] + " WHERE id = " + strconv.Itoa(page.Id)
	_, err = db.Exec(sql)
	if err != nil {
		return err
	}
	return nil
}

func Delete(id int) (error) {
	db, err := connect()
	if err != nil {
		return err
	}
	defer db.Close()

	_, err = db.Exec("DELETE FROM page WHERE id = ?", id)
	if err != nil {
		return err
	}
	return nil
}
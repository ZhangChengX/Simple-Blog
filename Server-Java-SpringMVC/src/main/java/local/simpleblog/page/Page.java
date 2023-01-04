package local.simpleblog.page;

public class Page {

    // @TableId(type = IdType.AUTO)
    private int id;
    private int user_id;
    private String url;
    private String title;
    private String content;
    private int date_published;
    private int date_modified;

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public int getUserId() {
        return user_id;
    }

    public void setUserId(int userId) {
        this.user_id = userId;
    }

    public String getUrl() {
        return url;
    }

    public void setUrl(String url) {
        this.url = url;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getContent() {
        return content;
    }

    public void setContent(String content) {
        this.content = content;
    }

    public int getDatePublished() {
        return date_published;
    }

    public void setDatePublished(int datePublished) {
        this.date_published = datePublished;
    }

    public int getDateModified() {
        return date_modified;
    }

    public void setDateModified(int dateModified) {
        this.date_modified = dateModified;
    }

}

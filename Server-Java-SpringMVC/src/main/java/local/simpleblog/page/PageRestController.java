package local.simpleblog.page;

import java.util.ArrayList;
import java.util.List;
import java.util.Map;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.CrossOrigin;
import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PostMapping;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestParam;

import local.simpleblog.Message;

@RestController
@CrossOrigin
@RequestMapping("/api")
public class PageRestController {

    @Autowired
    private PageMapper pageMapper;

    @GetMapping("/page")
    public Message getPage(@RequestParam(name = "id") String id) {
        // if id == all, get all pages
        List<Page> pageList = new ArrayList<Page>();
        if (id.equals("all")) {
            pageList = pageMapper.getAll();
        } else {
            pageList.add(pageMapper.getById(Integer.parseInt(id)));
        }
        return new Message("Success", pageList);
    }

    @PostMapping("/page")
    public Message postPage(@RequestParam Map<String, String> request) {
        Page page = new Page();
        page.setUserId(Integer.parseInt(request.get("user_id")));
        page.setUrl(request.get("url"));
        page.setTitle(request.get("title"));
        page.setContent(request.get("content"));
        page.setDatePublished(Integer.parseInt(request.get("date_published")));
        page.setDateModified(Integer.parseInt(request.get("data_modified")));
        pageMapper.add(page);
        return new Message("Success", "Success");
    }

    @PutMapping("/page")
    public Object putPage(@RequestParam Map<String, String> request) {
        Page page = new Page();
        page.setId(Integer.parseInt(request.get("id")));
        page.setUserId(Integer.parseInt(request.get("user_id")));
        page.setUrl(request.get("url"));
        page.setTitle(request.get("title"));
        page.setContent(request.get("content"));
        page.setDatePublished(Integer.parseInt(request.get("date_published")));
        page.setDateModified(Integer.parseInt(request.get("data_modified")));
        pageMapper.updateById(page);
        return new Message("Success", "Success");
    }

    @DeleteMapping("/page")
    public Message deletePage(@RequestParam(name = "id") int id) {
        pageMapper.deleteById(id);
        return new Message("Success", "Delete successfully");
    }

}
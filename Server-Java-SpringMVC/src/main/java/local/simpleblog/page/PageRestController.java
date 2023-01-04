package local.simpleblog.page;

import java.util.ArrayList;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestParam;

import local.simpleblog.Message;

@RestController
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

}
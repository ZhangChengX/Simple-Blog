package local.simpleblog.page;

import java.sql.Timestamp;
import java.util.List;

import org.apache.commons.lang3.math.NumberUtils;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;

import local.simpleblog.user.UserMapper;

@Controller
public class PageController {

    @Autowired
    private PageMapper pageMapper;
    @Autowired
    private UserMapper userMapper;

    @GetMapping("/page")
    public String getPageList(Model model) {
        String title = "Page List";
        List<Page> pageList = pageMapper.getAll();
		model.addAttribute("title", title);
        model.addAttribute("pageList", pageList);
		return "page_list";
    }

    @GetMapping("/page/{id}")
    public String getPageByIdOrUrl(@PathVariable String id,
            Model model) {
        Page page;
        if (NumberUtils.isParsable(id)) {
            page = pageMapper.getById(Integer.parseInt(id));
        } else {
            page = pageMapper.getByUrl(id);
        }
        // if DateModified is 0, set date to DatePublished
        Timestamp tsDate;
        if (page.getDateModified() != 0) {
            tsDate = new Timestamp(page.getDateModified());
        } else {
            tsDate = new Timestamp(page.getDatePublished());
        }
        // Get username
        int userId = page.getUserId();
        if(userId <= 0) {
            userId = 1;
        }
        String username = userMapper.getById(userId).getUsername();
        // Assign attribute
        model.addAttribute("title", page.getUrl());
        model.addAttribute("content", page.getContent());
        model.addAttribute("username", username);
        model.addAttribute("date", tsDate.toString());
        return "page";
    }

}

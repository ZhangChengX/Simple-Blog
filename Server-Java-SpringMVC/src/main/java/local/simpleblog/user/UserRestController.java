package local.simpleblog.user;

import javax.servlet.ServletException;
import javax.servlet.http.HttpServletRequest;

import org.springframework.security.core.context.SecurityContextHolder;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.bind.annotation.RequestParam;

import local.simpleblog.Message;

@RestController
@RequestMapping("/api")
public class UserRestController {

    @RequestMapping("/user/login")
    public Message login(
            @RequestParam(name = "username", defaultValue = "") String username,
            @RequestParam(name = "password", defaultValue = "") String password,
            HttpServletRequest request
            ) {
        
        try {
            request.login(username, password);
            return new Message("success", username + " logged in successfully.");
        } catch (ServletException e) {
            if (username.isEmpty() && password.isEmpty()) {
                return new Message("error", "Please login.");
            } else {
                // e.printStackTrace();
                // return new Message("error", "Invalid Credentials provided.");
                return new Message("error", e.getMessage());
            }
        }
    }

    @RequestMapping("/user/logout")
    public Message logout(HttpServletRequest request) {
        try {
            request.logout();
            return new Message("success", "Logged out successfully.");
        } catch (ServletException e) {
            // e.printStackTrace();
            return new Message("error", e.getMessage());
        }
    }

    @GetMapping("/user")
    public Message status() {
        Object principal = SecurityContextHolder.getContext().getAuthentication().getPrincipal();
        String loggedUsername = "";
        if (principal instanceof UserDetails) {
            loggedUsername = ((UserDetails)principal).getUsername();
        } else {
            loggedUsername = principal.toString();
        }
        return new Message("info", loggedUsername);
    }

}

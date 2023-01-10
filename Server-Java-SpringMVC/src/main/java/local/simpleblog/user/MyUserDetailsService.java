package local.simpleblog.user;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.userdetails.User;
import org.springframework.security.core.userdetails.UserDetails;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.core.userdetails.UsernameNotFoundException;

public class MyUserDetailsService implements UserDetailsService {

    @Autowired
	private UserMapper userMapper;
    
    @Override
    public UserDetails loadUserByUsername(String username) throws UsernameNotFoundException {
        if (username.equals("")) {
            throw new UsernameNotFoundException(username);
        }
        local.simpleblog.user.User user = userMapper.getByUsername(username);
        if (user == null) {
            throw new UsernameNotFoundException(username);
        }
        UserDetails userDetails = User.withUsername(user.getUsername())
                                .password("{noop}" + user.getPassword()) // {noop} NoOpPasswordEncoder
                                .authorities("USER")
                                .build();
        return userDetails;
    }
    
}

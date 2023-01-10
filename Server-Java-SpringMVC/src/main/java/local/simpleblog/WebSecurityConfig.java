package local.simpleblog;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.core.userdetails.UserDetailsService;

import local.simpleblog.user.MyUserDetailsService;

@Configuration
@EnableWebSecurity
public class WebSecurityConfig {
	
    @Bean
	public SecurityFilterChain securityFilterChain(HttpSecurity http) throws Exception {
		http
			.cors().and().csrf().disable()
			.authorizeHttpRequests((requests) -> requests
				.antMatchers("/", "/index.html", "/page/**", "/api/user/**").permitAll()
				.anyRequest().authenticated()
			)
			.formLogin((form) -> form
				.loginPage("/api/user/login").permitAll()
			)
			.logout((logout) -> logout
				.logoutUrl("/api/user/logout").permitAll()
			);
			
		return http.build();
	}

	@Bean
    public UserDetailsService userDetailsService() {
        return new MyUserDetailsService();
    }

	// @Bean
	// public PasswordEncoder passwordEncoder() {
	// 	return NoOpPasswordEncoder.getInstance();
	// }

	// @Bean
    // public BCryptPasswordEncoder passwordEncoder() {
    //     return new BCryptPasswordEncoder();
    // }

}

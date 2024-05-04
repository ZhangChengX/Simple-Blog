using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController(UserContext _ctx, UserService userService) : ControllerBase
	{
		[HttpGet("login")]
		public ActionResult Login(string username, string? password)
		{
			var user = _ctx.User.FirstOrDefault(u => u.Username == username && u.Password == password);
			if (user == null)
			{
				return Unauthorized(new Message("error", "Invalid Credentials provided."));
			}

			var token = userService.GenerateToken(user.Id.ToString());
			return Ok(new Message("success", token));
		}

		[HttpGet("logout")]
		public ActionResult Logout(string? token)
		{
			if (token == null || !userService.VerifyToken(token))
			{
				return Unauthorized(new Message("error", "Please login."));
			}
			var refreshedToken = userService.RefreshToken(token, DateTime.Now.AddSeconds(-3600));
			return Ok(new Message("success", refreshedToken));
		}
	}
}

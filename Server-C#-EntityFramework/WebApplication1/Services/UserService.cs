using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebApplication1.Services
{
	public class UserService(IConfiguration config)
	{
		private readonly IConfiguration _config = config;

		public string GenerateToken(string userId)
		{
			if (!double.TryParse(_config["JWT:ExpirationSeconds"], out double expirationSeconds)) expirationSeconds = 3600;

			List<Claim> claims =
			[
				new Claim("user_id", userId),
				new Claim("expiry", DateTime.Now.AddSeconds(expirationSeconds).ToString()),
				//new Claim(ClaimTypes.Role, "Admin")
			];
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var jwtSecurityToken = new JwtSecurityToken(
				claims: claims,
				signingCredentials: credentials,
				expires: DateTime.Now.AddSeconds(expirationSeconds)
				);
			var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
			return token;
		}

		public bool VerifyToken(string token)
		{
			var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
			var expiry = jwtSecurityToken.Claims.First(claim => claim.Type == "expiry").Value;
			if(Convert.ToDateTime(expiry) < DateTime.Now)
			{
				Console.WriteLine("Token expired. " +
					"Token time: " + DateTime.Parse(expiry) + 
					" Current time: " + DateTime.Now.ToString()
					);
				return false;
			}
			return true;
		}

		public string RefreshToken(string token, DateTime expiry)
		{
			var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
			var userId = jwtSecurityToken.Claims.First(claim => claim.Type == "user_id").Value;

			List<Claim> claims = [
				new Claim("user_id", userId),
				new Claim("expiry", expiry.ToString()),
			];
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
			var refreshedJwtSecurityToken = new JwtSecurityToken(
				claims: claims,
				signingCredentials: credentials,
				expires: expiry
				);
			var refreshedToken = new JwtSecurityTokenHandler().WriteToken(refreshedJwtSecurityToken);
			return refreshedToken;
		}
	}
}

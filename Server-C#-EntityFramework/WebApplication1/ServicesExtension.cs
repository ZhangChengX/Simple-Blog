
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebApplication1
{
	public static class ServicesExtension
	{
		// Add CORS Policy
		public static readonly string CorsPolicy = "CorsPolicy";
		public static IServiceCollection AddCorsPolicy(this IServiceCollection services) 
		{
			return services.AddCors(options => 
			{
				options.AddPolicy(CorsPolicy, policy =>
				{
					policy.WithOrigins("*");
                    policy.WithMethods("GET", "POST", "OPTIONS", "PUT", "DELETE");
					policy.WithHeaders("Origin", "Content-Type", "Accept", "X-Requested-With", "Authorization");
                });
			});
		}
	}
}

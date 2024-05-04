using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
	{
		public DbSet<User> User { get; set; } = null!;
	}
}

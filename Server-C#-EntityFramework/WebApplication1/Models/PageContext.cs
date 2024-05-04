using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
	public class PageContext(DbContextOptions<PageContext> options) : DbContext(options)
	{
		public DbSet<Page> Page { get; set; } = null!;
	}
}

using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
	[Table("user")]
	public class User
	{
		[Column("id")]
		public required int Id { get; set; }
		[Column("username")]
		public required string Username { get; set; }
		[Column("password")]
		public required string Password { get; set; }
	}
}

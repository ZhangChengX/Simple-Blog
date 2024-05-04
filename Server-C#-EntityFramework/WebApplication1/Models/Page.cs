using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WebApplication1.Models
{
	[Table("page")]
	public class Page
	{
		[Column("id")]
		public int Id { get; set; }
		[Column("user_id")]
		[JsonPropertyName("user_id")]
		public required int UserId { get; set; }
		[Column("url")]
		public required string Url { get; set; }
		[Column("title")]
		public required string Title { get; set; }
		[Column("content")]
		public string? Content { get; set; }
		[Column("date_published")]
		[JsonPropertyName("date_published")]
		public long? DatePublished { get; set; }
		[Column("date_modified")]
		[JsonPropertyName("date_modified")]
		public long? DateModified { get; set; }
	}
}

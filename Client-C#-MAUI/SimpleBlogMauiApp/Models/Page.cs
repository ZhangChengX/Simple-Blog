using System.Text.Json.Serialization;

namespace SimpleBlogMauiApp.Models
{
	public class Page
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }

		[JsonPropertyName("user_id")]
		public int UserId { get; set; }

		[JsonPropertyName("url")]
		public string Url { get; set; }

		[JsonPropertyName("title")]
		public string Title { get; set; }

		[JsonPropertyName("content")]
		public string Content { get; set; }

		[JsonPropertyName("date_published")]
		public long? DatePublished { get; set; }
		
		[JsonPropertyName("date_modified")]
		public long? DateModified { get; set; }
	}
}

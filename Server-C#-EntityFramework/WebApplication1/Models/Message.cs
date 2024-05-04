namespace WebApplication1.Models
{
	public class Message(string type, object content)
	{
		public string Type { get; set; } = type;
		public object Content { get; set; } = content;
	}
}

using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace WebApplication1.TagHelpers
{
    public class TimestampToDateTimeTagHelper : TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            // Get the inner content of the custom tags
            var innerContent = output.GetChildContentAsync().Result.GetContent();
            //long timestamp = (long) Convert.ToDouble(innerContent);
            _ = long.TryParse(innerContent, out long timestamp);
            output.Content.SetContent(DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime.ToString("yyyy-MM-dd HH:mm"));
        }
    }
}

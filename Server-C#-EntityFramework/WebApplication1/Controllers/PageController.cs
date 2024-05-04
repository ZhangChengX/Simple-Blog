using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
	public class PageController(PageContext _ctx) : Controller
	{
        public IActionResult Index()
		{
            //ViewData["Pages"] = _ctx.Page.ToListAsync();
			var pages = _ctx.Page.ToList();
            return View(pages);
		}

        [Route("[controller]/{url}")]
        public IActionResult Detail(string url)
		{
            Page? page;
            if (int.TryParse(url, out int id))
			{
                page = _ctx.Page.Find(id);
            }
			else
			{
				page = _ctx.Page.FirstOrDefault(x => x.Url == url);
            }
            if (page != null) ViewData["Date"] = page.DateModified ?? page.DatePublished;
            return View(page);
		}
	}
}

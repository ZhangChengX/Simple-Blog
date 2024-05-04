using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Services
{
	public class PageService(PageContext _pageContext)
	{
		private static readonly List<Page> AllPages = [];

		public async Task<ActionResult<Page?>> GetPage(int id)
		{
			return await _pageContext.Page.FindAsync(id);
		}

		public async Task<ActionResult<IEnumerable<Page>>> GetPages()
		{
			return await _pageContext.Page.ToListAsync();
		}

		public Task CreatePage(Page item)
		{
			AllPages.Add(item);
			return Task.CompletedTask;
		}

		public Task<Page?> UpdatePage(int id, Page item)
		{
			var page = AllPages.FirstOrDefault(x => x.Id == id);
			if (page != null)
			{
				page.UserId = item.UserId;
				page.Url = item.Url;
				page.Title = item.Title;
				page.Content = item.Content;
				page.DatePublished = item.DatePublished;
				page.DateModified = item.DateModified;
			}
			return Task.FromResult(page);
		}

		public Task DeletePage(int id)
		{
			var page = AllPages.FirstOrDefault(x => x.Id == id);
			if (page != null)
			{
				AllPages.Remove(page);
			}

			return Task.CompletedTask;
		}

		public Task<List<Page>> GetAllPages()
		{
			return Task.FromResult(AllPages);
		}
	}
}

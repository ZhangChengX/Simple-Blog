using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Controllers.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class PageController(PageContext _ctx, UserService userService) : ControllerBase
	{
		//[Authorize]
		[HttpGet()]
		public async Task<ActionResult<IEnumerable<Page>>> GetPage(string id, string token)
		{
			if(token == null || !userService.VerifyToken(token))
			{
				return Unauthorized(new Message("error", "Please login."));
			}

			var pages = new List<Page>();
			if (int.TryParse(id, out int numericId))
			{
				var page = await _ctx.Page.FindAsync(numericId);
				if (page != null) pages.Add(page);
			}
			else if (id == "all")
			{
				pages = await _ctx.Page.ToListAsync();
			}

			if (pages.Count <= 0)
			{
				return NotFound(new Message("error", "Page not found."));
			}
			return Ok(new Message("success", pages));
		}

		[HttpPost]
		public async Task<ActionResult<Page>> CreatePage([FromBody] Page page, string token)
		{
			if (token == null || !userService.VerifyToken(token))
			{
				return Unauthorized(new Message("error", "Please login."));
			}

			_ctx.Page.Add(page);
			await _ctx.SaveChangesAsync();
			
			//return CreatedAtAction(nameof(GetPage), new { id = page.Id }, page);
			return Ok(new Message("success", "Page added successfully."));
		}

		[HttpPut()]
		public async Task<ActionResult> UpdatePost(int id, [FromBody] Page page, string token)
		{
			if (token == null || !userService.VerifyToken(token))
			{
				return Unauthorized(new Message("error", "Please login."));
			}

			page.Id = id;
			_ctx.Entry(page).State = EntityState.Modified;

			try
			{
				await _ctx.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!(_ctx.Page?.Any(e => e.Id == id)).GetValueOrDefault())
				{
					return NotFound(new Message("error", "Page not found."));
				}
				else
				{
					return BadRequest(new Message("error", "Unknown error."));
					//throw;
				}
			}

			return Ok(new Message("success", "Page updated successfully."));
		}

		[HttpDelete()]
		public async Task<ActionResult> DeletePage(int id, string token)
		{
			if (token == null || !userService.VerifyToken(token))
			{
				return Unauthorized(new Message("error", "Please login."));
			}

			var page = await _ctx.Page.FindAsync(id);
			if (page == null)
			{
				return NotFound(new Message("error", "Page not found."));
			}

			_ctx.Page.Remove(page);
			await _ctx.SaveChangesAsync();

			return Ok(new Message("success", "Page deleted successfully."));
		}

	}
}

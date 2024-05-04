using Microsoft.EntityFrameworkCore;
using WebApplication1;
using WebApplication1.Models;
using WebApplication1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<UserService>();

// Add DB Context
var connectionString = builder.Configuration.GetConnectionString("Sqlite"); // appsettings.json
builder.Services.AddDbContext<PageContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDbContext<UserContext>(options => options.UseSqlite(connectionString));

// CORS
builder.Services.AddCorsPolicy();

//builder.Services.AddAuthorization();
//builder.Services.AddJWTAuthentication();
//builder.Services.AddSingleton<PageService>();

var app = builder.Build();

//app.MapGet("/page", async (PageContext db) => await db.Page.ToListAsync());
//app.MapGet("/page/{id}", async (PageContext db, int id) => await db.Page.FindAsync(id));

app.UseCors(ServicesExtension.CorsPolicy);

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseStaticFiles();

//app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

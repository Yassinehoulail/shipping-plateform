using Microsoft.EntityFrameworkCore;
using HoulailYassine.Data;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<HoulailYassine.Repos.ProduitRepos>();
builder.Services.AddScoped<HoulailYassine.Repos.ClientRepos>();
builder.Services.AddScoped<HoulailYassine.Repos.CommandeRepos>();
builder.Services.AddScoped<HoulailYassine.Repos.PanierRepos>();

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<MyDbContextGlovo>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("myCnn")));

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});



var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

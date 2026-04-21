using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CivicAction.Data;
using Microsoft.AspNetCore.Identity;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<CivicActionContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("CivicActionContext") ?? throw new InvalidOperationException("Connection string 'CivicActionContext' not found.")));

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<CivicActionContext>();
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CivicActionContext>();
    context.Database.EnsureCreated();
}
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();

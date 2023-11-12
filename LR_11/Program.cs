using LR_11.Filters.Action;
using LR_11.Loggers.FileLogger;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<CustomerCountLoggerFilterAttribute>();
// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<ActionLoggerFilterAttribute>();
    options.Filters.Add(new ServiceFilterAttribute(typeof(CustomerCountLoggerFilterAttribute)));
});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

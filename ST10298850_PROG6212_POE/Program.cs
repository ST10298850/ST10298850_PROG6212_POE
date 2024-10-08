var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

//--------------------------------------------------------REFERENCES--------------------------------------------------------
// Microsoft. (2023). Introduction to ASP.NET Core. [online] Available at: https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0 [Accessed 10 September 2024].

//Japikse, P. & Troelsen, A. W., 2022. Pro C 10 with .NET 6:Foundational Principles and Practices in Programming. 11th ed. New York: Apress.

//--------------------------------------------------------END OF FILE--------------------------------------------------------
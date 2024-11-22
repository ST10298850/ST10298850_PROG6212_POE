using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ST10298850_PROG6212_POE.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register AppDbContext with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add session support
builder.Services.AddSession();

var app = builder.Build();

// Ensure database is migrated
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    context.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//--------------------------------------------------------REFERENCES--------------------------------------------------------
// Microsoft. (2023). Introduction to ASP.NET Core. [online] Available at: https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0 [Accessed 10 September 2024].

//Japikse, P. & Troelsen, A. W., 2022. Pro C 10 with .NET 6:Foundational Principles and Practices in Programming. 11th ed. New York: Apress.

//Stagner, J. (2020) [how do I:] get started with ASP.NET AJAX?, Microsoft Learn. Available at: https://learn.microsoft.com/en-us/aspnet/web-forms/videos/aspnet-ajax/how-do-i-get-started-with-aspnet-ajax (Accessed: 18 October 2024). 

//Microsoft. (2024) Understanding ASP.NET AJAX UpdatePanel Triggers. [online] Available at: https://learn.microsoft.com/en-us/previous-versions/aspnet/bb398874(v=vs.100) [Accessed 22 Nov. 2024].

//Microsoft. (2024) Inside the Microsoft AJAX Library. [online] MSDN Magazine. Available at: https://learn.microsoft.com/en-us/archive/msdn-magazine/2007/december/cutting-edge-inside-the-microsoft-ajax-library [Accessed 22 Nov. 2024].

//jQuery Foundation. (2024) jQuery. [online] Available at: https://jquery.com/ [Accessed 22 Nov. 2024].

//Microsoft. (2024) How to Write Text to a File. [online] Available at: https://learn.microsoft.com/en-us/dotnet/standard/io/how-to-write-text-to-a-file [Accessed 22 Nov. 2024].

//Microsoft. (2024) Exception Handling(C#). [online] Available at: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/exception-handling [Accessed 22 Nov. 2024].

//Microsoft. (2024) Exceptions(C# Fundamentals). [online] Available at: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/exceptions/ [Accessed 22 Nov. 2024].
//--------------------------------------------------------END OF FILE--------------------------------------------------------

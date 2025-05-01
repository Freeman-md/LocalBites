using LocalBites.Data;
using LocalBites.Interfaces.Repositories;
using LocalBites.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<LocalBitesContext>(
    options => options.UseSqlite("Data Source=LocalBites.db")
);

builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<LocalBitesContext>();
    dbContext.Database.Migrate();

    // SeedData.Initialize(dbContext);
}

app.Run();

public partial class Program { }
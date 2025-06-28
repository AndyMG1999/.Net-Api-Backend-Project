using api.Data;
using api.Interfaces;
using api.Models;
using api.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

// filepath: /Applications/Git Repos/NETApiExampleProject/api/Program.cs
builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseInMemoryDatabase("DummyDb");
});

builder.Services.AddScoped<IStockRepository, StockRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

var app = builder.Build();

// After app is built, before app.Run()
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
    // Adding Stocks
    db.Stocks.Add(new Stock { Symbol = "GOO", CompanyName = "Google", Purchase = 120.13m, Industry = "Technology", MarketCap = ((long)Random.Shared.Next(0, int.MaxValue) << 32) | (uint)Random.Shared.Next(0, int.MaxValue)});
    db.Stocks.Add(new Stock { Symbol = "VAL", CompanyName = "Valve", Purchase = 92.47m, Industry = "Technology", MarketCap = ((long)Random.Shared.Next(0, int.MaxValue) << 32) | (uint)Random.Shared.Next(0, int.MaxValue)});
    db.Stocks.Add(new Stock { Symbol = "WNB", CompanyName = "Warner Bros.", Purchase = 345.99m, Industry = "Entertainment", MarketCap = ((long)Random.Shared.Next(0, int.MaxValue) << 32) | (uint)Random.Shared.Next(0, int.MaxValue)});
    db.Stocks.Add(new Stock { Symbol = "TDJ", CompanyName = "Trader Joe's", Purchase = 75.00m, Industry = "Produce", MarketCap = ((long)Random.Shared.Next(0, int.MaxValue) << 32) | (uint)Random.Shared.Next(0, int.MaxValue)});

    // Adding Comments
    db.Comments.Add(new Comment { Title = "I love Google!", Content = "Google really is awesome, I love when they spy on me!", StockId = 1 });
    db.Comments.Add(new Comment { Title = "I also love Google!", Content = "I also think Google really is awesome, I love when they spy on me as well!", StockId = 1 });

    db.Comments.Add(new Comment { Title = "Tesla is such a poopy company 🙁", Content = "I just feel like I need to get that out of my system tbh...", StockId = 2 });
    db.Comments.Add(new Comment { Title = "Steam deck user here", Content = "Just wanted to say the steam deck is super cool and should definitely get one", StockId = 2 });
    db.Comments.Add(new Comment { Title = "😩", Content = "Valve pls give me money", StockId = 2 });

    db.Comments.Add(new Comment { Title = "TJ Flirtin!", Content = "why tj workers be flirtin all the dag time!", StockId = 4 });
    db.Comments.Add(new Comment { Title = "Bring back Pumkin Sauce", Content = "Trader Joe's... I need it...", StockId = 4 });
    db.Comments.Add(new Comment { Title = "i love flirting with curstomers", Content = "you guys aren't crazy, we do flirt with ya'll", StockId = 4 });
    db.SaveChanges();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Redirect root URL to Swagger UI
app.MapGet("/", context =>
{
    context.Response.Redirect("/swagger");
    return Task.CompletedTask;
});

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

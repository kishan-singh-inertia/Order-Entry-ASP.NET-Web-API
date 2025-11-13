using Ecom.Models;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// Configure CORS - add any local origins you need (http/https + ports)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocal", policy =>
    {
        policy.WithOrigins(
                "http://localhost:3000",    // if you have a React app
                "http://localhost:5075",    // your api http launch url
                "https://localhost:7063"    // your api https launch url
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// DbContext (Postgres)
string connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("Connection string 'Default' not found.");
builder.Services.AddDbContext<EcomDbContext>(options =>
    options.UseNpgsql(connectionString)
           .EnableDetailedErrors()
           .EnableSensitiveDataLogging() // dev only: remove or guard in production
);

// Build app
var app = builder.Build();

// Development-only middleware
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // shows detailed exception stack and inner exceptions
    // optional: enable database error page if using EF Core packages
}

// Middleware order
app.UseHttpsRedirection();

app.UseStaticFiles(); // serve wwwroot/* (put Order.html in wwwroot/UI/Order.html)

app.UseRouting();

app.UseCors("AllowLocal");

app.UseAuthorization();

// Map controllers
app.MapControllers();

app.Run();

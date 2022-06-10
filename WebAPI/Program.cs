using Microsoft.EntityFrameworkCore;
using ToysAndGames.DataAccess;
using ToysAndGames.Services.Contracts;
using ToysAndGames.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IProductImageService,ProductImageService>();

//DbContext
builder.Services.AddDbContext<ToysAndGamesContext>(options =>
options.UseSqlServer(
    builder.Configuration.GetConnectionString("Default")
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
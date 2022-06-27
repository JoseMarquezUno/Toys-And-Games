using Microsoft.EntityFrameworkCore;
using ToysAndGames.DataAccess;
using ToysAndGames.Services.Contracts;
using ToysAndGames.Services.Services;
using WebAPI.Services.DIHelper;
using WebAPI.Services.Mapper;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//TODO: Move this to an static or Extension method inside the service as a good practice.
//ToysAndGamesService.AddServices()


builder.Services.AddServices();
////DbContext
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
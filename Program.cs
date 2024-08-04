using AutoMapper;
using BACKEND_ASP.NET_WEB_API.Interfaces;
using BACKEND_ASP.NET_WEB_API.Models;
using BACKEND_ASP.NET_WEB_API.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// Service for ApiDbContext
builder.Services.AddDbContext<ApiDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnectionString")));

// Dependency injection for repositories
builder.Services.AddScoped<IUsersRepository, UsersRepository>();

// Dependency injection for AutoMapper
builder.Services.AddAutoMapper(typeof(Mapper));

// end Services
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

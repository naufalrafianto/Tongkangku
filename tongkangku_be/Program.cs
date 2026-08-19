using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Middlewares;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration
.GetConnectionString("TongkangkuDb");
builder.Services.AddDbContext<ApplicationDbContext>(
opt => opt.UseNpgsql(connStr));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
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

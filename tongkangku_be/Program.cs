using Microsoft.EntityFrameworkCore;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Middlewares;
using tongkangku_be.Middlewares.tongkangku_be.Middleware;
using tongkangku_be.Repositories;
using tongkangku_be.Services;

var builder = WebApplication.CreateBuilder(args);

var connStr = builder.Configuration
.GetConnectionString("TongkangkuDb");
builder.Services.AddDbContext<ApplicationDbContext>(
opt => opt.UseNpgsql(connStr));

// Add services to the container.
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>)
);




builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<NotFoundMiddleware>();

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

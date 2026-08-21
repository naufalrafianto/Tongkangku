using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using tongkangku_be.Data;
using tongkangku_be.Interfaces;
using tongkangku_be.Middlewares;
using tongkangku_be.Middlewares.tongkangku_be.Middleware;
using tongkangku_be.Repositories;
using tongkangku_be.Services;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var jwtKey = config["Jwt:Key"]
    ?? throw new InvalidOperationException("JWT Key belum dikonfigurasi.");

var connStr = builder.Configuration
.GetConnectionString("TongkangkuDb");
builder.Services.AddDbContext<ApplicationDbContext>(
opt => opt.UseNpgsql(connStr));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer =
                    builder.Configuration["Jwt:Issuer"],

                ValidAudience =
                    builder.Configuration["Jwt:Audience"],

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                NameClaimType =
                    ClaimTypes.NameIdentifier,

                RoleClaimType =
                    ClaimTypes.Role
            };
    });



builder.Services.AddScoped<IRentalRepository, RentalRepository>();

// Add services to the container.
builder.Services.AddScoped<IRentalContractRepository, RentalContractRepository>();
builder.Services.AddScoped<IRentalOfferRepository, RentalOfferRepository>();
builder.Services.AddScoped<IRentalOfferService, RentalOfferService>();


builder.Services.AddScoped<IRentalRepository, RentalRepository>();

builder.Services.AddScoped<IRentalContractService, RentalContractService>();

builder.Services.AddScoped<ILaytimeRecordRepository, LaytimeRecordRepository>();
builder.Services.AddScoped<ILaytimeRecordService, LaytimeRecordService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICargoTypeService, CargoTypeService>();
builder.Services.AddScoped<IVesselService, VesselService>();
builder.Services.AddScoped<IRentalService, RentalService>();
builder.Services.AddScoped<IVesselCategoryService, VesselCategoryService>();
builder.Services.AddScoped<IPortService, PortService>();
// Tambahkan registrasi repository ini di Program.cs
builder.Services.AddScoped<IRentalOfferRepository, RentalOfferRepository>();

// Pastikan RentalContractService juga sudah terdaftar:
builder.Services.AddScoped<IRentalContractService, RentalContractService>();
builder.Services.AddScoped(
    typeof(IRepository<>),
    typeof(Repository<>)
);

builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.NumberHandling =
            System.Text.Json.Serialization.JsonNumberHandling.AllowReadingFromString;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors("AllowAngular");
app.UseMiddleware<GlobalExceptionMiddleware>();
app.UseMiddleware<NotFoundMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

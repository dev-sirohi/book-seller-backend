using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BSB.src.Domain.Entities;
using BSB.src.Application.Interfaces;
using BSB.src.Application.Services;
using BSB.src.Common.Database;
using BSB.src.Common.Database.DBInterfaces;
using Microsoft.Extensions.Options;
using BSB.src.Common.Database.DBServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT configuration
var jwtSection = builder.Configuration.GetSection("Jwt");
builder.Services.Configure<JwtOptions>(jwtSection);
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "dev_secret_key_1234567890";
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "book-seller-api",
        ValidAudience = builder.Configuration["Jwt:Audience"] ?? "book-seller-client",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret))
    };
});

var sqlAppSettings = builder.Configuration.GetSection("SQL");
builder.Services.Configure<DBAppSettings>(sqlAppSettings);

builder.Services.AddScoped<IDBConnection>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<DBAppSettings>>().Value;
    if (string.IsNullOrWhiteSpace(settings.ConnectionStrings?["DefaultConnection"]))
    {
        throw new InvalidOperationException("DB:ConnectionString is not configured");
    }

    IDBConnection cn = new DBConnection(settings.ConnectionStrings["DefaultConnection"]);
    return cn;
});


// Dependency injection
builder.Services.AddScoped<JwtService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

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
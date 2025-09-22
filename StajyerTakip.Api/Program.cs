using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MediatR;

using StajyerTakip.Application;
using StajyerTakip.Infrastructure.Data;

// alias’lar (ÖNEMLİ)
using AppInt  = StajyerTakip.Application.Interfaces;        // Application arayüzleri
using ReposNS = StajyerTakip.Infrastructure.Repositories;   // Repo implementasyonları
using AuthNS  = StajyerTakip.Infrastructure.Auth;           // JWT/Refresh servisleri

var builder = WebApplication.CreateBuilder(args);

// Db
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// MediatR
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
});

// Repositories — DOĞRU interface ile kaydet
builder.Services.AddScoped<AppInt.IInternRepository, ReposNS.InternRepository>();
builder.Services.AddScoped<AppInt.IUserRepository,   ReposNS.DemoUserRepository>();

// Auth servisleri — DOĞRU interface ile kaydet
builder.Services.Configure<AuthNS.JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddScoped<AppInt.IJwtTokenService,     AuthNS.JwtTokenService>();
builder.Services.AddScoped<AppInt.IRefreshTokenService, AuthNS.RefreshTokenService>();

builder.Services.AddControllers();

// CORS
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("DevCors", p =>
        p.WithOrigins("http://localhost:5173")
         .AllowAnyHeader()
         .AllowAnyMethod());
});

// Swagger + JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "StajyerTakip API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header, Name = "Authorization",
        Type = SecuritySchemeType.Http, Scheme = "bearer", BearerFormat = "JWT",
        Description = "Bearer {token}"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// JWT doğrulama
var jwt = builder.Configuration.GetSection("Jwt");
var keyBytes = Encoding.UTF8.GetBytes(jwt["Secret"]!);

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, ValidateAudience = true, ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"], ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ClockSkew = TimeSpan.FromSeconds(30)
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("DevCors");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();

using AutoSinco.Domain.Contracts;
using AutoSinco.Domain.Contracts.ListaPreciosRepository;
using AutoSinco.Domain.Contracts.ReporteRepository;
using AutoSinco.Domain.Contracts.TipoVehiculoRepository;
using AutoSinco.Domain.Contracts.VehiculoRepository;
using AutoSinco.Domain.Contracts.VentaRepository;
using AutoSinco.Domain.Services;
using AutoSinco.Domain.Services.ListaPreciosRepository;
using AutoSinco.Domain.Services.ReporteRepository;
using AutoSinco.Domain.Services.TipoVehiculoRepository;
using AutoSinco.Domain.Services.VehiculoRepository;
using AutoSinco.Domain.Services.VentaRepository;
using AutoSinco.Infraestructure;
using AutoSinco.Shared.InDTO;
using AutoSinco.WebApi.Attributes;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PortalEmpleo.Domain.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configuración de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

// Configuración de la base de datos
builder.Services.AddDbContext<DBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// Configuración de settings
builder.Services.Configure<UsuarioSettings>(
    builder.Configuration.GetSection("UsuarioSettings")
);

// Registro de servicios principales
builder.Services.AddScoped<IAccesoRepository, AccesoRepository>();
builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITokenRepository, TokenRepository>();
builder.Services.AddScoped<IConstantesService, ConstantesService>();
builder.Services.AddScoped<ITipoVehiculoRepository, TipoVehiculoRepository>();
builder.Services.AddScoped<IVehiculoRepository, VehiculoRepository>();
builder.Services.AddScoped<IListaPreciosRepository, ListaPreciosRepository>();
builder.Services.AddScoped<IVentaRepository, VentaRepository>();
builder.Services.AddScoped<IReporteRepository, ReporteRepository>();

// Registro de filtros y atributos
builder.Services.AddScoped<AccesoAttribute>();
builder.Services.AddScoped<AutorizacionJwtAttribute>();
builder.Services.AddScoped<LogAttribute>();
builder.Services.AddScoped<ValidarModeloAttribute>();

// Configuración de filtros globales en los controladores
builder.Services.AddControllers(options =>
{
    options.Filters.AddService<LogAttribute>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Middleware pipeline
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// Manejo global de excepciones
app.UseExceptionHandler("/Error");
app.UseHsts();

app.MapControllers();

app.Run();
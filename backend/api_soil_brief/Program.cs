using api_soil_brief.Data;
using api_soil_brief.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using api_soil_brief.Helpers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);


var corsPolicy = "AllowSpecificOrigin";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsPolicy, policy =>
    {
        policy
            .WithOrigins("http://localhost:8081", "https://localhost:8081", "http://localhost:7137") // 🔹 URLs permitidas
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
              });
});

// Registrar DbContext
builder.Services.AddDbContext<DataBaseConfig>(options =>
    options.UseMySql(
        "server=localhost;database=db_soil;user=root;password=root;",
        new MySqlServerVersion(new Version(8, 0, 33))
    )
);

// Configurar autenticação JWT
var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
    };
});

// Registrar classe com IConfiguration
builder.Services.AddSingleton<JwtHelper>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return new JwtHelper(config);
});

// Configurar Swagger com Autenticação JWT
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "API Soil Software",
        Version = "v1"
    });

    // Configuração do botão "Authorize" no Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando o esquema Bearer. Exemplo: 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

// Registrar HttpClient (necessario para IHttpClientFactory)
builder.Services.AddHttpClient();

// Registrar HostedService
builder.Services.AddHostedService<ChamadaEsp32Service>();
builder.Services.AddScoped<SoloService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<SensorService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<CulturaService>();
builder.Services.AddScoped<HistoricoService>();

// Add controllers e Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseCors(corsPolicy);
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

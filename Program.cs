using _4kTiles_Backend.Context;
using _4kTiles_Backend.Services.Repositories;
using _4kTiles_Backend.Services.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

// --------------------------------------------------
// create builder instance
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. -------------------
// Add controllers to the services
builder.Services.AddControllers();

// Add swagger to the services
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(configs =>
{
    configs.CustomSchemaIds(type => type.FullName.Split('.').Last().Replace("DTO", ""));
    // API information
    configs.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "4kTiles API",
        Description = "An ASP.NET Core Web API for Unity game and WPF client",
    });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    configs.IncludeXmlComments(xmlFilePath);

    // Bearer token authentication
    configs.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Bearer",
        BearerFormat = "JWT",
        Scheme = "bearer",
        Description = "Specify the authorization token.",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
    });

    // Make sure swagger UI requires a Bearer token specified
    configs.AddSecurityRequirement(new OpenApiSecurityRequirement {
   {
     new OpenApiSecurityScheme
     {
       Reference = new OpenApiReference
       {
         Type = ReferenceType.SecurityScheme,
         Id = "Bearer"
       }
      },
      new string[] { }
    }
  });
});

// Add the database context to the services
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString("DevelopmentDB")
        : builder.Configuration.GetConnectionString("development"));
});

// Add AutoMapper to the services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add the repository to the services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Add authentication to the services
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:securityKey"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Manager", policy => policy.RequireRole(new[] { "Admin" }));
    options.AddPolicy("Creator", policy => policy.RequireRole(new[] { "User" }));
});

// Add cors
builder.Services.AddCors();
// --------------------------------------------------
// Build the application
var app = builder.Build();

// Enable Swagger/OpenAPI middleware
// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI(configs =>
{
    configs.SwaggerEndpoint("/swagger/v1/swagger.json", "4kTiles API");
    configs.RoutePrefix = "";
});
// }

// Enable cors
app.UseCors(builder =>
{
    builder.WithOrigins(new[] { "https://localhost", "http://fktiles.azurewebsites.net" });
    builder.AllowAnyHeader();
    builder.AllowAnyMethod();
    builder.AllowCredentials();
});

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
// --------------------------------------------------
using System.Net;
using System.Reflection;
using System.Text;

using _4kTiles_Backend.Context;
using _4kTiles_Backend.DataObjects.DTO.Response;
using _4kTiles_Backend.Services.Auth;
using _4kTiles_Backend.Services.Email;
using _4kTiles_Backend.Services.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
    configs.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// Add the database context to the services
builder.Services.AddDbContextPool<ApplicationDbContext>(options =>
{
    if (builder.Environment.IsDevelopment())
        options.UseNpgsql(builder.Configuration.GetConnectionString("DevelopmentDB"));
    else
    {
        var connectionUrl = Environment.GetEnvironmentVariable("DATABASE_URL")!;
        var databaseUri = new Uri(connectionUrl!);
        var db = databaseUri.LocalPath.TrimStart('/');
        var userInfo = databaseUri.UserInfo.Split(':', StringSplitOptions.RemoveEmptyEntries);
        options.UseNpgsql($"User ID={userInfo[0]};Password={userInfo[1]};Host={databaseUri.Host};Port={databaseUri.Port};Database={db};Pooling=true;SSL Mode=Require;Trust Server Certificate=True;");
    }
});

// Add AutoMapper to the services
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add Email service
var emailConfig = builder.Configuration.GetSection("Email").Get<EmailConfig>();
builder.Services.AddSingleton<IEmailService>(sp => new EmailService(emailConfig, builder.Environment));

// Add the repository to the services
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ILibraryRepository, LibraryRepository>();
builder.Services.AddScoped<ILeaderboardRepository, LeaderboardRepository>();
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<ISongReportRepository, SongReportRepository>();

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

// Add Authorization Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Manager", policy => policy.RequireRole(new[] { "Admin" }));
    options.AddPolicy("Creator", policy => policy.RequireRole(new[] { "User" }));
});

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
}).ConfigureApiBehaviorOptions(opt =>
{
    // Custom Model State response object
    opt.InvalidModelStateResponseFactory = actionContext =>
        new BadRequestObjectResult(new ResponseDTO<ModelStateDictionary>
        {
            StatusCode = 400,
            ErrorCode = -1990,
            Message = "Input value is(are) invalid",
            Data = actionContext.ModelState
        });
});

// Add cors
// builder.Services.AddCors();
// --------------------------------------------------
// Build the application
var app = builder.Build();

// Log if the email service is enabled
app.Logger.LogInformation($"Enable email service: {emailConfig.Enabled}");
app.Logger.LogInformation($"Email used: {emailConfig.MailAddress}");

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
// app.UseCors(corsPolicyBuilder =>
// {
//     corsPolicyBuilder.WithOrigins(new[] { "https://localhost", "http://fktiles.azurewebsites.net" });
//     corsPolicyBuilder.AllowAnyHeader();
//     corsPolicyBuilder.AllowAnyMethod();
//     corsPolicyBuilder.AllowCredentials();
// });

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
// --------------------------------------------------
using _4kTiles_Backend.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    // TODO: Try to find a way to add connection string in Docker
    // options.UseSqlServer("");
});
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

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

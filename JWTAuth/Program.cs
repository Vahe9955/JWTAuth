using JWTAuth.Data;
using JWTAuth.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;
using JWTAuth.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var config = builder.Configuration;
// Add services to the container.

services.AddDbContext<JWTAuthContext>(options =>
options.UseNpgsql(config.GetConnectionString("Context") ?? throw new InvalidOperationException("Connection string 'ConnectionStrings' not found.")));


services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = config["Jwt:Audience"],
        ValidIssuer = config["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]))
    };
});

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(option =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    option.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    option.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "GIS API",
        Description = "ASP.NET Core Web API"
    });
});
services.AddTransient<IEmployees, EmployeeRepository>();

//services.AddTransient<IEmployees, EmployeeRepository>();
services.AddControllers();

var app = builder.Build();

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

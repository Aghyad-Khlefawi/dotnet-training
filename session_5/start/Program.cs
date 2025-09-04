using System.Net;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using training;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmployeeRepository, MssqlEmployeeRepository>();

builder.Services.AddControllers();
builder.Services.AddDbContext<EmployeeAppDbContext>(options =>
{
  options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

builder.Services.AddScoped<IUserRepository, MockUserRepository>();
builder.Services.AddAuthentication()
  .AddJwtBearer(options =>
  {
    options.Authority = "http://localhost:8080/realms/master";
    options.Audience = "d0da8495-a5a9-48d5-a207-2d144541f327";
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuer = true,
      ValidateIssuerSigningKey = true
    };
  });

var app = builder.Build();


app.UseAuthorization();
// app.UserBasicAuth();
app.MapControllers();
app.Run();




public record CreateEmployeeDto(string FullName, string Address);
public record UpdateEmployeeDto(int Id, string FullName, string Address);

using Microsoft.AspNetCore.Mvc;
using training;

var builder = WebApplication.CreateBuilder(args);

//Register
if (builder.Environment.IsDevelopment())
    builder.Services.AddScoped<IEmployeeRepository, MssqlEmployeeRepository>();

//builder.Services.AddTransient<EmployeeRepository>();
//builder.Services.AddSingleton<EmployeeRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var userName = context.Request.Headers["Username"].ToString();
    var password = context.Request.Headers["Password"].ToString();

    if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
        await next();
    else
    {
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("Unauthorized");
    }
});

app.MapControllers();
app.Run();

public record Employee(int Id, string FullName);
public record CreateEmployeeDto(string FullName);
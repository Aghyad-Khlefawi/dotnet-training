using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using training;

var builder = WebApplication.CreateBuilder(args);

//Register
//builder.Services.AddSingleton<EmployeeRepository>();
if (builder.Environment.IsDevelopment())
   // builder.Services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
//else
    builder.Services.AddScoped<IEmployeeRepository, MssqlEmployeeRepository>();
//builder.Services.AddTransient<EmployeeRepository>();

var app = builder.Build();

app.MapGet("/", () => { return "Hello World"; });

app.MapGet("/employee", ([FromServices] IEmployeeRepository repo) => { return Results.Ok(repo.GetAllEmployees()); });

app.Run();

public record Employee(int Id, string FullName);
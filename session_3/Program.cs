using Microsoft.AspNetCore.Mvc;
using training;

new Thread(o =>
{
    Console.WriteLine("Thread running!");
}).Start();

ThreadPool.QueueUserWorkItem((state) =>
{
    Console.WriteLine("Thread pool started");
});

var task1 = Task.Run(() =>
{
    Console.WriteLine("Task1 running");
    throw new Exception("Failed");
});
var task2 = Task.Run(() => Console.WriteLine("Task2 running"));


task1.ContinueWith((_) =>
{
    Console.WriteLine("Task1 completed");
}); 

task2.ContinueWith((_) =>
{
    Console.WriteLine("Task2 completed");
});


var builder = WebApplication.CreateBuilder(args);

//Register
//builder.Services.AddSingleton<EmployeeRepository>();
if (builder.Environment.IsDevelopment())
   // builder.Services.AddScoped<IEmployeeRepository, InMemoryEmployeeRepository>();
//else
    builder.Services.AddScoped<IEmployeeRepository, MssqlEmployeeRepository>();
//builder.Services.AddTransient<EmployeeRepository>();

builder.Services.AddControllers();

var app = builder.Build();

app.MapControllers();
app.Run();

public record Employee(int Id, string FullName);
public record CreateEmployeeDto(string FullName);
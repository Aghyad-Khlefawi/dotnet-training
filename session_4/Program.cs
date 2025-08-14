using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using training;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IEmployeeRepository, MssqlEmployeeRepository>();

builder.Services.AddControllers();
builder.Services.AddDbContext<EmployeeAppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
});

var app = builder.Build();

app.MapControllers();
app.Run();




public record CreateEmployeeDto(string FullName,string Address);
public record UpdateEmployeeDto(int Id,string FullName,string Address);
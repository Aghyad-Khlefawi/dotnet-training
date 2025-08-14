using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace training;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployees();
    Task CreateEmployee(Employee employee);
    Task UpdateEmployee(int employeeId, Employee employee);
    Task DeleteEmployee(int employeeId);
}

public class MssqlEmployeeRepository : IEmployeeRepository
{
    private readonly IConfiguration _config;
    private readonly EmployeeAppDbContext _dbContext;

    public MssqlEmployeeRepository(IConfiguration config, EmployeeAppDbContext dbContext)
    {
        _config = config;
        _dbContext = dbContext;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var employees = await _dbContext.Employees.ToListAsync();
        return employees;
    }

    public async Task CreateEmployee(Employee employee)
    {
        employee.Id = _dbContext.Employees.Count();
        await _dbContext.Employees.AddAsync(employee);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateEmployee(int employeeId, Employee employee)
    {
        var employeeToUpdate = await _dbContext.Employees.FindAsync(employeeId);

        if (employeeToUpdate == null)
            throw new AggregateException("Employee not found");
        
        employeeToUpdate.FullName = employee.FullName;
        employeeToUpdate.Address = employee.Address;

        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteEmployee(int employeeId)
    {
        var employeeToDelete = await _dbContext.Employees.FindAsync(employeeId);

        if (employeeToDelete == null)
            throw new AggregateException("Employee not found");

        _dbContext.Employees.Remove(employeeToDelete);
        await _dbContext.SaveChangesAsync();
    }
}

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    private List<Employee> _employee = new List<Employee>
    {
        new Employee(0, "In mem")
    };

    public Task<List<Employee>> GetAllEmployees()
    {
        return Task.FromResult(_employee);
    }

    public Task CreateEmployee(Employee employee)
    {
        _employee.Add(employee);
        return Task.CompletedTask;
    }

    public Task UpdateEmployee(int employeeId, Employee employee)
    {
        throw new NotImplementedException();
    }

    public Task DeleteEmployee(int employeeId)
    {
        throw new NotImplementedException();
    }
}
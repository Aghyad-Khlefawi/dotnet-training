using System.Data;
using Microsoft.Data.SqlClient;

namespace training;

public interface IEmployeeRepository
{
    Task<List<Employee>> GetAllEmployees();
    Task CreateEmployee(Employee employee);
}

public class MssqlEmployeeRepository : IEmployeeRepository
{
    private readonly IConfiguration _config;

    public MssqlEmployeeRepository(IConfiguration config)
    {
        _config = config;
    }

    public async Task<List<Employee>> GetAllEmployees()
    {
        var sqlConnection = new SqlConnection(_config.GetConnectionString("DbConnection"));
        var sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = "select Id,FullName from employees";

        try
        {
            await sqlConnection.OpenAsync();
            var reader = await sqlCommand.ExecuteReaderAsync();
            var employees = new List<Employee>();
            while (await reader.ReadAsync())
            {
                employees.Add(new Employee(reader.GetInt32(0), reader.GetString(1)));
            }

            return employees;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            sqlConnection.Close();
        }
    }

    public async Task CreateEmployee(Employee employee)
    {
        var sqlConnection = new SqlConnection(_config.GetConnectionString("DbConnection"));
        var sqlCommand = sqlConnection.CreateCommand();

        sqlCommand.CommandText = "INSERT INTO Employees (FullName) values (@FullName)";
        sqlCommand.Parameters.Add("@FullName",SqlDbType.NVarChar,200);
        sqlCommand.Parameters["@FullName"].Value = employee.FullName;
        
        try
        {
            await sqlConnection.OpenAsync();
            await sqlCommand.ExecuteNonQueryAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        finally
        {
            sqlConnection.Close();
        }
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
}
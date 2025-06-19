using Microsoft.Data.SqlClient;

namespace training;

public interface IEmployeeRepository
{
    List<Employee> GetAllEmployees();
}

public class MssqlEmployeeRepository:IEmployeeRepository
{
    private readonly IConfiguration _config;

    public MssqlEmployeeRepository(IConfiguration config)
    {
        _config = config;
    }
    public List<Employee> GetAllEmployees()
    {
        var sqlConnection = new SqlConnection(_config.GetConnectionString("DbConnection"));
        var sqlCommand = sqlConnection.CreateCommand();
        sqlCommand.CommandText = "select Id,FullName from employees";

        sqlConnection.Open();
        try
        {
            var reader = sqlCommand.ExecuteReader();
            var employees = new List<Employee>();
            while (reader.Read())
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
}

public class InMemoryEmployeeRepository : IEmployeeRepository
{
    public List<Employee> GetAllEmployees()
    {
        return new List<Employee>
        {
            new Employee(0, "In mem")
        };
    }
}
namespace training;

public class Employee
{
    public Employee(int id, string fullName)
    {
        FullName = fullName;
        Id = id;
    }

    public Employee()
    {
    }

    public string Address { get; set; }
    public string FullName { get; set; }
    public int Id { get; set; }
}
using Microsoft.AspNetCore.Mvc;

namespace training.Controllers;

[Route("api/[controller]")]
public class EmployeeController:ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    [HttpGet("/")]
    public async Task<IActionResult> GetEmployees()
    {
        return Ok(await _employeeRepository.GetAllEmployees());
    }
}
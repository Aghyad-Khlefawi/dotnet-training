using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace training.Controllers;

[Route("api/employee")]
[Authorize]
public class EmployeeController:ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }
    
    [HttpGet()]
    public async Task<IActionResult> GetEmployees()
    {
        return Ok(await _employeeRepository.GetAllEmployees());
    }
    
    [HttpPost()]
    public async Task<IActionResult> CreateEmployee([FromBody]CreateEmployeeDto request)
    {
        await _employeeRepository.CreateEmployee(new Employee
        {
            FullName = request.FullName,
            Address = request.Address
        });
        return Ok();
    }
    
    [HttpPut()]
    public async Task<IActionResult> UpdateEmployee([FromBody]UpdateEmployeeDto request)
    {
        try
        {
            await _employeeRepository.UpdateEmployee(request.Id, new Employee
            {
                FullName = request.FullName,
                Address = request.Address
            });
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);
        }
        
        return Ok();
    }
}

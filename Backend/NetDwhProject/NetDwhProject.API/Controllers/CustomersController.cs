using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDwhProject.Core.Entities.Oltp;
using NetDwhProject.Core.Interfaces;

namespace NetDwhProject.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CustomersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();
        return Ok(customers);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null) return NotFound();
        return Ok(customer);
    }

    [HttpGet("{id}/orders")]
    public async Task<IActionResult> GetOrders(int id)
    {
        var orders = await _unitOfWork.Orders.FindAsync(o => o.CustomerId == id);
        return Ok(orders);
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string? email, [FromQuery] string? phone)
    {
        var customers = await _unitOfWork.Customers.GetAllAsync();

        if (!string.IsNullOrEmpty(email))
        {
            customers = customers.Where(c => c.Email.Contains(email, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrEmpty(phone))
        {
            customers = customers.Where(c => c.Phone.Contains(phone));
        }

        return Ok(customers);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Customer customer)
    {
        await _unitOfWork.Customers.AddAsync(customer);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Customer customer)
    {
        if (id != customer.Id) return BadRequest();
        _unitOfWork.Customers.Update(customer);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var customer = await _unitOfWork.Customers.GetByIdAsync(id);
        if (customer == null) return NotFound();

        // Check if customer has orders
        var orders = await _unitOfWork.Orders.FindAsync(o => o.CustomerId == id);
        if (orders.Any())
        {
            return BadRequest("Cannot delete customer with existing orders.");
        }

        _unitOfWork.Customers.Delete(customer);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}
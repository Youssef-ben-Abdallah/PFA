using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDwhProject.Core.Entities.Oltp;
using NetDwhProject.Core.Interfaces;
using NetDwhProject.Core.DTOs;

namespace NetDwhProject.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public OrdersController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _unitOfWork.Orders.GetAllAsync();
        var orderDtos = new List<OrderDto>();

        foreach (var order in orders)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId);
            var details = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id);

            var orderDto = MapToDto(order, customer, details);
            orderDtos.Add(orderDto);
        }

        return Ok(orderDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return NotFound();

        var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId);
        var details = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id);

        var orderDto = MapToDto(order, customer, details);
        return Ok(orderDto);
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(int customerId)
    {
        var orders = await _unitOfWork.Orders.FindAsync(o => o.CustomerId == customerId);
        var orderDtos = new List<OrderDto>();

        foreach (var order in orders)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId);
            var details = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id);

            var orderDto = MapToDto(order, customer, details);
            orderDtos.Add(orderDto);
        }

        return Ok(orderDtos);
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        var orders = await _unitOfWork.Orders.FindAsync(o => o.Status.ToLower() == status.ToLower());
        var orderDtos = new List<OrderDto>();

        foreach (var order in orders)
        {
            var customer = await _unitOfWork.Customers.GetByIdAsync(order.CustomerId);
            var details = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id);

            var orderDto = MapToDto(order, customer, details);
            orderDtos.Add(orderDto);
        }

        return Ok(orderDtos);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderDto createOrderDto)
    {
        // Validate customer exists
        var customer = await _unitOfWork.Customers.GetByIdAsync(createOrderDto.CustomerId);
        if (customer == null) return BadRequest("Customer does not exist.");

        // Validate products exist and have sufficient stock
        foreach (var detail in createOrderDto.OrderDetails)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);
            if (product == null) return BadRequest($"Product with ID {detail.ProductId} does not exist.");

            if (product.Stock < detail.Quantity)
                return BadRequest($"Insufficient stock for product {product.Name}. Available: {product.Stock}, Requested: {detail.Quantity}");
        }

        // Create order
        var order = new Order
        {
            CustomerId = createOrderDto.CustomerId,
            OrderDate = createOrderDto.OrderDate,
            Status = "Pending",
            TotalAmount = 0 // Will be calculated
        };

        await _unitOfWork.Orders.AddAsync(order);
        await _unitOfWork.CompleteAsync(); // Save to get order Id

        // Create order details and update stock
        decimal totalAmount = 0;
        foreach (var detail in createOrderDto.OrderDetails)
        {
            var product = await _unitOfWork.Products.GetByIdAsync(detail.ProductId);

            var orderDetail = new OrderDetail
            {
                OrderId = order.Id,
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice > 0 ? detail.UnitPrice : product!.Price // Use provided price or product price
            };

            await _unitOfWork.OrderDetails.AddAsync(orderDetail);
            totalAmount += orderDetail.LineTotal;

            // Update stock
            product!.Stock -= detail.Quantity;
            _unitOfWork.Products.Update(product);
        }

        // Update order total
        order.TotalAmount = totalAmount;
        _unitOfWork.Orders.Update(order);

        await _unitOfWork.CompleteAsync();

        // Return created order
        var savedOrder = await _unitOfWork.Orders.GetByIdAsync(order.Id);
        var savedCustomer = await _unitOfWork.Customers.GetByIdAsync(savedOrder!.CustomerId);
        var savedDetails = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == order.Id);

        var orderDto = MapToDto(savedOrder, savedCustomer, savedDetails);
        return CreatedAtAction(nameof(Get), new { id = orderDto.Id }, orderDto);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto statusDto)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return NotFound();

        order.Status = statusDto.Status;
        _unitOfWork.Orders.Update(order);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _unitOfWork.Orders.GetByIdAsync(id);
        if (order == null) return NotFound();

        // Delete order details first (cascade should handle this, but we'll do it explicitly)
        var details = await _unitOfWork.OrderDetails.FindAsync(od => od.OrderId == id);
        foreach (var detail in details)
        {
            _unitOfWork.OrderDetails.Delete(detail);
        }

        _unitOfWork.Orders.Delete(order);
        await _unitOfWork.CompleteAsync();

        return NoContent();
    }

    // Helper method to map entities to DTO
    private OrderDto MapToDto(Order order, Customer? customer, IEnumerable<OrderDetail> details)
    {
        var orderDto = new OrderDto
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            CustomerId = order.CustomerId,
            CustomerName = customer != null ? $"{customer.FirstName} {customer.LastName}" : "Unknown",
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            OrderDetails = new List<OrderDetailDto>()
        };

        foreach (var detail in details)
        {
            var product = _unitOfWork.Products.GetByIdAsync(detail.ProductId).Result; // Note: In real app, use async properly
            orderDto.OrderDetails.Add(new OrderDetailDto
            {
                Id = detail.Id,
                ProductId = detail.ProductId,
                ProductName = product?.Name ?? "Unknown",
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                LineTotal = detail.LineTotal
            });
        }

        return orderDto;
    }
}
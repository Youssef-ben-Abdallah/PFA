using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDwhProject.Core.Entities.Oltp;
using NetDwhProject.Core.Interfaces;

namespace NetDwhProject.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public CategoriesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return Ok(categories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return NotFound();
        return Ok(category);
    }

    [HttpGet("{id}/subcategories")]
    public async Task<IActionResult> GetSubCategories(int id)
    {
        var subCategories = await _unitOfWork.SubCategories.FindAsync(sc => sc.CategoryId == id);
        return Ok(subCategories);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(Category category)
    {
        await _unitOfWork.Categories.AddAsync(category);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(Get), new { id = category.Id }, category);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, Category category)
    {
        if (id != category.Id) return BadRequest();
        _unitOfWork.Categories.Update(category);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category == null) return NotFound();

        // Check if category has subcategories
        var subCategories = await _unitOfWork.SubCategories.FindAsync(sc => sc.CategoryId == id);
        if (subCategories.Any())
        {
            return BadRequest("Cannot delete category with existing subcategories.");
        }

        _unitOfWork.Categories.Delete(category);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}
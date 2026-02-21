using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NetDwhProject.Core.Entities.Oltp;
using NetDwhProject.Core.Interfaces;

namespace NetDwhProject.API.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SubCategoriesController : ControllerBase
{
    private readonly IUnitOfWork _unitOfWork;

    public SubCategoriesController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var subCategories = await _unitOfWork.SubCategories.GetAllAsync();
        return Ok(subCategories);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var subCategory = await _unitOfWork.SubCategories.GetByIdAsync(id);
        if (subCategory == null) return NotFound();
        return Ok(subCategory);
    }

    [HttpGet("by-category/{categoryId}")]
    public async Task<IActionResult> GetByCategory(int categoryId)
    {
        var subCategories = await _unitOfWork.SubCategories.FindAsync(sc => sc.CategoryId == categoryId);
        return Ok(subCategories);
    }

    [HttpGet("{id}/products")]
    public async Task<IActionResult> GetProducts(int id)
    {
        var products = await _unitOfWork.Products.FindAsync(p => p.SubCategoryId == id);
        return Ok(products);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(SubCategory subCategory)
    {
        // Verify category exists
        var category = await _unitOfWork.Categories.GetByIdAsync(subCategory.CategoryId);
        if (category == null) return BadRequest("Category does not exist.");

        await _unitOfWork.SubCategories.AddAsync(subCategory);
        await _unitOfWork.CompleteAsync();
        return CreatedAtAction(nameof(Get), new { id = subCategory.Id }, subCategory);
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, SubCategory subCategory)
    {
        if (id != subCategory.Id) return BadRequest();

        // Verify category exists if changed
        var category = await _unitOfWork.Categories.GetByIdAsync(subCategory.CategoryId);
        if (category == null) return BadRequest("Category does not exist.");

        _unitOfWork.SubCategories.Update(subCategory);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var subCategory = await _unitOfWork.SubCategories.GetByIdAsync(id);
        if (subCategory == null) return NotFound();

        // Check if subcategory has products
        var products = await _unitOfWork.Products.FindAsync(p => p.SubCategoryId == id);
        if (products.Any())
        {
            return BadRequest("Cannot delete subcategory with existing products.");
        }

        _unitOfWork.SubCategories.Delete(subCategory);
        await _unitOfWork.CompleteAsync();
        return NoContent();
    }
}
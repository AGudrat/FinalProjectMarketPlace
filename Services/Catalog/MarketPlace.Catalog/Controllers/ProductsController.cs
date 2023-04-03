using MarketPlace.Catalog.DTOs;
using MarketPlace.Catalog.Services;
using MarketPlace.Shared.ControllerBases;
using Microsoft.AspNetCore.Mvc;

namespace MarketPlace.Catalog.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : CustomBaseController
{
    private readonly IProductServices _productServices;
    public ProductsController(IProductServices productServices)
    {
        _productServices = productServices;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _productServices.GetAllAsync();
        return CreateActionResult(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _productServices.GetByIdAsync(id);
        return CreateActionResult(response);
    }
    [HttpGet("[action]/{userId}")]
    public async Task<IActionResult> GetAllByUserId(string userId)
    {
        var response = await _productServices.GetAllByUserId(userId);
        return CreateActionResult(response);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ProductCreateDto courceCreateDto)
    {
        var response = await _productServices.CreateAsync(courceCreateDto);
        return CreateActionResult(response);
    }
    [HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductUpdateDto courceUpdateDto)
    {
        var response = await _productServices.UpdateAsync(courceUpdateDto);
        return CreateActionResult(response);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        var response = await _productServices.Delete(id);
        return CreateActionResult(response);
    }
}

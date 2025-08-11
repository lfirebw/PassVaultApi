using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PasswordListing.Application.DTOs.Item;
using PasswordListing.Application.Interfaces;

namespace PasswordListing.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class ItemController(IItemService itemService) : ControllerBase
    {
        private readonly IItemService _itemService = itemService;
        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemService.GetAllAsync();
            return Ok(items);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetItem(string id)
        {
            var item = await _itemService.GetByIdAsync(id);
            return item != null ? 
                Ok(item) : NotFound("Item not found");
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] CreateItemRequest request)
        {
            return await _itemService.CreateAsync(request) ? 
                Ok("Request Successfully") : BadRequest("Item could not be created");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem([FromRoute] string id, [FromBody] UpdateItemRequest request)
        {
            return await _itemService.UpdateAsync(id, request) ? 
                Ok("Request Successfully") : NotFound("Item not found");
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            return await _itemService.DeleteAsync(id) ? 
                Ok("Request Successfully") : NotFound("Item not found");
        }
    }
}

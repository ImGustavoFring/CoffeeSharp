using Domain.DTOs.Menu.Requests;
using Domain.DTOs.Shared;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Logic.BusinessServices.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/menus")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;

        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("presets")]
        public async Task<IActionResult> GetAllPresets(
            [FromQuery] string? name = null,
            [FromQuery] string? description = null,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _menuService.GetAllPresetsAsync(
                name, description,
                pageIndex, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());

            var dtos = items.Select(p => new MenuPresetDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description
            });

            return Ok(dtos);
        }

        [HttpGet("presets/{id}")]
        public async Task<IActionResult> GetPresetById(long id)
        {
            MenuPreset? preset = await _menuService.GetPresetByIdAsync(id);

            if (preset == null)
            {
                return NotFound();
            }

            var presetDto = new MenuPresetDto
            {
                Id = preset.Id,
                Name = preset.Name,
                Description = preset.Description
            };

            return Ok(presetDto);
        }

        [HttpPost("presets")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreatePreset([FromBody] CreateMenuPresetRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var preset = new MenuPreset
            {
                Name = request.Name,
                Description = request.Description,
                MenuPresetItems = request.Items
                    .Select(i => new MenuPresetItem { ProductId = i.ProductId })
                    .ToList()
            };

            MenuPreset created = await _menuService.AddPresetAsync(preset);

            var presetDto = new MenuPresetDto
            {
                Id = created.Id,
                Name = created.Name,
                Description = created.Description
            };

            return CreatedAtAction(nameof(GetPresetById), new { id = presetDto.Id }, presetDto);
        }

        [HttpPut("presets/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdatePreset(long id,
            [FromBody] UpdateMenuPresetRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var preset = new MenuPreset
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description
            };

            MenuPreset updated = await _menuService.UpdatePresetAsync(preset);

            var presetDto = new MenuPresetDto
            {
                Id = updated.Id,
                Name = updated.Name,
                Description = updated.Description
            };

            return Ok(presetDto);
        }

        [HttpDelete("presets/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeletePreset(long id)
        {
            await _menuService.DeletePresetAsync(id);

            return NoContent();
        }

        [HttpGet("items")]
        public async Task<IActionResult> GetAllPresetItems(
            [FromQuery] long? menuPresetId = null,
            [FromQuery] long? productId = null,
            [FromQuery] int pageIndex = 0,
            [FromQuery] int pageSize = 50)
        {
            var (items, total) = await _menuService.GetAllPresetItemsAsync(
                menuPresetId, productId,
                pageIndex, pageSize);

            Response.Headers.Add("X-Total-Count", total.ToString());

            var dtos = items.Select(i => new MenuPresetItemDto
            {
                Id = i.Id,
                ProductId = i.ProductId,
                MenuPresetId = i.MenuPresetId
            });

            return Ok(dtos);
        }

        [HttpGet("items/{id}")]
        public async Task<IActionResult> GetPresetItemById(long id)
        {
            MenuPresetItem? item = await _menuService.GetPresetItemByIdAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            var itemDto = new MenuPresetItemDto
            {
                Id = item.Id,
                ProductId = item.ProductId,
                MenuPresetId = item.MenuPresetId
            };

            return Ok(itemDto);
        }

        [HttpPost("items")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> CreatePresetItem([FromBody] CreateMenuPresetItemRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new MenuPresetItem
            {
                ProductId = request.ProductId,
                MenuPresetId = request.MenuPresetId
            };

            MenuPresetItem created = await _menuService.AddPresetItemAsync(item);

            var itemDto = new MenuPresetItemDto
            {
                Id = created.Id,
                ProductId = created.ProductId,
                MenuPresetId = created.MenuPresetId
            };

            return CreatedAtAction(nameof(GetPresetItemById), new { id = itemDto.Id }, itemDto);
        }

        [HttpPut("items/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> UpdatePresetItem(long id,
            [FromBody] UpdateMenuPresetItemRequest request)
        {
            if (id != request.Id)
            {
                ModelState.AddModelError("Id", "URL id does not match request body id.");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = new MenuPresetItem
            {
                Id = request.Id,
                ProductId = request.ProductId,
                MenuPresetId = request.MenuPresetId
            };

            MenuPresetItem updated = await _menuService.UpdatePresetItemAsync(item);

            var itemDto = new MenuPresetItemDto
            {
                Id = updated.Id,
                ProductId = updated.ProductId,
                MenuPresetId = updated.MenuPresetId
            };

            return Ok(itemDto);
        }

        [HttpDelete("items/{id}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> DeletePresetItem(long id)
        {
            await _menuService.DeletePresetItemAsync(id);

            return NoContent();
        }
    }
}

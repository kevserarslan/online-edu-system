// OnlineEduSystem.API/Controllers/UsersController.cs
using Microsoft.AspNetCore.Mvc;
using OnlineEduSystem.Application.DTOs;
using OnlineEduSystem.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineEduSystem.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _svc;
        public UsersController(IUserService svc) => _svc = svc;

        [HttpGet]
        public async Task<ActionResult<List<UserDto>>> GetAll()
            => Ok(await _svc.GetAllAsync());

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> Get(string id)
        {
            var dto = await _svc.GetByIdAsync(id);
            return dto == null ? NotFound() : Ok(dto);
        }

        [HttpGet("instructors")]
        public async Task<ActionResult<List<UserDto>>> GetInstructors()
            => Ok(await _svc.GetInstructorsAsync());

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(CreateUserDto dto)
        {
            var created = await _svc.CreateAsync(dto);
            return CreatedAtAction(nameof(Get), new { id = created!.Id }, created);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(
            string id, UpdateUserDto dto)
        {
            var updated = await _svc.UpdateAsync(id, dto);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var deleted = await _svc.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }
    }
}

using ChatApp.Core.Interfaces.Services;
using ChatApp.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChatApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class KeysController : ControllerBase
    {
        private readonly IKeyService _keyService;

        public KeysController(IKeyService keyService)
        {
            _keyService = keyService;
        }

        // POST: api/keys/upload
        [HttpPost("upload")]
        public async Task<IActionResult> UploadKeyBundle([FromBody] KeyBundle keyBundle)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Console.WriteLine(userIdString);
            if (!Guid.TryParse(userIdString, out Guid userId))
            {
                return BadRequest("Invalid user ID.");
            }

            keyBundle.UserId = userId;
            await _keyService.StoreKeyBundleAsync(keyBundle);
            return Ok();
        }

        // GET: api/keys/{userId}
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetKeyBundle(Guid userId)
        {
            var keyBundle = await _keyService.GetKeyBundleAsync(userId);
            if (keyBundle == null)
            {
                return NotFound();
            }

            return Ok(keyBundle);
        }
    }
}
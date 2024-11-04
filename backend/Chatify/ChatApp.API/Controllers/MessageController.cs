using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ChatApp.Core.Interfaces.Services;

namespace ChatApp.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IUserService _userService;

        public MessagesController(IMessageService messageService, IUserService userService)
        {
            _messageService = messageService;
            _userService = userService;
        }

        // GET: api/Messages/Recent
        [HttpGet("Recent")]
        public async Task<IActionResult> GetRecentMessages()
        {
            var user = HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdString != null)
                {
                    Guid userId = Guid.Parse(userIdString);
                    var userInfo = await _userService.GetUser(userId).ConfigureAwait(false);
                    var conversations = await _messageService.GetRecentConversationsAsync(userInfo.Id);
                    return Ok(conversations);
                }
            }
            return BadRequest();
        }

        // GET: api/Messages/Conversation/{contactId}
        [HttpGet("Conversation/{contactId}")]
        public async Task<IActionResult> GetConversation(Guid contactId)
        {
            var user = HttpContext.User;
            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userIdString = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userIdString != null)
                { 
                    var userInfo = await _userService.GetUser(ClaimTypes.NameIdentifier).ConfigureAwait(false);
                    var messages = await _messageService.GetConversationMessagesAsync(userInfo.Id, contactId);
                    return Ok(messages);
                }
            }
            return BadRequest();
        }
    }
}
using HRManagementSystem.BL.DTOs.ChatDTO;
using HRManagementSystem.BL.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRManagementSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatbotController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatbotController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] chatDto chat)
        {
            var reply = await _chatbotService.GetChatReplyAsync(chat);
            return Ok(new { reply });
        }
    }
}

using HRManagementSystem.BL.DTOs.ChatDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRManagementSystem.BL.Interfaces
{
    public interface IChatbotService
    {
        Task<string> GetChatReplyAsync(chatDto chat);
    }
}

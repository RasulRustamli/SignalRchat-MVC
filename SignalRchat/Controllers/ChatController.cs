using System;
using System.Threading.Tasks;
using Database;
using Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRchat.Models;

namespace Controllers
{
    [Authorize]
    [Route("[controller]")]
    public class ChatController : Controller
    {
        private readonly IHubContext<ChatHub> _chat;
        public ChatController(IHubContext<ChatHub> chat)
        {
            _chat = chat;
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string roomId)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, roomId);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{roomId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string roomId)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, roomId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(string message, int chatId, [FromServices] AppDbContext context)
        {
            var Message = new Message
            {
                ChatId = chatId,
                Text = message,
                Name = User.Identity.Name,
                Timestamp = DateTime.Now
            };

            context.Messages.Add(Message);

            await context.SaveChangesAsync();

            await _chat.Clients.Group(chatId.ToString())
                .SendAsync("ReceiveMessage", new {
                    Text = Message.Text,
                    Name = Message.Name,
                    Timestamp = Message.Timestamp.ToString("dd/MM/yyyy HH:mm:ss")
                });

            return Ok();
        }
    }
}
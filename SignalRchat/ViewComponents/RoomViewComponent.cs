using System.Linq;
using System.Security.Claims;
using SignalRchat.Models;
using Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ViewComponents
{
    public class RoomViewComponent : ViewComponent
    {
        private AppDbContext _context;

        public RoomViewComponent(AppDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var chats = _context.ChatUsers.Include(x => x.Chat)
                                          .Where(x => x.UserId == userId && x.Chat.Type == ChatType.Room)
                                          .Select(x => x.Chat)
                                          .ToList();
            return View(chats);
        }
    }
}
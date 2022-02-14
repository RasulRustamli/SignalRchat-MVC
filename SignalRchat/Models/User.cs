using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Models;

namespace SignalRchat.Models
{
    public class User : IdentityUser
    {
        public ICollection<ChatUser> Chats { get; set; }
    }
}
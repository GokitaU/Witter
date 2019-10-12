using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public float Score { get; set; } = 0;
        public bool IsAdmin { get; set; } = false;
        public bool PermanentBan { get; set; } = false;
        public DateTime? Ban { get; set; } = null;

    }
}

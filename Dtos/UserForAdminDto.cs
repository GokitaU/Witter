using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Dtos
{
    public class UserForAdminDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public float Score { get; set; }
        public bool IsAdmin { get; set; }
        public bool PermanentBan { get; set; }
        public DateTime? Ban { get; set; }
    }
}

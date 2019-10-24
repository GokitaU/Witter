using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Dtos
{
    public class UserForBanDto
    {
        public int Id { get; set; }
        public bool PermanentBan { get; set; }
        public DateTime? Ban { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Models
{
    public class UserInLeague
    {
        public int UserId { get; set; }
        public int LeagueId { get; set; }
        public User User { get; set; }
        public League League { get; set; }
    }
}

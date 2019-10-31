using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Dtos
{
    public class LeagueForListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Prize { get; set; }
        public UserForReturnDto Admin { get; set; }
        public int UserCount { get; set; }
        public int Position { get; set; }
    }
}

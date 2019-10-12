using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Dtos
{
    public class TeamForDetailedDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Coach { get; set; }
        public string PhotoUrl { get; set; }
        public IEnumerable<Match> Matches { get; set; }
    }
}

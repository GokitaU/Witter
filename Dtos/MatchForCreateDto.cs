using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Dtos
{
    public class MatchForCreateDto
    {
        public int TeamAId { get; set; }
        public int TeamBId { get; set; }
        public DateTime Date { get; set; }
        public float TeamAOdds { get; set; }
        public float TeamBOdds { get; set; }
        public float DrawOdds { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Witter.Models;

namespace Witter.Dtos
{
    public class BetForClientDto
    {
        public int Id { get; set; }
        public Match Match { get; set; }
        public int UserId { get; set; }
        public int Prediction { get; set; }
        public float Odds { get; set; }
    }
}

using System;

namespace Witter.Models
{
    public class Match
    {
        public int Id { get; set; }
        public Team TeamA { get; set; }
        public Team TeamB { get; set; }
        public DateTime Date { get; set; }
        public float TeamAOdds { get; set; }
        public float TeamBOdds { get; set; }
        public float DrawOdds { get; set; }
        public Score Score { get; set; } = null; 
    }
}
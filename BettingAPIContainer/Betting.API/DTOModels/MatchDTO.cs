using System;
using System.Collections.Generic;

namespace Betting.API.DTOModels
{
    public class MatchDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime MatchDate { get; set; }
        public DateTime MatchTime { get; set; }
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public string Sport { get; set; } = "Football";
        public List<MatchOddDTO>? MatchOdds { get; set; } = new List<MatchOddDTO>();
    }
}

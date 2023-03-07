using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace Betting.Domain
{
    public class Match : BaseEntity
    {
        public string Description { get; set; }
        public DateTime MatchDate { get; set; }
        public DateTime MatchTime { get; set; }//Na to kanw Timespan
        public string TeamA { get; set; }
        public string TeamB { get; set; }
        public SportValues Sport { get; set; }
        public List<MatchOdds>? MatchOdds { get; set; } = new List<MatchOdds>();
        public enum SportValues
        {
            Football = 1,
            Basketball
        }
    }
}

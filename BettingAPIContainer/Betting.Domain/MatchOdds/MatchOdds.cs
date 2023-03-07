using System.Text.Json.Serialization;

namespace Betting.Domain
{
    public class MatchOdds : BaseEntity
    {
        public int MatchId { get; set; }
        public string Specifier { get; set; }
        public decimal Odd { get; set; }
    }
}

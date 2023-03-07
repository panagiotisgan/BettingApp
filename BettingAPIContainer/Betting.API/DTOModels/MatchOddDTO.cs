namespace Betting.API.DTOModels
{
    public class MatchOddDTO
    {
        public int Id { get; set; } 
        public int MatchId { get; set; }
        public string Specifier { get; set; }
        public decimal Odd { get; set; }
    }
}

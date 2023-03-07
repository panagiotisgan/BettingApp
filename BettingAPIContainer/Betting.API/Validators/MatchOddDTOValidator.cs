using Betting.API.DTOModels;
using FluentValidation;

namespace Betting.API.Validators
{
    public class MatchOddDTOValidator : AbstractValidator<MatchOddDTO>
    {
        public MatchOddDTOValidator()
        {
            RuleFor(x => x.MatchId).GreaterThanOrEqualTo(1);
        }
    }
}

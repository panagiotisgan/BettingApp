using Betting.API.DTOModels;
using FluentValidation;
using System;

namespace Betting.API.Validators
{
    public class MatchDTOValidator : AbstractValidator<MatchDTO>
    {
        public MatchDTOValidator()
        {
            RuleFor(x => x.TeamA).NotEmpty().WithMessage("The Home(TeamA) cannot be empty.");
            RuleFor(x => x.TeamB).NotEmpty().WithMessage("The Away(TeamB) cannot be empty.");
            RuleFor(x => x.MatchDate).GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("The match cannot be be in the past tense.");
        }
    }
}

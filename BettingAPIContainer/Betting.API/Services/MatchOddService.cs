using Betting.API.DTOModels;
using Betting.Domain;
using FluentValidation;
using FluentValidation.Results;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betting.API.Services
{
    public class MatchOddService : IMatchOddService
    {
        private readonly IMatchOddsRepository _matchOddsRepository;
        private IValidator<MatchOddDTO> _matchOddsValidator;
        public MatchOddService(IMatchOddsRepository matchOddsRepository, IValidator<MatchOddDTO> matchOddsValidator)
        {
            _matchOddsRepository = matchOddsRepository;
            _matchOddsValidator = matchOddsValidator;
        }

        public async Task<ResponseDTO<List<MatchOdds>>> CreateOdd(List<MatchOddDTO> matchOddDTO)
        {
            List<ValidationResult> results = new List<ValidationResult>();
            foreach (var item in matchOddDTO)
            {
                results.Add(_matchOddsValidator.Validate(item));
            }

            if (results.Any(x => !x.IsValid))
            {
                ResponseDTO<List<MatchOdds>> responseDTO = new ResponseDTO<List<MatchOdds>>();
                responseDTO.ErrorMessages = results.SelectMany(x=>x.Errors.Where(x=>x.ErrorMessage !=null))
                        .Select(x => x.ErrorMessage).ToList();
                return responseDTO;
            }

            List<MatchOdds> matchOdds = new List<MatchOdds>();
            matchOddDTO.ForEach(x => 
                        matchOdds.Add(new MatchOdds
                        {
                             MatchId = x.MatchId,
                             Odd = x.Odd,
                             Specifier = x.Specifier 
                        }));

            await _matchOddsRepository.AddRangeAsync(matchOdds);
            await _matchOddsRepository.SaveAsync();

            return new ResponseDTO<List<MatchOdds>>
            {
                Message = "Match odd create successfully",
                Data = matchOdds,
                ErrorMessages = null
            };
        }

        public async Task<ResponseDTO<List<MatchOdds>>> GetOdds(int matchId)
        {
            var matchOdds = await _matchOddsRepository.FindAllByExpressionAsync((md) => md.MatchId == matchId);
            return new ResponseDTO<List<MatchOdds>>
            {
                Data = matchOdds
            };
        }

        public async Task<ResponseDTO<MatchOdds>> RemoveMatchOdd(int matchOddId)
        {
            MatchOdds matchOdds = await _matchOddsRepository.GetByIdAsync(matchOddId);
            if (matchOdds == null)
                return new ResponseDTO<MatchOdds> { ErrorMessages = new List<string> { "The entity you want to update doesn't exist." } };

            await _matchOddsRepository.DeleteAsync(matchOdds);
            await _matchOddsRepository.SaveAsync();

            return new ResponseDTO<MatchOdds>
            {
                Data = matchOdds,
                Message = "Match odd deleted successfully."
            };
        }

        public async Task<ResponseDTO<MatchOdds>> UpdateOdd(MatchOddDTO matchOdd)
        {

            MatchOdds dbEntity = await _matchOddsRepository.GetByIdAsync(matchOdd.Id);

            if (dbEntity == null)
                return new ResponseDTO<MatchOdds>
                {
                    Data = null,
                    Message = null,
                    ErrorMessages = new List<string>() { "The entity you try to update does not exist" }
                };

            dbEntity.Specifier = matchOdd.Specifier;
            dbEntity.Odd = matchOdd.Odd;
            dbEntity.MatchId = matchOdd.MatchId != 0 ? matchOdd.MatchId : dbEntity.MatchId;

            await _matchOddsRepository.SaveAsync();

            return new ResponseDTO<MatchOdds>
            { 
                Data = dbEntity,
                Message = "Entity updated successfully"
            };
        }
    }

    public interface IMatchOddService
    {
        Task<ResponseDTO<List<MatchOdds>>> CreateOdd(List<MatchOddDTO> matchOdd);
        Task<ResponseDTO<List<MatchOdds>>> GetOdds(int matchId);
        Task<ResponseDTO<MatchOdds>> UpdateOdd(MatchOddDTO matchOdd);
        Task<ResponseDTO<MatchOdds>> RemoveMatchOdd(int matchOddId);
    }
}

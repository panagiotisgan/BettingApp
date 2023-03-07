using Betting.API.DTOModels;
using Betting.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using static Betting.Domain.Match;
using System.Collections.Generic;
using FluentValidation;
using FluentValidation.Results;


namespace Betting.API.Services
{
    public class MatchService : IMatchService
    {
        private readonly IMatchRepository _matchRepository;
        private readonly IMatchOddsRepository _matchOddsRepository;
        private IValidator<MatchDTO> _matchDTOValidator;
        public MatchService(IMatchRepository matchRepository, IValidator<MatchDTO> matchDTOValidator, IMatchOddsRepository matchOddsRepository)
        {
            _matchRepository = matchRepository;
            _matchDTOValidator = matchDTOValidator;
            _matchOddsRepository = matchOddsRepository;
        }

        public async Task<ResponseDTO<Match>> CreateMatch(MatchDTO matchDTO)
        {
            ValidationResult validationResult = _matchDTOValidator.Validate(matchDTO);
            if (validationResult.IsValid)
            {
                List<MatchOdds> matchOdds = new List<MatchOdds>();
                if (matchDTO.MatchOdds.Any())
                {
                    foreach (var matchOdd in matchDTO.MatchOdds)
                    {
                        var _matchOdd = new MatchOdds
                        {
                            Odd = matchOdd.Odd,
                            Specifier = matchOdd.Specifier
                        };

                        matchOdds.Add(_matchOdd);
                    }
                }

                Match matchDB = new Match
                {
                    Description = matchDTO.Description,
                    MatchDate = matchDTO.MatchDate,
                    MatchTime = matchDTO.MatchTime,
                    TeamA = matchDTO.TeamA,
                    TeamB = matchDTO.TeamB,
                    Sport = (SportValues)Enum.Parse(typeof(SportValues), matchDTO.Sport),
                    MatchOdds = matchOdds
                };

                await _matchRepository.CreateAsync(matchDB);
                await _matchRepository.SaveAsync();

                return new ResponseDTO<Match>
                {
                    Data = matchDB,
                    ErrorMessages = null,
                    Message = "Match created successfully"
                };
            }

            return new ResponseDTO<Match>
            {
                Data = null,
                ErrorMessages = validationResult.Errors
                .Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage))
                .Select(x => x.ErrorMessage).ToList()
            };
        }

        public async Task<Match> GetMatch(int id)
        {
            return await _matchRepository.FindByExpressionAsync((x) => x.Id == id, new string[] { "MatchOdds" });
        }

        public async Task<ResponseDTO<Match>> RemoveMatch(int id)
        {
            Match entity = await _matchRepository.GetByIdAsync(id);
            if (entity == null)
                return new ResponseDTO<Match>
                {
                    Data = null,
                    ErrorMessages = new List<string>() { "The entity does not exist" }
                };

            await _matchRepository.DeleteAsync(entity);
            await _matchRepository.SaveAsync();

            return new ResponseDTO<Match>
            {
                Data = entity,
                Message = "Match removed successfully",
                ErrorMessages = null
            };

        }

        public async Task<ResponseDTO<Match>> UpdateMatch(MatchDTO matchDTO)
        {
            ValidationResult validationResult = _matchDTOValidator.Validate(matchDTO);
            ResponseDTO<Match> responseDTO = new ResponseDTO<Match>();
            if (!validationResult.IsValid)
            {
                responseDTO.ErrorMessages = validationResult.Errors
                    .Where(x => !string.IsNullOrWhiteSpace(x.ErrorMessage))
                    .Select(x => x.ErrorMessage).ToList();
                responseDTO.Data = null;
                return responseDTO;
            }

            Match dbEntity = await _matchRepository.FindByExpressionAsync((x) => x.Id == matchDTO.Id, new string[] { "MatchOdds" }); ;

            if (dbEntity == null)
            {
                responseDTO.Data = null;
                responseDTO.ErrorMessages.Add("The entity you want to update doesn't exist.");
                return responseDTO;
            }

            dbEntity.Description = string.IsNullOrWhiteSpace(matchDTO.Description) ? dbEntity.Description: matchDTO.Description;
            dbEntity.MatchDate = matchDTO.MatchDate;
            dbEntity.MatchTime = matchDTO.MatchTime;
            dbEntity.Sport = (SportValues)Enum.Parse(typeof(SportValues), matchDTO.Sport);
            dbEntity.TeamA = matchDTO.TeamA;
            dbEntity.TeamB = matchDTO.TeamB;
            dbEntity.MatchOdds = dbEntity.MatchOdds == null ? null : dbEntity.MatchOdds;

            if (dbEntity.MatchOdds != null)
                await _matchOddsRepository.SaveAsync();

            await _matchRepository.SaveAsync();

            responseDTO.Data = dbEntity;
            responseDTO.ErrorMessages = null;
            responseDTO.Message = "The entity updated successfully";

            return responseDTO;
        }
    }

    public interface IMatchService
    {
        Task<ResponseDTO<Match>> CreateMatch(MatchDTO matchDTO);
        Task<Match> GetMatch(int id);
        Task<ResponseDTO<Match>> UpdateMatch(MatchDTO matchDTO);
        Task<ResponseDTO<Match>> RemoveMatch(int id);
    }
}

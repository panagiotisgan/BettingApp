using Betting.API.DTOModels;
using Betting.API.Services;
using Betting.Domain;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Betting.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly IMatchOddService _matchOddService;
        private ILogger<MatchesController> _logger;
        public MatchesController(
            ILogger<MatchesController> logger,
            IMatchService matchService,
            IMatchOddService matchOddService)
        {
            _logger = logger;
            _matchService = matchService;
            _matchOddService = matchOddService;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post([FromBody] MatchDTO match)
        {
            ResponseDTO<Match> response = new ResponseDTO<Match>();
            try
            {
                response = await _matchService.CreateMatch(match);

                if (response.ErrorMessages != null)
                    return NoContent();
                

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured with message: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                Match result = await _matchService.GetMatch(id);
                if (result is null)
                    return NoContent();

                return Ok(new ResponseDTO<Match>
                {
                    Data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while try to get Match with message: {ex.Message}");
                return BadRequest();
            }

        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromBody] MatchDTO match)
        {
            try
            {
                var response = await _matchService.UpdateMatch(match);

                if (response.ErrorMessages != null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while attempting to update a match with message: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var response = await _matchService.RemoveMatch(id);
                if(response.ErrorMessages != null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while attempting to delete a match with message: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("matchOdds")]
        public async Task<IActionResult> Post([FromBody] List<MatchOddDTO> matchOddsDTO)
        {
            try
            {
                var response = await _matchOddService.CreateOdd(matchOddsDTO);
                if (response.ErrorMessages != null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while try to create match odd with message: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("MatchOdds/{matchId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetMatchOdds(int matchId)
        {
            try
            {

                var response  = await _matchOddService.GetOdds(matchId);
                if (response is null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured after try to get match odds with message: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("MatchOdds")]
        public async Task<IActionResult> UpdateMatchOdds([FromBody] MatchOddDTO matchOddDTO)
        {
            try
            {
                var response = await _matchOddService.UpdateOdd(matchOddDTO);

                if (response.ErrorMessages != null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured with message: {ex.Message}");
                return BadRequest();
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Route("matchOdds/{id}")]
        public async Task<IActionResult> DeleteOdd(int id)
        {
            try
            {
                var response = await _matchOddService.RemoveMatchOdd(id);

                if(response.ErrorMessages != null)
                    return NoContent();

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while try to delete match odd with message: {ex.Message}");
                return BadRequest();
            }
        }
    }
}

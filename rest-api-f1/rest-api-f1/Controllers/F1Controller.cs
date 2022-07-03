using Microsoft.AspNetCore.Mvc;
using rest_api_f1.Logic;
using rest_api_f1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace rest_api_f1.Controllers
{
    [Route("api/[controller]/{comparisonName}")]
    [ApiController]
    public class F1Controller : ControllerBase
    {
        private readonly ComparisonsRepository comparisonsRepository = new();

        [HttpGet("rank")]
        public IActionResult GetRank(string comparisonName, [FromQuery] IEnumerable<string> competitors)
        {
            var ranking = comparisonsRepository.Rank(comparisonName, competitors);
            return Ok(ranking);
        }

        [HttpGet("occurances")]
        public IActionResult GetCompetitors(string comparisonName)
        {
            var occurances = comparisonsRepository.GetCompetitorsOccurances(comparisonName);
            return Ok(occurances);
        }

        [HttpPost("")]
        public IActionResult Post(string comparisonName)
        {
            if (comparisonsRepository.TryAddComparison(comparisonName))
            {
                return Ok("New compariosn created");
            }
            else
            {
                return BadRequest("Comparison with such a name already exists");
            }
        }
        
        [HttpPut("race")]
        public async Task<IActionResult> PutRaceResult(string comparisonName, [FromBody] CompetitionInfo competitionInfo)
        {
            await comparisonsRepository.TryAddCompetiton(comparisonName, competitionInfo, CompetitionType.Results);
            return Ok();

        }

        [HttpPut("qualifying")]
        public async Task<IActionResult> PutRaceQualifying(string comparisonName, [FromBody] CompetitionInfo competitionInfo)
        {
            await comparisonsRepository.TryAddCompetiton(comparisonName, competitionInfo, CompetitionType.Qualifying);
            return Ok();
        }

        [HttpPut("sesonResult")]
        public async Task<IActionResult> PutSesonResult(string comparisonName, [FromBody] CompetitionSeasonInfo competitionSeasonInfo)
        {
            var competitionInfo = new CompetitionInfo { Priority = competitionSeasonInfo.Priority, Season = competitionSeasonInfo.Season };
            await comparisonsRepository.TryAddCompetiton(comparisonName, competitionInfo, CompetitionType.DriverStanding);
            return Ok();
        }

        [HttpDelete("")]
        public void Delete(string comparisonName)
        {
            comparisonsRepository.DeleteComparison(comparisonName);
        }
    }
}

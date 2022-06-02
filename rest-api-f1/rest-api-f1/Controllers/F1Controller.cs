using Microsoft.AspNetCore.Mvc;
using rest_api_f1.Logic;
using rest_api_f1.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace rest_api_f1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class F1Controller : ControllerBase
    {
        private readonly ComparisonsRepository comparisonsRepository = new();

        [HttpGet("Rank/{comparisonName}")]
        public IActionResult GetRank(string comparisonName, [FromQuery] IEnumerable<string> competitors)
        {
            if (comparisonsRepository.Contains(comparisonName))
            {
                var ranking = comparisonsRepository.Rank(comparisonName, competitors);
                return Ok(ranking);
            }
            else
            {
                return NotFound("Comparison with such a name don't exist");
            }
        }

        [HttpGet("Occurances/{comparisonName}")]
        public IActionResult GetCompetitors(string comparisonName)
        {
            if (comparisonsRepository.Contains(comparisonName))
            {
                var occurances = comparisonsRepository.GetCompetitorsOccurances(comparisonName);
                return Ok(occurances);
            }
            else
            {
                return NotFound("Comparison with such a name don't exist");
            }
        }

        [HttpPost("{comparisonName}")]
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
        
        [HttpPut("AddCompetiton/RaceResult/{comparisonName}")]
        public async Task<IActionResult> PutRaceResult(string comparisonName, [FromBody] CompetitionInfo competitionInfo)
        {
            if (comparisonsRepository.Contains(comparisonName))
            {
                var created = await comparisonsRepository.TryAddCompetiton(comparisonName, competitionInfo, "results");
                return Ok();
            }
            else
            {
                return NotFound("Comparison with such a name don't exist");
            }
            
        }

        [HttpPut("AddCompetiton/RaceQualifying/{comparisonName}")]
        public async Task<IActionResult> PutRaceQualifying(string comparisonName, [FromBody] CompetitionInfo competitionInfo)
        {
            if (comparisonsRepository.Contains(comparisonName))
            {
                var created = await comparisonsRepository.TryAddCompetiton(comparisonName, competitionInfo, "qualifying");
                return Ok();
            }
            else
            {
                return NotFound("Comparison don't exist or competition don't exist");
            }
        }

        [HttpPut("AddCompetiton/SesonResult/{comparisonName}")]
        public async Task<IActionResult> PutSesonResult(string comparisonName, [FromBody] CompetitionSeasonInfo competitionSeasonInfo)
        {
            if (comparisonsRepository.Contains(comparisonName))
            {
                var competitionInfo = new CompetitionInfo { Priority = competitionSeasonInfo.Priority, Season = competitionSeasonInfo.Season };
                var created = await comparisonsRepository.TryAddCompetiton(comparisonName, competitionInfo, "driverStandings");
                return Ok();
            }
            else
            {
                return NotFound("Comparison don't exist or competition don't exist");
            }
        }

        [HttpDelete("{comparisonName}")]
        public void Delete(string comparisonName)
        {
            comparisonsRepository.DeleteComparison(comparisonName);
        }
    }
}

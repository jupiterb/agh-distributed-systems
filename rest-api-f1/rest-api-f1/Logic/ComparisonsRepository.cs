using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using rest_api_f1.Models;
using rest_api_f1.Exceptions;

namespace rest_api_f1.Logic
{
    public class ComparisonsRepository
    {
        private static List<string> comaprisonNames = new List<string>();

        private static Dictionary<string, List<Competition>> competitions = new Dictionary<string, List<Competition>>();

        public bool TryAddComparison(string comparisonName)
        {
            if (!comaprisonNames.Contains(comparisonName))
            {
                comaprisonNames.Add(comparisonName);

                competitions.Add(comparisonName, new List<Competition>());

                return true;
            }
            else
            {
                return false;
            }
        }

        public void DeleteComparison(string comparisonName)
        {
            comaprisonNames.Remove(comparisonName);

            competitions.Remove(comparisonName);
        }

        public async Task<bool> TryAddCompetiton(string comparisonName, CompetitionInfo competitionInfo, string details)
        {
            AssertComparisonExists(comparisonName);

            var competition = competitions[comparisonName]
                .FirstOrDefault(c => c.Season == competitionInfo.Season && c.Round == competitionInfo.Round && c.Details == details);

            if (competition == null)
            {
                var allCompetitons = new List<Competition>();
                competitions.Values.ToList().ForEach(criteriaList => criteriaList.ForEach(allCompetitons.Add));

                var newCompetition = new Competition(competitionInfo.Priority, competitionInfo.Season, competitionInfo.Round, details);
                await newCompetition.GetResults(allCompetitons);

                if (newCompetition.HasResults)
                {
                    competitions[comparisonName].Add(newCompetition);
                    return true;
                }
            }
            else
            {
                competition.Priority = competitionInfo.Priority;
            }
            return false;
        }

        public Dictionary<string, int> GetCompetitorsOccurances(string comparisonName)
        {
            AssertComparisonExists(comparisonName);

            Dictionary<string, int> competitorsOccurances = new();

            competitions[comparisonName]
                .ForEach(c => c.Results.Keys.ToList()
                .ForEach(name => CountCompetitor(name, competitorsOccurances)));

            return competitorsOccurances;
        }

        public ResultTable Rank(string comparisonName, IEnumerable<string> competitors)
        {
            AssertComparisonExists(comparisonName);
            return Ranker.Rank(competitions[comparisonName], competitors);
        }

        private static void AssertComparisonExists(string comparisonName)
        {
            if (!comaprisonNames.Contains(comparisonName))
            {
                throw new ComparisonNotExistException(comparisonName);
            }
        }

        private static void CountCompetitor(string name, Dictionary<string, int> competitorsOccurances)
        {
            if (competitorsOccurances.ContainsKey(name))
            {
                competitorsOccurances[name]++;
            }
            else
            {
                competitorsOccurances.Add(name, 1);
            }
        }
    }
}
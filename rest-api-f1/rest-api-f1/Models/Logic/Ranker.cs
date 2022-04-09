using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rest_api_f1.Models.Logic
{
    public class Ranker
    {
        public static ResultTable Rank(List<Competition> competitions, IEnumerable<string> competitors)
        {
            var results = new ResultTable();
            foreach(var competitor in competitors)
            {
                results.Table.Add(competitor, new());

                double? weightedPositions = 0;
                double weights = 0;

                foreach(var competition in competitions)
                {
                    float? position = competition.Results.FirstOrDefault(pair => pair.Key == competitor).Value;
                    results.Table[competitor].Add(competition.ToString(), position);

                    if (position == null)
                    {
                        weightedPositions = null;
                    }
                    if (weightedPositions != null)
                    {
                        weightedPositions += competition.Priority * Math.Log((double)position);
                        weights += competition.Priority;
                    }
                }

                float? ranking = weightedPositions != null? (float)Math.Exp((double)weightedPositions / weights) : null;

                results.Table[competitor].Add("ranking", ranking);
            }
            return results;
        }
    }
}

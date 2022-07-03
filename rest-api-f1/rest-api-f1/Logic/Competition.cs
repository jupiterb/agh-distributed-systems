using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace rest_api_f1.Logic
{
    public class Competition
    {
        public int Season { get; private set; }

        public int? Round { get; private set; }

        public CompetitionType Type { get; private set; }

        public int Priority { get; set; }

        public Dictionary<string, float> Results { get; private set; } = new();

        public bool HasResults { get; set; }

        private static readonly HttpClient client = new HttpClient();

        public Competition(int priority, int season, int? round, CompetitionType type)
        {
            Priority = priority;
            Type = type;
            Season = season;
            Round = round;
        }

        public async Task GetResults(IEnumerable<Competition> others)
        {
            if (!TryCopyResultsFromOthers(others))
            {
                await RequestResults();
            }
        }

        private bool TryCopyResultsFromOthers(IEnumerable<Competition> others)
        {
            foreach (var other in others)
            {
                if (other.HasResults && other.Equals(this))
                {
                    Results = other.Results;
                    HasResults = true;
                    return true;
                }
            }
            return false;
        }

        private async Task RequestResults()
        {
            static async Task Main(Competition competition)
            {
                HttpResponseMessage response;

                string resultName = string.Empty;
                string resultEndpoint = competition.Type.ToString().ToLower();

                if (competition.Round != null)
                {
                    if (competition.Type == CompetitionType.Results)
                    {
                        resultName = "Result";
                    }
                    else if (competition.Type == CompetitionType.Qualifying)
                    {
                        resultName = "QualifyingResult";
                    }
                    response = await client.GetAsync($"https://ergast.com/api/f1/{competition.Season}/{competition.Round}/{resultEndpoint}");
                }
                else
                {
                    resultName = competition.Type.ToString();
                    response = await client.GetAsync($"https://ergast.com/api/f1/{competition.Season}/{resultEndpoint}");
                }

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();

                XDocument doc = XDocument.Parse(responseBody);
                foreach (XElement element in doc.Descendants().Where(d => d.Name.LocalName == resultName))
                {
                    float position = (float)element.Attribute("position");
                    string competitor = (string)element.Descendants().First(d => d.Name.LocalName == "FamilyName");

                    competition.Results.Add(competitor, position);
                }
                competition.HasResults = true;
            }

            await Main(this);
        }

        public bool Equals(int season, int? round, CompetitionType type)
        {
            return Season == season && Round == round && Type == type;
        }

        public bool Equals(Competition other)
        {
            return Equals(other.Season, other.Round, other.Type);
        }

        public override string ToString()
        {
            return $"{Season}{(Round == null ? string.Empty : "/" + Round)}/{Type}";
        }
    }
}
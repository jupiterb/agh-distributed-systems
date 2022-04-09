using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rest_api_f1.Models
{
    public class CompetitionInfo
    {
        public int Priority { get; set; }

        public int Season { get; set; }

        public int? Round { get; set; }
    }
}

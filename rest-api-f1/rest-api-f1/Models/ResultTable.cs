using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace rest_api_f1.Models
{
    public class ResultTable
    {
        public Dictionary<string, Dictionary<string, float?>> Table { get; set; } = new();
    }
}
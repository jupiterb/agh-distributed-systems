using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace rest_api_f1.Exceptions
{
    public class ComparisonNotExistException : Exception
    {
        public ComparisonNotExistException(string comparisonName) : base($"Comparison {comparisonName} don't exist")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsPerformancePredictionTool_CW2.Models
{
    public class PredictionModel
    {
        public string Subject { get; set; }
        public double PredictedGrade { get; set; }
        public double HoursToPass { get; set; }
        public double HoursToDistinction { get; set; }
    }
}
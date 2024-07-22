using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsPerformancePredictionTool_CW2.Models
{
    public class BreakSessionModel
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
    }
}
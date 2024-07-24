using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentsPerformancePredictionTool_CW2.Models
{
    public class ReportModel
    {
        public DateTime Date { get; set; }
        public double StudyHours { get; set; }
        public int StudySessions { get; set; }
        public double BreakHours { get; set; }
        public int BreakSessions { get; set; }
    }
}
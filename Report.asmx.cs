using StudentsPerformancePredictionTool_CW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace StudentsPerformancePredictionTool_CW2
{
    /// <summary>
    /// Summary description for Report
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Report : System.Web.Services.WebService
    {

        [WebMethod]
        public List<ReportModel> GenerateReport(string userName, DateTime startDate, DateTime endDate)
        {
            // Get study sessions
            var studyService = new StudySession();
            var studySessions = studyService.GetStudySessions(userName).ToList();


            // Get break sessions
            var breakService = new BreakSession();
            var breakSessions = breakService.GetBreakSessions(userName).ToList();


            // Group and aggregate data by date
            var reportData = from date in EachDay(startDate, endDate)
                             let studyData = studySessions.Where(s => s.Date.Date == date.Date)
                             let breakData = breakSessions.Where(b => b.Date.Date == date.Date)
                             select new ReportModel
                             {
                                 Date = date,
                                 StudyHours = studyData.Sum(s => s.Hours),
                                 StudySessions = studyData.Count(),
                                 BreakHours = breakData.Sum(b => b.Hours),
                                 BreakSessions = breakData.Count()
                             };

            var reportList = reportData.ToList();

            return reportList;
        }

        private IEnumerable<DateTime> EachDay(DateTime from, DateTime to)
        {
            for (var day = from.Date; day.Date <= to.Date; day = day.AddDays(1))
                yield return day;
        }
    }
}

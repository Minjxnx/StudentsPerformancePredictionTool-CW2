using StudentsPerformancePredictionTool_CW2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace StudentsPerformancePredictionTool_CW2
{
    /// <summary>
    /// Summary description for Prediction
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Prediction : System.Web.Services.WebService
    {

        [WebMethod]
        public List<PredictionModel> PredictGrades(String user)
        {
            var predictedGrades = new Dictionary<string, double>();

            var studyService = new StudySession();

            var studySessions = new List<StudySessionModel>();

            studySessions = studyService.GetStudySessions(user).ToList();

            foreach (var session in studySessions)
            {
                if (!predictedGrades.ContainsKey(session.Subject))
                    predictedGrades[session.Subject] = 0;

                // Assuming each hour contributes 0.1 to the grade with diminishing returns after 10 hours
                if (session.Hours <= 10)
                {
                    predictedGrades[session.Subject] += session.Hours * 0.1;
                }
                else
                {
                    predictedGrades[session.Subject] += 10 * 0.1 + (session.Hours - 10) * 0.05;
                }
            }

            // Cap grades at 100
            foreach (var subject in predictedGrades.Keys.ToList())
            {
                predictedGrades[subject] = Math.Min(predictedGrades[subject], 100);
            }

            var predictions = new List<PredictionModel>();
            foreach (var subject in predictedGrades.Keys)
            {
                double predictedGrade = predictedGrades[subject];
                double hoursToPass = CalculateRequiredHours(predictedGrade, 50);
                double hoursToDistinction = CalculateRequiredHours(predictedGrade, 75);

                predictions.Add(new PredictionModel
                {
                    Subject = subject,
                    PredictedGrade = predictedGrade,
                    HoursToPass = hoursToPass,
                    HoursToDistinction = hoursToDistinction
                });
            }

            return predictions;
        }

        private double CalculateRequiredHours(double currentGrade, double targetGrade)
        {
            if (currentGrade >= targetGrade)
            {
                return 0;
            }

            double additionalGradeNeeded = targetGrade - currentGrade;
            double additionalHours;

            if (currentGrade < 10)
            {
                additionalHours = additionalGradeNeeded / 0.1;
            }
            else
            {
                additionalHours = (10 - currentGrade) / 0.1;
                additionalGradeNeeded -= (10 - currentGrade);
                additionalHours += additionalGradeNeeded / 0.05;
            }

            return additionalHours;
        }
    }
}

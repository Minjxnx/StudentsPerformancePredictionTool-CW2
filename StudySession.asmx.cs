using StudentsPerformancePredictionTool_CW2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Xml.Linq;

namespace StudentsPerformancePredictionTool_CW2
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class StudySession : System.Web.Services.WebService
    {

        public string GetXmlFilePath(String userName)
        {
            return Server.MapPath($"~/App_Data/User_{userName}_Sessions.xml");
        }

        [WebMethod]
        public List<StudySessionModel> GetStudySessions(String userName)
        {
            var filePath = GetXmlFilePath(userName);
            return GetStudySessionsFromPath(filePath);
        }

        public List<StudySessionModel> GetStudySessionsFromPath(String filePath)
        {
            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                return doc.Root.Elements("StudySession").Select(e => new StudySessionModel
                {
                    Id = (int)e.Attribute("Id"),
                    Date = DateTime.Parse(e.Element("Date").Value),
                    Hours = double.Parse(e.Element("Hours").Value),
                    Subject = e.Element("Subject").Value
                }).ToList();
            }
            return new List<StudySessionModel>();
        }
        [WebMethod]
        public StudySessionModel GetStudySessionById(String userName, int sessionId)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var element = doc.Root.Elements("StudySession").FirstOrDefault(e => (int)e.Attribute("Id") == sessionId);
                if (element != null)
                {
                    return new StudySessionModel
                    {
                        Id = (int)element.Attribute("Id"),
                        Date = DateTime.Parse(element.Element("Date").Value),
                        Hours = double.Parse(element.Element("Hours").Value),
                        Subject = element.Element("Subject").Value
                    };
                }
            }
            return null;
        }

        [WebMethod]
        public string AddStudySession(String userName, DateTime date, double hours, string subject)
        {
            var filePath = GetXmlFilePath(userName);
            XDocument doc;

            if (File.Exists(filePath))
            {
                doc = XDocument.Load(filePath);
            }
            else
            {
                doc = new XDocument(new XElement("Sessions"));
            }

            int newId = (doc.Root.Elements("StudySession").Max(e => (int?)e.Attribute("Id")) ?? 0) + 1;

            doc.Root.Add(new XElement("StudySession",
                new XAttribute("Id", newId),
                new XElement("Date", date.ToString("o")),
                new XElement("Hours", hours),
                new XElement("Subject", subject)
            ));

            doc.Save(filePath);
            return "Study session added successfully!";
        }

        [WebMethod]
        public string UpdateStudySession(String userName, int sessionId, DateTime date, double hours, string subject)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var element = doc.Root.Elements("StudySession").FirstOrDefault(e => (int)e.Attribute("Id") == sessionId);
                if (element != null)
                {
                    element.SetElementValue("Date", date.ToString("o"));
                    element.SetElementValue("Hours", hours);
                    element.SetElementValue("Subject", subject);
                    doc.Save(filePath);
                    return "Study session updated successfully!";
                }
            }
            return "Study session not found!";
        }

        [WebMethod]
        public string DeleteStudySession(String userName, int sessionId)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var element = doc.Root.Elements("StudySession").FirstOrDefault(e => (int)e.Attribute("Id") == sessionId);
                if (element != null)
                {
                    element.Remove();
                    doc.Save(filePath);
                    return "Study session deleted successfully!";
                }
            }
            return "Study session not found!";
        }
    }
}

using StudentsPerformancePredictionTool_CW2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Xml.Linq;

namespace StudentsPerformancePredictionTool_CW2
{
    /// <summary>
    /// Summary description for BreakSession
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class BreakSession : System.Web.Services.WebService
    {
        private string GetXmlFilePath(String userName)
        {
            return Server.MapPath($"~/App_Data/User_{userName}_Sessions.xml");
        }

        [WebMethod]
        public List<BreakSessionModel> GetBreakSessions(String userName)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                return doc.Root.Elements("BreakSession").Select(e => new BreakSessionModel
                {
                    Id = (int)e.Attribute("Id"),
                    Date = DateTime.Parse(e.Element("Date").Value),
                    Hours = double.Parse(e.Element("Hours").Value),
                }).ToList();
            }
            return new List<BreakSessionModel>();
        }

        [WebMethod]
        public BreakSessionModel GetBreakSessionById(String userName, int sessionId)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var element = doc.Root.Elements("BreakSession").FirstOrDefault(e => (int)e.Attribute("Id") == sessionId);
                if (element != null)
                {
                    return new BreakSessionModel
                    {
                        Id = (int)element.Attribute("Id"),
                        Date = DateTime.Parse(element.Element("Date").Value),
                        Hours = double.Parse(element.Element("Hours").Value),
                    };
                }
            }
            return null;
        }

        [WebMethod]
        public string AddBreakSession(String userName, DateTime date, double hours)
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

            int newId = (doc.Root.Elements("BreakSession").Max(e => (int?)e.Attribute("Id")) ?? 0) + 1;

            doc.Root.Add(new XElement("BreakSession",
                new XAttribute("Id", newId),
                new XElement("Date", date.ToString("o")),
                new XElement("Hours", hours)
            ));

            doc.Save(filePath);
            return "Break session added successfully!";
        }

        [WebMethod]
        public string UpdateBreakSession(String userName, int sessionId, DateTime date, double hours)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var element = doc.Root.Elements("BreakSession").FirstOrDefault(e => (int)e.Attribute("Id") == sessionId);
                if (element != null)
                {
                    element.SetElementValue("Date", date.ToString("o"));
                    element.SetElementValue("Hours", hours);
                    doc.Save(filePath);
                    return "Break session updated successfully!";
                }
            }
            return "Break session not found!";
        }

        [WebMethod]
        public string DeleteBreakSession(String userName, int sessionId)
        {
            var filePath = GetXmlFilePath(userName);

            if (File.Exists(filePath))
            {
                XDocument doc = XDocument.Load(filePath);
                var element = doc.Root.Elements("BreakSession").FirstOrDefault(e => (int)e.Attribute("Id") == sessionId);
                if (element != null)
                {
                    element.Remove();
                    doc.Save(filePath);
                    return "Break session deleted successfully!";
                }
            }
            return "Break session not found!";
        }
    }
}

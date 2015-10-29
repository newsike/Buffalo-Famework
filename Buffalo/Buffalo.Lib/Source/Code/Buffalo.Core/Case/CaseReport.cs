using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace Buffalo.Core.Case
{

    public enum CaseStatus
    {
        Faild=0,
        Pass=1
    }

    public class CaseReportMessageItem
    {
        public int CodeIndex
        {
            set;
            get;
        }

        public string CodeMessage
        {
            set;
            get;
        }

    }

    public class CaseReport
    {

        public XmlDocument reportDoc
        {
            set;
            get;
        }

        public string reportFile
        {
            set;
            get;
        }

        public string reportMessage
        {
            set;
            get;
        }

        public Queue<CaseReportMessageItem> reportMessageItems
        {
            get;
            set;
        }

        public static CaseReport CreateInstance(string caseName)
        {
            if (caseName == "")
                return null;
            else
            {          
                CaseReport newObj = new CaseReport();
                newObj.reportDoc = new XmlDocument();
                newObj.reportDoc.LoadXml("<root></root>");
                XmlNode rootNode = newObj.reportDoc.SelectSingleNode("/root");
                Buffalo.Basic.Data.XmlHelper.SetAttribute(rootNode, "des", "This is the report created by Buffalo Automation Test Framework.");
                XmlNode createNode = Buffalo.Basic.Data.XmlHelper.CreateNode(newObj.reportDoc, "created", DateTime.Now.ToString());
                XmlNode idNode = Buffalo.Basic.Data.XmlHelper.CreateNode(newObj.reportDoc, "guid", Guid.NewGuid().ToString());
                XmlNode detailNode = Buffalo.Basic.Data.XmlHelper.CreateNode(newObj.reportDoc, "detail", "");
                XmlNode faildNode = Buffalo.Basic.Data.XmlHelper.CreateNode(newObj.reportDoc, "faild", "");
                detailNode.AppendChild(faildNode);
                XmlNode passedNode = Buffalo.Basic.Data.XmlHelper.CreateNode(newObj.reportDoc, "passed", "");
                detailNode.AppendChild(passedNode);
                rootNode.AppendChild(createNode);
                rootNode.AppendChild(idNode);
                rootNode.AppendChild(detailNode);
                string caseFile = CheckReportStore(caseName);
                newObj.reportDoc.Save(caseFile);
                newObj.reportFile = caseFile;
                newObj.reportMessageItems = new Queue<CaseReportMessageItem>();
                return newObj;
            }
        }

        public static string CheckReportStore(string CaseName)
        {
            DirectoryInfo reportDI = new DirectoryInfo(Environment.CurrentDirectory + "\\CaseReport");
            if (!reportDI.Exists)            
                reportDI = Directory.CreateDirectory(Environment.CurrentDirectory + "\\CaseReport");
            DirectoryInfo caseStoreDI = new DirectoryInfo(Environment.CurrentDirectory + "\\CaseReport\\" + CaseName);
            if (!caseStoreDI.Exists)
                caseStoreDI = Directory.CreateDirectory(Environment.CurrentDirectory + "\\CaseReport\\" + CaseName);
            string caseReportFileName = "";
            while (true)
            {
                caseReportFileName = CaseName + "_" + DateTime.Now.Year + DateTime.Now.Month + DateTime.Now.Day + DateTime.Now.Hour + DateTime.Now.Minute + DateTime.Now.Second + ".xml";
                FileInfo caseReportFile = new FileInfo(Environment.CurrentDirectory + "\\CaseReport\\" + CaseName + "\\" + caseReportFileName);
                if (!caseReportFile.Exists)
                {
                    caseReportFileName = Environment.CurrentDirectory + "\\CaseReport\\" + CaseName + "\\" + caseReportFileName;
                    break;
                }
            }
            return caseReportFileName;
        }

        public void InsertFaildItem(int LineIndex,string Message,bool Abort)
        {
            XmlNode failItemNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "item", "");
            XmlNode sourceIndexNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "line", LineIndex.ToString());
            failItemNode.AppendChild(sourceIndexNode);
            XmlNode messageNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "message", Message);
            failItemNode.AppendChild(messageNode);
            XmlNode abortNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "abort", Abort ? "1" : "0");
            failItemNode.AppendChild(abortNode);
            XmlNode faildNode = reportDoc.SelectSingleNode("/root/detail/faild");
            faildNode.AppendChild(failItemNode);
            CaseReportMessageItem newMessageItem = new CaseReportMessageItem();
            newMessageItem.CodeIndex = LineIndex;
            newMessageItem.CodeMessage = Message;
            reportMessageItems.Enqueue(newMessageItem);
            reportDoc.Save(reportFile);
        }

        public void InsertPassItem(int LineIndex,string Message)
        {
            XmlNode passedItemNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "item", "");
            XmlNode sourceIndexNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "line", LineIndex.ToString());
            passedItemNode.AppendChild(sourceIndexNode);
            XmlNode messageNode = Buffalo.Basic.Data.XmlHelper.CreateNode(reportDoc, "message", Message);
            passedItemNode.AppendChild(messageNode);
            XmlNode passedNode = reportDoc.SelectSingleNode("/root/passed");
            passedNode.AppendChild(passedItemNode);
            reportDoc.Save(reportFile);
        }

        

    }
}

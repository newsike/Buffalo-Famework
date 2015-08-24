using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using Buffalo.Basic.Data;

namespace Buffalo.Basic.Base
{
    public class LogServices
    {
        static LogServices()
        {
            DirectoryInfo logActiveInfo = new DirectoryInfo(Environment.CurrentDirectory + "\\AppLog");
            if(!logActiveInfo.Exists)            
                logActiveInfo.Create();                        
        }

        public static void WriteLog(string header,List<string> msgs)
        {
            string timeStamp = DateTime.Now.ToString("yyyyMMdd");
            FileInfo activeLogInfo = new FileInfo(Environment.CurrentDirectory + "\\AppLog\\" + "LogFile_" + timeStamp + ".xml");
            XmlDocument logDoc=new XmlDocument();
            if (!activeLogInfo.Exists)
            {
                logDoc.LoadXml("<root></root>");
                logDoc.Save(Environment.CurrentDirectory + "\\AppLog\\" + "LogFile_" + timeStamp + ".xml");
            }
            logDoc.Load(Environment.CurrentDirectory + "\\AppLog\\" + "LogFile_" + timeStamp + ".xml");
            XmlNode newItemNode = XmlHelper.CreateNode(logDoc, "item", "");
            XmlHelper.SetAttribute(newItemNode, "recordedtime", DateTime.Now.ToString());
            XmlHelper.SetAttribute(newItemNode, "header", header);
            foreach(string msg in msgs)
            {
                XmlNode msgNode = XmlHelper.CreateNode(logDoc, "msg", msg);
                newItemNode.AppendChild(msgNode);
            }
            logDoc.SelectSingleNode("/root").AppendChild(newItemNode);
            lock(logDoc)
            {
                logDoc.Save(Environment.CurrentDirectory + "\\AppLog\\" + "LogFile_" + timeStamp + ".xml");
            }            
        }

    }
}

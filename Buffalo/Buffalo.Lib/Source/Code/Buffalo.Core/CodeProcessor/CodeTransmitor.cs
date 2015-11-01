using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Data;

namespace Buffalo.Core.CodeProcessor
{
    public class CodeTransmitor
    {
        public static Case.CaseContentItem CodeTransmit_Action_CreateContentInstance()
        {
            return new Case.CaseContentItem();
        }

        public static Case.CaseMethodItem CodeTransmit_Action_CreateMethodInstance()
        {
            return new Case.CaseMethodItem();
        }

        public static void CodeTransmit_Action_Case(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            if (activeSelectLine.KeyCode == Container.KeyWordMap.Case)
            {
                foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                {
                    switch (paramName)
                    {
                        case "casename":
                            refTestCase.CaseName = activeSelectLine.ParamsPool[paramName];
                            break;
                    }
                }
            }
        }


        public static void CodeTransmit_Action_ImportXML(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            if (refTestCase != null && activeSelectLine.KeyCode == Container.KeyWordMap.ImportDB)
            {
                if (activeSelectLine.ParamsPool.ContainsKey("name") && activeSelectLine.ParamsPool.ContainsKey("source"))
                {
                    string sourceFile = activeSelectLine.ParamsPool["source"];
                    XmlDocument tmpDoc = new XmlDocument();
                    XmlDocument sourceDataDoc = new XmlDocument();
                    sourceDataDoc.LoadXml("<root></root>");
                    try
                    {
                        tmpDoc.Load(sourceFile);
                        XmlNodeList rowNodes = tmpDoc.SelectNodes("/root/row");
                        int rowIndex = 1;
                        foreach (XmlNode rowNode in rowNodes)
                        {
                            XmlNode newRowNode = Buffalo.Basic.Data.XmlHelper.CreateNode(sourceDataDoc, "row", "");
                            Buffalo.Basic.Data.XmlHelper.SetAttribute(newRowNode, "index", rowIndex.ToString());
                            foreach (XmlAttribute activeAttr in rowNode.Attributes)
                            {
                                string value = Buffalo.Basic.Data.XmlHelper.GetNodeValue("@" + activeAttr.Name, rowNode);
                                Buffalo.Basic.Data.XmlHelper.SetAttribute(newRowNode, "column_" + activeAttr.Name, value);
                            }
                            sourceDataDoc.SelectSingleNode("/root").AppendChild(newRowNode);
                            rowIndex++;
                        }

                    }
                    catch (Exception err)
                    {
                        refTestCase.SingleInterrupt = true;
                        refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Import XML : " + err.Message, true);
                    }
                }
                else
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Import XML : invalidate word namd or source.", true);

                }
            }          
        }

        public static void CodeTransmit_Action_ImportDB(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            if (refTestCase != null && activeSelectLine.KeyCode == Container.KeyWordMap.ImportDB)
            {
                if (activeSelectLine.ParamsPool.ContainsKey("name") && activeSelectLine.ParamsPool.ContainsKey("server") && activeSelectLine.ParamsPool.ContainsKey("table") && activeSelectLine.ParamsPool.ContainsKey("db") && activeSelectLine.ParamsPool.ContainsKey("uid") && activeSelectLine.ParamsPool.ContainsKey("pwd"))
                {
                    try
                    {
                        string sourceName = activeSelectLine.ParamsPool["name"];
                        string server = activeSelectLine.ParamsPool["server"];
                        string db = activeSelectLine.ParamsPool["db"];
                        string uid = activeSelectLine.ParamsPool["uid"];
                        string pwd = activeSelectLine.ParamsPool["pwd"];
                        string table = activeSelectLine.ParamsPool["table"];
                        Buffalo.Basic.Data.Data_SqlConnectionHelper _ConnectionObj = new Basic.Data.Data_SqlConnectionHelper();
                        _ConnectionObj.Set_NewConnectionItem(sourceName, server, uid, pwd, db);
                        Buffalo.Basic.Data.Data_SqlDataHelper _SqlDataHelper = new Basic.Data.Data_SqlDataHelper();
                        _SqlDataHelper.ActiveConnection = _ConnectionObj.Get_ActiveConnection(sourceName);
                        DataTable activeDataTable = new DataTable();
                        _SqlDataHelper.Action_ExecuteForDT("select * from " + table, out activeDataTable);
                        XmlDocument sourceDataDoc = new XmlDocument();
                        sourceDataDoc.LoadXml("<root></root>");
                        List<string> columnNameList = new List<string>();
                        foreach (DataColumn dc in activeDataTable.Columns)
                        {
                            columnNameList.Add(dc.ColumnName);
                        }
                        int rowIndex = 1;
                        foreach (DataRow dr in activeDataTable.Rows)
                        {
                            XmlNode newRowItem = Buffalo.Basic.Data.XmlHelper.CreateNode(sourceDataDoc, "row", "");
                            Buffalo.Basic.Data.XmlHelper.SetAttribute(newRowItem, "index", rowIndex.ToString());
                            foreach (string columnName in columnNameList)
                            {
                                string result = "";
                                _SqlDataHelper.Static_GetColumnData(dr, columnName, out result);
                                Buffalo.Basic.Data.XmlHelper.SetAttribute(newRowItem, "column_" + columnName, result);
                            }
                            sourceDataDoc.SelectSingleNode("/root").AppendChild(newRowItem);
                        }
                    }
                    catch (Exception err)
                    {
                        refTestCase.SingleInterrupt = true;
                        refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Import DB : " + err.Message, true);
                    }
                }
                else
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Import DB : invalidate word.", true);

                }
            }          
        }

        public static void CodeTransmit_Action_FillExistedData(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            foreach (string paramName in activeSelectLine.ParamsPool.Keys)
            {
                string paramValue = activeSelectLine.ParamsPool[paramName];
                if (paramValue.Contains("%D"))
                {
                    string dataName = paramValue.Replace("%D", "");
                    if (refTestCase.ActiveDataBuffer.ContainsKey(dataName))
                        activeSelectLine.ParamsPool[paramName] = refTestCase.ActiveDataBuffer[dataName];
                    else
                    {
                        refTestCase.SingleInterrupt = true;
                        refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : No data : " + dataName, true);
                        return;
                    }
                }
            }
        }

        public static void CodeTransmit_Action_DataSet(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            if (refTestCase != null && activeSelectLine.KeyCode == Container.KeyWordMap.DataSet)
            {
                if (!activeSelectLine.ParamsPool.ContainsKey("dataname"))
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Set : missing dataname.", true);
                    return;
                }
                else
                {
                    if (!activeSelectLine.ParamsPool.ContainsKey("value"))
                    {
                        refTestCase.SingleInterrupt = true;
                        refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Set : missing data value.", true);
                        return;
                    }
                    else
                    {
                        string dataName = activeSelectLine.ParamsPool["dataname"];
                        string dataValue = activeSelectLine.ParamsPool["value"];
                        if (refTestCase.ActiveDataBuffer.ContainsKey(dataName))
                            refTestCase.ActiveDataBuffer[dataName] = dataValue;
                        else
                            refTestCase.ActiveDataBuffer.Add(dataName, dataValue);
                    }
                }
            }
        }

        public static void CodeTransmit_Action_DataFill(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            if (refTestCase != null && activeSelectLine.KeyCode == Container.KeyWordMap.DataFill)
            {
                if (!activeSelectLine.ParamsPool.ContainsKey("sourcename"))
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : missing sourcename.", true);
                    return;
                }
                else
                {
                    if (!activeSelectLine.ParamsPool.ContainsKey("dataname"))
                    {
                        refTestCase.SingleInterrupt = true;
                        refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : missing dataname.", true);
                        return;
                    }
                    else
                    {
                        string sourcename = activeSelectLine.ParamsPool["sourcename"];
                        if (!refTestCase.ActiveCaseDataSourcePool.ContainsKey(sourcename))
                        {
                            refTestCase.SingleInterrupt = true;
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : can not foune datasource : " + sourcename + ".", true);
                            return;
                        }
                        XmlDocument sourceDoc = refTestCase.ActiveCaseDataSourcePool[sourcename];
                        if (sourceDoc != null)
                        {
                            if (activeSelectLine.ParamsPool.ContainsKey("row") && activeSelectLine.ParamsPool.ContainsKey("column"))
                            {
                                XmlNodeList rowNodes = sourceDoc.SelectNodes("/root/row");
                                int MaxRow = rowNodes.Count;
                                if (MaxRow <= 0)
                                {
                                    refTestCase.SingleInterrupt = true;
                                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : no data row in data source.", true);
                                    return;
                                }
                                string RowValue = activeSelectLine.ParamsPool["row"];
                                string ColumnName = activeSelectLine.ParamsPool["column"];
                                int RowIndex = 1;
                                if (RowValue == "rand")
                                {
                                    Random rnd = new Random();
                                    RowIndex = rnd.Next(1, MaxRow);
                                }
                                else
                                {
                                    int.TryParse(activeSelectLine.ParamsPool["row"], out RowIndex);
                                }
                                XmlNode selectedRowNode = sourceDoc.SelectSingleNode("/root/row[@index='" + RowIndex + "']");
                                if (selectedRowNode != null)
                                {
                                    string value = Buffalo.Basic.Data.XmlHelper.GetNodeValue("@column_" + ColumnName, selectedRowNode);
                                    string dataname = activeSelectLine.ParamsPool["dataname"];
                                    if (refTestCase.ActiveDataBuffer.ContainsKey(dataname))
                                        refTestCase.ActiveDataBuffer[dataname] = value;
                                    else
                                        refTestCase.ActiveDataBuffer.Add(dataname, value);
                                }
                                else
                                {
                                    refTestCase.SingleInterrupt = true;
                                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : can not found row node.", true);
                                    return;
                                }
                            }
                            else
                            {
                                refTestCase.SingleInterrupt = true;
                                refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : missing row or column paramter.", true);
                                return;
                            }
                        }
                        else
                        {
                            refTestCase.SingleInterrupt = true;
                            refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Data Fill : datasource : " + sourcename + " is Null.", true);
                            return;
                        }
                    }
                }
            }
        }

        public static void CodeTransmit_Action_ImportExcel(Case.BasicTestCase refTestCase, CodeLine activeSelectLine)
        {
            if (refTestCase != null && activeSelectLine.KeyCode == Container.KeyWordMap.ImportExcel)
            {
                if (activeSelectLine.ParamsPool.ContainsKey("name") && activeSelectLine.ParamsPool.ContainsKey("excel") && activeSelectLine.ParamsPool.ContainsKey("sheet") && activeSelectLine.ParamsPool.ContainsKey("rows") && activeSelectLine.ParamsPool.ContainsKey("columns"))
                {
                    try
                    {
                        string sourceName = activeSelectLine.ParamsPool["name"];
                        Buffalo.Basic.Data.ExcelConnectionHelper _ConnectionObj = new Basic.Data.ExcelConnectionHelper();
                        string guid = Guid.NewGuid().ToString();
                        _ConnectionObj.AddNewExcelConnection(guid, activeSelectLine.ParamsPool["excel"], Basic.Data.ExcelConnectionType.NPOI, false);
                        Buffalo.Basic.Data.ExcelConnection_NPIO activeExcelConnection = (Buffalo.Basic.Data.ExcelConnection_NPIO)_ConnectionObj.GetConnection(guid);
                        string str_maxRow = activeSelectLine.ParamsPool["rows"];
                        string str_maxColumn = activeSelectLine.ParamsPool["columns"];
                        int i_maxRow = 0;
                        int i_maxColumn = 0;
                        Int32.TryParse(str_maxRow, out i_maxRow);
                        Int32.TryParse(str_maxColumn, out i_maxColumn);
                        XmlDocument sourceDataDoc = new XmlDocument();
                        sourceDataDoc.LoadXml("<root></root>");
                        for (int i = 1; i <= i_maxRow; i++)
                        {
                            XmlNode rowNode = Buffalo.Basic.Data.XmlHelper.CreateNode(sourceDataDoc, "row", "");
                            Buffalo.Basic.Data.XmlHelper.SetAttribute(rowNode, "index", i.ToString());
                            for (int y = 1; y <= i_maxColumn; y++)
                            {
                                string value = activeExcelConnection.GetCellValue(activeSelectLine.ParamsPool["sheet"], i, y);
                                Buffalo.Basic.Data.XmlHelper.SetAttribute(rowNode, "column_" + y, value);
                            }
                            sourceDataDoc.SelectSingleNode("/root").AppendChild(rowNode);
                        }
                        if (!refTestCase.ActiveCaseDataSourcePool.ContainsKey(sourceName))
                        {
                            refTestCase.ActiveCaseDataSourcePool.Add(sourceName, sourceDataDoc);
                        }
                        else
                        {
                            refTestCase.ActiveCaseDataSourcePool[sourceName] = sourceDataDoc;
                        }
                    }
                    catch (Exception err)
                    {
                        refTestCase.SingleInterrupt = true;
                        refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Import Excel : " + err.Message, true);
                    }
                }             
            }
        }

        public static void CodeTransmit_Action_Connect(Case.BasicTestCase refTestCase, WebDriver.WebBrowserDriver refWebBrowserDriver, CodeLine activeSelectLine)
        {
            if (activeSelectLine.KeyCode == Container.KeyWordMap.Connect)
            {
                try
                {
                    foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                    {
                        if (paramName == "type")
                        {
                            switch (activeSelectLine.ParamsPool[paramName])
                            {
                                case "Chrome":
                                case "chrome":
                                case "CHROME":
                                default:
                                    refWebBrowserDriver = WebDriver.WebBrowserDriver.CreateInstanceForWebDriver(WebDriver.WebBrowserType.Chrome);
                                    break;
                                case "IE":
                                case "ie":
                                    refWebBrowserDriver = WebDriver.WebBrowserDriver.CreateInstanceForWebDriver(WebDriver.WebBrowserType.InternetExplorer);
                                    break;
                                case "firefox":
                                case "Firefox":
                                    refWebBrowserDriver = WebDriver.WebBrowserDriver.CreateInstanceForWebDriver(WebDriver.WebBrowserType.FireFox);
                                    break;
                            }
                        }
                    }
                    refTestCase.ActionWebBrowserActionsObject = new WebDriver.WebBrowserActions(refWebBrowserDriver.ActiveWebDriver);
                    refTestCase.ActiveWebBrowserDriverObject = refWebBrowserDriver;
                    refTestCase.ActiveElementSelectorObject = new ElementActions.ElementSelector(refWebBrowserDriver.ActiveWebDriver);
                }
                catch (Exception err)
                {
                    refTestCase.SingleInterrupt = true;
                    refTestCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Connect : " + err.Message, true);
                }
            }
        }

        public static void CodeTransmit_Action_WebBrowser(Case.CaseMethodItem refCaseMethodItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseMethodItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.WBAction)
            {
                try
                {
                    refCaseMethodItemObj.Index = activeSelectLine.CodeIndex;
                    refCaseMethodItemObj.ActiveMethod = new ElementActions.MethodItem();
                    refCaseMethodItemObj.ActiveMethod.ActiveType = typeof(WebDriver.WebBrowserActions);
                    refActiveCase.ActiveCaseWebBrowserPool.Add(refCaseMethodItemObj.Index, refCaseMethodItemObj);
                    object[] methodParam;
                    foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                    {
                        switch (paramName)
                        {
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_StartBrowser:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_StartBrowser);
                                if (activeSelectLine.ParamsPool[paramName] != Container.KeyWordMap.Null)
                                {
                                    methodParam = new object[] { activeSelectLine.ParamsPool[paramName] };
                                    refCaseMethodItemObj.ActiveMethod.MethodParams = methodParam;
                                }
                                break;
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_SwitchToWindow:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_SwitchToWindow);
                                if (activeSelectLine.ParamsPool[paramName] != Container.KeyWordMap.Null)
                                {
                                    methodParam = new object[] { activeSelectLine.ParamsPool[paramName] };
                                    refCaseMethodItemObj.ActiveMethod.MethodParams = methodParam;
                                }
                                break;
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_CloseWindow:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_CloseWindow);
                                break;
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_Action_IsAlert:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_Action_IsAlert);
                                break;
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_GetCurrentWindowHandle:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_SwitchToWindow);
                                break;
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_GetSourceOfPage:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_SwitchToWindow);
                                break;
                                
                            case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_Action_Wait:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_Action_Wait);
                                methodParam = new object[] { activeSelectLine.ParamsPool[paramName] };
                                refCaseMethodItemObj.ActiveMethod.MethodParams = methodParam;
                                break;
                        }
                    }
                }
                catch (Exception err)
                {
                    refActiveCase.SingleInterrupt = true;
                    refActiveCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : WBAction : " + err.Message, true);
                }
            }
        }

        public static void CodeTransmit_Action_ElementAction(Case.CaseMethodItem refCaseMethodItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseMethodItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.Action)
            {
                try
                {
                    refCaseMethodItemObj.Index = activeSelectLine.CodeIndex;
                    refCaseMethodItemObj.ActiveMethod = new ElementActions.MethodItem();
                    refCaseMethodItemObj.ActiveMethod.ActiveType = typeof(ElementActions.ElementActions);
                    refActiveCase.ActiveCaseMethodPool.Add(refCaseMethodItemObj.Index, refCaseMethodItemObj);
                    object[] methodParam;
                    foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                    {
                        switch (paramName)
                        {
                            case ElementActions.ActionMap.Method_Action_Click:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ActionMap.Method_Action_Click);
                                if (activeSelectLine.ParamsPool[paramName] != Container.KeyWordMap.Null)
                                {
                                    int delayTime = 0;
                                    int.TryParse(activeSelectLine.ParamsPool[paramName], out delayTime);
                                    methodParam = new object[] { delayTime };
                                    refCaseMethodItemObj.ActiveMethod.MethodParams = methodParam;
                                }
                                break;
                            case ElementActions.ActionMap.Method_Action_SetText:
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ActionMap.Method_Action_SetText);
                                string textParam = "";
                                textParam = activeSelectLine.ParamsPool[paramName];
                                methodParam = new object[] { textParam };
                                refCaseMethodItemObj.ActiveMethod.MethodParams = methodParam;
                                break;
                            case ElementActions.ActionMap.Method_Get_Content:
                                string datanameParam = "";
                                datanameParam = activeSelectLine.ParamsPool[paramName];
                                if (datanameParam != Container.KeyWordMap.Null)
                                {
                                    string contentFromElement = refActiveCase.ActiveElementActionObject.Get_Content();
                                    if (refActiveCase.ActiveDataBuffer.ContainsKey(datanameParam))
                                        refActiveCase.ActiveDataBuffer[datanameParam] = contentFromElement;
                                    else
                                        refActiveCase.ActiveDataBuffer.Add(datanameParam, contentFromElement);
                                }
                                break;
                        }
                    }
                }
                catch (Exception err)
                {
                    refActiveCase.SingleInterrupt = true;
                    refActiveCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Action : " + err.Message, true);
                }
            }
        }

        public static void CodeTransmit_Action_AnalyseContent(Case.CaseMethodItem refCaseMethodItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseMethodItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.Select)
            {
                if(activeSelectLine.ParamsPool.ContainsKey("action"))
                {
                    string actionValue = activeSelectLine.ParamsPool["action"];
                    if(actionValue==Analyse.AnalyseMap.Check_Equal_StrContent)
                    {
                        string sourceDataName = activeSelectLine.ParamsPool["sourcedata"];
                        string objectDataName = activeSelectLine.ParamsPool["objectdata"];
                    }
                }
                    
                    
            }
        }

        public static void CodeTransmit_Action_Select(Case.CaseMethodItem refCaseMethodItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseMethodItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.Select)
            {
                try
                {
                    refCaseMethodItemObj.Index = activeSelectLine.CodeIndex;
                    refCaseMethodItemObj.ActiveMethod = new ElementActions.MethodItem();
                    refCaseMethodItemObj.ActiveMethod.ActiveType = typeof(ElementActions.ElementSelector);
                    foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                    {
                        switch (paramName)
                        {
                            case "xpath":
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ElementSelectorMap.Action_SelectElementByXPATH, new Type[] { typeof(string) });
                                refCaseMethodItemObj.ActiveMethod.MethodParams = new object[] { activeSelectLine.ParamsPool[paramName] };
                                break;
                            case "name":
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ElementSelectorMap.Action_SelectElementByName, new Type[] { typeof(string) });
                                refCaseMethodItemObj.ActiveMethod.MethodParams = new object[] { activeSelectLine.ParamsPool[paramName] };
                                break;
                            case "id":
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ElementSelectorMap.Action_SelectElementByID, new Type[] { typeof(string) });
                                refCaseMethodItemObj.ActiveMethod.MethodParams = new object[] { activeSelectLine.ParamsPool[paramName] };
                                break;
                            case "classname":
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ElementSelectorMap.Action_SelectElementByClassname, new Type[] { typeof(string) });
                                refCaseMethodItemObj.ActiveMethod.MethodParams = new object[] { activeSelectLine.ParamsPool[paramName] };
                                break;
                            case "linktext":
                                refCaseMethodItemObj.ActiveMethod.ActiveMethod = refCaseMethodItemObj.ActiveMethod.ActiveType.GetMethod(ElementActions.ElementSelectorMap.Action_SelectElementByLinkText, new Type[] { typeof(string) });
                                refCaseMethodItemObj.ActiveMethod.MethodParams = new object[] { activeSelectLine.ParamsPool[paramName] };
                                break;
                        }
                        refActiveCase.ActiveCaseSelectorPool.Add(refCaseMethodItemObj.Index, refCaseMethodItemObj);
                    }
                }
                catch (Exception err)
                {
                    refActiveCase.SingleInterrupt = true;
                    refActiveCase.ActiveTestCaseReport.InsertFaildItem(activeSelectLine.CodeIndex, "Fail to transmit : Select : " + err.Message, true);
                }
            }

        }
    }
}

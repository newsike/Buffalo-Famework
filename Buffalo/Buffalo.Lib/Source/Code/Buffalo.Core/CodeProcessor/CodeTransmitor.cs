using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static void CodeTransmit_Action_Connect(WebDriver.WebBrowserDriver refWebBrowserDriver,CodeLine activeSelectLine)
        {
            if (activeSelectLine.KeyCode == Container.KeyWordMap.Connect)
            {
                foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                {
                    if(paramName=="type")
                    {
                        switch( activeSelectLine.ParamsPool[paramName])
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
            }
        }

        public static void CodeTransmit_Action_WebBrowser(Case.CaseMethodItem refCaseMethodItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseMethodItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.WBAction)
            {
                refCaseMethodItemObj.Index = activeSelectLine.CodeIndex;
                refCaseMethodItemObj.ActiveMethod = new ElementActions.MethodItem();
                refCaseMethodItemObj.ActiveMethod.ActiveType = typeof(WebDriver.WebBrowserActions);
                object[] methodParam;
                foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                {
                    switch (paramName)
                    {
                        case WebDriver.WebBrowserActionsMap.Method_Action_WB_Action_StartBrowser:

                            break;
                    }
                }
            }
        }

        public static void CodeTransmit_Action_Action(Case.CaseMethodItem refCaseMethodItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseMethodItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.Action)
            {
                refCaseMethodItemObj.Index = activeSelectLine.CodeIndex;
                refCaseMethodItemObj.ActiveMethod = new ElementActions.MethodItem();
                refCaseMethodItemObj.ActiveMethod.ActiveType = typeof(ElementActions.ElementActions);
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
                    }
                }
            }
        }


        public static void CodeTransmit_Action_Select(Case.CaseContentItem refCaseContentItemObj, Case.BasicTestCase refActiveCase, CodeLine activeSelectLine)
        {
            if (refCaseContentItemObj != null && activeSelectLine.KeyCode == Container.KeyWordMap.Select)
            {
                Case.CaseContentItem newCaseContentItem = refCaseContentItemObj;
                newCaseContentItem.Index = activeSelectLine.CodeIndex;
                foreach (string paramName in activeSelectLine.ParamsPool.Keys)
                {
                    switch (paramName)
                    {
                        case "xpath":
                            newCaseContentItem.ActiveElementItem = refActiveCase.ActiveElementSelectorObject.Action_SelectElementByXPATH(activeSelectLine.ParamsPool[paramName]);
                            break;
                        case "name":
                            newCaseContentItem.ActiveElementItem = refActiveCase.ActiveElementSelectorObject.Action_SelectElementByName(activeSelectLine.ParamsPool[paramName]);
                            break;
                        case "id":
                            newCaseContentItem.ActiveElementItem = refActiveCase.ActiveElementSelectorObject.Action_SelectElementByID(activeSelectLine.ParamsPool[paramName]);
                            break;
                        case "classname":
                            newCaseContentItem.ActiveElementItem = refActiveCase.ActiveElementSelectorObject.Action_SelectElementByClassname(activeSelectLine.ParamsPool[paramName]);
                            break;
                    }
                    if (newCaseContentItem.ActiveElementItem == null)
                        Container.GlobalObjsPoolContainer.GlobalObject_MessageContainer.Action_InsertMessage(newCaseContentItem.Index, Container.CodeErrMessage.UnselectedElement, Container.ErrLevel.Normal, true);
                }                
            }           
        }
    }
}

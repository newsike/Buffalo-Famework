﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Xml;


namespace Buffalo.Core.Engine
{
    public class BuffaloEngine
    {
        private Queue<string> _TaskCode = new Queue<string>();
        private Queue<Case.BasicTestCase> _RunningTaskPool = new Queue<Case.BasicTestCase>();

        private Thread _TaskThread_FecthTask;
        private Thread _TaskThread_InvokeTask;

        private Buffalo.Core.CodeProcessor.CodeTransmitor _Obj_Transmitor = new CodeProcessor.CodeTransmitor();
        private WebDriver.WebBrowserDriver _Obj_WebBrowserDriver = new WebDriver.WebBrowserDriver();

        private List<Thread> _FetchMessageThreadsPool = new List<Thread>();
        private List<Thread> _ExecuteThreadsPool = new List<Thread>();
                
        
        public void StartServices_ASY()
        {
            _TaskThread_FecthTask = new Thread(new ThreadStart(Action_FetchTask));
            _TaskThread_FecthTask.Start();
            _TaskThread_InvokeTask = new Thread(new ThreadStart(Action_InvokeTestCase));
            _TaskThread_InvokeTask.Start();
        }
        
        private void StopServices_ASY()
        {
            _TaskThread_FecthTask.Abort();
            _TaskThread_InvokeTask.Abort();
            foreach (Thread activeThread in _FetchMessageThreadsPool)
                activeThread.Abort();
        }

         
        public bool Action_InsertTask(string Code)
        {
            if (Code != "")
            {
                Code = Code.Replace("\n", "");
                Code = Code.Replace("\r", "");
                _TaskCode.Enqueue(Code);
                return true;
            }
            else
                return false;
        }

        public void Action_FetchConsoleMessage(object testCase)
        {
            while (true)
            {
                Case.BasicTestCase activeTestCase = (Case.BasicTestCase)testCase;
                if (activeTestCase.ActiveTestCaseReport.reportMessageItems.Count > 0)
                {
                    Case.CaseReportMessageItem activeItem = activeTestCase.ActiveTestCaseReport.reportMessageItems.Dequeue();
                    Console.WriteLine(">| Buffalo Core |< - >| " + activeItem.CodeIndex + ":" + activeItem.CodeMessage);
                }
                else
                    Thread.Sleep(500);
            }
        }        

        public void Action_TransmitTask(string Code)
        {
            Buffalo.Core.CodeProcessor.CodeScaner _Obj_Scaner = new CodeProcessor.CodeScaner();
            _Obj_Scaner.Action_Scan(Code);
            Case.BasicTestCase testCase = new Case.BasicTestCase();
            testCase.CaseName = "";
            testCase.CaseCodeLineCount = _Obj_Scaner.CodePool.Count;
            string idCase = Guid.NewGuid().ToString();
            testCase.CaseID = idCase;
            if (testCase.CaseName == "")
                testCase.CaseName = Guid.NewGuid().ToString();
            testCase.ActiveTestCaseReport = Case.CaseReport.CreateInstance(testCase.CaseName);
            testCase.SingleInterrupt = false;
            Thread newThreadFetchMessage = new Thread(new ParameterizedThreadStart(Action_FetchConsoleMessage));
            _FetchMessageThreadsPool.Add(newThreadFetchMessage);
            newThreadFetchMessage.Start(testCase);
            foreach (int codeIndex in _Obj_Scaner.CodePool.Keys)
            {
                Case.CaseContentItem caseContent = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateContentInstance();
                Case.CaseMethodItem caseMethod = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateMethodInstance();
                Case.CaseMethodItem caseSelectMethod = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateMethodInstance();
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Case(testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Connect(testCase, _Obj_WebBrowserDriver, _Obj_Scaner.CodePool[codeIndex]); 
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_WebBrowser(caseMethod, testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Select(caseSelectMethod, testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_ElementAction(caseMethod, testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_ImportXML(testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_ImportDB(testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_ImportExcel(testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_DataFill(testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_DataSet(testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_FillExistedData(testCase, _Obj_Scaner.CodePool[codeIndex]);
                if (testCase.SingleInterrupt == true)
                    return;
            }            
            _RunningTaskPool.Enqueue(testCase);
        }

        public void Action_FetchTask()
        {
            while(true)
            {
                if(_TaskCode.Count>0)
                {
                    string code=_TaskCode.Dequeue();
                    Action_TransmitTask(code);
                }
                Thread.Sleep(100);
            }
        }

        public void Action_CallExecuteTestCase(Object activeCase)
        {
            Case.BasicTestCase activeBasicCase = (Case.BasicTestCase)activeCase;
            Action_ExecuteTestCase(activeBasicCase);
        }

        public void Action_ExecuteTestCase(Case.BasicTestCase activeCase)
        {
            ElementActions.ElementItem activeSelectedElement = null;
            for (int codeIndex = 1; codeIndex <= activeCase.CaseCodeLineCount; codeIndex++)
            {
                try
                {
                    if (activeCase.ActiveCaseContentPool.ContainsKey(codeIndex))
                    {
                        activeSelectedElement = activeCase.ActiveCaseContentPool[codeIndex].ActiveElementItem;
                        continue;
                    }
                    else if (activeCase.ActiveCaseMethodPool.ContainsKey(codeIndex))
                    {
                        if (activeSelectedElement != null)
                        {
                            object[] consParams = new object[] { activeSelectedElement };
                            activeCase.ActiveCaseMethodPool[codeIndex].ActiveMethod.DoInvoke(consParams);
                            continue;
                        }
                    }
                    else if (activeCase.ActiveCaseWebBrowserPool.ContainsKey(codeIndex))
                    {
                        object[] conParam = new object[] { activeCase.ActiveWebBrowserDriverObject.ActiveWebDriver };
                        activeCase.ActiveCaseWebBrowserPool[codeIndex].ActiveMethod.DoInvoke(conParam);
                        continue;
                    }
                    else if (activeCase.ActiveCaseSelectorPool.ContainsKey(codeIndex))
                    {
                        object[] conParam = new object[] { activeCase.ActiveWebBrowserDriverObject.ActiveWebDriver };
                        activeCase.ActiveCaseSelectorPool[codeIndex].ActiveMethod.DoInvoke(conParam);
                        activeSelectedElement = (ElementActions.ElementItem)activeCase.ActiveCaseSelectorPool[codeIndex].ActiveMethod.MethodReturnValue;
                        continue;
                    }
                }
                catch
                {
                    activeCase.ActiveTestCaseReport.InsertFaildItem(codeIndex, "Faild Execut : Code Line : " + codeIndex, true);
                    return;
                }
            }
            activeCase.ActionWebBrowserActionsObject.Action_CloseAllWindow();
            activeCase.ActionWebBrowserActionsObject.Action_Close();
        }

        public void Action_InvokeTestCase()
        {
            while(true)
            {
                if(_RunningTaskPool.Count>0)
                {
                    Case.BasicTestCase activeCase = _RunningTaskPool.Dequeue();                    
                    if (activeCase != null)
                    {
                        Thread newThread = new Thread(new ParameterizedThreadStart(Action_CallExecuteTestCase));
                        _ExecuteThreadsPool.Add(newThread);
                        newThread.Start(activeCase);
                    }
                }
                Thread.Sleep(100);
            }
        }

           


    }
}

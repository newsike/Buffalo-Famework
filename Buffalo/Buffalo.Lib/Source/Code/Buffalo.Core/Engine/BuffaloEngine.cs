using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;


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
        private ElementActions.ElementSelector _Obj_ElementSelector;
        
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

        public void Action_TransmitTask(string Code)
        {
            Buffalo.Core.CodeProcessor.CodeScaner _Obj_Scaner = new CodeProcessor.CodeScaner();
            _Obj_Scaner.Action_Scan(Code);
            Case.BasicTestCase testCase = new Case.BasicTestCase();
            testCase.CaseCodeLineCount = _Obj_Scaner.CodePool.Count;
            string idCase = Guid.NewGuid().ToString();
            testCase.CaseID = idCase;
            foreach (int codeIndex in _Obj_Scaner.CodePool.Keys)
            {
                Case.CaseContentItem caseContent = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateContentInstance();
                Case.CaseMethodItem caseMethod = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateMethodInstance();
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Connect(testCase, _Obj_WebBrowserDriver, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Select(caseContent, testCase, _Obj_Scaner.CodePool[codeIndex]);
                Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_ElementAction(caseMethod, testCase, _Obj_Scaner.CodePool[codeIndex]);
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

        public void Action_ExecuteTestCase(Case.BasicTestCase activeCase)
        {
            ElementActions.ElementItem activeSelectedElement = null;
            for (int codeIndex = 1; codeIndex <= activeCase.CaseCodeLineCount; codeIndex++)
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
                    activeCase.ActiveCaseWebBrowserPool[codeIndex].ActiveMethod.DoInvoke();
                    continue;
                }

            }
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
                        Action_ExecuteTestCase(activeCase);
                    }
                }
                Thread.Sleep(100);
            }
        }

           


    }
}

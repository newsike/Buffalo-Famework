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
        private Buffalo.Core.CodeProcessor.CodeScaner _Obj_Scaner = new CodeProcessor.CodeScaner();
        private Buffalo.Core.CodeProcessor.CodeTransmitor _Obj_Transmitor = new CodeProcessor.CodeTransmitor();
        private WebDriver.WebBrowserDriver _Obj_WebBrowserDriver = new WebDriver.WebBrowserDriver();
        private ElementActions.ElementSelector _Obj_ElementSelector;
        private Dictionary<string, Case.BasicTestCase> _Pool_TestCase = new Dictionary<string, Case.BasicTestCase>();
        
        
        public bool Action_InsertTask(string Code)
        {
            if (Code != "")
            {
                _TaskCode.Enqueue(Code);
                return true;
            }
            else
                return false;
        }



        public void Action_ScanCode()
        {
            while(true)
            {
                if(_TaskCode.Count>0)
                {
                    string code=_TaskCode.Dequeue();
                    _Obj_Scaner.Action_Scan(code);
                    Case.BasicTestCase testCase = new Case.BasicTestCase();
                    string idCase = Guid.NewGuid().ToString();
                    testCase.CaseID = idCase;
                    foreach (int codeIndex in _Obj_Scaner.CodePool.Keys)
                    {
                        Case.CaseContentItem caseContent = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateContentInstance();
                        Case.CaseMethodItem caseMethod = Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_CreateMethodInstance();
                        Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Connect(testCase, _Obj_WebBrowserDriver, _Obj_Scaner.CodePool[codeIndex]);
                        Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Select(caseContent, testCase, _Obj_Scaner.CodePool[codeIndex]);
                        Core.CodeProcessor.CodeTransmitor.CodeTransmit_Action_Action(caseMethod, testCase, _Obj_Scaner.CodePool[codeIndex]);                        
                    }
                    if (!_Pool_TestCase.ContainsKey(idCase))
                        _Pool_TestCase.Add(idCase, testCase);
                }
                Thread.Sleep(100);
            }
        }

        public void Action_InvokeTestCase()
        {
            while(true)
            {
                if(_Pool_TestCase.Count>0)
                {
                    Case.BasicTestCase activeCase=_Pool_TestCase[]
                }
            }
        }

           


    }
}

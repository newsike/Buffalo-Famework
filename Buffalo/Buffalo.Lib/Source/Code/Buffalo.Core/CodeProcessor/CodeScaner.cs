using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.CodeProcessor
{
 

    public class CodeLine
    {
        public int CodeIndex
        {
            set;
            get;
        }

        public string KeyCode
        {
            set;
            get;
        }

        public Dictionary<string, string> ParamsPool
        {
            set;
            get;
        }

        public bool RunningStam
        {
            set;
            get;
        }

        public CodeLine()
        {
            ParamsPool = new Dictionary<string, string>();
        }

        public void SetParam(string paramName,string paramValue)
        {
            if(!ParamsPool.ContainsKey(paramName))            
                ParamsPool.Add(paramName, paramValue);            
        }

        public string GetParam(string paramName)
        {
            try
            {
                if (ParamsPool.ContainsKey(paramName))
                    return ParamsPool[paramName];
                else
                    return "";
            }
            catch
            {
                return "";
            }
        }               
       
    }    

    public class CodeScaner
    {

        private Container.KeyCodeContainer _KeyCodeContainerObj = new Container.KeyCodeContainer();
        private Dictionary<int, CodeLine> _CodePool = new Dictionary<int, CodeLine>();
        

        public Dictionary<int,CodeLine> CodePool
        {
            get
            {
                return _CodePool;
            }
        }

        public void Action_FlushCodePool()
        {
            List<CodeLine> tmpCodeLinePool = new List<CodeLine>();
            foreach(int codeLineIndex in _CodePool.Keys)            
                tmpCodeLinePool.Add(_CodePool[codeLineIndex]);
            for (int i = 1; i <= tmpCodeLinePool.Count; i++)
            {
                tmpCodeLinePool[i - 1].CodeIndex = i;
                _CodePool.Add(i, tmpCodeLinePool[i - 1]);
            }
        }

        public void Action_Scan(string code)
        {
            string[] CodeLines = code.Split(';');
            int countIndex=1;
            foreach(string CodeLine in CodeLines)
            {
                if (!string.IsNullOrEmpty(CodeLine))
                {
                    string[] KeyCodes = CodeLine.Split(' ');                    
                    if (!_KeyCodeContainerObj.Checking_KeyCode(KeyCodes[0]))
                        Container.GlobalObjsPoolContainer.GlobalObject_MessageContainer.Action_InsertMessage(countIndex, Container.CodeErrMessage.InvalidatedKeyWord, Container.ErrLevel.Normal, true);
                    else
                    {
                        CodeLine newLine = new CodeLine();
                        newLine.KeyCode = KeyCodes[0];
                        if (KeyCodes.Length >= 2)
                        {
                            for (int i = 1; i < KeyCodes.Length; i++)
                            {
                                string[] activeParam = KeyCodes[i].Split('$');
                                if (activeParam.Length == 2)
                                {
                                    activeParam[1] = activeParam[1].Replace("%S", " ");
                                    newLine.SetParam(activeParam[0], activeParam[1]);
                                }
                                else
                                    newLine.SetParam("Param", activeParam[0]);
                            }
                        }
                        newLine.CodeIndex = countIndex;
                        _CodePool.Add(_CodePool.Count + 1, newLine);
                    }                    
                }
                countIndex++;
            }
        }
    }
}

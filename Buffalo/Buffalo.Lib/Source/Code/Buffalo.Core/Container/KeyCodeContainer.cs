using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.Container
{

    public class KeyWordMap
    {
        public const string Select = "Select";
        public const string Action = "Action";
        public const string WBAction = "WBAction";
        public const string Connect = "Connect";
        public const string Case = "Case";
        public const string ImportExcel = "ImportExcel";
        public const string ImportDB = "ImportDB";
        public const string ImportXML = "ImportXML";
        public const string DataFill = "DataFill";
        public const string DataSet = "DataSet";
        public const string Null = "Null";
    }

    public class KeyCodeContainer
    {
        private List<string> _KeyCodePool = new List<string>();        

        public KeyCodeContainer()
        {
            _KeyCodePool.Add(KeyWordMap.Select);
            _KeyCodePool.Add(KeyWordMap.Action);
            _KeyCodePool.Add(KeyWordMap.Null);
            _KeyCodePool.Add(KeyWordMap.Case);
            _KeyCodePool.Add(KeyWordMap.Connect);
            _KeyCodePool.Add(KeyWordMap.WBAction);
            _KeyCodePool.Add(KeyWordMap.ImportExcel);
            _KeyCodePool.Add(KeyWordMap.ImportDB);
            _KeyCodePool.Add(KeyWordMap.ImportXML);
            _KeyCodePool.Add(KeyWordMap.DataFill);
            _KeyCodePool.Add(KeyWordMap.DataSet);
        }

        public bool Checking_KeyCode(string KeyCode)
        {
            if (KeyCode != "")
            {
                if (_KeyCodePool.Contains(KeyCode))
                    return true;
                else
                    return false;
            }
            else
                return false;
        }

    }
}

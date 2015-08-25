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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Buffalo.Controls
{
    public class BrowserHistory
    {
        private int _currentURLIndex;
        private List<string> _hisrotyList;
        private UCWebBrowserEx _ucWebBrowserEx;

        public BrowserHistory(UCWebBrowserEx browserEx)
        {
            this._currentURLIndex = -1;
            this._hisrotyList = new List<string>();
            this._ucWebBrowserEx = browserEx;
        }

        public int CurrentURLIndex
        {
            get
            {
                return this._currentURLIndex;
            }
        }

        public string CurrentURLString
        {
            get
            {
                return this._hisrotyList[this._currentURLIndex];
            }
        }

        public int HistoryCount
        {
            get
            {
                return this._hisrotyList.Count;
            }
        }

        public void addHistory(string urlStr)
        {
            this._currentURLIndex++;
            this._hisrotyList.Add(urlStr);
        }

        public string goBack()
        {
            if (this._ucWebBrowserEx.GoBack())
            {
                if (this._currentURLIndex > 0)
                {
                    this._currentURLIndex--;
                }
                return this._hisrotyList[this._currentURLIndex];
            }
            else
            {
                return "";
            }
        }

        public string goFoward()
        {
            if (this._ucWebBrowserEx.GoForward())
            {
                if (this._currentURLIndex < this._hisrotyList.Count - 1)
                {
                    this._currentURLIndex++;
                }
                return this._hisrotyList[this._currentURLIndex];
            }
            else
            {
                return "";
            }
        }
    }
}

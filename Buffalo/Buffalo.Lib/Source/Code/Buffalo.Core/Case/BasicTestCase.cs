using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.Case
{

    public class LoopItem
    {
        public int StartIndex
        {
            set;
            get;
        }

        public int EndIndex
        {
            set;
            get;
        }

        public int LoopCount
        {
            set;
            get;
        }

        public int ActiveLoopCount
        {
            set;
            get;
        }

    }

    public class BasicTestCase
    {

        public Dictionary<int, CaseContentItem> ActiveCaseContentPool
        {
            set;
            get;
        }

        public Dictionary<int, CaseMethodItem> ActiveCaseMethodPool
        {
            set;
            get;
        }

        public List<LoopItem> ActiveLoopPool
        {
            set;
            get;
        }

        public WebDriver.WebBrowserDriver ActiveWebBrowserDriverObject
        {
            set;
            get;
        }

        public WebDriver.WebBrowserActions ActionWebBrowserActionsObject
        {
            set;
            get;
        }

        public ElementActions.ElementActions ActiveElementActionObject
        {
            set;
            get;
        }

        public ElementActions.ElementSelector ActiveElementSelectorObject
        {
            set;
            get;
        }

        public BasicTestCase()
        {
            ActiveCaseContentPool = new Dictionary<int, CaseContentItem>();
            ActiveCaseMethodPool = new Dictionary<int, CaseMethodItem>();
            ActiveLoopPool = new List<LoopItem>();
        }
        
        public string CaseName
        {
            set;
            get;
        }

        public string CaseID
        {
            set;
            get;
        }



    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

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

        public CaseReport ActiveTestCaseReport
        {
            set;
            get;
        }

        public string TestcaseCuurentFile
        {
            set;
            get;
        }

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

        public Dictionary<int, CaseMethodItem> ActiveCaseWebBrowserPool
        {
            set;
            get;
        }

        public Dictionary<int, CaseMethodItem> ActiveCaseSelectorPool
        {
            set;
            get;
        }

        public Dictionary<string, XmlDocument> ActiveCaseDataSourcePool
        {
            set;
            get;
        }

        public bool SingleInterrupt
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

        public Dictionary<string, string> ActiveDataBuffer
        {
            set;
            get;
        }

        public BasicTestCase()
        {
            ActiveCaseContentPool = new Dictionary<int, CaseContentItem>();
            ActiveCaseMethodPool = new Dictionary<int, CaseMethodItem>();
            ActiveCaseWebBrowserPool = new Dictionary<int, CaseMethodItem>();
            ActiveCaseSelectorPool = new Dictionary<int, CaseMethodItem>();
            ActiveCaseDataSourcePool = new Dictionary<string, XmlDocument>();
            ActiveLoopPool = new List<LoopItem>();
            ActiveTestCaseReport = new CaseReport();
            ActiveDataBuffer = new Dictionary<string, string>();
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

        public int CaseCodeLineCount
        {
            set;
            get;
        }

    }
}

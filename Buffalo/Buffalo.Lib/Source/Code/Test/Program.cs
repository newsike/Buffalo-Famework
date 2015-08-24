using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Buffalo.Basic;
using Buffalo.Core;
using Buffalo.CaseBasic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            Console.WriteLine("Testing");
            Buffalo.Core.WebDriver.WebBrowserDriver obj = Buffalo.Core.WebDriver.WebBrowserDriver.CreateInstanceForWebDriver(Buffalo.Core.WebDriver.WebBrowserType.Chrome);
            Buffalo.Core.WebDriver.WebBrowserActions actiobObj = new Buffalo.Core.WebDriver.WebBrowserActions(obj.ActiveWebDriver);
            actiobObj.Action_StartBrowser("http://www.baidu.com");
            Buffalo.Core.ElementActions.ElementSelector selector = new Buffalo.Core.ElementActions.ElementSelector(obj.ActiveWebDriver);
            selector.Action_SelectElementByXPATH("/html/body");            
            Console.ReadLine();
            obj.QuitInstance();
            
             */
            Form1 obj = new Form1();
            obj.ShowDialog();
        }
    }
}

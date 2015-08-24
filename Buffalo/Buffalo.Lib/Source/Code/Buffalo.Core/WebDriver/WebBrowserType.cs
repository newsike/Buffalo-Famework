using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Buffalo.Core.WebDriver
{
    public enum WebBrowserType
    {
        InternetExplorer=1,
        Chrome=2,
        FireFox=3,
        Safari=4,
        InternetExplorerForRC=5,
        ChromeForRC=6,
        FireFoxForRC=7,
        SafariForRC=8
    }
 
    public enum SeleniumConnectedType
    {
        WebDriver=1,
        SeleniumServer=2
    }

}

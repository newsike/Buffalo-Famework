using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Selenium.Internal.SeleniumEmulation;
using Selenium;
using Selenium.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using Buffalo.Basic.Base;

namespace Buffalo.Core.WebDriver
{
    public class WebBrowserDriver
    {

        public bool IsRemoteRC
        {
            set;
            get;
        }

        public WebBrowserType ActiveBrowserType
        {
            set;
            get;
        }

        public SeleniumConnectedType ConnectedType
        {
            set;        
            get;
        }

        public IWebDriver ActiveWebDriver
        {
            set;
            get;
        }

        public ISelenium ActiveConnectedServerObject
        {
            set;
            get;
        }

        public static WebBrowserDriver CreateInstanceForSeleniumServer(WebBrowserType BrowserType,string Server,int Port,string StartURL,string OptionPathForBrowser)
        {
            try
            {
                WebBrowserDriver activeObj = new WebBrowserDriver();
                switch (BrowserType)
                {
                    default:
                    case WebBrowserType.InternetExplorerForRC:
                        activeObj.ActiveConnectedServerObject = new DefaultSelenium(Server, Port, OptionPathForBrowser == "" ? "*iexplore" : OptionPathForBrowser, StartURL);
                        break;
                    case WebBrowserType.FireFoxForRC:
                        activeObj.ActiveConnectedServerObject = new DefaultSelenium(Server, Port, OptionPathForBrowser == "" ? "*firefox" : OptionPathForBrowser, StartURL);
                        break;
                    case WebBrowserType.ChromeForRC:
                    case WebBrowserType.SafariForRC:
                        activeObj.ActiveConnectedServerObject = new DefaultSelenium(Server, Port, OptionPathForBrowser, StartURL);
                        break;
                }
                activeObj.ActiveBrowserType = BrowserType;
                activeObj.ConnectedType = SeleniumConnectedType.SeleniumServer;
                activeObj.IsRemoteRC = true;
                return activeObj;
            }
            catch(ExtendedExcptions err)
            {
                return null;
            }
        }

        public void QuitInstance()
        {
            if(IsRemoteRC==false)
            {
                if(ActiveWebDriver!=null)                
                    ActiveWebDriver.Quit();                
            }
            else
            {
                if (ActiveConnectedServerObject != null)
                    ActiveConnectedServerObject.Close();
            }
        }
       
        public static WebBrowserDriver CreateInstanceForWebDriver(WebBrowserType BrowserType)
        {
            try
            {
                WebBrowserDriver activeObj = new WebBrowserDriver();
                switch (BrowserType)
                {
                    case WebBrowserType.Chrome:
                        activeObj.ActiveWebDriver = new ChromeDriver();
                        break;
                    case WebBrowserType.FireFox:
                        activeObj.ActiveWebDriver = new FirefoxDriver();
                        break;
                    case WebBrowserType.InternetExplorer:
                        activeObj.ActiveWebDriver = new InternetExplorerDriver();
                        break;
                    case WebBrowserType.Safari:
                        activeObj.ActiveWebDriver = new SafariDriver();
                        break;
                    default:
                        activeObj.ActiveWebDriver = new InternetExplorerDriver();
                        break;

                }
                activeObj.ActiveBrowserType = BrowserType;
                activeObj.ConnectedType = SeleniumConnectedType.WebDriver;
                activeObj.IsRemoteRC = false;
                return activeObj;
            }
            catch(ExtendedExcptions err)
            {
                return null;
            }
        }
    }
}

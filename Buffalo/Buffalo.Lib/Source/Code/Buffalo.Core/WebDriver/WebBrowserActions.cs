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
using System.Reflection;

namespace Buffalo.Core.WebDriver
{

    public class WebBrowserMethodItem
    {

        private object _returnValue;

        public Type ActiveType
        {
            set;
            get;
        }

        public MethodInfo ActiveMethod
        {
            set;
            get;
        }

        public object[] MethodParams
        {
            set;
            get;
        }

        public object MethodReturnValue
        {
            get
            {
                return _returnValue;
            }
        }

        public void DoInvoke()
        {
            if (ActiveType != null && ActiveMethod != null)
            {
                object activeObject = System.Activator.CreateInstance(ActiveType);
                _returnValue = ActiveMethod.Invoke(activeObject, MethodParams);
            }
        }

        public int CountCalled
        {
            set;
            get;
        }
    }

    public class WebBrowserActionsMap
    {
        public const string Method_Action_WB_Action_StartBrowser = "Action_StartBrowser";
        public const string Method_Action_WB_Action_CloseWindow = "Action_CloseWindow";
        public const string Method_Action_WB_Action_SwitchToWindow = "Action_SwitchToWindow";
        public const string Method_Action_WB_Action_GetSourceOfPage = "Action_GetSourceOfPage";
        public const string Method_Action_WB_Action_GetCurrentWindowHandle = "Action_GetCurrentWindowHandle";
        public const string Method_Action_WB_Action_Action_IsAlert = "Action_IsAlert";
        public const string Method_Action_WB_Action_Action_Wait = "Action_Wait";
        public const string Method_Action_WB_Action_Action_GetCurrentURL = "Action_GetCurrentURL";
    }

    public class WebBrowserActions
    {
        private IWebDriver _activeWebDriverObj;

        public WebBrowserActions(IWebDriver activeWebDriverObj)
        {
            _activeWebDriverObj = activeWebDriverObj;
        }

        public void ResetWebDriverObj()
        {
            _activeWebDriverObj = null;
        }

        public void Action_SetWebDriverObj(IWebDriver activeWebDriverObj)
        {
            _activeWebDriverObj = activeWebDriverObj;
        }

        public void Action_Wait(string Time)
        {
            int timeSpan=1000;
            int.TryParse(Time,out timeSpan);
            Thread.Sleep(timeSpan);
        }

        public void Action_StartBrowser(string URL)
        {
            if(_activeWebDriverObj!=null)
            {
                _activeWebDriverObj.Navigate().GoToUrl(URL);                
            }
        }
 
        public void Action_Close()
        {
            try
            {
                _activeWebDriverObj.Close();
            }
            catch
            {

            }
        }
        
        public void Action_CloseWindow(string windowsHandle="")
        {
            if(windowsHandle=="")
            {
                string currentHandle = _activeWebDriverObj.CurrentWindowHandle;
                _activeWebDriverObj.SwitchTo().Window(currentHandle).Close();                
            }
            else
            {
                IList<string> allWindowHandles = _activeWebDriverObj.WindowHandles;
                if (allWindowHandles.Contains(windowsHandle))
                    _activeWebDriverObj.SwitchTo().Window(windowsHandle).Close();                
            }
        }

        public void Action_CloseAllWindow()
        {
            IList<string> allWindowHandles = _activeWebDriverObj.WindowHandles;
            foreach (string activeWindowHandle in allWindowHandles)
            {
                try
                {
                    _activeWebDriverObj.SwitchTo().Window(activeWindowHandle).Close();
                }
                catch
                {
                    continue;
                }
            }
        }

        public void Action_SwitchToWindow(string windowsHandleTitle="")
        {
            IList<string> allWindowHandles = _activeWebDriverObj.WindowHandles;
            string currentHandle= _activeWebDriverObj.CurrentWindowHandle;
            foreach(string activeWindowsHandel in allWindowHandles)
            {
                _activeWebDriverObj.SwitchTo().Window(activeWindowsHandel);
                string Title=_activeWebDriverObj.Title;
                if(Title==windowsHandleTitle)
                {
                    return;
                }
            }
            _activeWebDriverObj.SwitchTo().Window(currentHandle);    
                  
        }

        public string Action_GetCurrentURL()
        {
            return _activeWebDriverObj.Url;
        }

        public string Action_GetSourceOfPage(string windowsHandle="")
        {
            if(windowsHandle=="")
            {
                string currentWindowsHandle=_activeWebDriverObj.CurrentWindowHandle;
                return _activeWebDriverObj.SwitchTo().Window(currentWindowsHandle).PageSource;                
            }
            else
            {
                Action_SwitchToWindow(windowsHandle);
                return _activeWebDriverObj.SwitchTo().Window(windowsHandle).PageSource;
            }
        }

        public string Action_GetCurrentWindowHandle()
        {                     
            return _activeWebDriverObj.CurrentWindowHandle;
        }

        public bool Action_IsAlert()
        {
            try
            {
                _activeWebDriverObj.SwitchTo().Alert();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public string Action_GetMessageFromCurrentAlerts()
        {
            if (Action_IsAlert())
                return _activeWebDriverObj.SwitchTo().Alert().Text;
            else
                return "";

        }

        public void Action_NavigateSelectedWindow(string windowsHandle,string URL,bool switchToNewWindow=false,string optionalWindowHandle="")
        {
            if (windowsHandle != "")
                Action_SwitchToWindow(windowsHandle);
            Action_Navigate(URL, switchToNewWindow, optionalWindowHandle);
        }

        public bool Action_Navigate(string URL,bool switchToNewWindow=false,string optionalWindowHandle="")
        {
            if (_activeWebDriverObj != null)
            {
                try
                {

                    string currentHandle = "";
                    if (switchToNewWindow)
                        currentHandle = _activeWebDriverObj.CurrentWindowHandle;
                    _activeWebDriverObj.Navigate().GoToUrl(URL);                    
                    if(switchToNewWindow)
                    {
                        
                        IList<string> newWindowHandlesList = _activeWebDriverObj.WindowHandles;
                        if (newWindowHandlesList.Contains(optionalWindowHandle))
                            _activeWebDriverObj.SwitchTo().Window(optionalWindowHandle);
                        else
                            if (newWindowHandlesList.Count > 0)
                                _activeWebDriverObj.SwitchTo().Window(newWindowHandlesList[0]);
                            else
                                return false;
                    }
                }
                catch (ExtendedExcptions err)
                {
                    return false;
                }
            }           
            return true;
        }

    }
}

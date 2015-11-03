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

namespace Buffalo.Core.ElementActions
{

    public class MethodItem
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

        public void DoInvoke(object[] ConsParams)
        {
            if (ActiveType != null && ActiveMethod != null)
            {
                object activeObject = System.Activator.CreateInstance(ActiveType, ConsParams);
                _returnValue = ActiveMethod.Invoke(activeObject, MethodParams);
            }
        }

        public int CountCalled
        {
            set;
            get;
        }
    }

    public class ActionMap
    {
        public const string Method_Action_Click="Action_Click";
        public const string Method_Action_SetText = "Action_SetText";
        public const string Method_Action_ClearContent = "Action_ClearContent";
        public const string Method_Action_ReplaceContent = "Action_ReplaceContent";
        public const string Method_Get_Content = "Get_Content";
        public const string Method_Get_IsEnabled = "Get_IsEnabled";
        public const string Method_Get_IsSelected = "Get_IsSelected";
        public const string Method_Get_IsDisplayed = "Get_IsDisplayed";
        public const string Method_Action_SelectOptionByText = "Action_SelectOptionByText";
        public const string Method_Action_SelectOptionByIndex = "Action_SelectOptionByIndex";
        public const string Method_Action_SelectOptionByValue = "Action_SelectOptionByValue";
        public const string Method_Get_Content_Store = "Get_Content_Store";

    }    

    public class ElementActions
    {

        private ElementItem _selectedElementItem;

        public ElementActions(ElementItem selectedElementItem)
        {
            _selectedElementItem = selectedElementItem;
        }

        public void Action_Click()
        {
            if (_selectedElementItem.refElement != null)
                _selectedElementItem.refElement.Click();
        }

        public void Action_SetText(string text)
        {
            _selectedElementItem.refElement.SendKeys(text);
        }

        public void Action_ClearContent()
        {
            _selectedElementItem.refElement.Clear();
        }

        public void Action_ReplaceContent(string text)
        {
            _selectedElementItem.refElement.Clear();
            _selectedElementItem.refElement.SendKeys(text);                        

        }

        public string Get_Content()
        {
            return _selectedElementItem.refElement.Text;            
        }

        public void Get_Content_Store(Case.BasicTestCase refTestCase,string DataPonitName)
        {
            string value = _selectedElementItem.refElement.Text;
            if (refTestCase.ActiveDataBuffer.ContainsKey(DataPonitName))
                refTestCase.ActiveDataBuffer[DataPonitName] = value;
            else
                refTestCase.ActiveDataBuffer.Add(DataPonitName, value);
        }

        public bool Get_IsEnabled()
        {
            return _selectedElementItem.refElement.Enabled;
        }

        public bool Get_IsSelected()
        {
            return _selectedElementItem.refElement.Selected;
        }

        public bool Get_IsDisplayed()
        {
            return _selectedElementItem.refElement.Displayed;
        }

        public void Action_SelectOptionByText(string text)
        {
            SelectElement selectedElementObj = new SelectElement(_selectedElementItem.refElement);
            if(selectedElementObj!=null)            
                selectedElementObj.SelectByText(text);                        
        }

        public void Action_SelectOptionByIndex(int index)
        {
            SelectElement selectedElementObj = new SelectElement(_selectedElementItem.refElement);
            if (selectedElementObj != null)
            {
                if (index >= 0)
                {
                    selectedElementObj.SelectByIndex(index);
                }
            }
        }

        public void Action_SelectOptionByValue(string value)
        {
            SelectElement selectedElementObj = new SelectElement(_selectedElementItem.refElement);
            if (selectedElementObj != null)
                selectedElementObj.SelectByValue(value);            
        }         

    }
}

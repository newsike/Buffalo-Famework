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

namespace Buffalo.Core.ElementActions
{

    public class HtmlElementDes
    {
        public const string HtmlDIV = "div";
        public const string HtmlSpan = "span";
        public const string HtmlImg = "img";
        public const string HtmlLink = "Link";
        public const string HtmlP = "p";
        public const string HtmlHeader = "head";
        public const string HtmlInput = "input";
        public const string HtmlTable = "table";
        public const string HtmlTableRow = "tr";
        public const string HtmlTableCell = "td";
    }

    public class HtmlFindMapType
    {
        public const string byXpath = "xpath";
        public const string byTag = "tag";
        public const string byID = "id";
        public const string byClassName = "classname";
        public const string byLinkText = "linktext";
        public const string byName = "name";
    }

    public class ElementItem
    {

        public ElementItem()
        {
            ElementFindMap = new Dictionary<string, string>();
        }

        public string ElementDes
        {
            set;
            get;
        }

        public IWebElement refElement
        {
            set;
            get;
        }

        public string ElementTag
        {
            set;
            get;        
        }        

        public Dictionary<string, string> ElementFindMap
        {
            get;
            set;
        }

    }    

    public class ElementSelector
    {
        private IWebDriver _activeWebDriverObj;

        public ElementSelector(IWebDriver activeWebDriverObj)
        {
            _activeWebDriverObj = activeWebDriverObj;
        }

        public void SetWebDriverObj(IWebDriver activeWebDriverObj)
        {
            _activeWebDriverObj = activeWebDriverObj;
        }

        public void ResetWebDriverObj()
        {
            _activeWebDriverObj = null;
        }


        public void Action_SelectElementByName(string Name, ElementItem refElementItem)
        {
            try
            {
                if (Name != "" && refElementItem != null)
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.Name(Name));
                    if (selectedElement != null)
                    {
                        if (!refElementItem.ElementFindMap.ContainsKey(HtmlFindMapType.byName))
                            refElementItem.ElementFindMap.Add(HtmlFindMapType.byName, Name);
                        else
                            refElementItem.ElementFindMap[HtmlFindMapType.byName] = Name;
                        refElementItem.ElementTag = selectedElement.TagName;
                        refElementItem.refElement = selectedElement;
                    }
                }
            }
            catch
            {
            }
        }

        public ElementItem Action_SelectElementByName(string Name)
        {
            try
            {
                if (Name != "")
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.Name(Name));
                    if (selectedElement != null)
                    {
                        ElementItem newElementItem = new ElementItem();
                        newElementItem.ElementFindMap.Add(HtmlFindMapType.byName, Name);
                        newElementItem.ElementTag = selectedElement.TagName;
                        newElementItem.refElement = selectedElement;
                        return newElementItem;
                    }
                    return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        } 

        public void Action_SelectElementByLinkText(string LinkText, ElementItem refElementItem)
        {
            try
            {
                if (LinkText != "" && refElementItem != null)
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.LinkText(LinkText));
                    if (selectedElement != null)
                    {
                        if (!refElementItem.ElementFindMap.ContainsKey(HtmlFindMapType.byLinkText))
                            refElementItem.ElementFindMap.Add(HtmlFindMapType.byLinkText, LinkText);
                        else
                            refElementItem.ElementFindMap[HtmlFindMapType.byLinkText] = LinkText;
                        refElementItem.ElementTag = selectedElement.TagName;
                        refElementItem.refElement = selectedElement;
                    }
                }
            }
            catch
            {
            }
        }

        public ElementItem Action_SelectElementByLinkText(string LinkText)
        {
            try
            {
                if (LinkText != "")
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.LinkText(LinkText));
                    if (selectedElement != null)
                    {
                        ElementItem newElementItem = new ElementItem();
                        newElementItem.ElementFindMap.Add(HtmlFindMapType.byLinkText, LinkText);
                        newElementItem.ElementTag = selectedElement.TagName;
                        newElementItem.refElement = selectedElement;
                        return newElementItem;
                    }
                    return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        } 

        public void Action_SelectElementByClassname(string Classname, ElementItem refElementItem)
        {
            try
            {
                if (Classname != "" && refElementItem != null)
                {                    
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.ClassName(Classname));
                    if (selectedElement != null)
                    {
                        if (!refElementItem.ElementFindMap.ContainsKey(HtmlFindMapType.byTag))
                            refElementItem.ElementFindMap.Add(HtmlFindMapType.byClassName, Classname);
                        else
                            refElementItem.ElementFindMap[HtmlFindMapType.byClassName] = Classname;
                        refElementItem.ElementTag = selectedElement.TagName;
                        refElementItem.refElement = selectedElement;
                    }
                }
            }
            catch
            {
            }
        }

        public ElementItem Action_SelectElementByClassname(string Classname)
        {
            try
            {
                if (Classname != "")
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.ClassName(Classname));
                    if (selectedElement != null)
                    {
                        ElementItem newElementItem = new ElementItem();
                        newElementItem.ElementFindMap.Add(HtmlFindMapType.byClassName, Classname);
                        newElementItem.ElementTag = selectedElement.TagName;
                        newElementItem.refElement = selectedElement;
                        return newElementItem;
                    }
                    return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        } 

        public void Action_SelectElementByID(string ID, ElementItem refElementItem)
        {
            try
            {
                if (ID != "" && refElementItem != null)
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.Id(ID));
                    if (selectedElement != null)
                    {
                        if (!refElementItem.ElementFindMap.ContainsKey(HtmlFindMapType.byTag))
                            refElementItem.ElementFindMap.Add(HtmlFindMapType.byID, ID);
                        else
                            refElementItem.ElementFindMap[HtmlFindMapType.byID] = ID;
                        refElementItem.ElementTag = selectedElement.TagName;
                        refElementItem.refElement = selectedElement;
                    }
                }
            }
            catch
            {
            }
        }

        public ElementItem Action_SelectElementByID(string ID)
        {
            try
            {
                if (ID != "")
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.Id(ID));
                    if (selectedElement != null)
                    {
                        ElementItem newElementItem = new ElementItem();
                        newElementItem.ElementFindMap.Add(HtmlFindMapType.byID, ID);
                        newElementItem.ElementTag = selectedElement.TagName;
                        newElementItem.refElement = selectedElement;
                        return newElementItem;
                    }
                    return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        } 

        public void Action_SelectElementByTag(string Tag,ElementItem refElementItem)
        {
            try
            {
                if (Tag != "" && refElementItem != null)
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.TagName(Tag));
                    if (selectedElement != null)
                    {
                        if (!refElementItem.ElementFindMap.ContainsKey(HtmlFindMapType.byTag))
                            refElementItem.ElementFindMap.Add(HtmlFindMapType.byTag, Tag);
                        else
                            refElementItem.ElementFindMap[HtmlFindMapType.byTag] = Tag;
                        refElementItem.ElementTag = selectedElement.TagName;
                        refElementItem.refElement = selectedElement;
                    }
                }
            }
            catch
            {
            }
        }

        public ElementItem Action_SelectElementByTag(string Tag)
        {
            try
            {
                if (Tag != "")
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.TagName(Tag));
                    if (selectedElement != null)
                    {
                        ElementItem newElementItem = new ElementItem();
                        newElementItem.ElementFindMap.Add(HtmlFindMapType.byTag, Tag);
                        newElementItem.ElementTag = selectedElement.TagName;
                        newElementItem.refElement = selectedElement;
                        return newElementItem;
                    }
                    return null;
                }
                else
                    return null;
            }
            catch
            {
                return null;
            }
        } 

        public void Action_SelectElementByXPATH(string XPATH,ElementItem refElementItem)
        {
            try
            {
                if (XPATH != "" && refElementItem!=null)
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.XPath(XPATH));
                    if (selectedElement != null)
                    {
                        if (!refElementItem.ElementFindMap.ContainsKey(HtmlFindMapType.byXpath))
                            refElementItem.ElementFindMap.Add(HtmlFindMapType.byXpath, XPATH);
                        else
                            refElementItem.ElementFindMap[HtmlFindMapType.byXpath] = XPATH;
                        refElementItem.ElementTag = selectedElement.TagName;
                        refElementItem.refElement = selectedElement;
                    }                   
                }
             }
            catch
            {                
            }
        }

        public ElementItem Action_SelectElementByXPATH(string XPATH)
        {
            try
            {
                if (XPATH != "")
                {
                    IWebElement selectedElement = _activeWebDriverObj.FindElement(By.XPath(XPATH));
                    if (selectedElement != null)
                    {
                        ElementItem newElementItem = new ElementItem();
                        newElementItem.ElementFindMap.Add(HtmlFindMapType.byXpath, XPATH);
                        newElementItem.ElementTag = selectedElement.TagName;
                        newElementItem.refElement = selectedElement;
                        return newElementItem;
                    }
                    return null;
                }
                else
                    return null;                
            }
            catch
            {
                return null;
            }
        }

    }
}

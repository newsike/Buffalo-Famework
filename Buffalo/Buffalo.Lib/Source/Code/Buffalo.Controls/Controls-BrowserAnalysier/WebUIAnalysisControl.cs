using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;

namespace Buffalo.Controls
{
    public partial class WebUIAnalysisControl: UserControl
    {
        public WebUIAnalysisControl()
        {
            InitializeComponent();
        }

        private string getTagString(HtmlElement element)
        {
            string detailStr = element.OuterHtml;
            if (!string.IsNullOrEmpty(element.InnerHtml))
            {
                detailStr = detailStr.Replace(element.InnerHtml, "");
            }
            int tmpIndex = detailStr.IndexOf("><");
            if (tmpIndex >= 0)
            {
                detailStr = detailStr.Substring(0, tmpIndex + 1);
            }
            return detailStr;
        }

        private void btn_StartAnalyse_Click(object sender, EventArgs e)
        {
            UCWebBrowserEx browser = this.ucWebBrowser.Browser;
            HtmlElement body = browser.Document.Body;            
            this.treeView_Tags.Nodes.Clear();
            this.treeView_Tags.Nodes.Add(this.analysisElement(body));           
        }

        private void btn_FindElement_Click(object sender, EventArgs e)
        {
            this.btn_StartAnalyse_Click(sender, e);
            this.ucWebBrowser.addBehavior(this.methodMouseOverOnBrowser, this.methodOnClickOnBrowser);
        }

        private TreeNode analysisElement(HtmlElement element)
        {            
            TreeNode newNode = new TreeNode(element.TagName.ToUpper() + "  " + this.getTagString(element));
            foreach(HtmlElement ele in  element.Children)
            {  
                newNode.Nodes.Add(this.analysisElement(ele));
            }
            return newNode;
        }

        private void methodMouseOverOnBrowser(HtmlElement element)
        {
            this.displayElementProperty(element);
        }

        private void methodOnClickOnBrowser(HtmlElement element)
        {
            this.displayElementProperty(element);
            this.lockElementInTree(element);
        }

        private void displayElementProperty(HtmlElement element)
        {
            this.listView_Properties.Items.Clear();

            HTMLElementProperty property = new HTMLElementProperty(element);

            ListViewItem newItem = new ListViewItem("Tag Name");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.TagName));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Id");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.Id));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Name");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.Name));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Class Name");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.ClassName));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Style");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.Style));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Client Top");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.ClientTop));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Client Left");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.ClientLeft));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Client Width");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.ClientWidth));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Client Height");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.ClientHeight));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Offset Top");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.OffsetTop));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Offset Left");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.OffsetLeft));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Offset Width");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.OffsetWidth));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Offset Height");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.OffsetHeight));
            this.listView_Properties.Items.Add(newItem);

            newItem = new ListViewItem("Inner Text");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.InnerText));
            this.listView_Properties.Items.Add(newItem);
            newItem = new ListViewItem("Inner HTML");
            newItem.SubItems.Add(new ListViewItem.ListViewSubItem(newItem, property.InnerHtml));
            this.listView_Properties.Items.Add(newItem);
        }

        private void lockElementInTree(HtmlElement element)
        {
            UCWebBrowserEx browser = this.ucWebBrowser.Browser;
            HtmlElement body = browser.Document.Body;
            this.treeView_Tags.Nodes.Clear();
            this.treeView_Tags.Nodes.Add(this.analysisElement(body));

            this.treeView_Tags.Nodes[0].Collapse();
            List<int> positionList = new List<int>();
            this.getElementPositionInDom(element, ref positionList);
            positionList.Reverse();
            this.findNodeInTree(positionList);
        }

        private void findNodeInTree(List<int> positionList)
        {
            TreeNode node = treeView_Tags.Nodes[0];
            node.Expand();
            foreach(int pos in positionList)
            {
                node = node.Nodes[pos];
                node.Expand();
            }
            node.BackColor = Color.Red;
        }

        private void getElementPositionInDom(HtmlElement element, ref List<int> positionList)
        {
            HtmlElement parent = element.Parent;
            positionList.Add(this.getIndexOfParentElement(element));
            if(parent.TagName.ToLower()!="body")
            {  
                this.getElementPositionInDom(parent, ref positionList);
            }            
        }

        private int getIndexOfParentElement(HtmlElement element)
        {
            HtmlElement parent = element.Parent;
            int index = 0;
            foreach (HtmlElement ele in parent.Children)
            {
                if (ele.Equals(element))
                {  
                    break;
                }
                index++;
            }
            return index;
        }

        private void treeView_Tags_AfterSelect(object sender, TreeViewEventArgs e)
        {
            List<int> positionList = new List<int>();
            TreeNode node = e.Node;
            while(!node.Equals(treeView_Tags.Nodes[0]))
            {
                positionList.Add(node.Index);
                node = node.Parent;
            }
            positionList.Reverse();

            this.ucWebBrowser.addBehavior(this.methodMouseOverOnBrowser, this.methodOnClickOnBrowser);
            this.findElementInDom(positionList);
        }

        private void findElementInDom(List<int> positionList)
        {
            UCWebBrowserEx browser = this.ucWebBrowser.Browser;
            HtmlElement element = browser.Document.Body;
            
            foreach (int pos in positionList)
            {
                element = element.Children[pos];
            }

            this.ucWebBrowser.addBehavior(this.methodMouseOverOnBrowser, this.methodOnClickOnBrowser);
            this.ucWebBrowser.MarkElement(element);
        }
    }

    public class HTMLElementProperty
    {
        private HtmlElement _element;
        private string _tagName;
        private string _id;
        private string _className;
        private string _style;
        private string _name;        
        private string _clientHeight;
        private string _clientWidth;
        private string _clientTop;
        private string _clientLeft;
        private string _offsetHeight;
        private string _offsetWidth;
        private string _offsetTop;
        private string _offsetLeft;
        private string _innerText;
        private string _innerHtml;

        public HTMLElementProperty(HtmlElement element)
        {
            this._element = element;
        }

        public HtmlElement Element
        {
            get
            {
                return this._element;
            }
        }

        public string TagName
        {
            get
            {
                return this._element.TagName;
            }
        }

        public string Id
        {
            get
            {
                return this._element.Id;
            }
        }

        public string ClassName
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).className ;
            }
        }

        public string Style
        {
            get
            {
                return this._element.GetAttribute("style");
            }
        }

        public string Name
        {
            get
            {
                return this._element.Name;
            }
        }

        public string ClientHeight
        {
            get
            {
                return ((IHTMLElement2)this._element.DomElement).clientHeight.ToString();
            }
        }

        public string ClientWidth
        {
            get
            {
                return ((IHTMLElement2)this._element.DomElement).clientWidth.ToString();
            }
        }

        public string ClientTop
        {
            get
            {
                return ((IHTMLElement2)this._element.DomElement).clientTop.ToString();
            }
        }

        public string ClientLeft
        {
            get
            {
                return ((IHTMLElement2)this._element.DomElement).clientLeft.ToString();
            }
        }

        public string OffsetHeight
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).offsetHeight.ToString();
            }
        }

        public string OffsetWidth
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).offsetWidth.ToString();
            }
        }

        public string OffsetTop
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).offsetTop.ToString();
            }
        }

        public string OffsetLeft
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).offsetLeft.ToString();
            }
        }

        public string InnerText
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).innerText;
            }
        }

        public string InnerHtml
        {
            get
            {
                return ((IHTMLElement)this._element.DomElement).innerHTML;
            }
        }
    }
}

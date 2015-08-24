using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WebUIAnalysisControlLib
{
    [ComVisibleAttribute(true)]
    public partial class UCWebBrowser : UserControl
    {  
        private Dictionary<int, BrowserHistory> urlHistoryDict;
        private BrowserHistory currentHistory;

        private MouseOverOnBrowser _methodMouseOverOnBrowser = null;
        private OnClickOnBrowser _methodOnClickOnBrowser = null;
        public delegate void MouseOverOnBrowser(HtmlElement targetEl);
        public delegate void OnClickOnBrowser(HtmlElement targetEl);

        public UCWebBrowser()
        {
            InitializeComponent();
            this.urlHistoryDict = new Dictionary<int, BrowserHistory>();
            this.currentHistory = new BrowserHistory(this.ucWBEx_Main);
            this.ucWBEx_Main.Url =new Uri("about:blank");
            this.Url = "about:blank";
            this.tabControl_Browser.TabPages[0].Text = "about:blank";
        }
        
        public UCWebBrowserEx Browser
        {
            get
            {
                return this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex);
            }
        }

        public string Url
        {
            get
            {
                return this.cmb_URL.Text;
            }
            set
            {
                this.cmb_URL.Text = value;
            }
        }

        private void ucWBEx_Main_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private UCWebBrowserEx getBrowserByTabIndex(int tabIndex)
        {
            return (UCWebBrowserEx)this.tabControl_Browser.TabPages[tabIndex].Controls[0];
        }

        private void changeButtonState()
        {
            this.btn_Browser_GoBack.Enabled = false;
            this.btn_Browser_GoForward.Enabled = false;
            if (this.currentHistory.CurrentURLIndex < this.currentHistory.HistoryCount - 1)
            {
                this.btn_Browser_GoForward.Enabled = true;
            }
            if (this.currentHistory.CurrentURLIndex > 0)
            {
                this.btn_Browser_GoBack.Enabled = true;
            }
        }

        private void tabControl_Browser_TabIndexChanged(object sender, EventArgs e)
        {
            if (!this.urlHistoryDict.Keys.Contains(this.tabControl_Browser.TabIndex))
            {
                this.urlHistoryDict.Add(this.tabControl_Browser.TabIndex, new BrowserHistory(this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex)));
            }
            this.currentHistory = this.urlHistoryDict[this.tabControl_Browser.TabIndex];
            this.cmb_URL.Text = this.currentHistory.CurrentURLString;
            this.changeButtonState();
        }

        private string validateURL(string urlStr)
        {
            if (urlStr.IndexOf("http://") != 0 && urlStr.IndexOf("https://") != 0 && urlStr.IndexOf("ftp://") != 0)
            {
                urlStr = "http://" + urlStr;
            }

            Regex regex = new Regex(@"((http|ftp|https)://)(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\&%_\./-~-]*)?");
            Match match = regex.Match(urlStr);
            if (!match.Success)
            {
                urlStr = "";
            }
            return urlStr;
        }

        private void navigateToURL(string urlStr)
        {
            this.cmb_URL.Items.Add(this.cmb_URL.Text);
            if (urlStr == "")
            {
                urlStr = this.validateURL(this.cmb_URL.Text.ToLower());
                if(urlStr != "")
                {
                    this.cmb_URL.Text = urlStr;
                    this.btn_Browser_GoBack.Enabled = true;
                    this.btn_Browser_GoForward.Enabled = false;
                    if (!this.urlHistoryDict.Keys.Contains(this.tabControl_Browser.TabIndex))
                    {
                        this.urlHistoryDict.Add(this.tabControl_Browser.TabIndex, new BrowserHistory(this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex)));
                    }
                    this.currentHistory = this.urlHistoryDict[this.tabControl_Browser.TabIndex];
                    currentHistory.addHistory(urlStr);
                }
            }
            else
            {
                urlStr = this.validateURL(urlStr);
            }
            if (urlStr != "")
            {
                this.ucWBEx_Main.Url = new Uri(urlStr);
                this.tabControl_Browser.SelectedTab.Text = urlStr;
            }
        }

        private void cmb_URL_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string urlStr = this.cmb_URL.Text.ToLower();
                foreach (string tmpStr in this.cmb_URL.Items)
                {
                    if (tmpStr.ToLower().IndexOf(urlStr) >= 0)
                    {
                        this.navigateToURL(tmpStr);
                        return;
                    }
                }
                this.navigateToURL("");
            }
        }

        private void btn_Browser_Go_Click(object sender, EventArgs e)
        {
            this.navigateToURL("");
        }

        private void cmb_URL_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.navigateToURL(this.cmb_URL.SelectedItem.ToString());
        }

        private void btn_Browser_GoBack_Click(object sender, EventArgs e)
        {
            string urlStr = this.currentHistory.goBack();
            if (urlStr == "")
            {
                this.btn_Browser_GoBack.Enabled = false;
            }
            else
            {
                this.changeButtonState();
            }
        }

        private void btn_Browser_GoForward_Click(object sender, EventArgs e)
        {
            string urlStr = this.currentHistory.goFoward();
            if (urlStr == "")
            {
                this.btn_Browser_GoForward.Enabled = false;
            }
            else
            {
                this.changeButtonState();
            }
        }

        private void ucWBEx_Main_BeforeNewWindow(object sender, WebBrowserExtendedNavigatingEventArgs e)
        {
            try
            {
                WebBrowserExtendedNavigatingEventArgs tmpEvent = (WebBrowserExtendedNavigatingEventArgs)e;
                beforeNewWindow(tmpEvent);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void beforeNewWindow(WebBrowserExtendedNavigatingEventArgs e)
        {
            //cancel = true will block the popup window
            if (e.Url.ToLower() == "about:blank") 
            {
                e.Cancel = false;
                return;
            }
            UCWebBrowserEx newWebBrowser = new UCWebBrowserEx();
            newWebBrowser.BeforeNewWindow += new EventHandler<WebBrowserExtendedNavigatingEventArgs>(WebBrowser_BeforeNewWindow);
            newWebBrowser.ObjectForScripting = this;
            newWebBrowser.Url = new Uri(e.Url);
            newWebBrowser.Dock = DockStyle.Fill; 

            TabPage newTabPage = new TabPage(Guid.NewGuid().ToString());
            newTabPage.AutoScroll = true;
            newTabPage.Controls.Add(newWebBrowser);
            //newTabPage.Tag = newWebPage;
            this.tabControl_Browser.TabPages.Add(newTabPage);

            this.tabControl_Browser.SelectTab(newTabPage);
        }

        private void WebBrowser_BeforeNewWindow(object sender, WebBrowserExtendedNavigatingEventArgs e)
        {
            try
            {
                beforeNewWindow(e);
            }
            catch (Exception ex)
            {
            }
        }

        public void addBehavior(MouseOverOnBrowser methodMouseOverOnBrowser, OnClickOnBrowser methodOnClickOnBrowser)
        {
            UCWebBrowserEx currentWb = this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex);
            this._methodMouseOverOnBrowser = methodMouseOverOnBrowser;
            this._methodOnClickOnBrowser = methodOnClickOnBrowser;
            currentWb.AddBehaviorModel(this.methodBeforeAddBehavior, this.methodAfterAddBehavior, this.methodThrowExcepotion, this.methodOnClickOnBrowser, this.methodMouseOverOnBrowser);
        }

        private void methodAfterAddBehavior(HtmlElement element, MarkBehavior markBehavior)
        {

        }

        private void methodBeforeAddBehavior(HtmlElement element)
        {
            if (element == null || string.IsNullOrEmpty(element.Id))
            {
                return;
            }
            string elStr = element.OuterHtml;
            //if (this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex).Document.Body.OuterHtml != this.richTextBox1.Text)
            //{
            //    this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex).Text = this.getBrowserByTabIndex(this.tabControl_Browser.TabIndex).Document.Body.OuterHtml;
            //}
            //int sIndex = this.richTextBox1.Text.IndexOf(elStr);
            //if (sIndex > 0)
            //{
            //    this.richTextBox1.Select(sIndex, elStr.Length);
            //}
        }

        private void methodThrowExcepotion(string exMessage)
        {

        }

        private void methodMouseOverOnBrowser(HtmlElement element)
        {
            this._methodMouseOverOnBrowser(element);
        }

        private void methodOnClickOnBrowser(HtmlElement element)
        {
            this._methodOnClickOnBrowser(element);
        }

        public void MarkElement(HtmlElement element)
        {
            this.Browser.MarkElement(element);
        }
    }
}

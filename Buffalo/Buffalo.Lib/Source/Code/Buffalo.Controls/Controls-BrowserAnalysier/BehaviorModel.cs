using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Buffalo.Controls
{
    public class BehaviorModel
    {
        private UCWebBrowserEx _webBrowser;
        private int _currentBehaviorCode;
        private HtmlElement _currentElement = null;
        private bool _elementFixed = false;

        private BeforeAddBehavior _methodBeforeAddBehavior = null;
        private AfterAddBehavior _methodAfterAddBehavior = null;
        private ThrowException _methodThrowException = null;
        private OnClick _methodClick = null;
        private MouseOverOnBrowser _methodMouseOverOnBrowser = null;

        public delegate void BeforeAddBehavior(HtmlElement currentElement);
        public delegate void AfterAddBehavior(HtmlElement currentElement, MarkBehavior markBehavior);
        public delegate void OnClick(HtmlElement currentElement);
        public delegate void ThrowException(string exMessage);
        public delegate void MouseOverOnBrowser(HtmlElement targetEl);

        public UCWebBrowserEx WebBrowser
        {
            set
            {
                this._webBrowser = value;
            }
            get
            {
                return this._webBrowser;
            }
        }

        public BehaviorModel(UCWebBrowserEx wb, BeforeAddBehavior methodBeforeAddBehavior, AfterAddBehavior methodAfterAddBehavior, ThrowException methodThrowException, OnClick methodOnClick, MouseOverOnBrowser methodMouseOverOnBrowser)
        {
            this._webBrowser = wb;
            this._methodBeforeAddBehavior = methodBeforeAddBehavior;
            this._methodAfterAddBehavior = methodAfterAddBehavior;
            this._methodThrowException = methodThrowException;
            this._methodClick = methodOnClick;
            this._methodMouseOverOnBrowser = methodMouseOverOnBrowser;

            this.AttachEvent();
        }

        private BehaviorModel()
        {

        }

        private void DetachMonitor()
        {
            this._webBrowser.Document.Body.Parent.MouseOver -= new HtmlElementEventHandler(this.Html_MouseMove);
            //this._webBrowser.Document.Click -= new HtmlElementEventHandler(this.Document_click);
            //this._webBrowser.Document.MouseDown -= new HtmlElementEventHandler(this.Document_click);
            try
            {
                if (this._currentElement != null)
                {
                    this._currentElement.Click -= new HtmlElementEventHandler(this.Element_click);
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                this._methodThrowException(ex.Message);
            }
        }

        private void RemoveBehavior()
        {
            if (this._currentBehaviorCode > 0)
            {
                bool flag = ((IHTMLElement2)this._webBrowser.Document.Body.Parent.DomElement).removeBehavior(this._currentBehaviorCode);
                this._currentBehaviorCode = -1;
            }
        }

        public void RestartBehavior()
        {
            this.RemoveBehavior();
            this._currentBehaviorCode = -1;
            this._currentElement = null;
            this._elementFixed = false;
            this.AttachEvent();
        }

        public void DetachBehavior()
        {
            this.DetachMonitor();
            this.RemoveBehavior();
        }

        private void AttachEvent()
        {
            this._webBrowser.Document.Body.Parent.MouseOver += new HtmlElementEventHandler(this.Html_MouseMove);
            //this._webBrowser.Document.Click += new HtmlElementEventHandler(this.Document_click);
            //this._webBrowser.Document.MouseDown += new HtmlElementEventHandler(this.Document_click);
        }

        private Rectangle GetAbsoluteRectangle(HtmlElement el)
        {
            int width = el.OffsetRectangle.Size.Width;
            int height = el.OffsetRectangle.Size.Height;

            int top = 0;
            int left = 0;
            while (el.OffsetParent != null)
            {
                top += el.OffsetRectangle.Top;
                left += el.OffsetRectangle.Left;
                el = el.OffsetParent;
            }

            left -= this.GetWindowScrollLeft();
            top -= this.GetWindowScrollTop();

            return new Rectangle(left, top, width, height);
        }

        private int GetWindowScrollTop()
        {
            Object[] objArray = new Object[1];
            objArray[0] = (Object)"window.pageYOffset.toString()";
            object scrollObj = this._webBrowser.Document.InvokeScript("eval", objArray);
            int scroll = 0;

            if (scrollObj != null && !int.TryParse(scrollObj.ToString(), out scroll))
            {
                scroll = 0;
            }
            return scroll;
        }

        private int GetWindowScrollLeft()
        {
            Object[] objArray = new Object[1];
            objArray[0] = (Object)"window.pageXOffset.toString()";
            object scrollObj = this._webBrowser.Document.InvokeScript("eval", objArray);
            int scroll = 0;
            if (scrollObj != null && !int.TryParse(scrollObj.ToString(), out scroll))
            {
                scroll = 0;
            }
            return scroll;
        }

        private void Html_MouseMove(object sender, HtmlElementEventArgs e)
        {
            try
            {
                HtmlDocument htmlDoc = this._webBrowser.Document;
                HtmlElement targetEl = htmlDoc.GetElementFromPoint(e.ClientMousePosition);
                this.MarkElement(targetEl);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.Message);
                this._methodThrowException(ex.Message);
            }
        }

        public void MarkElement(HtmlElement targetEl)
        {
            try
            {
                HtmlDocument htmlDoc = this._webBrowser.Document;
                HtmlElement html = htmlDoc.Body.Parent;

                this._methodBeforeAddBehavior(targetEl);

                if (this._currentElement != null && this._currentElement.Equals(targetEl))
                {
                    return;
                }

                if (this._currentBehaviorCode > 0)
                {
                    bool flag = ((IHTMLElement2)html.DomElement).removeBehavior(this._currentBehaviorCode);
                    this._currentBehaviorCode = -1;
                }
                this._currentElement = targetEl;

                MarkBehavior currentBehavior = new MarkBehavior();
                currentBehavior.SourceRectangle = this.GetAbsoluteRectangle(targetEl);
                currentBehavior.SourceElement = (IHTMLElement2)html.DomElement;

                //((HTMLElementEvents_Event)((IHTMLElement)targetEl.DomElement)).onclick += new HTMLElementEvents_onclickEventHandler(this.Element_click);
                try
                {
                    this._elementFixed = false;
                    targetEl.Click -= new HtmlElementEventHandler(this.Element_click);
                    targetEl.Click += new HtmlElementEventHandler(this.Element_click);
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message);
                    this._methodThrowException(ex.Message);
                }

                object obj = currentBehavior;
                this._currentBehaviorCode = currentBehavior.SourceElement.addBehavior(null, ref obj);
                this._methodAfterAddBehavior(targetEl, currentBehavior);
                this._methodMouseOverOnBrowser(targetEl);
            }
            catch(Exception ex)
            {
                this._methodThrowException(ex.Message);
            }
        }

        private void Element_click(object sender, HtmlElementEventArgs e)
        {
            if (!this._elementFixed)
            {
                e.BubbleEvent = false;
                e.ReturnValue = false;
                this.DetachMonitor();
                this._elementFixed = true;
                this._currentElement.Click -= new HtmlElementEventHandler(this.Element_click);
                this._methodClick(this._currentElement);
            }
        }
    }

}

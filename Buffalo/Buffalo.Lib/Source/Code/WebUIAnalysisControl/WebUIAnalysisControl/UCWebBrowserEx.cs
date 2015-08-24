using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.ComponentModel;
using System.Security;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.CompilerServices;
using System.Security.Permissions;

namespace WebUIAnalysisControlLib
{
    public partial class UCWebBrowserEx : System.Windows.Forms.WebBrowser
    {
        System.Windows.Forms.AxHost.ConnectionPointCookie cookie;

        WebBrowserExtendedEvents events;
        private UnsafeNativeMethods.IWebBrowser2 axIWebBrowser2;
        private BehaviorModel behaviorModel;

        public delegate void BeforeAddBehavior(HtmlElement currentElement);
        public delegate void AfterAddBehavior(HtmlElement currentElement, MarkBehavior markBehavior);
        public delegate void OnClickOnBrowser(HtmlElement currentElement);
        public delegate void ThrowException(string exMessage);
        public delegate void MouseOverOnBrowser(HtmlElement targetEl);

        private BeforeAddBehavior _methodBeforeAddBehavior = null;
        private AfterAddBehavior _methodAfterAddBehavior = null;
        private ThrowException _methodThrowException = null;
        private OnClickOnBrowser _methodOnClickOnBrowser = null;
        private MouseOverOnBrowser _methodMouseOverOnBrowser = null;

        public UCWebBrowserEx()
        {
#warning we should enable the code line below when the software is stable
            this.ScriptErrorsSuppressed = true;

        }

        public void AddBehaviorModel(BeforeAddBehavior methodBeforeAddBehavior, AfterAddBehavior methodAfterAddBehavior, ThrowException methodThrowException, OnClickOnBrowser methodOnClickOnBrowser, MouseOverOnBrowser methodMouseOverOnBrowser)
        {
            if (this.behaviorModel == null)
            {
                this._methodBeforeAddBehavior = methodBeforeAddBehavior;
                this._methodAfterAddBehavior = methodAfterAddBehavior;
                this._methodThrowException = methodThrowException;
                this._methodOnClickOnBrowser = methodOnClickOnBrowser;
                this._methodMouseOverOnBrowser = methodMouseOverOnBrowser;
                this._methodMouseOverOnBrowser = methodMouseOverOnBrowser;
                this._methodOnClickOnBrowser = methodOnClickOnBrowser;
                this.behaviorModel = new BehaviorModel(this, this.methodBeforeAddBehavior, this.methodAfterAddBehavior, this.methodThrowExcepotion, this.methodOnClickOnBrowser, this.methodMouseOverOnBrowser);
            }
            else
            {
                this.behaviorModel.RestartBehavior();
            }
        }

        private void methodAfterAddBehavior(HtmlElement element, MarkBehavior markBehavior)
        {
            this._methodAfterAddBehavior(element, markBehavior);
        }

        private void methodBeforeAddBehavior(HtmlElement element)
        {
            this._methodBeforeAddBehavior(element);
        }

        private void methodThrowExcepotion(string exMessage)
        {
            this._methodThrowException(exMessage);
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
            this.behaviorModel.MarkElement(element);
        }

        /// <summary>
        /// Returns the automation object for the web browser
        /// </summary>
        public object Application
        {
            get { return axIWebBrowser2.Application; }
        }

        /// <summary>
        /// This method supports the .NET Framework infrastructure and is not intended to be used directly from your code. 
        /// Called by the control when the underlying ActiveX control is created. 
        /// </summary>
        /// <param name="nativeActiveXObject"></param> 
        protected override void AttachInterfaces(object nativeActiveXObject)
        {
            this.axIWebBrowser2 = (UnsafeNativeMethods.IWebBrowser2)nativeActiveXObject;
            base.AttachInterfaces(nativeActiveXObject);

        }

        /// <summary>
        /// This method supports the .NET Framework infrastructure and is not intended to be used directly from your code. 
        /// Called by the control when the underlying ActiveX control is discarded. 
        /// </summary> 
        protected override void DetachInterfaces()
        {
            this.axIWebBrowser2 = null;
            base.DetachInterfaces();
        }

        //This method will be called to give you a chance to create your own event sink
        protected override void CreateSink()
        {
            //MAKE SURE TO CALL THE BASE or the normal events won't fire
            base.CreateSink();
            events = new WebBrowserExtendedEvents(this);
            cookie = new System.Windows.Forms.AxHost.ConnectionPointCookie(this.ActiveXInstance, events, typeof(DWebBrowserEvents2));
        }

        protected override void DetachSink()
        {
            if (null != cookie)
            {
                cookie.Disconnect();
                cookie = null;
            }
            base.DetachSink();
        }

        //This new event will fire when the page is navigating
        public event EventHandler<WebBrowserExtendedNavigatingEventArgs> BeforeNavigate;
        public event EventHandler<WebBrowserExtendedNavigatingEventArgs> BeforeNewWindow;

        protected void OnBeforeNewWindow(WebBrowserExtendedNavigatingEventArgs el)
        {
            EventHandler<WebBrowserExtendedNavigatingEventArgs> h = BeforeNewWindow;

            if (null != h)
            {
                h(this, el);
            }
        }

        protected void OnBeforeNavigate(WebBrowserExtendedNavigatingEventArgs el)
        {
            EventHandler<WebBrowserExtendedNavigatingEventArgs> h = BeforeNavigate;

            if (null != h)
            {
                h(this, el);
            }
        }

        //This class will capture events from the WebBrowser
        //class WebBrowserExtendedEvents : System.Runtime.InteropServices.StandardOleMarshalObject, DWebBrowserEvents2
        class WebBrowserExtendedEvents : DWebBrowserEvents2
        {
            UCWebBrowserEx _Browser;
            public WebBrowserExtendedEvents() { }
            public WebBrowserExtendedEvents(UCWebBrowserEx browser) { _Browser = browser; }

            //Implement whichever events you wish
            public void BeforeNavigate2(object pDisp, ref object URL, ref object flags, ref object targetFrameName, ref object postData, ref object headers, ref bool cancel)
            {
                string tFrame = null;
                if (targetFrameName != null)
                    tFrame = targetFrameName.ToString();
                WebBrowserExtendedNavigatingEventArgs args = new WebBrowserExtendedNavigatingEventArgs(URL.ToString(), tFrame, pDisp);
                _Browser.OnBeforeNavigate(args);
                cancel = args.Cancel;
                pDisp = args.AutomationObject;
            }

            //public void NewWindow3(ref object pDisp, ref bool cancel, ref object flags, ref object URLContext, ref object URL)
            public void NewWindow3(ref object pDisp, ref bool cancel, uint flags, string URLContext, string URL)
            {
                WebBrowserExtendedNavigatingEventArgs args = new WebBrowserExtendedNavigatingEventArgs(URL.ToString(), null, pDisp);
                _Browser.OnBeforeNewWindow(args);
                cancel = args.Cancel;
                pDisp = args.AutomationObject;
            }

            public void DownloadBegin()
            {
            }
            public void DownloadComplete()
            {
            }
            public void StatusTextChange(ref string Text)
            {
            }
            public void CommandStateChange(ref long command, ref bool enable)
            {
            }
            //public void NewWindow2(object pDisp,ref bool cancel)
            //{

            //}
            public void NavigateComplete2(object pDisp, ref object URL)
            {
            }
            public void DocumentComplete(object pDisp, ref object URL)
            {
            }
        }

        // [System.Runtime.InteropServices.ComImport(), System.Runtime.InteropServices.Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D"),
        // System.Runtime.InteropServices.InterfaceTypeAttribute(System.Runtime.InteropServices.ComInterfaceType.InterfaceIsIDispatch),
        //System.Runtime.InteropServices.TypeLibType(System.Runtime.InteropServices.TypeLibTypeFlags.FHidden)]
        [ComImport, TypeLibType((short)0x1010), InterfaceType((short)2), Guid("34A715A0-6587-11D0-924A-0020AFC7AC4D")]
        public interface DWebBrowserEvents2
        {
            /*[System.Runtime.InteropServices.DispId(250)]
            void BeforeNavigate2(
                [System.Runtime.InteropServices.In, System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
                [System.Runtime.InteropServices.In] ref object URL,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object targetFrameName,
                [System.Runtime.InteropServices.In] ref object postData,
                [System.Runtime.InteropServices.In] ref object headers,
                [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] ref bool cancel); 
            [System.Runtime.InteropServices.DispId(273)]
            void NewWindow3(
                [System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] ref object pDisp,
                [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] ref bool cancel,
                [System.Runtime.InteropServices.In] ref object flags,
                [System.Runtime.InteropServices.In] ref object URLContext,
                [System.Runtime.InteropServices.In] ref object URL);*/
            [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(250)]
            void BeforeNavigate2([In, MarshalAs(UnmanagedType.IDispatch)] object pDisp, [In, MarshalAs(UnmanagedType.Struct)] ref object URL, [In, MarshalAs(UnmanagedType.Struct)] ref object Flags, [In, MarshalAs(UnmanagedType.Struct)] ref object TargetFrameName, [In, MarshalAs(UnmanagedType.Struct)] ref object PostData, [In, MarshalAs(UnmanagedType.Struct)] ref object Headers, [In, Out] ref bool Cancel);
            [PreserveSig, MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(0x111)]
            void NewWindow3([In, Out, MarshalAs(UnmanagedType.IDispatch)] ref object ppDisp, [In, Out] ref bool Cancel, [In] uint dwFlags, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrlContext, [In, MarshalAs(UnmanagedType.BStr)] string bstrUrl);
            [System.Runtime.InteropServices.DispId(106)]
            void DownloadBegin();
            [System.Runtime.InteropServices.DispId(104)]
            void DownloadComplete();
            [System.Runtime.InteropServices.DispId(102)]
            void StatusTextChange([System.Runtime.InteropServices.In] ref string Text);
            [System.Runtime.InteropServices.DispId(105)]
            void CommandStateChange([System.Runtime.InteropServices.In] ref long command, [System.Runtime.InteropServices.In] ref bool enable);
            //[System.Runtime.InteropServices.DispId(251)]
            //   void NewWindow2(
            //    [System.Runtime.InteropServices.In,
            //    System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp,
            //    [System.Runtime.InteropServices.In, System.Runtime.InteropServices.Out] ref bool cancel);
            [System.Runtime.InteropServices.DispId(252)]
            void NavigateComplete2([System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp, [System.Runtime.InteropServices.In] ref object URL);
            [System.Runtime.InteropServices.DispId(259)]
            void DocumentComplete([System.Runtime.InteropServices.In,
                System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.IDispatch)] object pDisp, [System.Runtime.InteropServices.In] ref object URL);
        }
    }

    public class WebBrowserExtendedNavigatingEventArgs : CancelEventArgs
    {
        private string _Url;
        public string Url
        {
            get { return _Url; }
        }

        private string _Frame;
        public string Frame
        {
            get { return _Frame; }
        }

        private object _pDisp;
        /// <summary>
        /// The pointer to ppDisp
        /// </summary>
        public object AutomationObject
        {
            get { return this._pDisp; }
            set { this._pDisp = value; }
        }

        public WebBrowserExtendedNavigatingEventArgs(string url, string frame)
            : base()
        {
            _Url = url;
            _Frame = frame;
        }

        public WebBrowserExtendedNavigatingEventArgs(string url, string frame, object automation)
            : base()
        {
            _Url = url;
            _Frame = frame;
            _pDisp = automation;
        }
    }
    static class NativeMethods
    {
        public enum OLECMDF
        {
            // Fields
            OLECMDF_DEFHIDEONCTXTMENU = 0x20,
            OLECMDF_ENABLED = 2,
            OLECMDF_INVISIBLE = 0x10,
            OLECMDF_LATCHED = 4,
            OLECMDF_NINCHED = 8,
            OLECMDF_SUPPORTED = 1
        }

        public enum OLECMDID
        {
            // Fields
            OLECMDID_PAGESETUP = 8,
            OLECMDID_PRINT = 6,
            OLECMDID_PRINTPREVIEW = 7,
            OLECMDID_PROPERTIES = 10,
            OLECMDID_SAVEAS = 4,
            // OLECMDID_SHOWSCRIPTERROR = 40
        }
        public enum OLECMDEXECOPT
        {
            // Fields
            OLECMDEXECOPT_DODEFAULT = 0,
            OLECMDEXECOPT_DONTPROMPTUSER = 2,
            OLECMDEXECOPT_PROMPTUSER = 1,
            OLECMDEXECOPT_SHOWHELP = 3
        }

        //[StructLayout(LayoutKind.Sequential)]
        //public class POINT
        //{
        //  public int x;
        //  public int y;
        //  public POINT() { }
        //  public POINT(int x, int y)
        //  {
        //    this.x = x;
        //    this.y = y;
        //  }
        //}

        //[ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown), Guid("B722BCCB-4E68-101B-A2BC-00AA00404770"), ComVisible(true)]
        //public interface IOleCommandTarget
        //{
        //  [return: MarshalAs(UnmanagedType.I4)]
        //  [PreserveSig]
        //  int QueryStatus(ref Guid pguidCmdGroup, int cCmds, [In, Out] NativeMethods.OLECMD prgCmds, [In, Out] IntPtr pCmdText);
        //  [return: MarshalAs(UnmanagedType.I4)]
        //  [PreserveSig]
        //  int Exec(ref Guid pguidCmdGroup, int nCmdID, int nCmdexecopt, [In, MarshalAs(UnmanagedType.LPArray)] object[] pvaIn, ref int pvaOut);
        //}

        //[StructLayout(LayoutKind.Sequential)]
        //public class OLECMD
        //{
        //  [MarshalAs(UnmanagedType.U4)]
        //  public int cmdID;
        //  [MarshalAs(UnmanagedType.U4)]
        //  public int cmdf;
        //  public OLECMD()
        //  {
        //  }
        //}

        //public const int S_FALSE = 1;
        //public const int S_OK = 0;


    }

    class UnsafeNativeMethods
    {
        private UnsafeNativeMethods()
        {
        }

        [ComImport, SuppressUnmanagedCodeSecurity, TypeLibType(TypeLibTypeFlags.FOleAutomation | (TypeLibTypeFlags.FDual | TypeLibTypeFlags.FHidden)), Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E")]
        public interface IWebBrowser2
        {
            [DispId(100)]
            void GoBack();
            [DispId(0x65)]
            void GoForward();
            [DispId(0x66)]
            void GoHome();
            [DispId(0x67)]
            void GoSearch();
            [DispId(0x68)]
            void Navigate([In] string Url, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);
            [DispId(-550)]
            void Refresh();
            [DispId(0x69)]
            void Refresh2([In] ref object level);
            [DispId(0x6a)]
            void Stop();
            [DispId(200)]
            object Application { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xc9)]
            object Parent { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xca)]
            object Container { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xcb)]
            object Document { [return: MarshalAs(UnmanagedType.IDispatch)] get; }
            [DispId(0xcc)]
            bool TopLevelContainer { get; }
            [DispId(0xcd)]
            string Type { get; }
            [DispId(0xce)]
            int Left { get; set; }
            [DispId(0xcf)]
            int Top { get; set; }
            [DispId(0xd0)]
            int Width { get; set; }
            [DispId(0xd1)]
            int Height { get; set; }
            [DispId(210)]
            string LocationName { get; }
            [DispId(0xd3)]
            string LocationURL { get; }
            [DispId(0xd4)]
            bool Busy { get; }
            [DispId(300)]
            void Quit();
            [DispId(0x12d)]
            void ClientToWindow(out int pcx, out int pcy);
            [DispId(0x12e)]
            void PutProperty([In] string property, [In] object vtValue);
            [DispId(0x12f)]
            object GetProperty([In] string property);
            [DispId(0)]
            string Name { get; }
            [DispId(-515)]
            int HWND { get; }
            [DispId(400)]
            string FullName { get; }
            [DispId(0x191)]
            string Path { get; }
            [DispId(0x192)]
            bool Visible { get; set; }
            [DispId(0x193)]
            bool StatusBar { get; set; }
            [DispId(0x194)]
            string StatusText { get; set; }
            [DispId(0x195)]
            int ToolBar { get; set; }
            [DispId(0x196)]
            bool MenuBar { get; set; }
            [DispId(0x197)]
            bool FullScreen { get; set; }
            [DispId(500)]
            void Navigate2([In] ref object URL, [In] ref object flags, [In] ref object targetFrameName, [In] ref object postData, [In] ref object headers);
            [DispId(0x1f5)]
            NativeMethods.OLECMDF QueryStatusWB([In] NativeMethods.OLECMDID cmdID);
            [DispId(0x1f6)]
            void ExecWB([In] NativeMethods.OLECMDID cmdID, [In] NativeMethods.OLECMDEXECOPT cmdexecopt, ref object pvaIn, IntPtr pvaOut);
            [DispId(0x1f7)]
            void ShowBrowserBar([In] ref object pvaClsid, [In] ref object pvarShow, [In] ref object pvarSize);
            [DispId(-525)]
            WebBrowserReadyState ReadyState { get; }
            [DispId(550)]
            bool Offline { get; set; }
            [DispId(0x227)]
            bool Silent { get; set; }
            [DispId(0x228)]
            bool RegisterAsBrowser { get; set; }
            [DispId(0x229)]
            bool RegisterAsDropTarget { get; set; }
            [DispId(0x22a)]
            bool TheaterMode { get; set; }
            [DispId(0x22b)]
            bool AddressBar { get; set; }
            [DispId(0x22c)]
            bool Resizable { get; set; }
        }
    }
}

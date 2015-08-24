using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace WebUIAnalysisControlLib
{
    [ComVisible(true), Guid("3050F6A6-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IHTMLPainter
    {
        void Draw(
        [In, MarshalAs(UnmanagedType.I4)]
int leftBounds,
        [In, MarshalAs(UnmanagedType.I4)]
int topBounds,
        [In, MarshalAs(UnmanagedType.I4)]
int rightBounds,
        [In, MarshalAs(UnmanagedType.I4)]
int bottomBounds,
        [In, MarshalAs(UnmanagedType.I4)]
int leftUpdate,
        [In, MarshalAs(UnmanagedType.I4)]
int topUpdate,
        [In, MarshalAs(UnmanagedType.I4)]
int rightUpdate,
        [In, MarshalAs(UnmanagedType.I4)]
int bottomUpdate,
        [In, MarshalAs(UnmanagedType.U4)]
int lDrawFlags,
        [In]
IntPtr hdc,
        [In]
IntPtr pvDrawObject);

        //        void Draw(
        //        [In, MarshalAs(UnmanagedType.Struct)]
        //RECT rcBounds,
        //        [In, MarshalAs(UnmanagedType.Struct)]
        //RECT rcUpdate,
        //        [In, MarshalAs(UnmanagedType.U4)]
        //int lDrawFlags,
        //        [In]
        //IntPtr hdc,
        //        [In]
        //IntPtr pvDrawObject);

        void OnResize(
        [In, MarshalAs(UnmanagedType.I4)]
int cx,
        [In, MarshalAs(UnmanagedType.I4)]
int cy);

        void GetPainterInfo(
        [Out]
HTML_PAINTER_INFO htmlPainterInfo);

        [return: MarshalAs(UnmanagedType.Bool)]
        bool HitTestPoint(
        [In, MarshalAs(UnmanagedType.I4)]
int ptx,
        [In, MarshalAs(UnmanagedType.I4)]
int pty,
        [Out, MarshalAs(UnmanagedType.LPArray)]
int[] pbHit,
        [Out, MarshalAs(UnmanagedType.LPArray)]
int[] plPartID);
    }

    [ComVisible(true), Guid("3050f6a7-98b5-11cf-bb82-00aa00bdce0b"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IHTMLPaintSite
    {
        void InvalidatePainterInfo();

        void InvalidateRect(
        [In]
IntPtr pRect);
    }

    [ComVisible(true), StructLayout(LayoutKind.Sequential)]
    public class HTML_PAINTER_INFO
    {
        [MarshalAs(UnmanagedType.I4)]
        public int lFlags;

        [MarshalAs(UnmanagedType.I4)]
        public int lZOrder;

        [MarshalAs(UnmanagedType.Struct)]
        public Guid iidDrawObject;

        [MarshalAs(UnmanagedType.Struct)]
        public RECT rcBounds;
    }

    [ComVisible(true), StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public RECT(int left, int top, int right, int bottom)
        {
            this.left = left;
            this.top = top;
            this.right = right;
            this.bottom = bottom;
        }

        public static RECT FromXYWH(int x, int y, int width, int height)
        {
            return new RECT(x, y, x + width, y + height);
        }
    }

    [ComVisible(true), Guid("3050F425-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IElementBehavior
    {
        void Init(
        [In, MarshalAs(UnmanagedType.Interface)]
IElementBehaviorSite pBehaviorSite);

        void Notify(
        [In, MarshalAs(UnmanagedType.U4)]
int dwEvent,
        [In]
IntPtr pVar);

        void Detach();
    }

    [ComVisible(true), Guid("3050F429-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IElementBehaviorFactory
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        IElementBehavior FindBehavior(
        [In, MarshalAs(UnmanagedType.BStr)]
string bstrBehavior,
        [In, MarshalAs(UnmanagedType.BStr)]
string bstrBehaviorUrl,
        [In, MarshalAs(UnmanagedType.Interface)]
IElementBehaviorSite pSite);
    }

    [ComVisible(true), Guid("3050F427-98B5-11CF-BB82-00AA00BDCE0B"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    public interface IElementBehaviorSite
    {
        [return: MarshalAs(UnmanagedType.Interface)]
        IHTMLElement GetElement();

        void RegisterNotification(
        [In, MarshalAs(UnmanagedType.I4)]
int lEvent);
    }

    //After that you can define for example a behavior to show an icon in the left top of your tables like this:
    public class MarkBehavior : IElementBehaviorFactory, IElementBehavior, IHTMLPainter
    {
        private IHTMLElement m_Element;
        private IHTMLElement2 _sourceElement;
        private Rectangle _sourceRectangle;

        #region IElementBehaviorFactory Members
        public IElementBehavior FindBehavior(string bstrBehavior, string bstrBehaviorUrl, IElementBehaviorSite pSite)
        {
            return this;
        }
        #endregion

        #region IElementBehavior Members
        public void Notify(int lEvent, IntPtr pVar)
        {
            // TODO: Add GridBehavior.Notify implementation
        }

        public void Init(IElementBehaviorSite pBehaviorSite)
        {
            // then you can extract some information from the element
            //m_Element = pBehaviorSite.GetElement();
            //IHTMLStyle2 style = (IHTMLStyle2)m_Element.style;
            ////style.position = "absolute";
            //IHTMLStyle style3 = (IHTMLStyle)m_Element.style;
            ////style3.zIndex = "10000";
        }

        public void Detach()
        {
            // TODO: Add GridBehavior.Detach implementation
        }
        #endregion

        #region IHTMLPainter Members
        public void Draw(int leftBounds, int topBounds, int rightBounds, int bottomBounds, int leftUpdate, int topUpdate, int rightUpdate, int bottomUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        {
            Graphics g;
            g = Graphics.FromHdc(hdc);
            g.PageUnit = GraphicsUnit.Pixel;
            int cornerRadius = 3;
            if (this._sourceRectangle.Width <= 6 || this._sourceRectangle.Height <= 6)
            {
                cornerRadius = 1;
            }
            using (GraphicsPath path = CreateRoundedRectanglePath(this._sourceRectangle, cornerRadius))
            {
                Pen pen = new Pen(Color.Red, 1);
                g.DrawPath(pen, path);

                Color maskColor = Color.FromArgb(50, Color.Red);
                SolidBrush brush = new SolidBrush(maskColor);
                g.FillPath(brush, path);
            }
        }

        //public void Draw(RECT rcBounds, RECT rcUpdate, int lDrawFlags, IntPtr hdc, IntPtr pvDrawObject)
        //{
        //    Graphics g;
        //    g = Graphics.FromHdc(hdc);
        //    g.PageUnit = GraphicsUnit.Pixel;
        //    int cornerRadius = 3;
        //    if (this._sourceRectangle.Width <= 6 || this._sourceRectangle.Height <= 6)
        //    {
        //        cornerRadius = 1;
        //    }
        //    using (GraphicsPath path = CreateRoundedRectanglePath(this._sourceRectangle, cornerRadius))
        //    {
        //        Pen pen = new Pen(Color.Red, 1);
        //        g.DrawPath(pen, path);

        //        Color maskColor = Color.FromArgb(50, Color.Red);
        //        SolidBrush brush = new SolidBrush(maskColor);
        //        g.FillPath(brush, path);
        //    }
        //}

        public void OnResize(int cx, int cy)
        {
            // TODO: Add GridBehavior.OnResize implementation
        }

        public void GetPainterInfo(HTML_PAINTER_INFO pInfo)
        {
            /*
             typedef enum _HTML_PAINT_ZORDER { 
                HTMLPAINT_ZORDER_NONE                   = 0,
                HTMLPAINT_ZORDER_REPLACE_ALL         = 1,
                HTMLPAINT_ZORDER_REPLACE_CONTENT     = 2,
                HTMLPAINT_ZORDER_REPLACE_BACKGROUND  = 3,
                HTMLPAINT_ZORDER_BELOW_CONTENT       = 4,
                HTMLPAINT_ZORDER_BELOW_FLOW          = 5,
                HTMLPAINT_ZORDER_ABOVE_FLOW          = 6,
                HTMLPAINT_ZORDER_ABOVE_CONTENT       = 7,
                HTMLPAINT_ZORDER_WINDOW_TOP          = 8
            } HTML_PAINT_ZORDER;
             */
            pInfo.lFlags = 0x000002;
            pInfo.lZOrder = 8;
            pInfo.iidDrawObject = Guid.Empty;
            pInfo.rcBounds = new RECT(0, 0, 0, 0);
        }

        public bool HitTestPoint(int ptx, int pty, int[] pbHit, int[] plPartID)
        {
            return false;
        }
        #endregion

        public IHTMLElement2 SourceElement
        {
            set
            {
                this._sourceElement = value;
            }
            get
            {
                return this._sourceElement;
            }
        }

        public Rectangle SourceRectangle
        {
            set
            {
                this._sourceRectangle = value;
            }
            get
            {
                return this._sourceRectangle;
            }
        }

        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int cornerRadius)
        {
            GraphicsPath roundedRect = new GraphicsPath();
            roundedRect.AddArc(rect.X, rect.Y, cornerRadius * 2, cornerRadius * 2, 180, 90);
            roundedRect.AddLine(rect.X + cornerRadius, rect.Y, rect.Right - cornerRadius * 2, rect.Y);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y, cornerRadius * 2, cornerRadius * 2, 270, 90);
            roundedRect.AddLine(rect.Right, rect.Y + cornerRadius * 2, rect.Right, rect.Y + rect.Height - cornerRadius * 2);
            roundedRect.AddArc(rect.X + rect.Width - cornerRadius * 2, rect.Y + rect.Height - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 0, 90);
            roundedRect.AddLine(rect.Right - cornerRadius * 2, rect.Bottom, rect.X + cornerRadius * 2, rect.Bottom);
            roundedRect.AddArc(rect.X, rect.Bottom - cornerRadius * 2, cornerRadius * 2, cornerRadius * 2, 90, 90);
            roundedRect.AddLine(rect.X, rect.Bottom - cornerRadius * 2, rect.X, rect.Y + cornerRadius * 2);
            roundedRect.CloseFigure();
            return roundedRect;
        }
    }
}

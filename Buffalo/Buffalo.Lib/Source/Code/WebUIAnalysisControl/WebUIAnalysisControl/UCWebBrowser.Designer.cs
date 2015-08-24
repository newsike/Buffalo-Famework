namespace WebUIAnalysisControlLib
{
    partial class UCWebBrowser
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer_Browser = new System.Windows.Forms.SplitContainer();
            this.splitContainer_ToolBar = new System.Windows.Forms.SplitContainer();
            this.btn_Browser_Go = new System.Windows.Forms.Button();
            this.btn_Browser_GoBack = new System.Windows.Forms.Button();
            this.btn_Browser_GoForward = new System.Windows.Forms.Button();
            this.cmb_URL = new System.Windows.Forms.ComboBox();
            this.tabControl_Browser = new System.Windows.Forms.TabControl();
            this.tabPage_StartPage = new System.Windows.Forms.TabPage();
            this.ucWBEx_Main = new WebUIAnalysisControlLib.UCWebBrowserEx();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Browser)).BeginInit();
            this.splitContainer_Browser.Panel1.SuspendLayout();
            this.splitContainer_Browser.Panel2.SuspendLayout();
            this.splitContainer_Browser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_ToolBar)).BeginInit();
            this.splitContainer_ToolBar.Panel1.SuspendLayout();
            this.splitContainer_ToolBar.Panel2.SuspendLayout();
            this.splitContainer_ToolBar.SuspendLayout();
            this.tabControl_Browser.SuspendLayout();
            this.tabPage_StartPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Browser
            // 
            this.splitContainer_Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Browser.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Browser.Name = "splitContainer_Browser";
            this.splitContainer_Browser.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Browser.Panel1
            // 
            this.splitContainer_Browser.Panel1.Controls.Add(this.splitContainer_ToolBar);
            this.splitContainer_Browser.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer_Browser.Panel2
            // 
            this.splitContainer_Browser.Panel2.Controls.Add(this.tabControl_Browser);
            this.splitContainer_Browser.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer_Browser.Size = new System.Drawing.Size(1004, 394);
            this.splitContainer_Browser.SplitterDistance = 27;
            this.splitContainer_Browser.SplitterWidth = 1;
            this.splitContainer_Browser.TabIndex = 1;
            // 
            // splitContainer_ToolBar
            // 
            this.splitContainer_ToolBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_ToolBar.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_ToolBar.Name = "splitContainer_ToolBar";
            // 
            // splitContainer_ToolBar.Panel1
            // 
            this.splitContainer_ToolBar.Panel1.Controls.Add(this.btn_Browser_Go);
            this.splitContainer_ToolBar.Panel1.Controls.Add(this.btn_Browser_GoBack);
            this.splitContainer_ToolBar.Panel1.Controls.Add(this.btn_Browser_GoForward);
            // 
            // splitContainer_ToolBar.Panel2
            // 
            this.splitContainer_ToolBar.Panel2.Controls.Add(this.cmb_URL);
            this.splitContainer_ToolBar.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 5, 0);
            this.splitContainer_ToolBar.Size = new System.Drawing.Size(1004, 27);
            this.splitContainer_ToolBar.SplitterDistance = 125;
            this.splitContainer_ToolBar.SplitterWidth = 1;
            this.splitContainer_ToolBar.TabIndex = 4;
            // 
            // btn_Browser_Go
            // 
            this.btn_Browser_Go.Location = new System.Drawing.Point(66, 3);
            this.btn_Browser_Go.Name = "btn_Browser_Go";
            this.btn_Browser_Go.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btn_Browser_Go.Size = new System.Drawing.Size(30, 23);
            this.btn_Browser_Go.TabIndex = 6;
            this.btn_Browser_Go.Text = "Go";
            this.btn_Browser_Go.UseVisualStyleBackColor = true;
            this.btn_Browser_Go.Click += new System.EventHandler(this.btn_Browser_Go_Click);
            // 
            // btn_Browser_GoBack
            // 
            this.btn_Browser_GoBack.Location = new System.Drawing.Point(5, 2);
            this.btn_Browser_GoBack.Name = "btn_Browser_GoBack";
            this.btn_Browser_GoBack.Size = new System.Drawing.Size(25, 23);
            this.btn_Browser_GoBack.TabIndex = 4;
            this.btn_Browser_GoBack.Text = "<-";
            this.btn_Browser_GoBack.UseVisualStyleBackColor = true;
            this.btn_Browser_GoBack.Click += new System.EventHandler(this.btn_Browser_GoBack_Click);
            // 
            // btn_Browser_GoForward
            // 
            this.btn_Browser_GoForward.Location = new System.Drawing.Point(35, 2);
            this.btn_Browser_GoForward.Name = "btn_Browser_GoForward";
            this.btn_Browser_GoForward.Size = new System.Drawing.Size(25, 23);
            this.btn_Browser_GoForward.TabIndex = 5;
            this.btn_Browser_GoForward.Text = "->";
            this.btn_Browser_GoForward.UseVisualStyleBackColor = true;
            this.btn_Browser_GoForward.Click += new System.EventHandler(this.btn_Browser_GoForward_Click);
            // 
            // cmb_URL
            // 
            this.cmb_URL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmb_URL.FormattingEnabled = true;
            this.cmb_URL.Location = new System.Drawing.Point(0, 3);
            this.cmb_URL.Name = "cmb_URL";
            this.cmb_URL.Size = new System.Drawing.Size(873, 20);
            this.cmb_URL.TabIndex = 3;
            this.cmb_URL.SelectedIndexChanged += new System.EventHandler(this.cmb_URL_SelectedIndexChanged);
            this.cmb_URL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cmb_URL_KeyDown);
            // 
            // tabControl_Browser
            // 
            this.tabControl_Browser.Controls.Add(this.tabPage_StartPage);
            this.tabControl_Browser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl_Browser.Location = new System.Drawing.Point(0, 0);
            this.tabControl_Browser.Name = "tabControl_Browser";
            this.tabControl_Browser.SelectedIndex = 0;
            this.tabControl_Browser.Size = new System.Drawing.Size(1004, 366);
            this.tabControl_Browser.TabIndex = 0;
            this.tabControl_Browser.TabIndexChanged += new System.EventHandler(this.tabControl_Browser_TabIndexChanged);
            // 
            // tabPage_StartPage
            // 
            this.tabPage_StartPage.Controls.Add(this.ucWBEx_Main);
            this.tabPage_StartPage.Location = new System.Drawing.Point(4, 22);
            this.tabPage_StartPage.Name = "tabPage_StartPage";
            this.tabPage_StartPage.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_StartPage.Size = new System.Drawing.Size(996, 340);
            this.tabPage_StartPage.TabIndex = 1;
            this.tabPage_StartPage.Text = "about:blank";
            this.tabPage_StartPage.UseVisualStyleBackColor = true;
            // 
            // ucWBEx_Main
            // 
            this.ucWBEx_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucWBEx_Main.Location = new System.Drawing.Point(3, 3);
            this.ucWBEx_Main.MinimumSize = new System.Drawing.Size(20, 18);
            this.ucWBEx_Main.Name = "ucWBEx_Main";
            this.ucWBEx_Main.ScriptErrorsSuppressed = true;
            this.ucWBEx_Main.Size = new System.Drawing.Size(990, 334);
            this.ucWBEx_Main.TabIndex = 0;
            this.ucWBEx_Main.BeforeNewWindow += new System.EventHandler<WebUIAnalysisControlLib.WebBrowserExtendedNavigatingEventArgs>(this.ucWBEx_Main_BeforeNewWindow);
            // 
            // UCWebBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer_Browser);
            this.Name = "UCWebBrowser";
            this.Size = new System.Drawing.Size(1004, 394);
            this.splitContainer_Browser.Panel1.ResumeLayout(false);
            this.splitContainer_Browser.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Browser)).EndInit();
            this.splitContainer_Browser.ResumeLayout(false);
            this.splitContainer_ToolBar.Panel1.ResumeLayout(false);
            this.splitContainer_ToolBar.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_ToolBar)).EndInit();
            this.splitContainer_ToolBar.ResumeLayout(false);
            this.tabControl_Browser.ResumeLayout(false);
            this.tabPage_StartPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Browser;
        private System.Windows.Forms.TabControl tabControl_Browser;
        private System.Windows.Forms.TabPage tabPage_StartPage;
        private System.Windows.Forms.SplitContainer splitContainer_ToolBar;
        private System.Windows.Forms.Button btn_Browser_Go;
        private System.Windows.Forms.Button btn_Browser_GoBack;
        private System.Windows.Forms.Button btn_Browser_GoForward;
        private System.Windows.Forms.ComboBox cmb_URL;
        private UCWebBrowserEx ucWBEx_Main;
    }
}

namespace WebUIAnalysisControlLib
{
    partial class WebUIAnalysisControl
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
            this.splitContainer_Main = new System.Windows.Forms.SplitContainer();
            this.ucWebBrowser = new WebUIAnalysisControlLib.UCWebBrowser();
            this.splitContainer_Controls = new System.Windows.Forms.SplitContainer();
            this.splitContainer_Analysis = new System.Windows.Forms.SplitContainer();
            this.splitContainer_Search = new System.Windows.Forms.SplitContainer();
            this.btn_FindElement = new System.Windows.Forms.Button();
            this.btn_StartAnalyse = new System.Windows.Forms.Button();
            this.btn_Search = new System.Windows.Forms.Button();
            this.txt_KeyWord = new System.Windows.Forms.TextBox();
            this.cmb_KeyWord = new System.Windows.Forms.ComboBox();
            this.cmb_SearchType = new System.Windows.Forms.ComboBox();
            this.treeView_Tags = new System.Windows.Forms.TreeView();
            this.listView_Properties = new System.Windows.Forms.ListView();
            this.ch_propName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ch_propValue = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).BeginInit();
            this.splitContainer_Main.Panel1.SuspendLayout();
            this.splitContainer_Main.Panel2.SuspendLayout();
            this.splitContainer_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Controls)).BeginInit();
            this.splitContainer_Controls.Panel1.SuspendLayout();
            this.splitContainer_Controls.Panel2.SuspendLayout();
            this.splitContainer_Controls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Analysis)).BeginInit();
            this.splitContainer_Analysis.Panel1.SuspendLayout();
            this.splitContainer_Analysis.Panel2.SuspendLayout();
            this.splitContainer_Analysis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Search)).BeginInit();
            this.splitContainer_Search.Panel1.SuspendLayout();
            this.splitContainer_Search.Panel2.SuspendLayout();
            this.splitContainer_Search.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer_Main
            // 
            this.splitContainer_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Main.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Main.Name = "splitContainer_Main";
            this.splitContainer_Main.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Main.Panel1
            // 
            this.splitContainer_Main.Panel1.Controls.Add(this.ucWebBrowser);
            this.splitContainer_Main.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            // 
            // splitContainer_Main.Panel2
            // 
            this.splitContainer_Main.Panel2.Controls.Add(this.splitContainer_Controls);
            this.splitContainer_Main.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer_Main.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.splitContainer_Main.Size = new System.Drawing.Size(800, 554);
            this.splitContainer_Main.SplitterDistance = 364;
            this.splitContainer_Main.TabIndex = 0;
            // 
            // ucWebBrowser
            // 
            this.ucWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.ucWebBrowser.Name = "ucWebBrowser";
            this.ucWebBrowser.Size = new System.Drawing.Size(800, 364);
            this.ucWebBrowser.TabIndex = 0;
            this.ucWebBrowser.Url = "about:blank";
            // 
            // splitContainer_Controls
            // 
            this.splitContainer_Controls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Controls.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Controls.Name = "splitContainer_Controls";
            // 
            // splitContainer_Controls.Panel1
            // 
            this.splitContainer_Controls.Panel1.Controls.Add(this.splitContainer_Analysis);
            // 
            // splitContainer_Controls.Panel2
            // 
            this.splitContainer_Controls.Panel2.Controls.Add(this.listView_Properties);
            this.splitContainer_Controls.Size = new System.Drawing.Size(800, 186);
            this.splitContainer_Controls.SplitterDistance = 615;
            this.splitContainer_Controls.TabIndex = 0;
            // 
            // splitContainer_Analysis
            // 
            this.splitContainer_Analysis.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Analysis.IsSplitterFixed = true;
            this.splitContainer_Analysis.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Analysis.Name = "splitContainer_Analysis";
            this.splitContainer_Analysis.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Analysis.Panel1
            // 
            this.splitContainer_Analysis.Panel1.Controls.Add(this.splitContainer_Search);
            // 
            // splitContainer_Analysis.Panel2
            // 
            this.splitContainer_Analysis.Panel2.Controls.Add(this.treeView_Tags);
            this.splitContainer_Analysis.Size = new System.Drawing.Size(615, 186);
            this.splitContainer_Analysis.SplitterDistance = 55;
            this.splitContainer_Analysis.SplitterWidth = 1;
            this.splitContainer_Analysis.TabIndex = 0;
            // 
            // splitContainer_Search
            // 
            this.splitContainer_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer_Search.IsSplitterFixed = true;
            this.splitContainer_Search.Location = new System.Drawing.Point(0, 0);
            this.splitContainer_Search.Name = "splitContainer_Search";
            this.splitContainer_Search.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer_Search.Panel1
            // 
            this.splitContainer_Search.Panel1.Controls.Add(this.btn_FindElement);
            this.splitContainer_Search.Panel1.Controls.Add(this.btn_StartAnalyse);
            // 
            // splitContainer_Search.Panel2
            // 
            this.splitContainer_Search.Panel2.Controls.Add(this.btn_Search);
            this.splitContainer_Search.Panel2.Controls.Add(this.txt_KeyWord);
            this.splitContainer_Search.Panel2.Controls.Add(this.cmb_KeyWord);
            this.splitContainer_Search.Panel2.Controls.Add(this.cmb_SearchType);
            this.splitContainer_Search.Size = new System.Drawing.Size(615, 55);
            this.splitContainer_Search.SplitterDistance = 25;
            this.splitContainer_Search.SplitterWidth = 2;
            this.splitContainer_Search.TabIndex = 0;
            // 
            // btn_FindElement
            // 
            this.btn_FindElement.Location = new System.Drawing.Point(95, 3);
            this.btn_FindElement.Name = "btn_FindElement";
            this.btn_FindElement.Size = new System.Drawing.Size(90, 21);
            this.btn_FindElement.TabIndex = 1;
            this.btn_FindElement.Text = "Find Element";
            this.btn_FindElement.UseVisualStyleBackColor = true;
            this.btn_FindElement.Click += new System.EventHandler(this.btn_FindElement_Click);
            // 
            // btn_StartAnalyse
            // 
            this.btn_StartAnalyse.Location = new System.Drawing.Point(4, 3);
            this.btn_StartAnalyse.Name = "btn_StartAnalyse";
            this.btn_StartAnalyse.Size = new System.Drawing.Size(85, 21);
            this.btn_StartAnalyse.TabIndex = 0;
            this.btn_StartAnalyse.Text = "Start Analyse";
            this.btn_StartAnalyse.UseVisualStyleBackColor = true;
            this.btn_StartAnalyse.Click += new System.EventHandler(this.btn_StartAnalyse_Click);
            // 
            // btn_Search
            // 
            this.btn_Search.Location = new System.Drawing.Point(537, 4);
            this.btn_Search.Name = "btn_Search";
            this.btn_Search.Size = new System.Drawing.Size(75, 21);
            this.btn_Search.TabIndex = 3;
            this.btn_Search.Text = "Search";
            this.btn_Search.UseVisualStyleBackColor = true;
            // 
            // txt_KeyWord
            // 
            this.txt_KeyWord.Location = new System.Drawing.Point(260, 4);
            this.txt_KeyWord.Name = "txt_KeyWord";
            this.txt_KeyWord.Size = new System.Drawing.Size(100, 21);
            this.txt_KeyWord.TabIndex = 2;
            // 
            // cmb_KeyWord
            // 
            this.cmb_KeyWord.FormattingEnabled = true;
            this.cmb_KeyWord.Location = new System.Drawing.Point(132, 4);
            this.cmb_KeyWord.Name = "cmb_KeyWord";
            this.cmb_KeyWord.Size = new System.Drawing.Size(121, 20);
            this.cmb_KeyWord.TabIndex = 1;
            // 
            // cmb_SearchType
            // 
            this.cmb_SearchType.FormattingEnabled = true;
            this.cmb_SearchType.Items.AddRange(new object[] {
            "Tag Name",
            "Class Name",
            "ID",
            "Type"});
            this.cmb_SearchType.Location = new System.Drawing.Point(4, 4);
            this.cmb_SearchType.Name = "cmb_SearchType";
            this.cmb_SearchType.Size = new System.Drawing.Size(121, 20);
            this.cmb_SearchType.TabIndex = 0;
            // 
            // treeView_Tags
            // 
            this.treeView_Tags.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView_Tags.Location = new System.Drawing.Point(0, 0);
            this.treeView_Tags.Name = "treeView_Tags";
            this.treeView_Tags.Size = new System.Drawing.Size(615, 130);
            this.treeView_Tags.TabIndex = 0;
            this.treeView_Tags.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_Tags_AfterSelect);
            // 
            // listView_Properties
            // 
            this.listView_Properties.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ch_propName,
            this.ch_propValue});
            this.listView_Properties.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView_Properties.FullRowSelect = true;
            this.listView_Properties.GridLines = true;
            this.listView_Properties.HideSelection = false;
            this.listView_Properties.Location = new System.Drawing.Point(0, 0);
            this.listView_Properties.MultiSelect = false;
            this.listView_Properties.Name = "listView_Properties";
            this.listView_Properties.Size = new System.Drawing.Size(181, 186);
            this.listView_Properties.TabIndex = 0;
            this.listView_Properties.UseCompatibleStateImageBehavior = false;
            this.listView_Properties.View = System.Windows.Forms.View.Details;
            // 
            // ch_propName
            // 
            this.ch_propName.Text = "Property Name";
            this.ch_propName.Width = 82;
            // 
            // ch_propValue
            // 
            this.ch_propValue.Text = "Property Value";
            this.ch_propValue.Width = 92;
            // 
            // WebUIAnalysisControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer_Main);
            this.MinimumSize = new System.Drawing.Size(800, 554);
            this.Name = "WebUIAnalysisControl";
            this.Size = new System.Drawing.Size(800, 554);
            this.splitContainer_Main.Panel1.ResumeLayout(false);
            this.splitContainer_Main.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Main)).EndInit();
            this.splitContainer_Main.ResumeLayout(false);
            this.splitContainer_Controls.Panel1.ResumeLayout(false);
            this.splitContainer_Controls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Controls)).EndInit();
            this.splitContainer_Controls.ResumeLayout(false);
            this.splitContainer_Analysis.Panel1.ResumeLayout(false);
            this.splitContainer_Analysis.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Analysis)).EndInit();
            this.splitContainer_Analysis.ResumeLayout(false);
            this.splitContainer_Search.Panel1.ResumeLayout(false);
            this.splitContainer_Search.Panel2.ResumeLayout(false);
            this.splitContainer_Search.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer_Search)).EndInit();
            this.splitContainer_Search.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer_Main;
        private System.Windows.Forms.SplitContainer splitContainer_Controls;
        private System.Windows.Forms.SplitContainer splitContainer_Analysis;
        private System.Windows.Forms.SplitContainer splitContainer_Search;
        private System.Windows.Forms.Button btn_StartAnalyse;
        private System.Windows.Forms.Button btn_Search;
        private System.Windows.Forms.TextBox txt_KeyWord;
        private System.Windows.Forms.ComboBox cmb_KeyWord;
        private System.Windows.Forms.ComboBox cmb_SearchType;
        private System.Windows.Forms.ListView listView_Properties;
        private System.Windows.Forms.ColumnHeader ch_propName;
        private System.Windows.Forms.ColumnHeader ch_propValue;
        private UCWebBrowser ucWebBrowser;
        private System.Windows.Forms.TreeView treeView_Tags;
        private System.Windows.Forms.Button btn_FindElement;
    }
}

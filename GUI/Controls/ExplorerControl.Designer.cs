namespace GUI.Controls
{
    partial class ExplorerControl
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
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ExplorerControl));
            treeView = new Utils.TreeViewDoubleBuffered();
            filterTextBox = new System.Windows.Forms.TextBox();
            fileContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            revealInFileExplorerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            recentFilesContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(components);
            clearRecentFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            fileContextMenuStrip.SuspendLayout();
            recentFilesContextMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // treeView
            // 
            treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            treeView.Location = new System.Drawing.Point(0, 23);
            treeView.Name = "treeView";
            treeView.Size = new System.Drawing.Size(581, 331);
            treeView.TabIndex = 2;
            treeView.NodeMouseClick += OnTreeViewNodeMouseClick;
            treeView.NodeMouseDoubleClick += OnTreeViewNodeMouseDoubleClick;
            // 
            // filterTextBox
            // 
            filterTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            filterTextBox.Location = new System.Drawing.Point(0, 0);
            filterTextBox.Name = "filterTextBox";
            filterTextBox.PlaceholderText = "Filter…";
            filterTextBox.Size = new System.Drawing.Size(581, 23);
            filterTextBox.TabIndex = 0;
            filterTextBox.TextChanged += OnFilterTextBoxTextChanged;
            // 
            // fileContextMenuStrip
            // 
            fileContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { revealInFileExplorerToolStripMenuItem });
            fileContextMenuStrip.Name = "fileContextMenuStrip";
            fileContextMenuStrip.Size = new System.Drawing.Size(189, 26);
            // 
            // revealInFileExplorerToolStripMenuItem
            // 
            revealInFileExplorerToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("revealInFileExplorerToolStripMenuItem.Image");
            revealInFileExplorerToolStripMenuItem.Name = "revealInFileExplorerToolStripMenuItem";
            revealInFileExplorerToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            revealInFileExplorerToolStripMenuItem.Text = "Reveal in File Explorer";
            revealInFileExplorerToolStripMenuItem.Click += OnRevealInFileExplorerClick;
            // 
            // recentFilesContextMenuStrip
            // 
            recentFilesContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { clearRecentFilesToolStripMenuItem });
            recentFilesContextMenuStrip.Name = "recentFilesContextMenuStrip";
            recentFilesContextMenuStrip.Size = new System.Drawing.Size(162, 26);
            // 
            // clearRecentFilesToolStripMenuItem
            // 
            clearRecentFilesToolStripMenuItem.Image = (System.Drawing.Image)resources.GetObject("clearRecentFilesToolStripMenuItem.Image");
            clearRecentFilesToolStripMenuItem.Name = "clearRecentFilesToolStripMenuItem";
            clearRecentFilesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            clearRecentFilesToolStripMenuItem.Text = "Clear recent files";
            clearRecentFilesToolStripMenuItem.Click += OnClearRecentFilesClick;
            // 
            // ExplorerControl
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(treeView);
            Controls.Add(filterTextBox);
            Name = "ExplorerControl";
            Size = new System.Drawing.Size(581, 354);
            VisibleChanged += OnVisibleChanged;
            fileContextMenuStrip.ResumeLayout(false);
            recentFilesContextMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.ContextMenuStrip fileContextMenuStrip;
        private Utils.TreeViewDoubleBuffered treeView;
        private System.Windows.Forms.ToolStripMenuItem revealInFileExplorerToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip recentFilesContextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearRecentFilesToolStripMenuItem;
    }
}

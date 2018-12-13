namespace AsmApp
{
	partial class AsmMainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmMainForm));
			this.tabPage3 = new System.Windows.Forms.TabPage();
			this.splitContainer2 = new System.Windows.Forms.SplitContainer();
			this.listBoxAttrs = new System.Windows.Forms.ListBox();
			this.contextMenuAttrs = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.toolStripMenuAttrsAdd = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuAttrsEdit = new System.Windows.Forms.ToolStripMenuItem();
			this.panel2 = new System.Windows.Forms.Panel();
			this.listBoxBlockTypes = new System.Windows.Forms.ListBox();
			this.contextMenuBlockTypes = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.RenameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btAddCont = new System.Windows.Forms.Button();
			this.btAddDoc = new System.Windows.Forms.Button();
			this.btParaDel = new System.Windows.Forms.Button();
			this.btEdit = new System.Windows.Forms.Button();
			this.btAddPara = new System.Windows.Forms.Button();
			this.btRefresh = new System.Windows.Forms.Button();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.tabPage3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
			this.splitContainer2.Panel1.SuspendLayout();
			this.splitContainer2.SuspendLayout();
			this.contextMenuAttrs.SuspendLayout();
			this.panel2.SuspendLayout();
			this.contextMenuBlockTypes.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabPage3
			// 
			this.tabPage3.Controls.Add(this.splitContainer2);
			this.tabPage3.Controls.Add(this.panel2);
			this.tabPage3.Location = new System.Drawing.Point(4, 22);
			this.tabPage3.Name = "tabPage3";
			this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage3.Size = new System.Drawing.Size(560, 546);
			this.tabPage3.TabIndex = 2;
			this.tabPage3.Text = "Блоки и Атрибуты";
			this.tabPage3.UseVisualStyleBackColor = true;
			// 
			// splitContainer2
			// 
			this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer2.Location = new System.Drawing.Point(203, 3);
			this.splitContainer2.Name = "splitContainer2";
			this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// splitContainer2.Panel1
			// 
			this.splitContainer2.Panel1.Controls.Add(this.listBoxAttrs);
			this.splitContainer2.Size = new System.Drawing.Size(354, 540);
			this.splitContainer2.SplitterDistance = 268;
			this.splitContainer2.TabIndex = 1;
			// 
			// listBoxAttrs
			// 
			this.listBoxAttrs.ContextMenuStrip = this.contextMenuAttrs;
			this.listBoxAttrs.Dock = System.Windows.Forms.DockStyle.Top;
			this.listBoxAttrs.FormattingEnabled = true;
			this.listBoxAttrs.Location = new System.Drawing.Point(0, 0);
			this.listBoxAttrs.Name = "listBoxAttrs";
			this.listBoxAttrs.Size = new System.Drawing.Size(352, 563);
			this.listBoxAttrs.TabIndex = 1;
			// 
			// contextMenuAttrs
			// 
			this.contextMenuAttrs.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuAttrsAdd,
            this.toolStripMenuAttrsEdit});
			this.contextMenuAttrs.Name = "contextMenuAttrs";
			this.contextMenuAttrs.Size = new System.Drawing.Size(155, 48);
			// 
			// toolStripMenuAttrsAdd
			// 
			this.toolStripMenuAttrsAdd.Name = "toolStripMenuAttrsAdd";
			this.toolStripMenuAttrsAdd.Size = new System.Drawing.Size(154, 22);
			this.toolStripMenuAttrsAdd.Text = "Добавить";
			this.toolStripMenuAttrsAdd.Click += new System.EventHandler(this.toolStripMenuAttrsAdd_Click);
			// 
			// toolStripMenuAttrsEdit
			// 
			this.toolStripMenuAttrsEdit.Name = "toolStripMenuAttrsEdit";
			this.toolStripMenuAttrsEdit.Size = new System.Drawing.Size(154, 22);
			this.toolStripMenuAttrsEdit.Text = "Редактировать";
			this.toolStripMenuAttrsEdit.Click += new System.EventHandler(this.toolStripMenuAttrsEdit_Click);
			// 
			// panel2
			// 
			this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel2.Controls.Add(this.listBoxBlockTypes);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(200, 540);
			this.panel2.TabIndex = 0;
			// 
			// listBoxBlockTypes
			// 
			this.listBoxBlockTypes.ContextMenuStrip = this.contextMenuBlockTypes;
			this.listBoxBlockTypes.Dock = System.Windows.Forms.DockStyle.Top;
			this.listBoxBlockTypes.FormattingEnabled = true;
			this.listBoxBlockTypes.Location = new System.Drawing.Point(0, 0);
			this.listBoxBlockTypes.Name = "listBoxBlockTypes";
			this.listBoxBlockTypes.Size = new System.Drawing.Size(198, 563);
			this.listBoxBlockTypes.TabIndex = 0;
			this.listBoxBlockTypes.SelectedIndexChanged += new System.EventHandler(this.listBoxBlockTypes_SelectedIndexChanged);
			// 
			// contextMenuBlockTypes
			// 
			this.contextMenuBlockTypes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.RenameToolStripMenuItem});
			this.contextMenuBlockTypes.Name = "contextMenuBlockTypes";
			this.contextMenuBlockTypes.Size = new System.Drawing.Size(162, 26);
			// 
			// RenameToolStripMenuItem
			// 
			this.RenameToolStripMenuItem.Name = "RenameToolStripMenuItem";
			this.RenameToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
			this.RenameToolStripMenuItem.Text = "Переименовать";
			this.RenameToolStripMenuItem.Click += new System.EventHandler(this.RenameToolStripMenuItem_Click);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage3);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 24);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(568, 572);
			this.tabControl1.TabIndex = 21;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.treeView1);
			this.tabPage1.Controls.Add(this.panel3);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(560, 546);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Хранилище";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(560, 424);
			this.treeView1.TabIndex = 24;
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.btAddCont);
			this.panel3.Controls.Add(this.btAddDoc);
			this.panel3.Controls.Add(this.btParaDel);
			this.panel3.Controls.Add(this.btEdit);
			this.panel3.Controls.Add(this.btAddPara);
			this.panel3.Controls.Add(this.btRefresh);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 424);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(560, 122);
			this.panel3.TabIndex = 23;
			// 
			// btAddCont
			// 
			this.btAddCont.Location = new System.Drawing.Point(211, 49);
			this.btAddCont.Name = "btAddCont";
			this.btAddCont.Size = new System.Drawing.Size(107, 23);
			this.btAddCont.TabIndex = 5;
			this.btAddCont.Text = "Новый контейнер";
			this.btAddCont.UseVisualStyleBackColor = true;
			this.btAddCont.Click += new System.EventHandler(this.btAddCont_Click);
			// 
			// btAddDoc
			// 
			this.btAddDoc.Location = new System.Drawing.Point(98, 49);
			this.btAddDoc.Name = "btAddDoc";
			this.btAddDoc.Size = new System.Drawing.Size(107, 23);
			this.btAddDoc.TabIndex = 4;
			this.btAddDoc.Text = "Новый документ";
			this.btAddDoc.UseVisualStyleBackColor = true;
			// 
			// btParaDel
			// 
			this.btParaDel.Location = new System.Drawing.Point(5, 78);
			this.btParaDel.Name = "btParaDel";
			this.btParaDel.Size = new System.Drawing.Size(87, 26);
			this.btParaDel.TabIndex = 3;
			this.btParaDel.Text = "Удалить абзац";
			this.btParaDel.UseVisualStyleBackColor = true;
			// 
			// btEdit
			// 
			this.btEdit.Location = new System.Drawing.Point(98, 14);
			this.btEdit.Name = "btEdit";
			this.btEdit.Size = new System.Drawing.Size(107, 26);
			this.btEdit.TabIndex = 2;
			this.btEdit.Text = "Редактировать";
			this.btEdit.UseVisualStyleBackColor = true;
			// 
			// btAddPara
			// 
			this.btAddPara.Location = new System.Drawing.Point(5, 46);
			this.btAddPara.Name = "btAddPara";
			this.btAddPara.Size = new System.Drawing.Size(87, 26);
			this.btAddPara.TabIndex = 1;
			this.btAddPara.Text = "Новый абзац";
			this.btAddPara.UseVisualStyleBackColor = true;
			// 
			// btRefresh
			// 
			this.btRefresh.Location = new System.Drawing.Point(5, 14);
			this.btRefresh.Name = "btRefresh";
			this.btRefresh.Size = new System.Drawing.Size(87, 26);
			this.btRefresh.TabIndex = 0;
			this.btRefresh.Text = "Обновить";
			this.btRefresh.UseVisualStyleBackColor = true;
			this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(568, 24);
			this.menuStrip1.TabIndex = 22;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// AsmMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 596);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.menuStrip1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AsmMainForm";
			this.Text = "Form1";
			this.tabPage3.ResumeLayout(false);
			this.splitContainer2.Panel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
			this.splitContainer2.ResumeLayout(false);
			this.contextMenuAttrs.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.contextMenuBlockTypes.ResumeLayout(false);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabPage tabPage3;
		private System.Windows.Forms.SplitContainer splitContainer2;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ListBox listBoxBlockTypes;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.ListBox listBoxAttrs;
		private System.Windows.Forms.ContextMenuStrip contextMenuBlockTypes;
		private System.Windows.Forms.ToolStripMenuItem RenameToolStripMenuItem;
		private System.Windows.Forms.ContextMenuStrip contextMenuAttrs;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuAttrsAdd;
		private System.Windows.Forms.ToolStripMenuItem toolStripMenuAttrsEdit;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button btAddCont;
		private System.Windows.Forms.Button btAddDoc;
		private System.Windows.Forms.Button btParaDel;
		private System.Windows.Forms.Button btEdit;
		private System.Windows.Forms.Button btAddPara;
		private System.Windows.Forms.Button btRefresh;
		private System.Windows.Forms.TreeView treeView1;
	}
}


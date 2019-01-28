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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AsmMainForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btAddCont = new System.Windows.Forms.Button();
			this.btAddDoc = new System.Windows.Forms.Button();
			this.btDelNode = new System.Windows.Forms.Button();
			this.btEdit = new System.Windows.Forms.Button();
			this.btAddPara = new System.Windows.Forms.Button();
			this.btRefresh = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(568, 596);
			this.tabControl1.TabIndex = 21;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.treeView1);
			this.tabPage1.Controls.Add(this.panel3);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Size = new System.Drawing.Size(560, 570);
			this.tabPage1.TabIndex = 3;
			this.tabPage1.Text = "Хранилище";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(560, 448);
			this.treeView1.TabIndex = 24;
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.btAddCont);
			this.panel3.Controls.Add(this.btAddDoc);
			this.panel3.Controls.Add(this.btDelNode);
			this.panel3.Controls.Add(this.btEdit);
			this.panel3.Controls.Add(this.btAddPara);
			this.panel3.Controls.Add(this.btRefresh);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 448);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(560, 122);
			this.panel3.TabIndex = 23;
			// 
			// btAddCont
			// 
			this.btAddCont.Location = new System.Drawing.Point(5, 46);
			this.btAddCont.Name = "btAddCont";
			this.btAddCont.Size = new System.Drawing.Size(107, 23);
			this.btAddCont.TabIndex = 5;
			this.btAddCont.Text = "Новый контейнер";
			this.btAddCont.UseVisualStyleBackColor = true;
			this.btAddCont.Click += new System.EventHandler(this.btAddCont_Click);
			// 
			// btAddDoc
			// 
			this.btAddDoc.Location = new System.Drawing.Point(118, 49);
			this.btAddDoc.Name = "btAddDoc";
			this.btAddDoc.Size = new System.Drawing.Size(107, 23);
			this.btAddDoc.TabIndex = 4;
			this.btAddDoc.Text = "Новый документ";
			this.btAddDoc.UseVisualStyleBackColor = true;
			this.btAddDoc.Click += new System.EventHandler(this.btAddDoc_Click);
			// 
			// btDelNode
			// 
			this.btDelNode.Location = new System.Drawing.Point(5, 78);
			this.btDelNode.Name = "btDelNode";
			this.btDelNode.Size = new System.Drawing.Size(87, 26);
			this.btDelNode.TabIndex = 3;
			this.btDelNode.Text = "Удалить";
			this.btDelNode.UseVisualStyleBackColor = true;
			this.btDelNode.Click += new System.EventHandler(this.btDelNode_Click);
			// 
			// btEdit
			// 
			this.btEdit.Location = new System.Drawing.Point(201, 17);
			this.btEdit.Name = "btEdit";
			this.btEdit.Size = new System.Drawing.Size(107, 26);
			this.btEdit.TabIndex = 2;
			this.btEdit.Text = "Редактировать";
			this.btEdit.UseVisualStyleBackColor = true;
			// 
			// btAddPara
			// 
			this.btAddPara.Location = new System.Drawing.Point(231, 49);
			this.btAddPara.Name = "btAddPara";
			this.btAddPara.Size = new System.Drawing.Size(87, 26);
			this.btAddPara.TabIndex = 1;
			this.btAddPara.Text = "Новый абзац";
			this.btAddPara.UseVisualStyleBackColor = true;
			this.btAddPara.Click += new System.EventHandler(this.btAddPara_Click);
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
			// AsmMainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(568, 596);
			this.Controls.Add(this.tabControl1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "AsmMainForm";
			this.Text = "Form1";
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Button btAddCont;
		private System.Windows.Forms.Button btAddDoc;
		private System.Windows.Forms.Button btDelNode;
		private System.Windows.Forms.Button btEdit;
		private System.Windows.Forms.Button btAddPara;
		private System.Windows.Forms.Button btRefresh;
		private System.Windows.Forms.TreeView treeView1;
	}
}


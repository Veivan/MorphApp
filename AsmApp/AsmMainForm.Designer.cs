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
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btMakeSynAn = new System.Windows.Forms.Button();
			this.btMakeDoc = new System.Windows.Forms.Button();
			this.btReadPara = new System.Windows.Forms.Button();
			this.btSavePara = new System.Windows.Forms.Button();
			this.btRestore = new System.Windows.Forms.Button();
			this.btTokenize = new System.Windows.Forms.Button();
			this.btSaveLex = new System.Windows.Forms.Button();
			this.btDBGetWord = new System.Windows.Forms.Button();
			this.memoOut = new System.Windows.Forms.TextBox();
			this.memoInp = new System.Windows.Forms.TextBox();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
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
			this.btEdit.Click += new System.EventHandler(this.btEdit_Click);
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
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.panel1);
			this.tabPage2.Controls.Add(this.memoOut);
			this.tabPage2.Controls.Add(this.memoInp);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(560, 570);
			this.tabPage2.TabIndex = 4;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btMakeSynAn);
			this.panel1.Controls.Add(this.btMakeDoc);
			this.panel1.Controls.Add(this.btReadPara);
			this.panel1.Controls.Add(this.btSavePara);
			this.panel1.Controls.Add(this.btRestore);
			this.panel1.Controls.Add(this.btTokenize);
			this.panel1.Controls.Add(this.btSaveLex);
			this.panel1.Controls.Add(this.btDBGetWord);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 467);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(554, 100);
			this.panel1.TabIndex = 8;
			// 
			// btMakeSynAn
			// 
			this.btMakeSynAn.Location = new System.Drawing.Point(5, 13);
			this.btMakeSynAn.Name = "btMakeSynAn";
			this.btMakeSynAn.Size = new System.Drawing.Size(75, 23);
			this.btMakeSynAn.TabIndex = 23;
			this.btMakeSynAn.Text = "SynAn";
			this.btMakeSynAn.UseVisualStyleBackColor = true;
			this.btMakeSynAn.Click += new System.EventHandler(this.btMakeSynAn_Click);
			// 
			// btMakeDoc
			// 
			this.btMakeDoc.Location = new System.Drawing.Point(138, 14);
			this.btMakeDoc.Name = "btMakeDoc";
			this.btMakeDoc.Size = new System.Drawing.Size(75, 20);
			this.btMakeDoc.TabIndex = 22;
			this.btMakeDoc.Text = "Make Doc";
			this.btMakeDoc.UseVisualStyleBackColor = true;
			// 
			// btReadPara
			// 
			this.btReadPara.Location = new System.Drawing.Point(219, 40);
			this.btReadPara.Name = "btReadPara";
			this.btReadPara.Size = new System.Drawing.Size(75, 20);
			this.btReadPara.TabIndex = 21;
			this.btReadPara.Text = "Read Para";
			this.btReadPara.UseVisualStyleBackColor = true;
			// 
			// btSavePara
			// 
			this.btSavePara.Location = new System.Drawing.Point(138, 40);
			this.btSavePara.Name = "btSavePara";
			this.btSavePara.Size = new System.Drawing.Size(75, 20);
			this.btSavePara.TabIndex = 20;
			this.btSavePara.Text = "Save para";
			this.btSavePara.UseVisualStyleBackColor = true;
			this.btSavePara.Click += new System.EventHandler(this.btSavePara_Click);
			// 
			// btRestore
			// 
			this.btRestore.Location = new System.Drawing.Point(300, 14);
			this.btRestore.Name = "btRestore";
			this.btRestore.Size = new System.Drawing.Size(75, 20);
			this.btRestore.TabIndex = 19;
			this.btRestore.Text = "Restore";
			this.btRestore.UseVisualStyleBackColor = true;
			// 
			// btTokenize
			// 
			this.btTokenize.Location = new System.Drawing.Point(219, 14);
			this.btTokenize.Name = "btTokenize";
			this.btTokenize.Size = new System.Drawing.Size(75, 20);
			this.btTokenize.TabIndex = 18;
			this.btTokenize.Text = "Tokenize";
			this.btTokenize.UseVisualStyleBackColor = true;
			// 
			// btSaveLex
			// 
			this.btSaveLex.Location = new System.Drawing.Point(138, 66);
			this.btSaveLex.Name = "btSaveLex";
			this.btSaveLex.Size = new System.Drawing.Size(78, 23);
			this.btSaveLex.TabIndex = 17;
			this.btSaveLex.Text = "SaveLex";
			this.btSaveLex.UseVisualStyleBackColor = true;
			// 
			// btDBGetWord
			// 
			this.btDBGetWord.Location = new System.Drawing.Point(222, 66);
			this.btDBGetWord.Name = "btDBGetWord";
			this.btDBGetWord.Size = new System.Drawing.Size(75, 23);
			this.btDBGetWord.TabIndex = 16;
			this.btDBGetWord.Text = "DBGetWord";
			this.btDBGetWord.UseVisualStyleBackColor = true;
			// 
			// memoOut
			// 
			this.memoOut.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoOut.Location = new System.Drawing.Point(3, 251);
			this.memoOut.Multiline = true;
			this.memoOut.Name = "memoOut";
			this.memoOut.ReadOnly = true;
			this.memoOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memoOut.Size = new System.Drawing.Size(554, 316);
			this.memoOut.TabIndex = 7;
			// 
			// memoInp
			// 
			this.memoInp.Dock = System.Windows.Forms.DockStyle.Top;
			this.memoInp.Location = new System.Drawing.Point(3, 3);
			this.memoInp.Multiline = true;
			this.memoInp.Name = "memoInp";
			this.memoInp.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memoInp.Size = new System.Drawing.Size(554, 248);
			this.memoInp.TabIndex = 5;
			this.memoInp.Text = "Мама мыла раму.";
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
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.panel1.ResumeLayout(false);
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
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox memoInp;
		private System.Windows.Forms.TextBox memoOut;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btMakeSynAn;
		private System.Windows.Forms.Button btMakeDoc;
		private System.Windows.Forms.Button btReadPara;
		private System.Windows.Forms.Button btSavePara;
		private System.Windows.Forms.Button btRestore;
		private System.Windows.Forms.Button btTokenize;
		private System.Windows.Forms.Button btSaveLex;
		private System.Windows.Forms.Button btDBGetWord;
	}
}


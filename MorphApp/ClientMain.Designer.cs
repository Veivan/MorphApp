namespace MorphApp
{
    partial class ClientMain
    {
        /// <summary>
        /// Требуется переменная конструктора.
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
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel3 = new System.Windows.Forms.Panel();
			this.btRefresh = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.memoOut = new System.Windows.Forms.TextBox();
			this.memoInp = new System.Windows.Forms.TextBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.btReadPara = new System.Windows.Forms.Button();
			this.btSavePara = new System.Windows.Forms.Button();
			this.btRestore = new System.Windows.Forms.Button();
			this.btTokenize = new System.Windows.Forms.Button();
			this.btSaveLex = new System.Windows.Forms.Button();
			this.btDBGetWord = new System.Windows.Forms.Button();
			this.btMakeSynAn = new System.Windows.Forms.Button();
			this.btGetMorph = new System.Windows.Forms.Button();
			this.statusStrip1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel3.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
			this.statusStrip1.Location = new System.Drawing.Point(0, 648);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(633, 22);
			this.statusStrip1.TabIndex = 3;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(97, 17);
			this.toolStripStatusLabel1.Text = "Версия словаря:";
			// 
			// toolStripStatusLabel2
			// 
			this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
			this.toolStripStatusLabel2.Size = new System.Drawing.Size(0, 17);
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(633, 648);
			this.tabControl1.TabIndex = 20;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.panel3);
			this.tabPage1.Controls.Add(this.panel2);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(625, 622);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "tabPage1";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.btRefresh);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(3, 555);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(619, 64);
			this.panel3.TabIndex = 22;
			// 
			// btRefresh
			// 
			this.btRefresh.Location = new System.Drawing.Point(5, 14);
			this.btRefresh.Name = "btRefresh";
			this.btRefresh.Size = new System.Drawing.Size(87, 26);
			this.btRefresh.TabIndex = 0;
			this.btRefresh.Text = "Refresh";
			this.btRefresh.UseVisualStyleBackColor = true;
			this.btRefresh.Click += new System.EventHandler(this.btRefresh_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.treeView1);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(3, 3);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(619, 616);
			this.panel2.TabIndex = 21;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(324, 616);
			this.treeView1.TabIndex = 21;
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.memoOut);
			this.tabPage2.Controls.Add(this.memoInp);
			this.tabPage2.Controls.Add(this.panel1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(625, 622);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "tabPage2";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// memoOut
			// 
			this.memoOut.Dock = System.Windows.Forms.DockStyle.Fill;
			this.memoOut.Location = new System.Drawing.Point(3, 251);
			this.memoOut.Multiline = true;
			this.memoOut.Name = "memoOut";
			this.memoOut.ReadOnly = true;
			this.memoOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memoOut.Size = new System.Drawing.Size(619, 268);
			this.memoOut.TabIndex = 6;
			// 
			// memoInp
			// 
			this.memoInp.Dock = System.Windows.Forms.DockStyle.Top;
			this.memoInp.Location = new System.Drawing.Point(3, 3);
			this.memoInp.Multiline = true;
			this.memoInp.Name = "memoInp";
			this.memoInp.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memoInp.Size = new System.Drawing.Size(619, 248);
			this.memoInp.TabIndex = 4;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btReadPara);
			this.panel1.Controls.Add(this.btSavePara);
			this.panel1.Controls.Add(this.btRestore);
			this.panel1.Controls.Add(this.btTokenize);
			this.panel1.Controls.Add(this.btSaveLex);
			this.panel1.Controls.Add(this.btDBGetWord);
			this.panel1.Controls.Add(this.btMakeSynAn);
			this.panel1.Controls.Add(this.btGetMorph);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 519);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(619, 100);
			this.panel1.TabIndex = 0;
			// 
			// btReadPara
			// 
			this.btReadPara.Location = new System.Drawing.Point(392, 11);
			this.btReadPara.Name = "btReadPara";
			this.btReadPara.Size = new System.Drawing.Size(75, 20);
			this.btReadPara.TabIndex = 21;
			this.btReadPara.Text = "Read Para";
			this.btReadPara.UseVisualStyleBackColor = true;
			this.btReadPara.Click += new System.EventHandler(this.btReadPara_Click);
			// 
			// btSavePara
			// 
			this.btSavePara.Location = new System.Drawing.Point(311, 11);
			this.btSavePara.Name = "btSavePara";
			this.btSavePara.Size = new System.Drawing.Size(75, 20);
			this.btSavePara.TabIndex = 20;
			this.btSavePara.Text = "Save para";
			this.btSavePara.UseVisualStyleBackColor = true;
			this.btSavePara.Click += new System.EventHandler(this.btSavePara_Click);
			// 
			// btRestore
			// 
			this.btRestore.Location = new System.Drawing.Point(230, 11);
			this.btRestore.Name = "btRestore";
			this.btRestore.Size = new System.Drawing.Size(75, 20);
			this.btRestore.TabIndex = 19;
			this.btRestore.Text = "Restore";
			this.btRestore.UseVisualStyleBackColor = true;
			this.btRestore.Click += new System.EventHandler(this.btRestore_Click);
			// 
			// btTokenize
			// 
			this.btTokenize.Location = new System.Drawing.Point(149, 11);
			this.btTokenize.Name = "btTokenize";
			this.btTokenize.Size = new System.Drawing.Size(75, 20);
			this.btTokenize.TabIndex = 18;
			this.btTokenize.Text = "Tokenize";
			this.btTokenize.UseVisualStyleBackColor = true;
			this.btTokenize.Click += new System.EventHandler(this.btTokenize_Click);
			// 
			// btSaveLex
			// 
			this.btSaveLex.Location = new System.Drawing.Point(392, 37);
			this.btSaveLex.Name = "btSaveLex";
			this.btSaveLex.Size = new System.Drawing.Size(78, 23);
			this.btSaveLex.TabIndex = 17;
			this.btSaveLex.Text = "SaveLex";
			this.btSaveLex.UseVisualStyleBackColor = true;
			this.btSaveLex.Click += new System.EventHandler(this.btSaveLex_Click);
			// 
			// btDBGetWord
			// 
			this.btDBGetWord.Location = new System.Drawing.Point(311, 37);
			this.btDBGetWord.Name = "btDBGetWord";
			this.btDBGetWord.Size = new System.Drawing.Size(75, 23);
			this.btDBGetWord.TabIndex = 16;
			this.btDBGetWord.Text = "DBGetWord";
			this.btDBGetWord.UseVisualStyleBackColor = true;
			this.btDBGetWord.Click += new System.EventHandler(this.btDBGetWord_Click);
			// 
			// btMakeSynAn
			// 
			this.btMakeSynAn.Location = new System.Drawing.Point(230, 37);
			this.btMakeSynAn.Name = "btMakeSynAn";
			this.btMakeSynAn.Size = new System.Drawing.Size(75, 23);
			this.btMakeSynAn.TabIndex = 15;
			this.btMakeSynAn.Text = "SynAn";
			this.btMakeSynAn.UseVisualStyleBackColor = true;
			this.btMakeSynAn.Click += new System.EventHandler(this.btMakeSynAn_Click);
			// 
			// btGetMorph
			// 
			this.btGetMorph.Location = new System.Drawing.Point(149, 37);
			this.btGetMorph.Name = "btGetMorph";
			this.btGetMorph.Size = new System.Drawing.Size(75, 23);
			this.btGetMorph.TabIndex = 14;
			this.btGetMorph.Text = "MorphAn";
			this.btGetMorph.UseVisualStyleBackColor = true;
			this.btGetMorph.Click += new System.EventHandler(this.btGetMorph_Click);
			// 
			// ClientMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(633, 670);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.statusStrip1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ClientMain";
			this.Text = "MorphApp";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel3.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TreeView treeView1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox memoOut;
		private System.Windows.Forms.TextBox memoInp;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btReadPara;
		private System.Windows.Forms.Button btSavePara;
		private System.Windows.Forms.Button btRestore;
		private System.Windows.Forms.Button btTokenize;
		private System.Windows.Forms.Button btSaveLex;
		private System.Windows.Forms.Button btDBGetWord;
		private System.Windows.Forms.Button btMakeSynAn;
		private System.Windows.Forms.Button btGetMorph;
		private System.Windows.Forms.Button btRefresh;
    }
}


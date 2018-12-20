namespace AsmApp
{
	partial class FormParaEdit
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel4 = new System.Windows.Forms.Panel();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.memoHeader = new System.Windows.Forms.TextBox();
			this.panel5 = new System.Windows.Forms.Panel();
			this.btClose = new System.Windows.Forms.Button();
			this.btParaSave = new System.Windows.Forms.Button();
			this.panel2 = new System.Windows.Forms.Panel();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.memoBody = new System.Windows.Forms.TextBox();
			this.rtbBody = new RicherTextBox.RicherTextBox();
			this.panel4.SuspendLayout();
			this.panel5.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel4
			// 
			this.panel4.Controls.Add(this.textBox2);
			this.panel4.Controls.Add(this.memoHeader);
			this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel4.Location = new System.Drawing.Point(0, 0);
			this.panel4.Name = "panel4";
			this.panel4.Size = new System.Drawing.Size(521, 66);
			this.panel4.TabIndex = 23;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(3, 3);
			this.textBox2.Name = "textBox2";
			this.textBox2.ReadOnly = true;
			this.textBox2.Size = new System.Drawing.Size(100, 20);
			this.textBox2.TabIndex = 26;
			this.textBox2.Text = "Заголовок";
			// 
			// memoHeader
			// 
			this.memoHeader.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.memoHeader.Location = new System.Drawing.Point(0, 29);
			this.memoHeader.Multiline = true;
			this.memoHeader.Name = "memoHeader";
			this.memoHeader.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memoHeader.Size = new System.Drawing.Size(521, 37);
			this.memoHeader.TabIndex = 25;
			// 
			// panel5
			// 
			this.panel5.Controls.Add(this.btClose);
			this.panel5.Controls.Add(this.btParaSave);
			this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel5.Location = new System.Drawing.Point(0, 550);
			this.panel5.Name = "panel5";
			this.panel5.Size = new System.Drawing.Size(521, 39);
			this.panel5.TabIndex = 25;
			// 
			// btClose
			// 
			this.btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btClose.Location = new System.Drawing.Point(443, 6);
			this.btClose.Name = "btClose";
			this.btClose.Size = new System.Drawing.Size(75, 20);
			this.btClose.TabIndex = 29;
			this.btClose.Text = "Выход";
			this.btClose.UseVisualStyleBackColor = true;
			this.btClose.Click += new System.EventHandler(this.btClose_Click);
			// 
			// btParaSave
			// 
			this.btParaSave.Location = new System.Drawing.Point(362, 6);
			this.btParaSave.Name = "btParaSave";
			this.btParaSave.Size = new System.Drawing.Size(75, 20);
			this.btParaSave.TabIndex = 28;
			this.btParaSave.Text = "Сохранить";
			this.btParaSave.UseVisualStyleBackColor = true;
			this.btParaSave.Click += new System.EventHandler(this.btParaSave_Click);
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.rtbBody);
			this.panel2.Controls.Add(this.textBox3);
			this.panel2.Controls.Add(this.memoBody);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel2.Location = new System.Drawing.Point(0, 66);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(521, 484);
			this.panel2.TabIndex = 26;
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(3, 18);
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size(100, 20);
			this.textBox3.TabIndex = 27;
			this.textBox3.Text = "Содержание";
			// 
			// memoBody
			// 
			this.memoBody.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.memoBody.Location = new System.Drawing.Point(0, 359);
			this.memoBody.Multiline = true;
			this.memoBody.Name = "memoBody";
			this.memoBody.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.memoBody.Size = new System.Drawing.Size(521, 125);
			this.memoBody.TabIndex = 25;
			// 
			// rtbBody
			// 
			this.rtbBody.AlignCenterVisible = true;
			this.rtbBody.AlignLeftVisible = true;
			this.rtbBody.AlignRightVisible = true;
			this.rtbBody.BoldVisible = true;
			this.rtbBody.BulletsVisible = false;
			this.rtbBody.ChooseFontVisible = true;
			this.rtbBody.FindReplaceVisible = true;
			this.rtbBody.FontColorVisible = true;
			this.rtbBody.FontFamilyVisible = true;
			this.rtbBody.FontSizeVisible = true;
			this.rtbBody.GroupAlignmentVisible = true;
			this.rtbBody.GroupBoldUnderlineItalicVisible = true;
			this.rtbBody.GroupFontColorVisible = true;
			this.rtbBody.GroupFontNameAndSizeVisible = true;
			this.rtbBody.GroupIndentationAndBulletsVisible = false;
			this.rtbBody.GroupInsertVisible = false;
			this.rtbBody.GroupSaveAndLoadVisible = true;
			this.rtbBody.GroupZoomVisible = false;
			this.rtbBody.INDENT = 10;
			this.rtbBody.IndentVisible = true;
			this.rtbBody.InsertPictureVisible = false;
			this.rtbBody.ItalicVisible = true;
			this.rtbBody.LoadVisible = true;
			this.rtbBody.Location = new System.Drawing.Point(0, 44);
			this.rtbBody.Name = "rtbBody";
			this.rtbBody.OutdentVisible = true;
			this.rtbBody.Rtf = "{\\rtf1\\ansi\\deff0{\\fonttbl{\\f0\\fnil\\fcharset204 Microsoft Sans Serif;}}\r\n\\viewkin" +
	"d4\\uc1\\pard\\lang1049\\f0\\fs18 richerTextBox1\\par\r\n}\r\n";
			this.rtbBody.SaveVisible = true;
			this.rtbBody.SeparatorAlignVisible = true;
			this.rtbBody.SeparatorBoldUnderlineItalicVisible = true;
			this.rtbBody.SeparatorFontColorVisible = true;
			this.rtbBody.SeparatorFontVisible = true;
			this.rtbBody.SeparatorIndentAndBulletsVisible = false;
			this.rtbBody.SeparatorInsertVisible = false;
			this.rtbBody.SeparatorSaveLoadVisible = true;
			this.rtbBody.Size = new System.Drawing.Size(518, 181);
			this.rtbBody.TabIndex = 31;
			this.rtbBody.ToolStripVisible = true;
			this.rtbBody.UnderlineVisible = true;
			this.rtbBody.WordWrapVisible = true;
			this.rtbBody.ZoomFactorTextVisible = false;
			this.rtbBody.ZoomInVisible = false;
			this.rtbBody.ZoomOutVisible = false;
			// 
			// FormParaEdit
			// 
			this.AcceptButton = this.btParaSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btClose;
			this.ClientSize = new System.Drawing.Size(521, 589);
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel5);
			this.Controls.Add(this.panel4);
			this.Name = "FormParaEdit";
			this.Text = "FormParaEdit";
			this.Load += new System.EventHandler(this.FormParaEdit_Load);
			this.panel4.ResumeLayout(false);
			this.panel4.PerformLayout();
			this.panel5.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel4;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox memoHeader;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Button btParaSave;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.TextBox memoBody;
		private System.Windows.Forms.Button btClose;
		private RicherTextBox.RicherTextBox rtbBody;
	}
}
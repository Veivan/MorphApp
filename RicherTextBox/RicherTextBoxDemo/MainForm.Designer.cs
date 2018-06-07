namespace RicherTextBoxDemo
{
    partial class MainForm
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
            this.richerTextBox1 = new RicherTextBox.RicherTextBox();
            this.SuspendLayout();
            // 
            // richerTextBox1
            // 
            this.richerTextBox1.AlignCenterVisible = true;
            this.richerTextBox1.AlignLeftVisible = true;
            this.richerTextBox1.AlignRightVisible = true;
            this.richerTextBox1.BoldVisible = true;
            this.richerTextBox1.BulletsVisible = true;
            this.richerTextBox1.ChooseFontVisible = true;
            this.richerTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richerTextBox1.FindReplaceVisible = true;
            this.richerTextBox1.FontColorVisible = true;
            this.richerTextBox1.FontFamilyVisible = true;
            this.richerTextBox1.FontSizeVisible = true;
            this.richerTextBox1.GroupAlignmentVisible = true;
            this.richerTextBox1.GroupBoldUnderlineItalicVisible = true;
            this.richerTextBox1.GroupFontColorVisible = true;
            this.richerTextBox1.GroupFontNameAndSizeVisible = true;
            this.richerTextBox1.GroupIndentationAndBulletsVisible = true;
            this.richerTextBox1.GroupInsertVisible = true;
            this.richerTextBox1.GroupSaveAndLoadVisible = true;
            this.richerTextBox1.GroupZoomVisible = true;
            this.richerTextBox1.INDENT = 10;
            this.richerTextBox1.IndentVisible = true;
            this.richerTextBox1.InsertPictureVisible = true;
            this.richerTextBox1.ItalicVisible = true;
            this.richerTextBox1.LoadVisible = true;
            this.richerTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richerTextBox1.Name = "richerTextBox1";
            this.richerTextBox1.OutdentVisible = true;
            this.richerTextBox1.Rtf = "{\\rtf1\\ansi\\ansicpg1251\\deff0\\deflang1026{\\fonttbl{\\f0\\fnil\\fcharset204 Microsoft" +
                " Sans Serif;}}\r\n\\viewkind4\\uc1\\pard\\f0\\fs18\\par\r\n}\r\n";
            this.richerTextBox1.SaveVisible = true;
            this.richerTextBox1.SeparatorAlignVisible = true;
            this.richerTextBox1.SeparatorBoldUnderlineItalicVisible = true;
            this.richerTextBox1.SeparatorFontColorVisible = true;
            this.richerTextBox1.SeparatorFontVisible = true;
            this.richerTextBox1.SeparatorIndentAndBulletsVisible = true;
            this.richerTextBox1.SeparatorInsertVisible = true;
            this.richerTextBox1.SeparatorSaveLoadVisible = true;
            this.richerTextBox1.Size = new System.Drawing.Size(637, 400);
            this.richerTextBox1.TabIndex = 0;
            this.richerTextBox1.ToolStripVisible = true;
            this.richerTextBox1.UnderlineVisible = true;
            this.richerTextBox1.WordWrapVisible = true;
            this.richerTextBox1.ZoomFactorTextVisible = true;
            this.richerTextBox1.ZoomInVisible = true;
            this.richerTextBox1.ZoomOutVisible = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(637, 400);
            this.Controls.Add(this.richerTextBox1);
            this.Name = "MainForm";
            this.Text = "RicherTextBox Demo by Svetoslav Savov";
            this.ResumeLayout(false);

        }

        #endregion

        private RicherTextBox.RicherTextBox richerTextBox1;
    }
}


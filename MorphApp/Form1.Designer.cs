namespace MorphApp
{
    partial class Form1
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
            this.btGetMorph = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.memoInp = new System.Windows.Forms.TextBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.memoOut = new System.Windows.Forms.TextBox();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btGetMorph
            // 
            this.btGetMorph.Location = new System.Drawing.Point(12, 316);
            this.btGetMorph.Name = "btGetMorph";
            this.btGetMorph.Size = new System.Drawing.Size(75, 23);
            this.btGetMorph.TabIndex = 0;
            this.btGetMorph.Text = "Morph";
            this.btGetMorph.UseVisualStyleBackColor = true;
            this.btGetMorph.Click += new System.EventHandler(this.btGetMorph_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(12, 290);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 20);
            this.textBox1.TabIndex = 1;
            // 
            // memoInp
            // 
            this.memoInp.Location = new System.Drawing.Point(12, 12);
            this.memoInp.Multiline = true;
            this.memoInp.Name = "memoInp";
            this.memoInp.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.memoInp.Size = new System.Drawing.Size(363, 123);
            this.memoInp.TabIndex = 2;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 345);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(387, 22);
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
            // memoOut
            // 
            this.memoOut.Location = new System.Drawing.Point(12, 161);
            this.memoOut.Multiline = true;
            this.memoOut.Name = "memoOut";
            this.memoOut.ReadOnly = true;
            this.memoOut.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.memoOut.Size = new System.Drawing.Size(363, 123);
            this.memoOut.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(387, 367);
            this.Controls.Add(this.memoOut);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.memoInp);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.btGetMorph);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btGetMorph;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox memoInp;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.TextBox memoOut;
    }
}


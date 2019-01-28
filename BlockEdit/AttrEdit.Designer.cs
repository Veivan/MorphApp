namespace BlockEdit
{
	partial class AttrEdit
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.panel2 = new System.Windows.Forms.Panel();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cbMandatory = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.cbAttrTypes = new System.Windows.Forms.ComboBox();
			this.btSave = new System.Windows.Forms.Button();
			this.button2 = new System.Windows.Forms.Button();
			this.edNameKey = new System.Windows.Forms.TextBox();
			this.edNameUI = new System.Windows.Forms.TextBox();
			this.edOrder = new System.Windows.Forms.TextBox();
			this.panel1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.edOrder);
			this.panel1.Controls.Add(this.edNameUI);
			this.panel1.Controls.Add(this.edNameKey);
			this.panel1.Controls.Add(this.cbAttrTypes);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.cbMandatory);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(439, 253);
			this.panel1.TabIndex = 0;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button2);
			this.panel2.Controls.Add(this.btSave);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(0, 193);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(439, 60);
			this.panel2.TabIndex = 1;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 18);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(33, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Ключ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 43);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Видимое значение";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 71);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(114, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Порядок следования";
			// 
			// cbMandatory
			// 
			this.cbMandatory.AutoSize = true;
			this.cbMandatory.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cbMandatory.Location = new System.Drawing.Point(12, 102);
			this.cbMandatory.Name = "cbMandatory";
			this.cbMandatory.Size = new System.Drawing.Size(161, 17);
			this.cbMandatory.TabIndex = 3;
			this.cbMandatory.Text = "Обязателен к заполнению";
			this.cbMandatory.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 141);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(66, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Тип данных";
			// 
			// cbAttrTypes
			// 
			this.cbAttrTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbAttrTypes.FormattingEnabled = true;
			this.cbAttrTypes.Location = new System.Drawing.Point(132, 138);
			this.cbAttrTypes.Name = "cbAttrTypes";
			this.cbAttrTypes.Size = new System.Drawing.Size(121, 21);
			this.cbAttrTypes.TabIndex = 5;
			// 
			// btSave
			// 
			this.btSave.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btSave.Location = new System.Drawing.Point(259, 25);
			this.btSave.Name = "btSave";
			this.btSave.Size = new System.Drawing.Size(75, 23);
			this.btSave.TabIndex = 0;
			this.btSave.Text = "Сохранить";
			this.btSave.UseVisualStyleBackColor = true;
			this.btSave.Click += new System.EventHandler(this.btSave_Click);
			// 
			// button2
			// 
			this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.button2.Location = new System.Drawing.Point(352, 25);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 1;
			this.button2.Text = "Отмена";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// edNameKey
			// 
			this.edNameKey.Location = new System.Drawing.Point(132, 17);
			this.edNameKey.Name = "edNameKey";
			this.edNameKey.Size = new System.Drawing.Size(100, 20);
			this.edNameKey.TabIndex = 6;
			// 
			// edNameUI
			// 
			this.edNameUI.Location = new System.Drawing.Point(132, 43);
			this.edNameUI.Name = "edNameUI";
			this.edNameUI.Size = new System.Drawing.Size(100, 20);
			this.edNameUI.TabIndex = 7;
			// 
			// edOrder
			// 
			this.edOrder.Location = new System.Drawing.Point(132, 68);
			this.edOrder.Name = "edOrder";
			this.edOrder.Size = new System.Drawing.Size(61, 20);
			this.edOrder.TabIndex = 8;
			// 
			// AttrEdit
			// 
			this.AcceptButton = this.btSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.button2;
			this.ClientSize = new System.Drawing.Size(439, 253);
			this.ControlBox = false;
			this.Controls.Add(this.panel2);
			this.Controls.Add(this.panel1);
			this.Name = "AttrEdit";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "AttrEdit";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.ComboBox cbAttrTypes;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox cbMandatory;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button btSave;
		private System.Windows.Forms.TextBox edOrder;
		private System.Windows.Forms.TextBox edNameUI;
		private System.Windows.Forms.TextBox edNameKey;
	}
}
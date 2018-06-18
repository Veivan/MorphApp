using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using RicherTextBox;

namespace RicherTextBoxDemo
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            richerTextBox1.HideToolstripItemsByGroup(
                RicherTextBoxToolStripGroups.Alignment |
                RicherTextBoxToolStripGroups.BoldUnderlineItalic,
                false);

            richerTextBox1.HideToolstripItemsByGroup(RicherTextBoxToolStripGroups.Alignment, true);
            richerTextBox1.HideToolstripItemsByGroup(RicherTextBoxToolStripGroups.BoldUnderlineItalic, true);

            richerTextBox1.ToggleBold();
            richerTextBox1.SetFontSize(8.25f);
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MorphApp
{
    public partial class Form1 : Form
    {
        GrenHelper gren = new GrenHelper();
		//string dict = @"D:\Work\Framework\RussianGrammaticalDictionary64\bin-windows64\dictionary.xml";
		string dict = @"D:\Work\Framework\GrammarEngine\src\bin-windows64\dictionary.xml";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            gren.Init(dict);
            toolStripStatusLabel2.Text = gren.GetDictVersion();
        }

        private void btGetMorph_Click(object sender, EventArgs e)
        {
            memoOut.Text = gren.GetMorphInfo(memoInp.Text);
        }

        private void btMakeSynAn_Click(object sender, EventArgs e)
        {
            memoOut.Text = gren.GetSynInfo(memoInp.Text);
        }

    }
}

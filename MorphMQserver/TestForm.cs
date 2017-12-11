using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MorphMQserver
{
	public partial class TestForm : Form
	{
		GrenHelper gren = new GrenHelper();

		public TestForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			gren.Init();
			toolStripStatusLabel2.Text = gren.GetDictVersion();
            memoInp.Text = "Мама мыла красную раму.";
            //memoInp.Text = "Мама мыла раму. Пила злобно лежит на дубовом столе.";
		}

		private void btGetMorph_Click(object sender, EventArgs e)
		{
			memoOut.Text = gren.GetMorphInfo(memoInp.Text);
		}

		private void btMakeSynAn_Click(object sender, EventArgs e)
		{
			memoOut.Text = gren.GetSynInfo(memoInp.Text);
		}

        private void btTokenize_Click(object sender, EventArgs e)
        {
            memoOut.Text = gren.TokenizeIt(memoInp.Text);
        }

		private void btRestore_Click(object sender, EventArgs e)
		{
			memoOut.Text = gren.MakeNRestoreSentence(memoInp.Text);
		}

	}
}

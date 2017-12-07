#define DEBUG_GREN

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
		Courier courier = new Courier();
		GrenHelperTest gren = new GrenHelperTest();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
#if (DEBUG_GREN)
			gren.Init();
			toolStripStatusLabel2.Text = gren.GetDictVersion();
            memoInp.Text = "Мама мыла раму.";
            //memoInp.Text = "Мама мыла раму. Пила злобно лежит на дубовом столе.";
#endif
		}

		private void btGetMorph_Click(object sender, EventArgs e)
		{
#if (DEBUG_GREN)
			memoOut.Text = gren.GetMorphInfo(memoInp.Text);
#else 
            courier.servType = TMorph.Schema.ServType.svMorph;
            courier.command = TMorph.Schema.ComType.Morph;
            memoOut.Text = courier.sendit(memoInp.Text);
#endif
		}

		private void btMakeSynAn_Click(object sender, EventArgs e)
		{
#if (DEBUG_GREN)
			memoOut.Text = gren.GetSynInfo(memoInp.Text);
#else 
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			memoOut.Text = courier.sendit(memoInp.Text);
#endif
		}

		private void btDBGetWord_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.GetWord;
			memoOut.Text = courier.sendit(memoInp.Text);
		}

		private void btSaveLex_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.SaveLex;
			memoOut.Text = courier.sendit(memoInp.Text);
		}

        private void btTokenize_Click(object sender, EventArgs e)
        {
#if (DEBUG_GREN)
            memoOut.Text = gren.TokenizeIt(memoInp.Text);
/*#else 
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			memoOut.Text = courier.sendit(memoInp.Text); */
#endif

        }

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;

namespace MorphApp
{
	public partial class Form1 : Form
	{
		Courier courier = new Courier();
        Paragraph para = new Paragraph();

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
        }

		private void btGetMorph_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Morph;
			memoOut.Text = courier.sendit(memoInp.Text);
		}

		private void btMakeSynAn_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			memoOut.Text = courier.sendit(memoInp.Text);
		}

		private void btDBGetWord_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.GetWord;
			memoOut.Text = courier.sendit(memoInp.Text);
		}

		private void btSaveLex_Click(object sender, EventArgs e)
		{
			/*courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.SaveLex;
			memoOut.Text = courier.sendit(memoInp.Text); */
            var plist = para.GetParagraph();
            SentProps ms = plist[0];
            ms.sentence  += "qq";
            plist[0] = ms;

        }

		private void btTokenize_Click(object sender, EventArgs e)
		{
			/*courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			memoOut.Text = courier.sendit(memoInp.Text);*/

            var slist = new ArrayList();
            slist.Add(memoInp.Text);
            slist.Add("Предл один.");
            para.AddParagraph(slist);
 
		}

		private void btRestore_Click(object sender, EventArgs e)
		{
            var plist = para.GetParagraph();
            var sb = new StringBuilder();
            foreach (var sent in plist)
            {
                sb.Append(sent.sentence);
            }
            memoOut.Text = sb.ToString();

                
			/*courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			memoOut.Text = courier.sendit(memoInp.Text);*/
		}

	}
}

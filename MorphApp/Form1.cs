using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Collections;
using Schemas;

namespace MorphApp
{
	public partial class Form1 : Form
	{
		Courier courier = new Courier();
		Paragraph para = new Paragraph();

		SentenceMap sent;

		public Form1()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			memoInp.Text = "Мама мыла";
		}

		private void btSavePara_Click(object sender, EventArgs e)
		{
			//UpdateParagraph();
			SaveParagraphBD();
		}

		private void btReadPara_Click(object sender, EventArgs e)
		{
			ReadParagraphBD();
		}

		/// <summary>
		/// Формирование содержимого внутреннего объекта Paragraph.
		/// </summary>
		private void UpdateParagraph()
		{
			// Разделение текста на предложения
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Separ;
			courier.SendText(memoInp.Text);
			var sents = courier.GetSeparatedSentsList();
			para.RefreshParagraph(new ArrayList(sents));

			// Выполнение синтана для неактуальных предложений.
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			var sentlist = para.GetParagraph(SentTypes.enstNotActual);
			foreach (var sent in sentlist)
			{
				courier.SendText(sent.sentence);
				var sentlistRep = courier.GetSentenceStructList();
				if (sentlistRep.Count > 0)
					para.UpdateSentStruct(sent.order, sentlistRep[0]);
			}
		}

		/// <summary>
		/// Сохранение внутреннего объекта Paragraph в БД.
		/// </summary>
		private void SaveParagraphBD()
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.SavePara;
			courier.SendParagraph(this.para);
			var paramlist = courier.GetParamsList();
			if (paramlist == null)
			{
				return;
			}

			foreach (var par in paramlist)
				if (par.Name == "ParagraphID")
				{
					this.para.ParagraphID = Convert.ToInt32(par.Value, 10);
					break;
				}
			memoOut.Text = this.para.ParagraphID.ToString();
		}

		/// <summary>
		/// Формирование внутреннего объекта Paragraph из БД.
		/// </summary>
		private void ReadParagraphBD()
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.ReadPara;
			courier.SendParagraph(this.para);
			// Через параметры передать -1 в случае ошибки, либо ID параграфа
			var paramlist = courier.GetParamsList();
			var sentlist = courier.GetSentenceStructList();
			para.RefreshParagraph(new ArrayList(sentlist));

			//TODO прочитался параграф из БД - надо его ресторить и выдать на просмотр

			var sb = new StringBuilder();
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Repar;
			foreach (var sent in sentlist)
			{
				courier.SendStruct(sent);
				var sents = courier.GetSeparatedSentsList();
				foreach (var sentrep in sents)
					sb.Append(sent + "\r\n");
			}
			memoOut.Text = sb.ToString();
		}

		private void btTokenize_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Separ;
			courier.SendText(memoInp.Text);
			var sents = courier.GetSeparatedSentsList();
			var sb = new StringBuilder();
			foreach (var sent in sents)
				sb.Append(sent + "\r\n");
			memoOut.Text = sb.ToString();
		}

		private void btMakeSynAn_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			courier.SendText(memoInp.Text);
			var sentlistRep = courier.GetSentenceStructList();
			if (sentlistRep.Count > 0)
				sent = sentlistRep[0];
			else
				return;

			this.para.AddSentStruct(5, sent);

			var sb = new StringBuilder();
			for (int i = 0; i < sent.Capasity; i++)
			{
				var word = sent.GetWordByOrder(i);
				sb.Append(word.EntryName + "\r\n");
			}

			var ordlist = sent.GetTreeList();
			foreach (var x in ordlist)
			{
				sb.Append(new String('\t', x.Level) +
					String.Format("{0} Level {1} link {2} \r\n",
						sent.GetWordByOrder(x.index).EntryName, x.Level, x.linktype));
			}

			memoOut.Text = sb.ToString();
		}

		private void btRestore_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			courier.SendText(memoInp.Text);
			var sentstr = courier.GetSentenceStructList();
			if (sentstr == null) return;
			var sentlistRep = courier.GetSentenceStructList();
			if (sentlistRep.Count == 0)
				return;

			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Repar;
			courier.SendStruct(sentlistRep[0]);
			var sents = courier.GetSeparatedSentsList();
			var sb = new StringBuilder();
			foreach (var sent in sents)
				sb.Append(sent + "\r\n");
			memoOut.Text = sb.ToString();
		}

		private void btDBGetWord_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.GetWord;
			courier.SendText(memoInp.Text);
			var paramlist = courier.GetParamsList();
			var sb = new StringBuilder();
			if (paramlist == null)
				sb.Append("Empty params \r\n");
			else
				foreach (var par in paramlist)
					sb.Append(par.Name + "\t" + par.Value + "\r\n");
			memoOut.Text = sb.ToString();
		}

		private void btSaveLex_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.SaveLex;
			courier.SendText(memoInp.Text);
			var paramlist = courier.GetParamsList();
			var sb = new StringBuilder();
			if (paramlist == null)
				sb.Append("Empty params \r\n");
			else
				foreach (var par in paramlist)
					sb.Append(par.Name + "\t" + par.Value + "\r\n");
			memoOut.Text = sb.ToString();
		}

		private void btGetMorph_Click(object sender, EventArgs e)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Morph;
			courier.SendText(memoInp.Text);
			memoOut.Text = "";
		}

	}
}

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
            MyFontList fonts = new MyFontList();
            for (int i = 0; i < FontFamily.Families.Length; i++)
            {
                if (FontFamily.Families[i].IsStyleAvailable(FontStyle.Regular))
                    fonts.Add(new Font(FontFamily.Families[i], 11.0F, FontStyle.Regular));
            }

           MyNodesList nodes = new MyNodesList();
            for (int i = 0; i < 3; i++)
            {
                nodes.Add(new mmNode(i.ToString(), i * 2));
            }

            binding1.DataSource = nodes;
            listBox1.DataSource = binding1;
            listBox1.DisplayMember = "name";

            dataGridView1.DataSource = binding1;
            dataGridView1.AutoGenerateColumns = true;
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
			this.para.ParagraphID = 7;
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
					sb.Append(sentrep + "\r\n");

                // Отображение синт связей
                var ordlist = sent.GetTreeList();
                foreach (var x in ordlist)
                {
                    sb.Append(new String('\t', x.Level) +
                        String.Format("{0} Level {1} link {2} \r\n",
                            sent.GetWordByOrder(x.index).EntryName, x.Level, x.linktype));
                }

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

            // Отображение синт связей
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

        private void btSelect_Click(object sender, EventArgs e)
        {


         /*   courier.servType = TMorph.Schema.ServType.svSUBD;
            courier.command = TMorph.Schema.ComType.GetParags;
            courier.SendText(""); */
            
            /*    DataTable dTable = new DataTable();
     String sqlQuery;

     if (m_dbConn.State != ConnectionState.Open)
     {
         MessageBox.Show("Open connection with database");
         return;
     }
    
     try
     {
         sqlQuery = "SELECT * FROM Catalog";
         SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
         adapter.Fill(dTable);

         if (dTable.Rows.Count > 0)
         {
             dgvViewer.Rows.Clear();

             for (int i = 0; i < dTable.Rows.Count; i++)
                 dgvViewer.Rows.Add(dTable.Rows[i].ItemArray);
         }
         else
             MessageBox.Show("Database is empty");
     }
     catch (SQLiteException ex)
     {               
         MessageBox.Show("Error: " + ex.Message);
     }           */
        }

        public class mmNode
        {
            public string name { get; set; }
            public int Level;
            private string p;
            private int p_2;

            public mmNode(string p, int p_2)
            {
                this.name = p;
                this.Level = p_2;
            }
        }
        public class MyNodesList : BindingList<mmNode>
        {
            protected override bool SupportsSearchingCore
            {
                get { return true; }
            }
            protected override int FindCore(PropertyDescriptor prop, object key)
            {
                /*/ Ignore the prop value and search by family name.
                for (int i = 0; i < Count; ++i)
                {
                    if (Items[i].FontFamily.Name.ToLower() == ((string)key).ToLower())
                        return i;

                } */
                return -1;
            }
        }

        public class MyFontList : BindingList<Font>
        {

            protected override bool SupportsSearchingCore
            {
                get { return true; }
            }
            protected override int FindCore(PropertyDescriptor prop, object key)
            {
                // Ignore the prop value and search by family name.
                for (int i = 0; i < Count; ++i)
                {
                    if (Items[i].FontFamily.Name.ToLower() == ((string)key).ToLower())
                        return i;

                }
                return -1;
            }


        }
	}
}

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
using DirectDBconnector;

namespace MorphApp
{
	public partial class ClientMain : Form
	{
		/*/ Работа с БД через сервер сообщений
		SagaStoreServer dbServer = new SagaStoreServer();
		CLInnerStore store = new CLInnerStore(); */

		// Работа с БД напрямую
		SagaDBServer dbServer = new SagaDBServer();
		CLInnerStoreDB store = new CLInnerStoreDB();

		Courier courier = new Courier();
		ParagraphMap para = new ParagraphMap();

		SentenceMap sent;

		public ClientMain()
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
			this.para.ParagraphID = 7;
			var sentlist = dbServer.ReadParagraphDB(this.para.ParagraphID);
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

        }

		private void btRefresh_Click(object sender, EventArgs e)
		{
			RefreshInnerStore();
		}

		/// <summary>
		/// Заполнение внутреннего хранилища.
		/// </summary>
		private void RefreshInnerStore()
		{
			var list = dbServer.GetChildrenContainers(Session.MainStroreID, tpList.tplDBtable);
			store.FillContainers(list);

			var dTable = list.dtable;
			var list_ids = new List<string>();
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var strID = dTable.Rows[i].Field<long>("ct_id");
				list_ids.Add(strID.ToString());
			}
			list = dbServer.GetDocsInContainerList(list_ids);
			store.FillDocs(list);
			list_ids.Clear();
			dTable = list.dtable;
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var strID = dTable.Rows[i].Field<long>("doc_id");
				list_ids.Add(strID.ToString());
			}
			list = dbServer.ReadParagraphsInDocsList(tpList.tplDBtable, list_ids);
			store.FillDocsParagraphs(list);

			PopulateTreeView();

			/*var list = dbServer.GetChildrenContainers(Session.MainStroreID);
			store.FillContainers(list);

			var dTable = list.list;
			var list_ids = new List<string>();
			for (int i = 0; i < dTable.Count; i++)
			{
				var strID = (dTable[i] as ContainerMap).ContainerID;
				list_ids.Add(strID.ToString());
			}
			list = dbServer.GetDocsInContainerList(list_ids);
			store.FillDocs(list); */
			PopulateTreeView();
		}

		private void PopulateTreeView()
		{
			TreeNode rootNode; 
			treeView1.Nodes.Clear();

			rootNode = new TreeNode("Хранилище");
            rootNode.Tag = clNodeType.clnContainer;
            foreach (var cont in store.containers)
			{
				var aNode = new TreeNode(cont.Name, 0, 0);
				aNode.Name = cont.ContainerID.ToString();
				aNode.Tag = clNodeType.clnContainer;
				PopulateTreeDocuments(cont, aNode);
				rootNode.Nodes.Add(aNode);
			}
			treeView1.Nodes.Add(rootNode);
		}

		private void PopulateTreeDocuments(ContainerNode container, TreeNode nodeToAddTo)
		{
			var docs = container.GetDocuments();
			foreach (var doc in docs)
			{
				var aNode = new TreeNode(doc.Name, 0, 0);
				aNode.Name = doc.DocumentID.ToString();
				aNode.Tag = clNodeType.clnDocument;
				PopulateTreeParags(doc, aNode);
				nodeToAddTo.Nodes.Add(aNode);
			}
		}

		private void PopulateTreeParags(DocumentMap dMap, TreeNode nodeToAddTo)
		{
			var parags = dMap.GetParagraphs();
			foreach (var paragraph in parags)
			{
				var aNode = new TreeNode();
				aNode.Name = paragraph.ParagraphID.ToString();
				aNode.Tag = clNodeType.clnParagraph;
				nodeToAddTo.Nodes.Add(aNode);
			}
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			//TODO Если Загружать документы из БД, если не загружены
			
			TreeNode aNode = e.Node;
 			switch ((clNodeType)aNode.Tag)
			{
				case clNodeType.clnDocument:
					{
						// Найти ID абзаца
						this.para.ParagraphID = 1;
						// Чтение данных о структурах предложений и заголовка абзаца из БД
						var sentlist = dbServer.ReadParagraphDB(this.para.ParagraphID);
						para.RefreshParagraph(new ArrayList(sentlist));
						// Восстановить заголовок по данным структуры
						//aNode.Name = dbServer.ReadParagraphsInDocsList(tpList.tplDBtable, list_ids);
						break;
					}
			}

		}

	}
}

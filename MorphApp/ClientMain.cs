﻿using System;
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
            memoInp.Text = "Мама мыла раму";
			//memoInp.Text = "Абзац 1";
        }

        private void btSavePara_Click(object sender, EventArgs e)
        {
            UpdateParagraph();
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
			var sents = store.MorphGetSeparatedSentsList(memoInp.Text);
            para.RefreshParagraph(new ArrayList(sents));

            // Выполнение синтана для неактуальных предложений.
			var sentlist = para.GetParagraph(SentTypes.enstNotActual);
			foreach (var sent in sentlist)
			{
				var sentlistRep = store.MorphMakeSyntan(sent.sentence);
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
            var ParagraphID = 1;
            var sentlist = dbServer.ReadParagraphDB(ParagraphID);

            // прочитался параграф из БД - надо его ресторить и выдать на просмотр

            var sb = new StringBuilder();
			var reparedsents = store.MorphGetReparedSentsList(sentlist);
			foreach (var sentrep in reparedsents)
				sb.Append(sentrep + "\r\n");

			// Отображение синт связей
			foreach (var sent in sentlist)
			{
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
			var sents = store.MorphGetSeparatedSentsList(memoInp.Text);
            var sb = new StringBuilder();
            foreach (var sent in sents)
                sb.Append(sent + "\r\n");
            memoOut.Text = sb.ToString();
        }

        private void btMakeSynAn_Click(object sender, EventArgs e)
        {
			var sentlistRep = store.MorphMakeSyntan(memoInp.Text);
			if (sentlistRep == null || sentlistRep.Count == 0)
				return;
			
			sent = sentlistRep[0];
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
			var sentlistRep = store.MorphMakeSyntan(memoInp.Text);
            if (sentlistRep == null || sentlistRep.Count == 0)
                return;

			var reparedsents = store.MorphGetReparedSentsList(sentlistRep);
			
            var sb = new StringBuilder();
			foreach (var sent in reparedsents)
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
			// Морф.анализ не нужен - он выполняется в синтане
        }


        private void btRefresh_Click(object sender, EventArgs e)
        {
            store.Refresh();
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
            TreeNode aNode = e.Node;
            switch ((clNodeType)aNode.Tag)
            {
                case clNodeType.clnDocument:
                    {
                        // Обновление заголовков абзацев в дереве
                        var contID = Convert.ToInt64(aNode.Parent.Name);
                        var docID = Convert.ToInt64(aNode.Name);
                        var dMap = store.RefreshParagraphs(contID, docID);

                        var parags = dMap.GetParagraphs();
						foreach (TreeNode node in aNode.Nodes)
                        {
                            var paragraph = parags.Where(x => x.ParagraphID == Convert.ToInt64(node.Name)).FirstOrDefault();
                            node.Text = paragraph.GetSentenseByOrder(-1);
                        }
                        break;
                    }
            }

        }

    }
}

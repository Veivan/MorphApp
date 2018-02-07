using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Schemas;

namespace MorphApp
{
    public partial class ClientMain : Form
    {
        // Работа с БД через сервер сообщений
        //CLInnerStore store = new CLInnerStore(); 

        // Работа с БД напрямую
        CLInnerStoreDB store = new CLInnerStoreDB();

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

        private void btRefresh_Click(object sender, EventArgs e)
        {
            store.Refresh();
            PopulateTreeView();
        }

        private void PopulateTreeView()
        {
            treeView1.Nodes.Clear();
            foreach (var cont in store.containers)
            {
                var aNode = new MorphNode(cont.Name);
                aNode.NodeType = clNodeType.clnContainer;
                aNode.bdID = cont.ContainerID;
                PopulateTreeChildrenConts(cont, aNode);
                PopulateTreeDocuments(cont, aNode);
                treeView1.Nodes.Add(aNode);
            }
        }

        private void PopulateTreeChildrenConts(ContainerNode container, TreeNode nodeToAddTo)
        {
            var chldrn = container.Children();
            foreach (var chld in chldrn)
            {
                if (FindNode(chld.ContainerID, nodeToAddTo) != null)
                    continue;

                var aNode = new MorphNode(chld.Name);
                aNode.NodeType = clNodeType.clnContainer;
                aNode.bdID = chld.ContainerID;
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private TreeNode FindNode(long bdID, TreeNode aNode)
        {
            foreach (MorphNode node in aNode.Nodes)
            {
                if (node.bdID == bdID)
                    return node;
            }
            return null;
        }

        private void PopulateTreeDocuments(ContainerNode container, TreeNode nodeToAddTo)
        {
            var docs = container.GetDocuments();
            foreach (var doc in docs)
            {
                if (FindNode(doc.DocumentID, nodeToAddTo) != null)
                    continue;
                var aNode = new MorphNode(doc.Name);
                aNode.NodeType = clNodeType.clnDocument;
                aNode.bdID = doc.DocumentID;
                PopulateTreeParags(doc, aNode);
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private void PopulateTreeParags(DocumentMap dMap, TreeNode nodeToAddTo)
        {
            var parags = dMap.GetParagraphs();
            foreach (var paragraph in parags)
            {
                var aNode = new MorphNode("");
                aNode.NodeType = clNodeType.clnParagraph;
                aNode.bdID = paragraph.ParagraphID;
                nodeToAddTo.Nodes.Add(aNode);
            }
        }

        private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var aNode = (MorphNode)e.Node;
            switch (aNode.NodeType)
            {
                case clNodeType.clnContainer:
                    {
                        // Обновление детей и документов в дереве
                        store.RefreshContainer(aNode.bdID);
                        var container = store.GetContainerByID(aNode.bdID);
                        PopulateTreeChildrenConts(container, aNode);
                        PopulateTreeDocuments(container, aNode);
                        var chldrn = container.Children();
                        foreach (var chldCont in chldrn)
                        {
                            var chNode = FindNode(chldCont.ContainerID, aNode);
                            //if (chNode == null)
                            PopulateTreeChildrenConts(chldCont, chNode);
                            PopulateTreeDocuments(chldCont, chNode);
                        }
                        break;
                    }
                case clNodeType.clnDocument:
                    {
                        // Обновление заголовков абзацев в дереве
                        var contID = (aNode.Parent as MorphNode).bdID;
                        var docID = aNode.bdID;
                        var dMap = store.RefreshParagraphs(contID, docID);

                        var parags = dMap.GetParagraphs();
                        foreach (MorphNode node in aNode.Nodes)
                        {
                            var paragraph = parags.Where(x => x.ParagraphID == node.bdID).FirstOrDefault();
                            node.Text = paragraph.GetHeader();
                        }
                        break;
                    }
            }

        }

        private void treeView1_DoubleClick(object sender, EventArgs e)
        {
            var tree = sender as TreeView;
            EditNode(tree.SelectedNode as MorphNode);
        }

        private void btAddPara_Click(object sender, EventArgs e)
        {
            var aNode = treeView1.SelectedNode as MorphNode;
            if (aNode == null) return;

            MorphNode docNode = null;
            switch (aNode.NodeType)
            {
                case clNodeType.clnDocument:
                    docNode = aNode;
                    break;
                case clNodeType.clnParagraph:
                    docNode = aNode.Parent as MorphNode;
                    break;
                default:
                    docNode = null;
                    break;
            }
            if (docNode == null) return;

            var docID = docNode.bdID;
            var contID = (docNode.Parent as MorphNode).bdID;
            long parID = -1;
            var fParaEdit = new FormParaEdit(store);
            fParaEdit.SetContext(contID, docID, parID, docNode, treeOper.toAdd);
            fParaEdit.Show();
        }

        private void btEdit_Click(object sender, EventArgs e)
        {
            EditNode(treeView1.SelectedNode as MorphNode);
        }

        private void btParaDel_Click(object sender, EventArgs e)
        {
            var aNode = treeView1.SelectedNode as MorphNode;
            if (aNode == null) return;
            aNode.Delete(store);
        }

        private void btAddCont_Click(object sender, EventArgs e)
        {
            var aNode = treeView1.SelectedNode as MorphNode;
            if (aNode == null) return;
            if (aNode.NodeType != clNodeType.clnContainer)
                return;
            string result = Microsoft.VisualBasic.Interaction.InputBox("Введите имя контейнера:");
            if (String.IsNullOrEmpty(result))
                return;
            var ParentContID = aNode.bdID;
            var id = store.SaveContainerBD(result, ParentContID);
        }

        private void btAddDoc_Click(object sender, EventArgs e)
        {
            var aNode = treeView1.SelectedNode as MorphNode;
            if (aNode == null) return;
            if (aNode.NodeType != clNodeType.clnContainer || aNode.bdID == Session.MainStroreID)
                return;
            string result = Microsoft.VisualBasic.Interaction.InputBox("Введите имя документа:");
            if (String.IsNullOrEmpty(result))
                return;
            var ContID = aNode.bdID;
            var id = store.SaveDocumentBD(result, ContID);
        }

        private void EditNode(MorphNode aNode)
        {
            if (aNode == null) return;

            switch (aNode.NodeType)
            {
                case clNodeType.clnParagraph:
                    {
                        var docNode = aNode.Parent as MorphNode;
                        var docID = docNode.bdID;
                        var contID = (docNode.Parent as MorphNode).bdID;
                        var parID = aNode.bdID;
                        var fParaEdit = new FormParaEdit(store);
                        fParaEdit.SetContext(contID, docID, parID, aNode, treeOper.toEdit);
                        fParaEdit.Show();
                        break;
                    }
            }
        }

        #region Тестовые методы

        private void btSavePara_Click(object sender, EventArgs e)
        {
            store.UpdateParagraph(this.para, memoInp.Text, false);

            var paramlist = store.SaveParagraphBD(this.para);
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

        private void btReadPara_Click(object sender, EventArgs e)
        {
            var ParagraphID = 1;
            var sentlist = store.ReadParagraphDB(ParagraphID);

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
                sb.Append(word.EntryName + " " + word.RealWord + "\r\n");
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
            var paramlist = store.GetLexema(memoInp.Text);
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
            var paramlist = store.SaveLexema(memoInp.Text);
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
        #endregion

    }
}

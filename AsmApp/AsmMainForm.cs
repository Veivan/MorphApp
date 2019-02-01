using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Schemas;
using Schemas.BlockPlatform;
using TMorph.Common;
using AsmApp.Types;

namespace AsmApp
{
	public partial class AsmMainForm : Form
	{

		ClientStoreMediator store =  ClientStoreMediator.Instance();

		public AsmMainForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			this.btRefresh_Click(null, null);
		}

		private void btRefresh_Click(object sender, EventArgs e)
		{
			store.Refresh();
			PopulateTreeView();
		}

		/// <summary>
		/// Заполнение узлами первого уровня дерева
		/// </summary>
		private void PopulateTreeView()
		{
			treeView1.Nodes.Clear();
			foreach (var cont in store.containers)
			{
				var aNode = new AsmNode(cont);
				PopulateChildrenTree(cont, aNode);
				treeView1.Nodes.Add(aNode);
			}
		}
		
		/// <summary>
		/// Заполнение узла дочерними узлами
		/// </summary>
		private void PopulateChildrenTree(AssemblyBase asm, AsmNode nodeToAddTo)
		{
			foreach (var chld in asm.Children)
			{
				if (FindNode(chld.BlockID, nodeToAddTo) != null)
					continue;
				var aNode = new AsmNode(chld, chld.Name);
				nodeToAddTo.Nodes.Add(aNode);
			}
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			var aNode = (AsmNode)e.Node;

			foreach (AsmNode chldNode in aNode.Nodes)
			{
				store.RefreshAsm(chldNode);
				var newNode = (AsmNode)FindNode(chldNode.Assembly.BlockID, chldNode);
				if (newNode == null)
					PopulateChildrenTree(chldNode.Assembly, chldNode);
			}
		}

		private TreeNode FindNode(long bdID, TreeNode aNode)
		{
			foreach (AsmNode node in aNode.Nodes)
			{
				if (node.Assembly.BlockID == bdID)
					return node;
			}
			return null;
		}

		private void btAddCont_Click(object sender, EventArgs e)
		{
			var nodeToAddTo = treeView1.SelectedNode as AsmNode;
			if (nodeToAddTo == null) return;
			if (!nodeToAddTo.Assembly.IsDataContainer)
				return;

			var result = Utils.InputBox("Создание контейнера", "Введите имя контейнера:", Session.DefaulContainerName);
			if (String.IsNullOrEmpty(result))
				return;
			var ParentContID = nodeToAddTo.Assembly.BlockID;
			var asm = store.CreateContainer(result, ParentContID);
			var aNode = new AsmNode(asm);
			nodeToAddTo.Nodes.Add(aNode);
		}

		private void btAddDoc_Click(object sender, EventArgs e)
		{
			var nodeToAddTo = treeView1.SelectedNode as AsmNode;
			if (nodeToAddTo == null) return;

			var result = Utils.InputBox("Создание документа", "Введите имя документа:", Session.DefaulDocumentName);
			if (String.IsNullOrEmpty(result))
				return;
			var ParentNodeID = nodeToAddTo.Assembly.BlockID;
			var asm = store.CreateDocument(result, ParentNodeID);
			var aNode = new AsmNode(asm);
			nodeToAddTo.Nodes.Add(aNode);
		}

		private void btAddPara_Click(object sender, EventArgs e)
		{
			var nodeToAddTo = treeView1.SelectedNode as AsmNode;
			if (nodeToAddTo == null || nodeToAddTo.NodeType == clNodeType.clnContainer ) return;

			var fParaEdit = new FormParaEdit(nodeToAddTo, treeOper.toAdd);
			fParaEdit.Show();
		}

		private void btEdit_Click(object sender, EventArgs e)
		{
			EditNode(treeView1.SelectedNode as AsmNode);
		}

		private void EditNode(AsmNode aNode)
		{
			if (aNode == null) return;

			switch (aNode.NodeType)
			{
				case clNodeType.clnParagraph:
					{
						var fParaEdit = new FormParaEdit(aNode, treeOper.toEdit);
						fParaEdit.Show();
						break;
					}
			}
		}


		private void btDelNode_Click(object sender, EventArgs e)
		{
			var aNode = treeView1.SelectedNode as AsmNode;
			if (aNode == null) return;
			aNode.Delete();
		}

		#region Отладка
		private void btMakeSynAn_Click(object sender, EventArgs e)
		{
			var sentlistRep = store.MorphMakeSyntan(memoInp.Text);
			if (sentlistRep == null || sentlistRep.Count == 0)
				return;

			var sent = Map2Asm.Convert(sentlistRep[0]);

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

		private void btSavePara_Click(object sender, EventArgs e)
		{
			var para = new ParagraphAsm();
			store.UpdateParagraph(para, memoInp.Text, false);

			/*			var paramlist = store.SaveParagraphBD(para);
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
						memoOut.Text = this.para.ParagraphID.ToString();*/
		}
		
		#endregion

	}
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Schemas.BlockPlatform;
using DirectDBconnector;
using TMorph.Common;
using AsmApp.Dialogs;

namespace AsmApp
{
	public partial class AsmMainForm : Form
	{

		// Работа с БД напрямую
		BlockDBServer lowStore = new BlockDBServer();

		ClientStoreMediator store = new ClientStoreMediator();

		public AsmMainForm()
		{
			InitializeComponent();
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			listBoxBlockTypes.DisplayMember = "NameUI";
			listBoxBlockTypes.DataSource = GetBlockTypes();
		}

		private IEnumerable<BlockType> GetBlockTypes()
		{
			return lowStore.GetAllBlockTypes();
			/*return new List<BlockType>()
			{
				new BlockType(1, "t1", ""),
				new BlockType(2, "t2", "")
			};*/
		}

		private IEnumerable<BlockAttribute> GetBlockAttrs(long btId = 0)
		{
			if (btId <= 0)
				return null;

			var collect = lowStore.GetAttrsCollection(btId);
			/*
			var collect = new AttrsCollection();
			collect.AddElement(new BlockAttribute(1, "qq" + btId, 1, new BlockType(btId, "qqb", "")));
			*/
			return collect.Attrs;
		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			listBoxBlockTypes.DataSource = GetBlockTypes();
		}

		private void listBoxBlockTypes_SelectedIndexChanged(object sender, EventArgs e)
		{
			var btId = ((BlockType)listBoxBlockTypes.SelectedItem).BlockTypeID;
			listBoxAttrs.DataSource = GetBlockAttrs(btId);
			listBoxAttrs.DisplayMember = "NameUI";
		}

		private void RenameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (listBoxBlockTypes.SelectedIndex < 0) return;

			var newVal = Utils.InputBox("Переименование типа блока", "Введите новое наименование типа блока", "");
			if (newVal == "")
				return;

			var SelectedItem = (BlockType)listBoxBlockTypes.SelectedItem;
			var newBT = new BlockType(SelectedItem.BlockTypeID, SelectedItem.NameKey, newVal);
			lowStore.BlockTypeChangeStrings(newBT);
			var position = listBoxBlockTypes.SelectedIndex;
			listBoxBlockTypes.DataSource = GetBlockTypes();
			listBoxBlockTypes.SelectedIndex = position;
		}

		private void toolStripMenuAttrsAdd_Click(object sender, EventArgs e)
		{
			var SelectedBlockType = (BlockType)listBoxBlockTypes.SelectedItem;
			var attrEditDialog = new AttrEdit();
			attrEditDialog.InitData(SelectedBlockType, null);
			attrEditDialog.ShowDialog();
			if (attrEditDialog.DialogResult == DialogResult.OK)
			{
				//MessageBox.Show("Ok");
				listBoxAttrs.DataSource = GetBlockAttrs(SelectedBlockType.BlockTypeID);
			}
		}

		private void toolStripMenuAttrsEdit_Click(object sender, EventArgs e)
		{
			var SelectedItem = (BlockAttribute)listBoxAttrs.SelectedItem;
			if (SelectedItem == null) return;

			var SelectedBlockType = (BlockType)listBoxBlockTypes.SelectedItem;
			var attrEditDialog = new AttrEdit();
			attrEditDialog.InitData(SelectedBlockType, SelectedItem);
			attrEditDialog.ShowDialog();
			if (attrEditDialog.DialogResult == DialogResult.OK)
			{
				listBoxAttrs.DataSource = GetBlockAttrs(SelectedBlockType.BlockTypeID);
			}
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
				PopulateTreeChildrenConts(cont, aNode);
				treeView1.Nodes.Add(aNode);
			}
		}

		/// <summary>
		/// Заполнение узла дочерними узлами
		/// </summary>
		private void PopulateTreeChildrenConts(AssemblyBase container, AsmNode nodeToAddTo)
		{
			foreach (var chld in container.Children)
			{
				if (FindNode(chld.BlockID, nodeToAddTo) != null)
					continue;
				var aNode = new AsmNode(chld, chld.Name);
				nodeToAddTo.Nodes.Add(aNode);
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
			string result = Microsoft.VisualBasic.Interaction.InputBox("Введите имя контейнера:");
			if (String.IsNullOrEmpty(result))
				return;
			var ParentContID = nodeToAddTo.Assembly.BlockID;
			var asm = store.CreateContainer(result, ParentContID);
			var aNode = new AsmNode(asm);
			nodeToAddTo.Nodes.Add(aNode);
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			var aNode = (AsmNode)e.Node;

			foreach (AsmNode chldNode in aNode.Nodes)
			{
				store.RefreshContainer(chldNode.Assembly);
				var newNode = (AsmNode)FindNode(chldNode.Assembly.BlockID, chldNode);
				if (newNode == null)
					PopulateTreeChildrenConts(chldNode.Assembly, chldNode);
			}
		}

		private void btDelNode_Click(object sender, EventArgs e)
		{
			var aNode = treeView1.SelectedNode as AsmNode;
			if (aNode == null) return;
			aNode.Delete();
		}
	}
}

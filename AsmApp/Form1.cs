using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//using LogicProcessor;
using Schemas.BlockPlatform;
using DirectDBconnector;
using TMorph.Common;
using AsmApp.Dialogs;

namespace AsmApp
{
	public partial class Form1 : Form
	{

		// Работа с БД напрямую
		BlockDBServer lowStore = new BlockDBServer();
		//AssemblyServer store = new AssemblyServer();

		public Form1()
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

	}
}

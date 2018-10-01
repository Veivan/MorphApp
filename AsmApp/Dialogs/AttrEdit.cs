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

namespace AsmApp.Dialogs
{
	public partial class AttrEdit : Form
	{
		// Работа с БД напрямую
		BlockDBServer lowStore = new BlockDBServer();

		private BlockType blockType;
		private BlockAttribute attr;

		public AttrEdit()
		{
			InitializeComponent();
		}

		public void InitData(BlockType _blockType, BlockAttribute _attr)
		{
			blockType = _blockType;
			attr = _attr;
			if (attr != null)
			{
				edNameKey.Text = attr.NameKey;
				edNameUI.Text = attr.NameUI;
				edOrder.Text = attr.Order.ToString();
				cbMandatory.Checked = attr.Mandatory;
				cbAttrTypes.SelectedItem = attr.AttrType;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			cbAttrTypes.DisplayMember = "Name";
			cbAttrTypes.DataSource = GetAttrTypes();
		}

		private IEnumerable<AttrType> GetAttrTypes()
		{
			return lowStore.GetAllAttrTypes();
		}

		private void btSave_Click(object sender, EventArgs e)
		{
			var order = Convert.ToInt32(edOrder.Text);
			var attrType = (AttrType)cbAttrTypes.SelectedItem;
			if (this.attr == null)
				lowStore.CreateAttribute(edNameKey.Text, edNameUI.Text, attrType.AttrTypeID,
					blockType.BlockTypeID, order, cbMandatory.Checked);
			else
			{
				attr.NameKey = edNameKey.Text;
				attr.NameUI = edNameUI.Text;
				attr.AttrType = attrType.Type;
				attr.BlockType = blockType;
				attr.Order = order;
				attr.Mandatory = cbMandatory.Checked;
				lowStore.AttributeUpdate(attr);
			}
		}
	}
}

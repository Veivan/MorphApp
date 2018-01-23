using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Schemas;

namespace MorphApp
{
	public partial class FormParaEdit : Form
	{
		// Работа с БД напрямую
		private CLInnerStoreDB store;

		private treeOper typeOper;
		private ParagraphMap paraMap;
		private long contID;
		private long docID;
		private long parID;
		private MorphNode theNode;

		public FormParaEdit(CLInnerStoreDB store)
		{
			this.store = store; 
			InitializeComponent();
		}

		public void SetContext(long contID, long docID, long parID, MorphNode theNode, treeOper tOper)
		{
			this.typeOper = tOper;
			this.contID = contID;
			this.docID = docID;
			this.parID = parID;
			this.theNode = theNode;
			ParagraphMap pMap = null;

			switch (tOper)
			{ 
				case(treeOper.toEdit) :
					pMap = store.GetParagraph(contID, docID, parID);
					break;
				case(treeOper.toAdd) : 
					pMap = new ParagraphMap(-1, docID);
					break;
			}
			this.paraMap = new ParagraphMap(pMap);
		}

		private void FormParaEdit_Load(object sender, EventArgs e)
		{
			memoHeader.Text = paraMap.GetHeader();
			memoBody.Text = paraMap.GetBody();
		}

		private void btParaSave_Click(object sender, EventArgs e)
		{
			try
			{
				store.UpdateParagraph(this.paraMap, memoHeader.Text, true);
				store.UpdateParagraph(this.paraMap, memoBody.Text, false);
				var paramlist = store.SaveParagraphBD(this.paraMap);

				if (this.typeOper == treeOper.toEdit)
				{
					var pMap = store.GetParagraph(contID, docID, parID);
					if (pMap != null)
						pMap = this.paraMap;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.Source);
			}
		}

		private void btClose_Click(object sender, EventArgs e)
		{
			switch (this.typeOper)
			{
				case (treeOper.toEdit):
					if (theNode != null) 
					{
						theNode.Text = memoHeader.Text; // TODO сделать механизм проверки того, что нужно обновлять
					}
					break;
				case (treeOper.toAdd):
					if (theNode != null) 
					{
						theNode.TreeView.Refresh();
					}
					break;
			}

			this.Close();
		}
	}
}

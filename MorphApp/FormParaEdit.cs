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
		private CLInnerStoreDB store = new CLInnerStoreDB();

		public ParagraphMap paraMap;
		private long docID;
		private long contID;
		private long parID = -1;
		private TreeNode theNode;

		public FormParaEdit()
		{
			InitializeComponent();
		}

		public void SetContext(long contID, long docID, TreeNode theNode)
		{
			this.theNode = theNode;
			if (theNode != null)
			{
				this.parID = Convert.ToInt64(theNode.Name);
				var pMap = store.GetParagraph(contID, docID, parID);
				this.paraMap = new ParagraphMap(pMap);
			}
		}

		private void FormParaEdit_Load(object sender, EventArgs e)
		{
			memoHeader.Text = String.Join("", paraMap.GetParagraphSents(SentTypes.enstHeader)
								.Select(x => x.sentence).ToList());
			memoBody.Text = String.Join("", paraMap.GetParagraphSents(SentTypes.enstBody)
								.Select(x => x.sentence).ToList());
		}

		private void btParaSave_Click(object sender, EventArgs e)
		{
			try
			{
				store.UpdateParagraph(this.paraMap, memoHeader.Text, true);
				store.UpdateParagraph(this.paraMap, memoBody.Text, false);
				var paramlist = store.SaveParagraphBD(this.paraMap);
				var pMap = store.GetParagraph(contID, docID, parID);
				if (pMap != null)
					pMap = this.paraMap;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.Source);
			}
		}

		private void btClose_Click(object sender, EventArgs e)
		{
			/*if (theNode != null) // TODO похоже не работает
			{
				theNode.Text = "qq";
				theNode.TreeView.Refresh();
			}*/
			this.Close();
		}
	}
}

using System;
using System.Windows.Forms;

using Schemas.BlockPlatform;
using AsmApp.Types;

namespace AsmApp
{
	public partial class FormParaEdit : Form
	{
		ClientStoreMediator store = ClientStoreMediator.Instance();

		private treeOper typeOper;
		private AsmNode theNode;
		private ParagraphAsm paraAsm;

		public FormParaEdit(AsmNode theNode, treeOper tOper)
		{		
			InitializeComponent();

			this.theNode = theNode;
			this.typeOper = tOper;

			AssemblyBase asm = null;
			switch (tOper)
			{
				case (treeOper.toEdit):
					asm = theNode.Assembly;
					break;
				case (treeOper.toAdd):
					asm = store.CreateParagraph(theNode.Assembly.BlockID);
					break;
			}
			this.paraAsm = new ParagraphAsm(asm);	
		}


	private void FormParaEdit_Load(object sender, EventArgs e)
		{
			rtbBody.FindReplaceVisible = false;
			rtbBody.HideToolstripItemsByGroup(RicherTextBox.RicherTextBoxToolStripGroups.SaveAndLoad, false);
			rtbBody.ToggleBold();
			rtbBody.SetFontSize(11.0f);

			if (paraAsm != null)
			{
				memoHeader.Text = paraAsm.GetHeader();
				//memoBody.Text = paraMap.GetBody();

				rtbBody.Text = paraAsm.GetBody();
				//rtbBody.Rtf = memoBody.Text;
			} 
		}

		private void btParaSave_Click(object sender, EventArgs e)
		{

/*			try
			{
				store.UpdateParagraph(this.paraMap, memoHeader.Text, true);
				//memoBody.Text = rtbBody.Text;

				store.UpdateParagraph(this.paraMap, rtbBody.Text, false);
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
			}*/
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

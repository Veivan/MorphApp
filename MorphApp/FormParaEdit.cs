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
       
        public FormParaEdit()
        {
            InitializeComponent();
        }

        private void FormParaEdit_Load(object sender, EventArgs e)
        {
            memoHeader.Text = //string.Join("", paraMap.GetParagraphPhrases(SentTypes.enstHeader).ToArray());

             String.Join("", paraMap.GetParagraphSents(SentTypes.enstHeader)
                                .Select(x => x.sentence).ToList());
            memoBody.Text = //string.Join(" ", paraMap.GetParagraphPhrases(SentTypes.enstBody).ToArray());
             String.Join("", paraMap.GetParagraphSents(SentTypes.enstBody)
                                .Select(x => x.sentence).ToList());
        }
        
        private void btParaSave_Click(object sender, EventArgs e)
        {
			try
			{
				store.UpdateParagraph(this.paraMap, memoHeader.Text, true);
				store.UpdateParagraph(this.paraMap, memoBody.Text, false);
				var paramlist = store.SaveParagraphBD(this.paraMap);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.Source);
			}
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}

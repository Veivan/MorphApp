using System;
using System.Windows.Forms;
using Schemas;


namespace MorphApp
{
    public class MorphNode : TreeNode
    {
        public MorphNode(string text) : base(text)
        {
        }

        public clNodeType NodeType { get; set; }

        // 
        public long bdID { get; set; }

        public void Delete(IntfInnerStore store)
        {
            switch (NodeType)
            {
                case clNodeType.clnParagraph:
                    {
                        var docNode = this.Parent as MorphNode;
                        var docID = docNode.bdID;
                        var contID = (docNode.Parent as MorphNode).bdID;
                        var parID = this.bdID;

                        try
                        {
                            store.DelParagraph(contID, docID, parID);
                            this.Remove();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, ex.Source);
                        }
                        break;
                    }
                case clNodeType.clnDocument:
                    {
                        var docID = this.bdID;
                        var contID = (this.Parent as MorphNode).bdID;

                        try
                        {
                            store.DelDocument(contID, docID);
                            this.Remove();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, ex.Source);
                        }
                        break;
                    }
                case clNodeType.clnContainer:
                    {
                        var contID = this.bdID;

                        try
                        {
                            if (store.CanRemoveContainer(contID))
                            {
                                store.DelContainer(contID);
                                this.Remove();
                            }
                            else
                                MessageBox.Show("Удалять можно только пустой контейенер!");

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, ex.Source);
                        }
                        break;
                    }
            }
        }
    }
}

using System;
using System.Windows.Forms;

using Schemas.BlockPlatform;

namespace AsmApp
{
	public class AsmNode : TreeNode
	{
		const string defaultName = "Объект";
		private AssemblyBase assembly;

		public clNodeType NodeType { get; private set; }

		public AsmNode(AssemblyBase _assembly, string Name = null) : base(_assembly == null ? Name ?? defaultName : _assembly.Name)
		{
			assembly = _assembly;
			switch(assembly.BlockType.NameKey)
			{
				case "DataContainer":
					NodeType = clNodeType.clnContainer;
					break;
				case "Document":
					NodeType = clNodeType.clnDocument;
					break;
				case "Paragraph":
					NodeType = clNodeType.clnParagraph;
					break;
			}
		}

		public AssemblyBase Assembly { get { return assembly; } set { assembly = value; } }

		public void Delete()
		{
			try
			{
				assembly.Delete();
				assembly.Save();
				this.Remove();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, ex.Source);
			}
		}

	}
}

using System;
using System.Windows.Forms;

using Schemas.BlockPlatform;

namespace AsmApp
{
	public class AsmNode : TreeNode
	{
		private AssemblyBase assembly;

		public AsmNode(string text, AssemblyBase _assembly = null) : base(text)
        {
			assembly = _assembly;
		}

		public AssemblyBase Assembly { get { return assembly; }  set { assembly = value; } }

	}
}

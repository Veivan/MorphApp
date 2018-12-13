using System;
using System.Windows.Forms;

using Schemas.BlockPlatform;

namespace AsmApp
{
	public class AsmNode : TreeNode
	{
		const string defaultName = "Объект";
		private AssemblyBase assembly;

		public AsmNode(AssemblyBase _assembly, string Name = null) : base(_assembly == null ? Name ?? defaultName : _assembly.Name)
		{
			assembly = _assembly;
		}

		public AssemblyBase Assembly { get { return assembly; }  set { assembly = value; } }

	}
}

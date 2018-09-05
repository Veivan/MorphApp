using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Schemas;
using Schemas.BlockPlatform;
using DirectDBconnector;

namespace LogicProcessor
{
	public class AssemblyServer : IAsmDealear
	{
		public BlockDBServer DBserver = new BlockDBServer();

		public override AssemblyBase CreateAssembly(BlockType type)
		{
			var id = DBserver.CreateBlock(type.BlockTypeID, -1, 1);
			var asm = new AssemblyBase(type);
			asm.RootBlock_id = id;
			return asm;
		}
	}
}

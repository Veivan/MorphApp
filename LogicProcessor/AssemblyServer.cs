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
			var asm = new AssemblyBase(type, id);
			return asm;
		}

		public override AssemblyBase CreateAssembly(AssemblyBase templAsm, FOLLOWMODE mode = FOLLOWMODE.Forget)
		{
			var id = DBserver.CreateBlock(templAsm.BlockType.BlockTypeID, -1, 1);
			var asm = new AssemblyBase(templAsm, id, mode);
			CreateChildrenRequrs(asm.Children);
			return asm;
		}

		/// <summary>
		/// Рекурсивное сохранение дочерних сборок в БД.
		/// </summary>
		/// <param name="src_children">перечень дочерних сборок</param>
		private void CreateChildrenRequrs(List<AssemblyBase> src_children)
		{
			int i = 0;
			foreach (var child in src_children)
			{
				long parentID = child.ParentAssembly == null ? -1 : child.ParentAssembly.RootBlock_id;
				var chid = DBserver.CreateBlock(child.BlockType.BlockTypeID, parentID, i);
				child.RootBlock_id = chid;
				CreateChildrenRequrs(child.Children);
				i++;
			}
		}

	}
}

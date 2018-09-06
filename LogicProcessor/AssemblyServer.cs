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
			var id = DBserver.CreateBlock(type.BlockTypeID, -1, 0);
			var asm = new AssemblyBase(type, id);
			return asm;
		}

		public override AssemblyBase CreateAssembly(AssemblyBase templAsm, FOLLOWMODE mode = FOLLOWMODE.Forget)
		{
			var id = DBserver.CreateBlock(templAsm.BlockType.BlockTypeID, -1, 0);
			var asm = new AssemblyBase(templAsm, id, mode);
			CreateChildrenRequrs(asm.Children);
			return asm;
		}

		public override AssemblyBase GetAssembly(long Addr, FOLLOWMODE mode = FOLLOWMODE.Forget)
		{
			var block = DBserver.GetBlock(Addr);
			var asm = new AssemblyBase(block);
			return asm;
		}

		/// <summary>
		/// Рекурсивное сохранение дочерних сборок в БД.
		/// </summary>
		/// <param name="src_children">перечень дочерних сборок</param>
		private void CreateChildrenRequrs(List<AssemblyBase> src_children)
		{
			foreach (var child in src_children)
			{
				var chid = DBserver.CreateBlock(child.BlockType.BlockTypeID, child.ParentAssemblyID, (int)child.Treeorder);
				child.RootBlock_id = chid;
				CreateChildrenRequrs(child.Children);
			}
		}

	}
}

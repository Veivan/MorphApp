using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Schemas;
using Schemas.BlockPlatform;
using BlockAddress = System.Int64;
using DirectDBconnector;

namespace LogicProcessor
{
	class ContainerServer : IContainerPerformer
	{
		const string containerTypeName = "DataContainer";
		public BlockDBServer DBserver = new BlockDBServer();

		public override long CreateContainer(string name, long parent, int treeorder)
		{
			var typeOfDict = DBserver.GetBlockTypeByNameKey(containerTypeName);
			BlockAddress id = DBserver.CreateBlock(typeOfDict.BlockTypeID, parent, treeorder);
			DBserver.AttrSetValue(id, "Name", name);
			return id;
		}

		public override ComplexValue GetChildrenInContainerList(tpList resulttype, List<string> list_ids)
		{
			ComplexValue rval = DBserver.GetChildren(resulttype, list_ids);
			return rval;
		}

	}
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using LogicProcessor;
using Schemas.BlockPlatform;
using DirectDBconnector;

namespace SagaTest
{
	[TestClass]
	public class AsmServerTest
	{
		public AssemblyServer AsmServer = new AssemblyServer();
		public BlockDBServer DBserver = new BlockDBServer();

		[TestMethod]
		public void Test_CreateAssembly()
		{
			var typeName = "документ";
			var tid = DBserver.GetBlockTypeByName(typeName);
			var type = new BlockType(tid, typeName);
			var asm = AsmServer.CreateAssembly(type);
			Assert.AreNotEqual(null, asm);
		}
	}
}

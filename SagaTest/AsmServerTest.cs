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
			var nameKey = "Document";
			var type = DBserver.GetBlockTypeByNameKey(nameKey);
			var asm = AsmServer.CreateAssembly(type);
			Assert.AreNotEqual(null, asm);
		}
		
		[TestMethod]
		public void Test_CreateAssemblyFromTemplate()
		{
			var nameKey = "Document";
			var type = DBserver.GetBlockTypeByNameKey(nameKey);
			var asm = AsmServer.CreateAssembly(type);
			Assert.AreNotEqual(null, asm);
		}

	}
}

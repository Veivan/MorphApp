using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDBconnector;

namespace SagaTest
{
	[TestClass]
	public class BlockDBServrTest
	{
		
		public BlockDBServer DBserver = new BlockDBServer();

		[TestMethod]
		public void Test_CreateBlockType()
		{
			var res = DBserver.CreateBlockType("треб");
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void Test_GetBlockTypeByName()
		{
			var res = DBserver.GetBlockTypeByName("треб");
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void Test_GetBlockTypeByAddr()
		{
			var res = DBserver.GetBlockTypeByAddr(2);
			Assert.AreNotEqual("", res);
		}

		[TestMethod]
		public void Test_CreateAttribute()
		{
			var id = DBserver.CreateAttribute("attr2", 1, 2, 0);
			Assert.AreNotEqual(0, id);
		}

		[TestMethod]
		public void Test_CreateBlock()
		{
			var id = DBserver.CreateBlock(2, -1, 1);
			Assert.AreNotEqual(0, id);
		}

		[TestMethod]
		public void Test_GetOrder()
		{
			var id = DBserver.GetOrder(1);
			Assert.AreNotEqual(-1, id);
		}
	}
}

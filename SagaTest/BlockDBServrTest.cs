using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDBconnector;
using Schemas;
using System.Collections.Generic;
using Schemas.BlockPlatform;

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

		[TestMethod]
		public void Test_SetFactData()
		{
			var attr1 = new AttrFactData();
			attr1.Type = enAttrTypes.mnint;
			attr1.Value = 10;
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blob = new Blob(list);

			var id = DBserver.SetFactData(1, blob);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod]
		public void Test_CreateDictionary()
		{

			string name = "TestDict2";
			long blockType = 1; // документ
			var id = DBserver.CreateDictionary(name, blockType);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod()]
		public void Test_GetDictionaryByName()
		{
			string name = "TestDict2";
			var id = DBserver.GetDictionaryByName(name);
			Assert.AreNotEqual(-1, id);
		}


	}
}

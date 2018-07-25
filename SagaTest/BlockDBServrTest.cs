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
			byte[] x = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var list = new List<enAttrTypes>();
			list.Add(enAttrTypes.mnint);
			var converter = new BlobConvertor(list);
			var attrtype = new AttrType(2, "целое число");
			var dict = new Dictionary<AttrType, object>();
			dict.Add(attrtype, x);
			Blob blob = converter.BlobSetData(dict);

			var id = DBserver.SetFactData(1, blob);
			Assert.AreNotEqual(-1, id);
		}

	}
}

﻿using System;
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

		#region Функции для работы с Типами блоков
		[TestMethod]
		public void Test_CreateBlockType()
		{
			var res = DBserver.CreateBlockType("Cabbige", "Капуста");
			Assert.AreNotEqual(null, res);
		}

		[TestMethod]
		public void Test_GetBlockTypeByNameKey()
		{
			var res = DBserver.GetBlockTypeByNameKey("Document");
			Assert.AreNotEqual(null, res);
		}

		[TestMethod]
		public void Test_GetBlockTypeByAddr()
		{
			var res = DBserver.GetBlockTypeByAddr(2);
			Assert.AreNotEqual("", res);
		}

		[TestMethod]
		public void Test_GetAllBlockTypes()
		{
			var res = DBserver.GetAllBlockTypes();
			Assert.AreNotEqual(null, res);
		}

		[TestMethod]
		public void Test_BlockTypeChangeStrings()
		{
			var res = DBserver.CreateBlockType("Cabbige", "Капуста");
			res.NameUI = res.NameUI + "бебе";
			DBserver.BlockTypeChangeStrings(res);
			var res2 = DBserver.GetBlockTypeByAddr(res.BlockTypeID);
			Assert.AreEqual(res.NameUI, res2.NameUI);
		}


		#endregion

		#region Функции для работы с атрибутами типов блоков

		[TestMethod]
		public void Test_CreateAttribute()
		{
			var id = DBserver.CreateAttribute("attr2", "Атрибут 2", 1, 2, 0);
			Assert.AreNotEqual(0, id);
		}

		[TestMethod]
		public void Test_GetFildsNames()
		{
			var typeid = 1;
			var reslist = DBserver.GetFildsNameKey(typeid);
			var res = reslist == null ? "null" : string.Join("," , reslist);
			Console.WriteLine("Attrs : " + res);
			Assert.AreNotEqual(null, reslist);
		}

		#endregion

		#region Функции для работы с Блоками
		[TestMethod]
		public void Test_CreateBlock()
		{
			var id = DBserver.CreateBlock(2, -1, 1);
			Assert.AreNotEqual(0, id);
		}

		[TestMethod]
		public void Test_GetBlock()
		{
			var blk = DBserver.GetBlock(9);
			Assert.AreNotEqual(0, blk.BlockID);
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
			var addr = 1;
			var newval = 12;

			var oldval = DBserver.AttrGetValue<int>(addr, 0);

			var attr1 = new AttrFactData();
			attr1.Type = enAttrTypes.mnlong;
			attr1.Value = newval;
			var list = new List<AttrFactData>();
			list.Add(attr1);
			Blob blob = new Blob(list);

			var id = DBserver.SetFactData(addr, blob);

			var blobdb = DBserver.GetFactData(addr);
			var dbval = blobdb.ValueList[0].Value;
			Console.WriteLine("Before : " + oldval.ToString() + " After : " + dbval.ToString());

			Assert.AreNotEqual(oldval, dbval);
		}

		[TestMethod]
		public void Test_GetFactData()
		{
			var addr = 2;
			var blobdb = DBserver.GetFactData(addr);
			var val = blobdb.ValueList[0].Value;
			var res = val == null ? "null" : val.ToString();
			Console.WriteLine("Attr0 : " + res);
			Assert.AreNotEqual(-1, val);
		}

		[TestMethod]
		public void Test_AttrGetValueByOrd()
		{
			var addr = 9;
			var val = DBserver.AttrGetValue<string>(addr, 0);
			var res = val == null ? "null" : val.ToString();
			Console.WriteLine("Attr0 : " + res);
			Assert.AreNotEqual(-1, val);
		}

		[TestMethod]
		public void Test_AttrGetValueByNameKey()
		{
			var addr = 9;
			var attrnamekey = "lx_id";
			var val = DBserver.AttrGetValue<long>(addr, attrnamekey);
			Console.WriteLine(attrnamekey + ": " + val.ToString());
			Assert.AreNotEqual(-1, val);
		}

		[TestMethod]
		public void Test_AttrSetValueByOrd()
		{
			var addr = 2;
			var ord = 0;
			var newval = "qq";
			var val = DBserver.AttrGetValue<string>(addr, ord);
			Console.WriteLine("Before : " + (val == null? "null" : val.ToString()));
			DBserver.AttrSetValue(addr, ord, newval);
			val = DBserver.AttrGetValue<string>(addr, ord);
			Console.WriteLine("After : " + (val == null ? "null" : val.ToString()));
			Assert.AreEqual(newval, val);
		}

		[TestMethod]
		public void Test_AttrSetValueByNameKey()
		{
			var addr = 2;
			var attrnamekey = "attr2";
			var newval = "qqw";
			var val = DBserver.AttrGetValue<string>(addr, attrnamekey);
			Console.WriteLine("Before : " + (val == null ? "null" : val.ToString()));
			DBserver.AttrSetValue(addr, attrnamekey, newval);
			val = DBserver.AttrGetValue<string>(addr, attrnamekey);
			Console.WriteLine("After : " + (val == null ? "null" : val.ToString()));
			Assert.AreEqual(newval, val);
		}

		[TestMethod()]
		public void Test_dbSearchBlocks()
		{
			var sess = Session.Instance();
			sess.Init(null, DBserver);

			var blockType = 10; //Lexema
			var grenPart = (long)GrenPart.NOUN_ru;
			var lemma = "мама";
			//var lemma = "папа";
			var args = new Dictionary<string, object>();
			args.Add("GrenPart", grenPart);
			args.Add("Lemma", lemma.ToLower());

			var result = DBserver.SearchBlocks(blockType, args);

			Assert.AreNotEqual(0, result.Count);
		}

		#endregion

		#region Функции для работы со Справочниками
		[TestMethod]
		public void Test_CreateDictionary()
		{
			string name = "TestDict3";
			int blockType = 1; // документ
			var id = DBserver.CreateDictionary(name, blockType);
			Assert.AreNotEqual(-1, id);
		}

		#endregion

	}
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDBconnector;
using Schemas;
using System.Collections.Generic;
using Schemas.BlockPlatform;

namespace DirectDBconnector.Tests
{ 
	[TestClass]
	public class SQLiteConnectorTest
	{
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

		[TestMethod]
		public void SaveLex_qq_Returned_graterThen0()
		{
			long res = dbConnector.SaveLex("qq", 6);
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void GetWord_qq_Returned_qq()
		{
			long res = dbConnector.GetWord("qq", 6);
			Assert.AreNotEqual(-1, res);
		}

		[TestMethod]
		public void Sys_ShowSelectAll()
		{
			dbConnector.selectAll(dbTables.mAttributes);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void Sys_PerformOperator()
		{
			dbConnector.PerformOperator();
		}

		[TestMethod]
		public void Sys_TestEmptyDB()
		{
			dbConnector.EmptyDB();
			dbConnector.selectAll(dbTables.tblParagraphs);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertSpeechParts()
		{
			//dbConnector.InsertSpeechParts();
			dbConnector.selectAll(dbTables.tblParts);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertGrenProperties()
		{
			dbConnector.InsertGrenProperties();
			dbConnector.selectAll(dbTables.tblSiGram);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertGrenLinks()
		{
			dbConnector.InsertGrenLinks();
			dbConnector.selectAll(dbTables.tblSiLinks);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertDocument()
		{
			dbConnector.InsertDocumentDB("docN", 1);
			dbConnector.selectAll(dbTables.tblDocuments);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertContainerDB()
		{
			var id = dbConnector.InsertContainerDB("Хранилище");
			dbConnector.selectAll(dbTables.tblContainers);
			Assert.AreNotEqual(-1, id);
		}


		#region Тестирование функций для BlockDBServer
		[TestMethod]
		public void Test_dbCreateBlockType()
		{
			var res = dbConnector.dbCreateBlockType("документ");
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void Test_dbGetBlockTypeByName()
		{
			var res = dbConnector.dbGetBlockTypeByName("документ");
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void Test_dbGetBlockTypeByAddr()
		{
			var res = dbConnector.dbGetBlockTypeByAddr(1);
			Assert.AreNotEqual("", res);
		}

		[TestMethod]
		public void Test_dbCreateAttribute()
		{
			var id = dbConnector.dbCreateAttribute("attr1", "Атрибут 1", 1, 1, 0);
			Assert.AreNotEqual(0, id);
		}

		[TestMethod]
		public void Test_dbCreateBlock()
		{
			var id = dbConnector.dbCreateBlock(1, -1, 1);
			Assert.AreNotEqual(0, id);
		}

		[TestMethod]
		public void Test_dbGetOrder()
		{
			var id = dbConnector.dbGetOrder(1);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod]
		public void Test_dbInsertFactData()
		{
			byte[] x = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var id = dbConnector.dbInsertFactData(x);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod()]
		public void Test_dbSetFactData()
		{
			var addr = 1;
			var number = 13;
			byte[] x = BitConverter.GetBytes(number);
			if (BitConverter.IsLittleEndian)
				Array.Reverse(x);
			var id = dbConnector.dbSetFactData(addr, x, false);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod()]
		public void Test_dbCreateDictionary()
		{
			string name = "TestDict";
			int blockType = 1; // документ
			var id = dbConnector.dbCreateDictionary(name, blockType);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod()]
		public void Test_dbDictPerfomElements()
		{
			byte[] x = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var id = 4;
			var result = "";
			try
			{
				dbConnector.dbDictPerfomElements(id, x);
			}
			catch (Exception ex)
			{
				result = ex.Message;
			}
			Assert.AreEqual("", result);
		}

		[TestMethod()]
		public void Test_dbGetAttrTypesList()
		{
			var addr = 1; 
			var reslist = dbConnector.dbGetAttrTypesList(addr);
			var result = string.Join(",", reslist);
			Console.WriteLine("Attrtypes : " + result);

			Assert.AreNotEqual(null, addr);
		}

		#endregion
	}
}

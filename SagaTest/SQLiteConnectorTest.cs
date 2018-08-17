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
		public void ShowSelectAll()
		{
			dbConnector.selectAll(dbTables.tblSyntNodes);
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestEmptyDB()
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

		[TestMethod]
		public void TestDropColumnDB()
		{
			dbConnector.selectAll(dbTables.tblParagraphs);
			dbConnector.DropColumn();
			dbConnector.selectAll(dbTables.tblParagraphs);
			Assert.AreNotEqual(-1, 0);
		}

		[TestMethod]
		public void TestAddColumnDB()
		{
			dbConnector.AddColumn();
			dbConnector.selectAll(dbTables.tblSyntNodes);
			Assert.AreNotEqual(-1, 0);
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
			var id = dbConnector.dbCreateAttribute("attr1", 1, 1, 0);
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
			byte[] x = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
			var id = dbConnector.dbSetFactData(2, x, false);
			Assert.AreNotEqual(-1, id);
		}

		[TestMethod()]
		public void Test_dbCreateDictionary()
		{
			string name = "TestDict";
			long blockType = 1; // документ
			var id = dbConnector.dbCreateDictionary(name, blockType);
			Assert.AreNotEqual(-1, id);
		}

		#endregion
	}
}

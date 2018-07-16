using System;

using DirectDBconnector;
using Schemas;
using NUnit.Framework;

namespace nUnitSaga
{
	[TestFixture]
	public class SQLiteConnectorTest
	{
		[Test]
		public void SaveLex_qq_Returned_graterThen0()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			long res = dbConnector.SaveLex("qq", 6);
			// assert
			Assert.AreNotEqual(0, res);
		}

		[Test]
		public void GetWord_qq_Returned_qq()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			long res = dbConnector.GetWord("qq", 6);
			// assert
			Assert.AreNotEqual(-1, res);
		}

		[Test]
		public void ShowSelectAll()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.selectAll(dbTables.tblSyntNodes);
			// assert
			Assert.AreEqual(0, 0);
		}

		[Test]
		public void TestEmptyDB()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.EmptyDB();
			// assert
			dbConnector.selectAll(dbTables.tblParagraphs);
			Assert.AreEqual(0, 0);
		}

		[Test]
		public void TestInsertSpeechParts()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.InsertSpeechParts();
			dbConnector.selectAll(dbTables.tblParts);
			// assert
			Assert.AreEqual(0, 0);
		}

		[Test]
		public void TestInsertGrenProperties()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.InsertGrenProperties();
			dbConnector.selectAll(dbTables.tblSiGram);
			// assert
			Assert.AreEqual(0, 0);
		}

		[Test]
		public void TestInsertGrenLinks()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.InsertGrenLinks();
			dbConnector.selectAll(dbTables.tblSiLinks);
			// assert
			Assert.AreEqual(0, 0);
		}

		[Test]
		public void TestInsertDocument()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.InsertDocumentDB("docN", 1);
			dbConnector.selectAll(dbTables.tblDocuments);
			// assert
			Assert.AreEqual(0, 0);
		}

		[Test]
		public void TestInsertContainerDB()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			var id = dbConnector.InsertContainerDB("Хранилище");
			dbConnector.selectAll(dbTables.tblContainers);
			// assert
			Assert.AreNotEqual(-1, id);
		}

		[Test]
		public void TestDropColumnDB()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.selectAll(dbTables.tblParagraphs);
			dbConnector.DropColumn();
			dbConnector.selectAll(dbTables.tblParagraphs);
			// assert
			Assert.AreNotEqual(-1, 0);
		}

		[Test]
		public void TestAddColumnDB()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			dbConnector.AddColumn();
			dbConnector.selectAll(dbTables.tblSyntNodes);
			// assert
			Assert.AreNotEqual(-1, 0);
		}

		[Test]
		public void Test_dbCreateBlockType()
		{
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			var res = dbConnector.dbCreateBlockType("Test1");
			Assert.AreNotEqual(0, res);
		}

		[Test]
		public void Test_dbGetBlockTypeByName()
		{
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			var res = dbConnector.dbGetBlockTypeByName("документ");
			Assert.AreNotEqual(0, res);
		}

		[Test]
		public void Test_dbGetBlockTypeByAddr()
		{

			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			var res = dbConnector.dbGetBlockTypeByAddr(1);
			Assert.AreNotEqual("", res);
		}

		[Test]
		public void Test_dbCreateAttribute()
		{

			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			var id = dbConnector.dbCreateAttribute("attr1", 1, 1, 0);
			Assert.AreNotEqual(0, id);
		}
	}
}

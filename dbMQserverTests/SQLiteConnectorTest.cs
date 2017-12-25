using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using dbMQserver;

namespace dbMQserverTests
{
	[TestClass]
	public class SQLiteConnectorTest
	{
		[TestMethod]
		public void SaveLex_qq_Returned_graterThen0()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			long res = dbConnector.SaveLex("qq", 6);

			// assert
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void GetWord_qq_Returned_qq()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			long res = dbConnector.GetWord("qq", 6);

			// assert
			Assert.AreNotEqual(-1, res);

		}
		
		[TestMethod]
		public void ShowSelectAll()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			dbConnector.selectAll("mSiLinks");

			// assert
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertSpeechParts()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			dbConnector.InsertSpeechParts();
			dbConnector.selectAll("mSpParts");

			// assert
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertGrenProperties()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			dbConnector.InsertGrenProperties();
			dbConnector.selectAll("mSiGram");

			// assert
			Assert.AreEqual(0, 0);
		}

		[TestMethod]
		public void TestInsertGrenLinks()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			dbConnector.InsertGrenLinks();
			dbConnector.selectAll("mSiLinks");

			// assert
			Assert.AreEqual(0, 0);
		}

	}
}

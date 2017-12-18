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
			long res = dbConnector.SaveLex("qq");

			// assert
			Assert.AreNotEqual(0, res);
		}

		[TestMethod]
		public void GetWord_qq_Returned_qq()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			long res = dbConnector.GetWord("qq");

			// assert
			Assert.AreNotEqual(-1, res);

		}
		
		[TestMethod]
		public void ShowSelectAll()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// act
			dbConnector.selectAll();

			// assert
			Assert.AreEqual(0, 0);
		}
	}
}

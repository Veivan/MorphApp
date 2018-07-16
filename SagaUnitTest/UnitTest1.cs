using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DirectDBconnector;
using Schemas;

namespace SagaUnitTest
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void TestMethod1()
		{
			// arrange
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			// act
			long res = dbConnector.GetWord("qq", 6);
			// assert
			Assert.AreNotEqual(-1, res);
		}
	}
}

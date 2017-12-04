using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbMQserver
{
	class ServerStart
	{
		static void Main(string[] args)
		{
			SQLiteConnector dbConnector = SQLiteConnector.Instance;
			long w1 = dbConnector.SaveLex("qq");
			long w2 = dbConnector.SaveLex("ww");
			long ph_id = dbConnector.SavePhrase(-1);
			long x = dbConnector.SavePhraseContent(ph_id, w1);
			Console.WriteLine(x.ToString());
			dbConnector.selectAll();
			Console.ReadKey();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MorphMQserver
{
	class Program
	{
		static void Main(string[] args)
		{
			GrenHelper gren = new GrenHelper();
			//string dict = @"D:\Work\Framework\RussianGrammaticalDictionary64\bin-windows64\dictionary.xml";
			string dict = @"C:\Work\Framework\GrammarEngine\src\bin-windows64\dictionary.xml";
			//gren.Init(dict);
			//Console.WriteLine(" Dictionary vers : {0} ", gren.GetDictVersion());
            MorphServer morphServer = new MorphServer();
            morphServer.Run();
		}
	}
}

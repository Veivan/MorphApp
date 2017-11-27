using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZeroMQ;

namespace MorphMQserver
{
	class Program
	{
		static void Main(string[] args)
		{
			GrenHelper gren = new GrenHelper();
			//string dict = @"D:\Work\Framework\RussianGrammaticalDictionary64\bin-windows64\dictionary.xml";
			string dict = @"D:\Work\Framework\GrammarEngine\src\bin-windows64\dictionary.xml";
			gren.Init(dict);
			Console.WriteLine(" Dictionary vers : {0} ", gren.GetDictVersion());

			// Create
			// using (var context = new ZContext())
			using (var responder = new ZSocket(ZSocketType.REP))
			{
				// Bind
				responder.Bind("tcp://*:5555");

				while (true)
				{
					// Receive
					using (ZFrame request = responder.ReceiveFrame())
					{
						var req = request.ReadString();
						Console.WriteLine("Received {0}", req);
						string resp = "";

						// Do some work
						if (req.StartsWith("morph"))
						{
							resp = gren.GetMorphInfo(req);
						}
						else
						{
							resp = gren.GetSynInfo(req);
						}
						//Thread.Sleep(1);

						// Send
						responder.Send(new ZFrame(resp));
					}
				}
			}

		}
	}
}

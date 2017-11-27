using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZeroMQ;

namespace MorphMQserver
{
	public enum Pars
	{
		UNDEF = -1,
		MORPH_an = 0,
		SYNT_an = 1
	}

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

						var command = Pars.UNDEF;
						var strip_req = strip_request(req, out command);

						// Do some work
						if (req.StartsWith("morph"))
						{
							resp = gren.GetMorphInfo(strip_req);
						}
						else
						{
							resp = gren.GetSynInfo(strip_req);
						}
						//Thread.Sleep(1);

						// Send
						responder.Send(new ZFrame(resp));
					}
				}
			}
		}

		static string strip_request(string requestText, out Pars command)
		{
			if (requestText.StartsWith("morph"))
				command = Pars.MORPH_an;
			else
				command = Pars.SYNT_an;
			var req = requestText.Split(' ');
			var stripped = "";
			for (int i = 1; i < req.Length; i++)
				stripped += req[i] + " ";
			return stripped.TrimEnd();
		}

	}
}

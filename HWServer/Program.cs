using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZeroMQ;
using FlatBuffers;
using TMorph.Schema;

namespace HWServer
{
	class Program
	{
		static void Main(string[] args)
		{
			if (args == null || args.Length < 1)
			{
				Console.WriteLine();
				Console.WriteLine("Usage: ./{0} HWServer [Name]", AppDomain.CurrentDomain.FriendlyName);
				Console.WriteLine();
				Console.WriteLine("    Name   Your name. Default: World");
				Console.WriteLine();
				args = new string[] { "World" };
			}

			string name = args[0];

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
						Console.WriteLine("Received {0}", request.ReadString());
						request.Position = 0;
						var buf = request.Read();

						PrintReq(buf);
						// Do some work
						Thread.Sleep(1);

						// Send
						responder.Send(new ZFrame(name));
					}
				}
			}
		}

		static void PrintReq(byte[] req)
		{
			var buf = new ByteBuffer(req);
			var message = Message.GetRootAsMessage(buf);
			Console.WriteLine(" ServerType : {0}", message.ServerType.ToString());
			Console.WriteLine(" Comtype : {0}", message.Comtype.ToString());

	
		}

		static FlatBufferBuilder SetReq()
		{
			var builder = new FlatBufferBuilder(1);
			var param1Name = builder.CreateString("phrase");
			var param1Val = builder.CreateString("мама мыла раму");
			var parms = new Offset<Param>[1];
			parms[0] = Param.CreateParam(builder, param1Name, param1Val);
			var paracol = Message.CreateParamsVector(builder, parms);

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mRequest);
			Message.AddServerType(builder, ServType.svMorph);
			Message.AddComtype(builder, ComType.Token);
			Message.AddParams(builder, paracol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);

			return builder;
		}
	
	}
}

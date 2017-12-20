using System;

using ZeroMQ;
using dbMQserver.Commands;

namespace dbMQserver
{
	class mqServer
	{
		CommandBuilder combuilder = new CommandBuilder();

		public void Run()
		{
			SQLiteConnector dbConnector = SQLiteConnector.Instance;

			// Create
			// using (var context = new ZContext())
			using (var responder = new ZSocket(ZSocketType.REP))
			{
				// Bind
				responder.Bind("tcp://*:5560");

				while (true)
				{
					// Receive
					using (ZFrame request = responder.ReceiveFrame())
					{
						request.Position = 0;
						var buf = request.Read();

						var order = combuilder.GetCommand(buf);
						Console.WriteLine("Received {0}", combuilder.CommandType);
						// Do some work
						order.Execute(dbConnector);
						// Send
						var foo = order.GetResultBytes();
						responder.Send(new ZFrame(foo));
					}
				}
			}
		}
	}
}

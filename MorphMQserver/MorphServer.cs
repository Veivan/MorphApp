using System;
using System.Collections.Generic;

using ZeroMQ;
using MorphMQserver.Commands;

namespace MorphMQserver
{
    class MorphServer
    {
		GrenHelper gren = new GrenHelper();
		CommandBuilder combuilder = new CommandBuilder();

        public void Run()
        {
			gren.Init();
			Console.WriteLine(" Dictionary vers : {0} ", gren.GetDictVersion());
			
			// Create
            // using (var context = new ZContext())
            using (var responder = new ZSocket(ZSocketType.REP))
            {
                // Bind
                responder.Bind("tcp://*:5559");

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
						order.Execute(gren);
						// Send
						var foo = order.GetResultBytes();
                        responder.Send(new ZFrame(foo));
                    }
                }
            }
        }

    }
}

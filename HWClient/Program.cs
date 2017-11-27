using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroMQ;

namespace HWClient
{
	class Program
	{
		static void Main(string[] args)
		{
			// Create
			// using (var context = new ZContext())
			using (var requester = new ZSocket(ZSocketType.REQ))
			{
				// Connect
				requester.Connect("tcp://127.0.0.1:5555");

				for (int n = 0; n < 10; ++n)
				{
					string requestText = "Hello";
					Console.Write("Sending {0}...", requestText);

					// Send
					requester.Send(new ZFrame(requestText));

					// Receive
					using (ZFrame reply = requester.ReceiveFrame())
					{
						Console.WriteLine(" Received: {0} {1}!", requestText, reply.ReadString());
					}
				}
			}
		}
	}
}

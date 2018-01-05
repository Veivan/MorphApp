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
			mqServer mqServer = new mqServer();
			mqServer.Run();
		}
	}
}

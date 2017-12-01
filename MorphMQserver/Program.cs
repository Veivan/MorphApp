using System;

namespace MorphMQserver
{
	class Program
	{
		static void Main(string[] args)
		{
            MorphServer morphServer = new MorphServer();
            morphServer.Run();
		}
	}
}

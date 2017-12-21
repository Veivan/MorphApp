using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbMQserver.Commands
{
	public interface ICommand
	{
        void Execute();
		byte[] GetResultBytes();
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MorphMQserver.Commands
{
	public interface ICommand
	{
		void Execute(GrenHelper gren);
		byte[] GetResultBytes();
	}
}

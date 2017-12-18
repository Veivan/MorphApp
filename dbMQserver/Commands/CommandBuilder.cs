using System;
using TMorph.Schema;
using Schemas;
using FlatBuffers;

namespace dbMQserver.Commands
{
	class CommandBuilder
	{
		private ComType command = ComType.Undef;
		public ComType CommandType
		{
			get
			{
				return command;
			}
		}

		private ICommand intCommand = null;

		public ICommand GetCommand(byte[] request)
		{
			intCommand = null;

			var buf = new ByteBuffer(request);
			var message = Message.GetRootAsMessage(buf);
			command = message.Comtype;
			switch (command)
			{
				case ComType.GetWord:
					{
						Param? par = message.Params(0);
						var strParam = "";
						if (par.HasValue)
							strParam = par.Value.Value;
						intCommand = new CommandGetWord(strParam);
						break;
					}
				case ComType.SaveLex:
					{
						Param? par = message.Params(0);
						var strParam = "";
						if (par.HasValue)
							strParam = par.Value.Value;
						intCommand = new CommandSaveLex(strParam);
						break;
					}
				/*case ComType.Repar:
					{
						var sentence = SentenceMap.BuildFromMessage(message);
						intCommand = new CommandRepar(sentence);
						break;
					} */
				default:
					break;
			}
			return intCommand;
		}
	}
}

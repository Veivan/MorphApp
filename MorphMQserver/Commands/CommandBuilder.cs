using System;
using TMorph.Schema;
using Schemas;
using FlatBuffers;

namespace MorphMQserver.Commands
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
				case ComType.Synt:
					{
						Param? par = message.Params(0);
						var strParam = "";
						if (par.HasValue)
							strParam = par.Value.Value;
						intCommand = new CommandSyntAn(strParam);
						break;
					}
				case ComType.Separ:
					{
						Param? par = message.Params(0);
						var strParam = "";
						if (par.HasValue)
							strParam = par.Value.Value;
						intCommand = new CommandSepar(strParam);
						break;
					}
				case ComType.Repar:
					{
						SentenceMap sentence = null;
						var sentlist = SentenceMap.BuildFromMessage(message);
						if (sentlist.Count > 0)
							sentence = sentlist[0];
						intCommand = new CommandRepar(sentence);
						break;
					}
				case ComType.Morph:
					break;
				default:
					break;
			}
			return intCommand;
		}
	}
}

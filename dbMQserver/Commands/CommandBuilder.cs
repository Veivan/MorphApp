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
				case ComType.SavePara:
					{
						var senttlist = SentenceMap.BuildFromMessage(message);
						intCommand = new CommandSaveParagraph(message.ParagraphID, senttlist);
						break;
					}
				case ComType.ReadPara:
					{
						var ParagraphID = message.ParagraphID;
						intCommand = new CommandReadParagraph(message.ParagraphID);
						break;
					}
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
				default:
					break;
			}
			return intCommand;
		}
	}
}

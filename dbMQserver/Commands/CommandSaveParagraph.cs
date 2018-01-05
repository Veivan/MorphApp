using System;
using System.Collections.Generic;
using TMorph.Schema;
using Schemas;
using FlatBuffers;
using DirectDBconnector;

namespace dbMQserver.Commands
{
	class CommandSaveParagraph : ICommand
	{
		private ComType command = ComType.SavePara;
		private List<SentenceMap> sentlist = new List<SentenceMap>();
		private long ParagraphID;

		public CommandSaveParagraph(long ParagraphID, List<SentenceMap> sentlist)
		{
			this.ParagraphID = ParagraphID;
			this.sentlist.AddRange(sentlist);
		}

		public void Execute()
		{
			var dbServer = new SagaDBServer();
			ParagraphID = dbServer.SaveParagraph(ParagraphID, sentlist);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var paramName = builder.CreateString("ParagraphID"); // TODO здесь надо передавать результат выполнения операции
			var paramVal = builder.CreateString(ParagraphID.ToString());
			var parms = new Offset<Param>[1];
			parms[0] = Param.CreateParam(builder, paramName, paramVal);
			var paracol = Message.CreateParamsVector(builder, parms);

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mReplay);
			Message.AddServerType(builder, ServType.svSUBD);
			Message.AddComtype(builder, command);
			Message.AddParams(builder, paracol);
			Message.AddDbID(builder, ParagraphID);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);
			return builder.SizedByteArray();
		}
	}
}

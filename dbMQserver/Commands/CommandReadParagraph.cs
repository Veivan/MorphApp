using System;
using System.Collections.Generic;
using TMorph.Schema;
using Schemas;
using FlatBuffers;
using DirectDBconnector;

namespace dbMQserver.Commands
{
	class CommandReadParagraph : ICommand
	{
		private ComType command = ComType.ReadPara;
		private List<SentenceMap> sentlist = new List<SentenceMap>();
		private long ParagraphID;

		public CommandReadParagraph(long ParagraphID)
		{
			this.ParagraphID = ParagraphID;
		}

		public void Execute()
		{
			var dbServer = new SagaDBServer();
			var sents = dbServer.ReadParagraphDB(ParagraphID);
			this.sentlist.AddRange(sents);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var paramName = builder.CreateString("ParagraphID"); // TODO здесь надо передавать результат выполнения операции
			var paramVal = builder.CreateString(ParagraphID.ToString());
			var parms = new Offset<Param>[1];
			parms[0] = Param.CreateParam(builder, paramName, paramVal);
			var paracol = Message.CreateParamsVector(builder, parms);
			VectorOffset sentscol = default(VectorOffset);
			sentscol = SentenceMap.BuildSentOffsetFromSentStructList(builder, this.sentlist);

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mReplay);
			Message.AddServerType(builder, ServType.svSUBD);
			Message.AddComtype(builder, command);
			Message.AddParams(builder, paracol);
			Message.AddSentences(builder, sentscol);
			Message.AddParagraphID(builder, ParagraphID);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);
			return builder.SizedByteArray();
		}
	}
}

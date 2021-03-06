﻿using System;
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
        private long DocID;
		private ParagraphMap pMap;

		public CommandSaveParagraph(long ParagraphID, long DocID, List<SentenceMap> sentlist)
		{
            this.ParagraphID = ParagraphID;
            this.DocID = DocID;
            this.sentlist.AddRange(sentlist);

			pMap = new ParagraphMap(ParagraphID, DocID);
		}

		public void Execute()
		{
			var dbServer = new SagaDBServer();
			ParagraphID = dbServer.SaveParagraph(pMap);
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

﻿using System;
using System.Collections.Generic;
using TMorph.Schema;
using Schemas;
using FlatBuffers;

namespace dbMQserver.Commands
{
	class CommandSaveParagraph: ICommand
	{
		private ComType command = ComType.SaveLex;
		private List<SentenceMap> sentlist = new List<SentenceMap>();
		private long ParagraphID;

		public CommandSaveParagraph(long ParagraphID, List<SentenceMap> sentlist)
		{
			this.ParagraphID = ParagraphID;
			this.sentlist.AddRange(sentlist);
		}

		public void Execute(SQLiteConnector dbConnector)
		{
			ParagraphID = dbConnector.SaveParagraph(ParagraphID, sentlist);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var param1Name = builder.CreateString("result");
			var param1Val = builder.CreateString(ParagraphID.ToString());
			var parms = new Offset<Param>[1];
			parms[0] = Param.CreateParam(builder, param1Name, param1Val);
			var paracol = Message.CreateParamsVector(builder, parms);

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mReplay);
			Message.AddServerType(builder, ServType.svSUBD);
			Message.AddComtype(builder, command);
			Message.AddParams(builder, paracol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);
			return builder.SizedByteArray();
		}
	}
}
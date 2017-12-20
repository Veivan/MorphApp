using System;
using System.Collections.Generic;
using Schemas;
using TMorph.Schema;
using FlatBuffers;

namespace MorphMQserver.Commands
{
	class CommandSyntAn : ICommand
	{
		private ComType command = ComType.Synt;
		private string param;
		private SentenceMap sentence;

		public CommandSyntAn(string param)
		{
			this.param = param;
		}

		public void Execute(GrenHelper gren)
		{
			sentence = gren.GetSynInfoMap(param);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			// Синтаксический анализ - выполняется для одного предложения
			VectorOffset sentscol = SentenceMap.BuildSentOffsetFromSentStructList(builder, sentence);
			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mReplay);
			Message.AddServerType(builder, ServType.svMorph);
			Message.AddComtype(builder, command);
			Message.AddSentences(builder, sentscol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);
			return builder.SizedByteArray();
		}

	}
}

using System;
using System.Collections.Generic;
using TMorph.Schema;
using FlatBuffers;

namespace MorphMQserver.Commands
{
	class CommandSepar : ICommand
	{
		private ComType command = ComType.Separ;
		private string param;
		private List<string> separated;

		public CommandSepar(string param)
		{
			this.param = param;
		}

		public void Execute(GrenHelper gren)
		{
			separated = gren.SeparateIt(param);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var sents = new Offset<Sentence>[separated.Count];
			for (short i = 0; i < separated.Count; i++)
			{
				var sentVal = builder.CreateString(separated[i]);
				sents[i] = Sentence.CreateSentence(builder, i, default(VectorOffset), default(VectorOffset), sentVal);
			}
			VectorOffset sentscol = Message.CreateSentencesVector(builder, sents);
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

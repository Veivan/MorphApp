using System;
using System.Collections.Generic;
using Schemas;
using TMorph.Schema;
using FlatBuffers;

namespace MorphMQserver.Commands
{
	class CommandRepar: ICommand
	{
		private ComType command = ComType.Repar;
		private SentenceMap sentence;
		private string restored;

		public CommandRepar(SentenceMap sentence)
		{
			this.sentence = sentence;
		}

		public void Execute(GrenHelper gren)
		{
			restored = gren.RestoreSentenceOnePass(sentence);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var sents = new Offset<Sentence>[1];
			var sentVal = builder.CreateString(restored);
			sents[0] = Sentence.CreateSentence(builder, 0, default(VectorOffset), default(VectorOffset), sentVal);
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

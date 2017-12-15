using System;
using System.Collections.Generic;

using TMorph.Schema;
using TMorph.Common;
using Schemas;
using ZeroMQ;
using FlatBuffers;

namespace MorphMQserver
{
    class MorphServer
    {
        ComType command = ComType.Undef;
		GrenHelper gren = new GrenHelper();
        
        // Возвращаемый значения GrenHelper
        List<string> separated;
        SentenceMap sentence;

		public MorphServer()
		{
		}

        public void Run()
        {
			gren.Init();
			Console.WriteLine(" Dictionary vers : {0} ", gren.GetDictVersion());
			
			// Create
            // using (var context = new ZContext())
            using (var responder = new ZSocket(ZSocketType.REP))
            {
                // Bind
                responder.Bind("tcp://*:5559");

                while (true)
                {
                    // Receive
                    using (ZFrame request = responder.ReceiveFrame())
                    {
                        request.Position = 0;
                        var buf = request.Read();

                        PrintReq(buf);
                        var req = GetReq(buf);
                        //var req = request.ReadString();
                        Console.WriteLine("Received {0}", req);
                        var resp = "";

                        // Do some work
                        switch (command)
                        {
                            case ComType.Separ:
                                Console.WriteLine("ComType.Separ");
                                separated = gren.SeparateIt(req);
                                break;
                            case ComType.Synt:
                                Console.WriteLine("ComType.Synt");
                                sentence = gren.GetSynInfoMap(req);
                                break;
                            case ComType.Morph:
                                Console.WriteLine("ComType.Morph");
                                //resp = req + " " + "ComType.Morph";
                                resp = gren.GetMorphInfo(req);
                                break;
                            default:
                                break;
                        }

                        // Send
                        var builder = SetRep(command);
						var foo = builder.SizedByteArray();

                        responder.Send(new ZFrame(foo));
                    }
                }
            }
        }

        private void PrintReq(byte[] req)
        {
            var buf = new ByteBuffer(req);
            var message = Message.GetRootAsMessage(buf);
            Console.WriteLine(" ServerType : {0}", message.ServerType.ToString());
            Console.WriteLine(" Comtype : {0}", message.Comtype.ToString());

            this.command = message.Comtype;

            for (int i = 0; i < message.ParamsLength; i++)
            {
                Param? par = message.Params(i);
                if (par == null) continue;
                Console.WriteLine(" Param : {0} = {1}", par.Value.Name, par.Value.Value);
            }
        }

        /// <summary>
        /// Чтение реквеста.
        /// </summary>
        private string GetReq(byte[] req)
        {
            var result = "";
            var buf = new ByteBuffer(req);
            var message = Message.GetRootAsMessage(buf);
            Param? par = message.Params(0);
            if (par.HasValue)
				//result = String.Format(" Param : {0} = {1}", par.Value.Name, par.Value.Value);
				result = par.Value.Value;
			return result;
        }

        /// <summary>
        /// Формирование реплая.
        /// </summary>
        private FlatBufferBuilder SetRep(ComType command)
        {
            var builder = new FlatBufferBuilder(1);
            VectorOffset sentscol = default(VectorOffset);

            switch (command)
            {
                case ComType.Synt:
                    {
                        #region Синтаксический анализ - выполняется для одного предложения
                        var sents = new Offset<Sentence>[1];

                        // Чтение слов
                        var words = new Offset<Lexema>[sentence.Capasity];
                        for (short i = 0; i < sentence.Capasity; i++)
                        {
                            var word = sentence.GetWordByOrder(i);
                            var EntryName = builder.CreateString(word.EntryName);

                            // Чтение граммем
                            var pairs = word.GetPairs();
                            var grammems = new Offset<Grammema>[pairs.Count];
                            short j = 0;
                            foreach (var pair in pairs)
                            { 
                                grammems[j] = Grammema.CreateGrammema(builder, (short)pair.Key, (short)pair.Value);
								j++;
                            }
                            var gramsCol = Lexema.CreateGrammemsVector(builder, grammems);
                            words[i] = Lexema.CreateLexema(builder, i, EntryName, word.ID_Entry, (short)word.ID_PartOfSpeech, gramsCol);
                        }
                        var wordsCol = Sentence.CreateWordsVector(builder, words);

                        var sentVal = builder.CreateString("");
                        sents[0] = Sentence.CreateSentence(builder, 0, default(VectorOffset), wordsCol, sentVal);
                        sentscol = Message.CreateSentencesVector(builder, sents);
                        break;
                        #endregion
                    }
                case ComType.Separ:
                    {
                        var sents = new Offset<Sentence>[separated.Count];
                        for (short i = 0; i < separated.Count; i++)
                        {
                            var sentVal = builder.CreateString(separated[i]);
                            sents[i] = Sentence.CreateSentence(builder, i, default(VectorOffset), default(VectorOffset), sentVal);
                        }
                        sentscol = Message.CreateSentencesVector(builder, sents);
                        break;
                    }
                case ComType.Morph:
                    break;
                default:
                    break;
            }

            Message.StartMessage(builder);
            Message.AddMessType(builder, MessType.mReplay);
            Message.AddServerType(builder, ServType.svMorph);
            Message.AddComtype(builder, command);

            switch (command)
            {
                case ComType.Synt:
                    Message.AddSentences(builder, sentscol);
                    break;
                case ComType.Separ:
                    Message.AddSentences(builder, sentscol);
                    break;
                default:
                    break;
                case ComType.Morph:
                    break;
            }

            var req = Message.EndMessage(builder);
            builder.Finish(req.Value);

            return builder;
        }


    }
}

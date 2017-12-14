﻿using System;
using System.Collections.Generic;

using TMorph.Schema;
using TMorph.Common;
using ZeroMQ;
using FlatBuffers;

namespace MorphMQserver
{
    class MorphServer
    {
        ComType command = ComType.Undef;
		GrenHelper gren = new GrenHelper();
        List<string> separated;

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
                            case ComType.Morph:
                                Console.WriteLine("ComType.Morph");
                                //resp = req + " " + "ComType.Morph";
								resp = gren.GetMorphInfo(req);
                                break;
                            case ComType.Synt:
                                Console.WriteLine("ComType.Synt");
                                resp = gren.GetSynInfo(req);
                                //resp = req + " " + "ComType.Synt";
                                break;
                            case ComType.Separ:
                                Console.WriteLine("ComType.Separ");
                                separated = gren.SeparateIt(req);
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

        private FlatBufferBuilder SetRep(ComType command)
        {
            var builder = new FlatBufferBuilder(1);
            VectorOffset sentscol = default(VectorOffset);

            switch (command)
            {
                case ComType.Morph:
                    break;
                case ComType.Synt:
                    break;
                case ComType.Separ:
                    var sents = new Offset<Sentence>[separated.Count];
                    for (short i = 0; i < separated.Count; i++)
                    {
                        var sentVal = builder.CreateString(separated[i]);
                        sents[i] = Sentence.CreateSentence(builder, i, default(VectorOffset), default(VectorOffset), sentVal);
                    }
                    sentscol = Message.CreateSentencesVector(builder, sents); 
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
                case ComType.Morph:
                    break;
                case ComType.Synt:
                    break;
                case ComType.Separ:
                    Message.AddSentences(builder, sentscol);
                    break;
                default:
                    break;
            }

            var req = Message.EndMessage(builder);
            builder.Finish(req.Value);

            return builder;
        }


    }
}

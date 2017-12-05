using System;

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

		public MorphServer()
		{
		}

        public void Run()
        {
			//string dict = @"D:\Work\Framework\RussianGrammaticalDictionary64\bin-windows64\dictionary.xml";
			string dict = @"C:\Work\Framework\GrammarEngine\src\bin-windows64\dictionary.xml";
			gren.Init(dict);
			Console.WriteLine(" Dictionary vers : {0} ", gren.GetDictVersion());
			
			// Create
            // using (var context = new ZContext())
            using (var responder = new ZSocket(ZSocketType.REP))
            {
                // Bind
                responder.Connect("tcp://127.0.0.1:5560");

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
                            default:
                                break;
                        }

                        // Send
                        var builder = SetRep(resp);
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

        private FlatBufferBuilder SetRep(string resultValue)
        {
            var builder = new FlatBufferBuilder(1);
            var param1Name = builder.CreateString("result");
            var param1Val = builder.CreateString(resultValue);
            var parms = new Offset<Param>[1];
            parms[0] = Param.CreateParam(builder, param1Name, param1Val);
            var paracol = Message.CreateParamsVector(builder, parms);

            Message.StartMessage(builder);
            Message.AddMessType(builder, MessType.mReplay);
            Message.AddServerType(builder, ServType.svMorph);
            Message.AddComtype(builder, ComType.Token);
            Message.AddParams(builder, paracol);
            var req = Message.EndMessage(builder);
            builder.Finish(req.Value);

            return builder;
        }


    }
}

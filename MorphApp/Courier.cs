using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroMQ;
using FlatBuffers;
using TMorph.Schema;
using TMorph.Common;

namespace MorphApp
{
    class Courier
    {
        public ComType command;

        public string sendit(string requestText)
        {
            string replay = "";
            using (var requester = new ZSocket(ZSocketType.REQ))
            {
                // Connect
                requester.Connect("tcp://127.0.0.1:5559");

                if (requestText != "")
                {
                    var builder = SetReq(requestText);
                    var buf = builder.DataBuffer;
					var foo = builder.SizedByteArray();
                    
                    // Send
                    requester.Send(new ZFrame(foo));
                    //requester.Send(new ZFrame(command + " " + requestText));
                    // Receive
                    using (ZFrame reply = requester.ReceiveFrame())
                    {
                        reply.Position = 0;
                        var bufrep = reply.Read();
                        //replay = reply.ReadString();
                        //PrintRep(bufrep);
                        replay = GetRep(bufrep);
                    }
                }
            }

            return replay;
        }

        private FlatBufferBuilder SetReq(string requestText)
        {
            var builder = new FlatBufferBuilder(1);
            var param1Name = builder.CreateString("phrase");
            var param1Val = builder.CreateString(requestText);
            var parms = new Offset<Param>[1];
            parms[0] = Param.CreateParam(builder, param1Name, param1Val);
            var paracol = Message.CreateParamsVector(builder, parms);

            Message.StartMessage(builder);
            Message.AddMessType(builder, MessType.mRequest);
            Message.AddServerType(builder, ServType.svSUBD);
            Message.AddComtype(builder, this.command);
            Message.AddParams(builder, paracol);
            var req = Message.EndMessage(builder);
            builder.Finish(req.Value);

            return builder;
        }

        private void PrintRep(byte[] req)
        {
            var buf = new ByteBuffer(req);
            var message = Message.GetRootAsMessage(buf);
            Console.WriteLine(" ServerType : {0}", message.ServerType.ToString());
            Console.WriteLine(" Comtype : {0}", message.Comtype.ToString());

            for (int i = 0; i < message.ParamsLength; i++)
            {
                Param? par = message.Params(i);
                if (par == null) continue;
                Console.WriteLine(" Param : {0} = {1}", par.Value.Name, par.Value.Value);
            }
        }

        private string GetRep(byte[] req)
        {
            var result = "";
            var buf = new ByteBuffer(req);
            var message = Message.GetRootAsMessage(buf);
            Param? par = message.Params(0);
            if (par.HasValue)
                result = String.Format(" Param : {0} = {1}", par.Value.Name, par.Value.Value);
            return result;
        }

    }
}

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
    public struct SimpleParam
    {
        public string Name;
        public string Value;
    }

	class Courier
	{
		public ComType command;
		public ServType servType;

		private ZFrame replay = null;

		public void sendit(string requestText)
		{
			var builder = SetReq(requestText);
			var buf = builder.SizedByteArray();
			ZFrame replay = SendMess(new ZFrame(buf));
		}

        /// <summary>
        /// Получение списка параметров из сообщения.
        /// </summary>
        public List<SimpleParam> GetParamsList()
		{
			if (replay == null) return null;
			var paramlist = new List<SimpleParam>();
			replay.Position = 0;
			var bufrep = replay.Read();
			var buf = new ByteBuffer(bufrep);
			var message = Message.GetRootAsMessage(buf);
			for (int i = 0; i < message.ParamsLength; i++)
			{
				var par = message.Params(i);
				if (par.HasValue) 
				{
					var spar = new SimpleParam();
					spar.Name = par.Value.Name;
					spar.Value = par.Value.Value;
					paramlist.Add(spar);
				}		
			}
			return paramlist;
		}

        /// <summary>
        /// Получение списка текстов предложений из сообщения.
        /// </summary>
        public List<string> GetSeparatedSentsList()
        {
            if (replay == null) return null;
            var outlist = new List<string>();
            replay.Position = 0;
            var bufrep = replay.Read();
            var buf = new ByteBuffer(bufrep);
            var message = Message.GetRootAsMessage(buf);
            for (int i = 0; i < message.SentencesLength; i++)
            {
                var sent = message.Sentences(i);
                if (sent.HasValue)
                {
                    var ss = sent.Value;
                    outlist.Add(ss.Phrase);
                }
            }
            return outlist;
        }

        /// <summary>
        /// Получение структуры предложения из сообщения.
        /// </summary>
        public List<string> GetSentenceStructList()
        {
            // TODO реализовать
            if (replay == null) return null;
            var outlist = new List<string>();
            replay.Position = 0;
            var bufrep = replay.Read();
            var buf = new ByteBuffer(bufrep);
            var message = Message.GetRootAsMessage(buf);
            for (int i = 0; i < message.SentencesLength; i++)
            {
                var sent = message.Sentences(i);
                if (sent.HasValue)
                {
                    var ss = sent.Value;
                    outlist.Add(ss.Phrase);
                }
            }
            return outlist;
        }

        private ZFrame SendMess(ZFrame frame)
		{
			replay = null;
			ZError error;
			ZMessage msg = null;

			using (var requesterMorph = new ZSocket(ZSocketType.REQ))
			using (var requesterDB = new ZSocket(ZSocketType.REQ))
			{
				// Connect
				requesterMorph.Connect("tcp://127.0.0.1:5559");
				requesterDB.Connect("tcp://127.0.0.1:5560");

				var poll = ZPollItem.CreateReceiver();

				// Send
				switch (this.servType)
				{
					case ServType.svMorph:
						//requesterMorph.Send(new ZFrame(foo));
						requesterMorph.Send(frame);
						break;
					case ServType.svSUBD:
						requesterDB.Send(frame);
						break;
				}

				// Process messages from both sockets
				if (requesterMorph.PollIn(poll, out msg, out error, TimeSpan.FromMilliseconds(1000)))
				//if (requesterMorph.PollIn(poll, out msg, out error))
				{
					// Process task
					replay = msg[0];
				}
				else
				{
					if (error == ZError.ETERM)
						return replay;    // Interrupted
					if (error != ZError.EAGAIN)
						throw new ZException(error);
				}

				if (requesterDB.PollIn(poll, out msg, out error, TimeSpan.FromMilliseconds(64)))
				{
					// Process task
					replay = msg[0];
				}
				else
				{
					if (error == ZError.ETERM)
						return replay;    // Interrupted
					if (error != ZError.EAGAIN)
						throw new ZException(error);
				}

				/*/ Receive
				using (ZFrame reply = requesterMorph.ReceiveFrame())
				{
					reply.Position = 0;
					var bufrep = reply.Read();
					//PrintRep(bufrep);
					replay = GetRep(bufrep);
				}*/


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
			Message.AddServerType(builder, this.servType);
			Message.AddComtype(builder, this.command);
			Message.AddParams(builder, paracol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);

			return builder;
		}
/*
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
*/
	}
}

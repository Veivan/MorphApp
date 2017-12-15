﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ZeroMQ;
using FlatBuffers;
using TMorph.Schema;
using TMorph.Common;
using Schemas;

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

		private SentenceMap sentstr;

		private ZFrame replay = null;

		public void SendText(string requestText)
		{
			var builder = SetReq(requestText);
			var buf = builder.SizedByteArray();
			ZFrame replay = SendMess(new ZFrame(buf));
		}

		public void SendStruct(SentenceMap sentstr)
		{
			this.sentstr = sentstr;
			var builder = SetReq("");
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
					outlist.Add(sent.Value.Phrase);
			}
			return outlist;
		}

		/// <summary>
		/// Получение структуры предложения из сообщения.
		/// </summary>
		public SentenceMap GetSentenceStruct()
		{
			if (replay == null)
				return null;
			replay.Position = 0;
			var bufrep = replay.Read();
			var buf = new ByteBuffer(bufrep);
			var message = Message.GetRootAsMessage(buf);
			return SentenceMap.BuildFromMessage(message);
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
			VectorOffset paracol = default(VectorOffset);
			VectorOffset sentscol = default(VectorOffset);

			switch (command)
			{
				case ComType.Separ:
				case ComType.Synt:
					{

						var param1Name = builder.CreateString("phrase");
						var param1Val = builder.CreateString(requestText);
						var parms = new Offset<Param>[1];
						parms[0] = Param.CreateParam(builder, param1Name, param1Val);
						paracol = Message.CreateParamsVector(builder, parms);
						break;
					}
				case ComType.Repar:
					{
						// Синтаксический анализ - выполняется для одного предложения
						sentscol = SentenceMap.BuildSentOffsetFromMessage(builder, this.sentstr);
						break;
					}
			}

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mRequest);
			Message.AddServerType(builder, this.servType);
			Message.AddComtype(builder, this.command);
			Message.AddParams(builder, paracol);
			Message.AddSentences(builder, sentscol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);

			return builder;
		}
	}
}

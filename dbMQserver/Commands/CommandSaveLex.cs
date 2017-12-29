using System;
using System.Collections.Generic;
using TMorph.Schema;
using FlatBuffers;
using DirectDBconnector;

namespace dbMQserver.Commands
{
	class CommandSaveLex: ICommand
	{
		private ComType command = ComType.SaveLex;
		private string param;
		private long lexID;

		public CommandSaveLex(string param)
		{
			this.param = param;
		}

        public void Execute()
        {
            var dbServer = new SagaDBServer();
            //lexID = dbServer.SaveLex(param);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var param1Name = builder.CreateString("result");
			var param1Val = builder.CreateString(lexID.ToString());
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

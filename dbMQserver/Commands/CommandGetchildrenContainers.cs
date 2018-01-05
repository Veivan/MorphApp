using System;
using System.Collections.Generic;
using TMorph.Schema;
using Schemas;
using FlatBuffers;
using DirectDBconnector;


namespace dbMQserver.Commands
{
	class CommandGetchildrenContainers : ICommand
	{
		private ComType command = ComType.ReadPara;
		private List<ContainerMap> reslist = new List<ContainerMap>();
		private long ParentID;

		public CommandGetchildrenContainers(long ParentID)
		{
			this.ParentID = ParentID;
		}

		public void Execute()
		{
			var dbServer = new SagaDBServer();
			var retval = dbServer.GetChildrenContainers(ParentID, tpList.tblList);
			foreach (var item in retval.list)
				this.reslist.Add((ContainerMap)item);
		}

		public byte[] GetResultBytes()
		{
			var builder = new FlatBufferBuilder(1);
			var paramName = builder.CreateString("ParagraphID"); // TODO здесь надо передавать результат выполнения операции
			var paramVal = builder.CreateString(ParentID.ToString());
			var parms = new Offset<Param>[1];
			parms[0] = Param.CreateParam(builder, paramName, paramVal);
			var paracol = Message.CreateParamsVector(builder, parms);
			VectorOffset contscol = default(VectorOffset);

			contscol = ContainerMap.BuildOffsetFromStructList(builder, this.reslist);

			Message.StartMessage(builder);
			Message.AddMessType(builder, MessType.mReplay);
			Message.AddServerType(builder, ServType.svSUBD);
			Message.AddComtype(builder, command);
			Message.AddParams(builder, paracol);
			Message.AddContainers(builder, contscol);
			var req = Message.EndMessage(builder);
			builder.Finish(req.Value);
			return builder.SizedByteArray();
		}
	}
}

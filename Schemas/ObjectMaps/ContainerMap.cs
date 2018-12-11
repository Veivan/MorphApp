using System;
using System.Collections.Generic;
using TMorph.Schema;
using FlatBuffers;
using Schemas.BlockPlatform;

using BlockAddress = System.Int64;

namespace Schemas
{
	/// <summary>
	/// Класс хранит представление одного контейнера.
	/// </summary>
	public class ContainerMap : BlockBase
	{
		private string name = "";

		/// <summary>
		/// Идентификатор контейнера в БД.
		/// </summary>
		public long ContainerID { get { return this.BlockID; } }

		/// <summary>
		/// Наименование документа
		/// </summary>
		public string Name { get { return name; } set { name = value; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public ContainerMap(string _name, BlockAddress _b_id, BlockType _bt, BlockAddress _parent, long _order,
			BlockAddress _fh_id, BlockAddress _predecessor, BlockAddress _successor, DateTime? _created_at = null) : 
			base(_b_id, _bt, _parent, _order, _fh_id, _predecessor, _successor, null, _created_at)
		{
			name = _name;
		}

		public static List<ContainerMap> BuildFromMessage(Message message)
		{
			List<ContainerMap> reslist = new List<ContainerMap>();
			for (int i = 0; i < message.ContainersLength; i++)
			{
				ContainerMap resStruct = null;
				var cont = message.Containers(i);
				if (cont.HasValue)
				{
					var contval = cont.Value;
					var created_at = DateTime.Parse(contval.CreatedAt);

					var bt = Session.Instance().GetBlockTypeByNameKey(Session.containerTypeName);
					long _order = 0;
					BlockAddress _fh_id = -1;
					BlockAddress _predecessor = -1;
					BlockAddress _successor = -1;
					resStruct = new ContainerMap(contval.Name, contval.CtId, bt, contval.ParentId, _order,
						_fh_id, _predecessor, _successor, created_at);
					reslist.Add(resStruct);
				}
			}
			return reslist;
		}

		public static VectorOffset BuildOffsetFromStructList(FlatBufferBuilder builder, List<ContainerMap> list)
		{
			VectorOffset rescol = default(VectorOffset);
			var sents = new Offset<Container>[list.Count];
			for (int i = 0; i < list.Count; i++)
			{
				var Name = builder.CreateString(list[i].Name);
				var Created = builder.CreateString(list[i].Created.ToString());
				sents[i] = Container.CreateContainer(builder, list[i].ContainerID, Created, Name, list[i].ParentID);
			}
			rescol = Message.CreateContainersVector(builder, sents);
			return rescol;
		}
	}
}

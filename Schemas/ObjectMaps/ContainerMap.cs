using System;
using System.Collections.Generic;
using TMorph.Schema;
using FlatBuffers;

namespace Schemas
{
	/// <summary>
	/// Класс хранит представление одного контейнера.
	/// </summary>
	public class ContainerMap
	{
		private long _ct_id = -1;
		private DateTime _created_at;
		private string _name = "";
		private long _parent_id = -1;

		/// <summary>
		/// Идентификатор контейнера в БД.
		/// </summary>
		public long ContainerID { get { return _ct_id; } set { _ct_id = value; } }
		/// <summary>
		/// Дата создания.
		/// </summary>
		public DateTime Created { get { return _created_at; } set { _created_at = value; } }
		/// <summary>
		/// Наименование документа
		/// </summary>
		public string Name { get { return _name; } set { _name = value; } }
		/// <summary>
		/// Идентификатор родительского контейнера в БД.
		/// </summary>
		public long ParentID { get { return _parent_id; } set { _parent_id = value; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public ContainerMap(long ct_id, string name, DateTime? created_at, long parent_id)
        {
			_ct_id = ct_id;
			_parent_id = parent_id;
			_name = name;
			if (created_at == null)	
				_created_at = DateTime.Now;
			else
				_created_at = (DateTime)created_at;
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
					resStruct = new ContainerMap(contval.CtId, contval.Name, created_at, contval.ParentId);
					reslist.Add(resStruct);
				}
			}
			return reslist;
		}

		public static VectorOffset BuildOffsetFromStructList(FlatBufferBuilder builder, List<ContainerMap> list)
		{
			VectorOffset rescol = default(VectorOffset);
			var sents = new Offset<Container>[list.Count];
			for (short i = 0; i < list.Count; i++)
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

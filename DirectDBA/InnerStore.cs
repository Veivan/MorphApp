using System;
using System.Collections.Generic;
using System.Data;
using Schemas;

namespace DirectDBA
{
	public class ContainerNode
	{
		private ContainerMap cMap;
		private List<ContainerNode> children = new List<ContainerNode>();
		
		/// <summary>
		/// Конструктор
		/// </summary>
		public ContainerNode(ContainerMap cMap)
		{
			this.cMap = cMap;
		}

		public string Name
		{
			get
			{
				return cMap.Name;
			}
		}
		public long ContainerID
		{
			get
			{
				return cMap.ContainerID;
			}
		}
	}

	/// <summary>
	/// Класс описывает Хранилище данных SAGA.
	/// </summary>
	public class InnerStore
	{
		public List<ContainerNode> containers = new List<ContainerNode>();

		public void FillContainers(DataTable dTable)
		{
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var ct_id = dTable.Rows[i].Field<long>("ct_id");
				var parent_id = dTable.Rows[i].Field<long>("parent_id");
				var name = dTable.Rows[i].Field<string>("name");
				var created_at = dTable.Rows[i].Field<DateTime?>("created_at");

				var cMap = new ContainerMap(ct_id, name, created_at, parent_id);
				var cont = new ContainerNode(cMap);
				containers.Add(cont);
			}
		}
	}
}

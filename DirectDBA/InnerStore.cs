using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Schemas;

namespace DirectDBA
{
	/// <summary>
	/// Реализация IntfInnerStore.
	/// Класс описывает внутреннее хранилище данных клиента DirectDBA.
	/// </summary>
	public class InnerStore : IntfInnerStore
	{
		public override void FillContainers(ComplexValue list)
		{
			DataTable dTable = list.dtable;
			containers.Clear();
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

		public override void FillDocs(ComplexValue list)
		{
			DataTable dTable = list.dtable;
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var doc_id = dTable.Rows[i].Field<long>("doc_id");
				var ct_id = dTable.Rows[i].Field<long>("ct_id");
				var name = dTable.Rows[i].Field<string>("name");
				var created_at = dTable.Rows[i].Field<DateTime?>("created_at");

				var dMap = new DocumentMap(doc_id, ct_id, name, created_at);
				var cont = containers.Where(x => x.ContainerID == dMap.ContainerID).FirstOrDefault();
				cont.AddDocument(dMap);
			}
		}
	}
}

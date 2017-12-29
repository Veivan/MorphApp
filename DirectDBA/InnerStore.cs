using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Schemas;

namespace DirectDBA
{
	public class ContainerNode
	{
		private ContainerMap cMap;
		private List<ContainerNode> children = new List<ContainerNode>();
		private List<DocumentMap> Documents = new List<DocumentMap>();
		
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

		public void AddDocument(DocumentMap dMap)
		{
			this.Documents.Add(dMap);
		}

		public List<DocumentMap> GetDocuments()
		{
			var reslist = new List<DocumentMap>();
			reslist.AddRange(Documents);
			return reslist;
		}
	}

	/// <summary>
	/// Класс описывает Хранилище данных SAGA.
	/// </summary>
	public class InnerStore
	{
		public List<ContainerNode> containers = new List<ContainerNode>();

		/// <summary>
		/// Заполнение Хранилище данными о контейнерах.
		/// </summary>
		/// <param name="dTable">Набор данных</param>
		/// <returns></returns>
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

		/// <summary>
		/// Заполнение Хранилище данными о документах.
		/// </summary>
		/// <param name="dTable">Набор данных</param>
		/// <returns></returns>
		public void FillDocs(DataTable dTable)
		{
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
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
}

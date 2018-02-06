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
		private List<DocumentMap> documents = new List<DocumentMap>();

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
			this.documents.Add(dMap);
		}

        public void AddChild(ContainerNode cMap)
        {
            this.children.Add(cMap);
        }

        public List<DocumentMap> GetDocuments()
		{
            return documents;
		}

        public List<ContainerNode> Children()
        {
            return children;
        }

        /// <summary>
        /// Поиск документа в списке по его ID.
        /// </summary>
        /// <param name="DocumentID">ID документа</param>
        /// <returns>DocumentMap</returns>
        public DocumentMap GetDocumentByID(long DocumentID)
        {
            var result = documents.Where(x => x.DocumentID == DocumentID).FirstOrDefault();
            return result;
        }

        public void RemoveDocument(long doc_id)
        {
            var dMap = documents.Where(x => x.DocumentID == doc_id).FirstOrDefault();
            if (dMap != null)
                documents.Remove(dMap);
        }
    }
}

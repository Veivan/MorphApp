using System;
using System.Collections.Generic;
using Schemas;

namespace MorphApp
{
	class SagaStoreServer : IDataDealer
	{
		#region Унаследованные методы
		/// <summary>
		/// Получение плоского списка контейнеров
		/// в виде List
		/// </summary>
		public override RetValue ReadContainers()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Получение плоского списка документов
		/// в виде List
		/// </summary>
		public override RetValue ReadDocuments()
		{
			throw new NotImplementedException();
		}

		public override RetValue GetChildrenContainers(long parentID)
		{
			throw new NotImplementedException();
		}

		/*public override RetValue GetDocsInContainer(long ct_id)
		{
			throw new NotImplementedException();
		}*/

		public override RetValue GetDocsInContainerList(List<string> list_ids)
		{
			throw new NotImplementedException();
		}

		public override long SaveParagraph(long pg_id, List<SentenceMap> sentlist)
		{
			throw new NotImplementedException();
		}

		public override List<SentenceMap> ReadParagraphDB(long pg_id)
		{
			throw new NotImplementedException();
		}
		#endregion


	}
}

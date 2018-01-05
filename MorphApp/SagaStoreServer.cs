﻿using System;
using System.Collections.Generic;
using Schemas;

namespace MorphApp
{
	class SagaStoreServer : IDataDealer
	{
		Courier courier = new Courier();

		#region Унаследованные методы
		/// <summary>
		/// Получение плоского списка контейнеров
		/// в виде List
		/// </summary>
		public override ComplexValue ReadContainers()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Получение плоского списка документов
		/// в виде List
		/// </summary>
		public override ComplexValue ReadDocuments()
		{
			throw new NotImplementedException();
		}

		public override ComplexValue GetChildrenContainers(long parentID, tpList resulttype = tpList.tblList)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.GetChildrenConts;
			courier.SendID(parentID);
			var contlist = courier.GetContainerMapsList();
			var resval = new ComplexValue();
			resval.list.AddRange(contlist);
			return resval;
		}

		/*public override RetValue GetDocsInContainer(long ct_id)
		{
			throw new NotImplementedException();
		}*/

		public override ComplexValue GetDocsInContainerList(List<string> list_ids)
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

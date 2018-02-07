using System;
using System.Collections.Generic;
using Schemas;

namespace MorphApp
{
	class SagaStoreServer : IDataDealer
	{
		Courier courier = new Courier();

        #region Унаследованные методы

        public override ComplexValue GetChildrenInContainerList(tpList resulttype, List<string> list_ids)
        {
            long parentID = Convert.ToInt64(list_ids[0]);
            courier.servType = TMorph.Schema.ServType.svSUBD;
            courier.command = TMorph.Schema.ComType.GetChildrenConts;
            courier.SendID(parentID);
            var contlist = courier.GetContainerMapsList();
            var resval = new ComplexValue();
            resval.list.AddRange(contlist);
            return resval;
        }

		public override ComplexValue GetDocsInContainerList(List<string> list_ids)
		{
			throw new NotImplementedException();
		}

		public override ComplexValue ReadParagraphsInDocsList(tpList resulttype, List<string> list_ids = null)
		{
			throw new NotImplementedException();
		}

		public override ComplexValue ReadPhrasesInParagraphsList(tpList resulttype, List<string> list_ids = null)
		{
			throw new NotImplementedException();
		}

		public override long SaveParagraph(ParagraphMap pMap)
		{
			long ParagraphID = -1;
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.SavePara;
			courier.SendParagraph(pMap);
			var paramlist = courier.GetParamsList();
			if (paramlist == null)
			{
				foreach (var par in paramlist)
					if (par.Name == "ParagraphID")
					{
						ParagraphID = Convert.ToInt32(par.Value, 10);
						break;
					}
			}
			return ParagraphID;
		}

		public override List<SentenceMap> ReadParagraphDB(long pg_id)
		{
			courier.servType = TMorph.Schema.ServType.svSUBD;
			courier.command = TMorph.Schema.ComType.ReadPara;
			courier.SendParagraph(new ParagraphMap(pg_id));
			// Через параметры передать -1 в случае ошибки, либо ID параграфа
			var paramlist = courier.GetParamsList();
			var sentlist = courier.GetSentenceStructList();
			return sentlist;
		}

		public override void DelParagraphDB(long pg_id)
		{
			throw new NotImplementedException();
		}

		public override long SaveContainerBD(string name, long parentID = -1)
		{
			throw new NotImplementedException();
		}

        public override long SaveDocumentBD(string name, long ct_ID)
        {
            throw new NotImplementedException();
        }

        public override void DelDocumentDB(long doc_id)
        {
            throw new NotImplementedException();
        }

        public override void DelContainerDB(long c_id)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

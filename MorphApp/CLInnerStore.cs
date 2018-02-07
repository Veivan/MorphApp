using System;
using System.Collections.Generic;
using Schemas;

namespace MorphApp
{
	/// <summary>
	/// Реализация IntfInnerStore.
	/// Класс описывает внутреннее хранилище данных клиента MorphApp.
	/// Данные приходят из хранилища в виде списков.
	/// </summary>
	public class CLInnerStore : IntfInnerStore
	{
        Courier courier = new Courier();

        // Работа с БД через сервер сообщений
        SagaStoreServer dbServer = new SagaStoreServer();
        
        public override void FillChildren(ContainerNode parentCont, ComplexValue list)
        {
            var dTable = list.list;
            foreach (ContainerMap item in dTable)
            {
                var cont = new ContainerNode(item);
                parentCont.AddChild(cont);
            }
        }

        public override void FillDocs(ContainerNode cont, ComplexValue list)
        {
            throw new NotImplementedException();
        }

        public override void FillDocsParagraphs(ComplexValue list)
		{
			throw new NotImplementedException();
		}
        

        public override void Refresh()
        {
            throw new NotImplementedException();
        }

        public override DocumentMap RefreshParagraphs(long contID, long docID)
        {
            throw new NotImplementedException();
        }

        public override List<SimpleParam> SaveParagraphBD(ParagraphMap pMap)
        {
            courier.servType = TMorph.Schema.ServType.svSUBD;
            courier.command = TMorph.Schema.ComType.SavePara;
            courier.SendParagraph(pMap);
            var paramlist = courier.GetParamsList();
            return paramlist;
        }

        public override List<SentenceMap> ReadParagraphDB(long pg_id)
        {
            throw new NotImplementedException();
        }

        public override List<SimpleParam> GetLexema(string word)
        {
            throw new NotImplementedException();
        }

        public override List<SimpleParam> SaveLexema(string word)
        {
            throw new NotImplementedException();
        }

		public override List<SentenceMap> MorphMakeSyntan(string text)
		{
			throw new NotImplementedException();
		}

		public override List<string> MorphGetReparedSentsList(List<SentenceMap> sentlist)
		{
			throw new NotImplementedException();
		}

		public override List<string> MorphGetSeparatedSentsList(string text)
		{
			throw new NotImplementedException();
		}

		public override void DelParagraph(long ct_id, long doc_id, long pg_id)
		{
			throw new NotImplementedException();
		}

		public override List<SimpleParam> SaveContainerBD(string name, long parentID = -1)
		{
			throw new NotImplementedException();
		}

        public override List<SimpleParam> SaveDocumentBD(string name, long ct_ID)
        {
            throw new NotImplementedException();
        }

        public override void DelDocument(long ct_id, long doc_id)
        {
            throw new NotImplementedException();
        }

        public override void DelContainer(long ct_id)
        {
            throw new NotImplementedException();
        }

        public override void RefreshContainer(long contID)
        {
            throw new NotImplementedException();
        }
    }
}

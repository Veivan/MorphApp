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
        
        public override void FillContainers(ComplexValue list)
		{
			var dTable = list.list;
			containers.Clear();
			foreach (ContainerMap item in dTable)
			{
				var cont = new ContainerNode(item);
				containers.Add(cont);
			}
		}

		public override void FillDocs(ComplexValue list)
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
    }
}

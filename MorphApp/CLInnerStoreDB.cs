using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Schemas;
using DirectDBconnector;
using System.Text;

namespace MorphApp
{

    /// <summary>
	/// Реализация IntfInnerStore.
	/// Класс описывает внутреннее хранилище данных клиента MorphApp.
	/// Данные приходят из хранилища в виде DataTable - напрямую из БД.
	/// </summary>
	public class CLInnerStoreDB : IntfInnerStore
	{
        const int PUNCTUATION_class = 21;          // class ПУНКТУАТОР

        Courier courier = new Courier();
        
        // Работа с БД напрямую
        SagaDBServer dbServer = new SagaDBServer();

        #region Методы работы с БД
        /// <summary>
        /// Заполнение внутреннего хранилища.
        /// Узел основного хранилища заполняется данными о дочерних контейнерах,
        /// чтобы у узла возник крестик.
        /// </summary>
        public override void Refresh()
        {
            containers.Clear();

			long parent_id = -1;
			long _order = 0;
			long _fh_id = -1;
			long _predecessor = -1;
			long _successor = -1;
			var MainMap = new ContainerMap(Session.MainStoreName, Session.MainStoreID, null, parent_id, _order,
				_fh_id, _predecessor, _successor, DateTime.Now);

			var maincontainer = new ContainerNode(MainMap);
            containers.Add(maincontainer);

            var list_ids = new List<string>();
            list_ids.Add(Session.MainStoreID.ToString());
            var list = dbServer.GetChildrenInContainerList(tpList.tplDBtable, list_ids);
            this.FillChildren(maincontainer, list);
        }

        public override void RefreshContainer(long contID)
        {
            var container = GetContainerByID(contID);
            var list_ids = new List<string>();
            // Получение документов контейнера
            list_ids.Add(contID.ToString());
            var list = dbServer.GetDocsInContainerList(list_ids);
            this.FillDocs(container, list);

            list_ids.Clear();
            // Получение списка ID дочерних контейнеров
            foreach (var chld in container.Children())
            {
                var strID = chld.ContainerID.ToString();
                list_ids.Add(strID);
            }

            // Обновление дочерних контейнеров данными из БД
            list = dbServer.GetChildrenInContainerList(tpList.tplDBtable, list_ids);
            this.FillChildren(null, list);
            list = dbServer.GetDocsInContainerList(list_ids);
            this.FillDocs(null, list);
            list_ids.Clear();
            var dTable = list.dtable;
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                var strID = dTable.Rows[i].Field<long>("doc_id");
                list_ids.Add(strID.ToString());
            }
            list = dbServer.ReadParagraphsInDocsList(tpList.tplDBtable, list_ids);
            this.FillDocsParagraphs(list);
        }

        public override DocumentMap RefreshParagraphs(long contID, long docID)
        {
            var container = GetContainerByID(contID);
            var dMap = container.GetDocumentByID(docID);
            var parags = dMap.GetParagraphs();
            foreach (var paragraph in parags)
            {
                // Чтение данных о структурах предложений и заголовках абзаца из БД
                var sentlist = dbServer.ReadParagraphDB(paragraph.ParagraphID);
                courier.servType = TMorph.Schema.ServType.svMorph;
                courier.command = TMorph.Schema.ComType.Repar;
                foreach (var sentstruct in sentlist)
                {
					string phrase = "";
					if (sentstruct.Capasity > 0)
					{
                        /*/ Восстановление предложения обращением к GREN
                        courier.SendStruct(sentstruct);
						var sentlistRep = courier.GetSeparatedSentsList();
						if (sentlistRep != null && sentlistRep.Count > 0)
							phrase = sentlistRep[0]; */
                        // Восстановление предложения обращением к БД
                        phrase = RestorePhrase(sentstruct);
                    }
                    paragraph.RefreshSentProp(phrase, sentstruct, true);
                }
            }
            return dMap;
        }

        /// <summary>
        /// Восстановление предложения по формам слов, хранящимся в структурах WordMap
        /// </summary>
        private string RestorePhrase(SentenceMap sentstruct)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < sentstruct.Capasity; i++)
            {
                var wmap = sentstruct.GetWordByOrder(i);
                if (i > 0 && i < sentstruct.Capasity && wmap.ID_PartOfSpeech != PUNCTUATION_class)
                    sb.Append(" ");
                sb.Append(wmap.RealWord);
            }
            return sb.ToString();
        }

        public override void FillChildren(ContainerNode in_parentCont, ComplexValue list)
        {
            DataTable dTable = list.dtable;
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                var ct_id = dTable.Rows[i].Field<long>("ct_id");
                var parent_id = dTable.Rows[i].Field<long>("parent_id");
                var name = dTable.Rows[i].Field<string>("name");
                var created_at = dTable.Rows[i].Field<DateTime?>("created_at");

				var cMap = new ContainerMap(name, ct_id, null, parent_id, 0,
					-1, -1, -1, created_at);

				var cont = new ContainerNode(cMap);
                ContainerNode parentCont = in_parentCont;
                if (in_parentCont == null)
                    parentCont = GetContainerByID(parent_id);
                parentCont.AddChild(cont);
            }
		}

        public override void FillDocs(ContainerNode in_cont, ComplexValue list)
		{
			DataTable dTable = list.dtable;
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var doc_id = dTable.Rows[i].Field<long>("doc_id");
				var ct_id = dTable.Rows[i].Field<long>("ct_id");
				var name = dTable.Rows[i].Field<string>("name");
				var created_at = dTable.Rows[i].Field<DateTime?>("created_at");

				var dMap = new DocumentMap(doc_id, ct_id, name, created_at);
                ContainerNode cont = in_cont;
                if (in_cont == null)
                    cont = GetContainerByID(ct_id);
				cont.AddDocument(dMap);
			}
		}

        public override void FillDocsParagraphs(ComplexValue list)
		{
			DataTable dTable = list.dtable;
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var pg_id = dTable.Rows[i].Field<long>("pg_id");
				var doc_id = dTable.Rows[i].Field<long>("doc_id");
				var created_at = dTable.Rows[i].Field<DateTime?>("created_at");
				var ct_id = dTable.Rows[i].Field<long>("ct_id");

                var pMap = new ParagraphMap(pg_id, doc_id, created_at, ct_id);
                var cont = GetContainerByID(ct_id);
                var doc = cont.GetDocuments().Where(x => x.DocumentID == doc_id).FirstOrDefault();
                if (doc != null)
				    doc.AddParagraph(pMap);
			}
		}

		public override List<SimpleParam> SaveContainerBD(string name, long parentID = -1)
		{
			var ContainerID = dbServer.SaveContainerBD(name, parentID);
			var paramlist = new List<SimpleParam>();
			var param = new SimpleParam();
			param.Name = "ContainerID";
			param.Value = ContainerID.ToString();
			paramlist.Add(param);
			return paramlist;
		}

        public override List<SimpleParam> SaveDocumentBD(string name, long ct_ID)
        {
            var DocumentID = dbServer.SaveDocumentBD(name, ct_ID);
            var paramlist = new List<SimpleParam>();
            var param = new SimpleParam();
            param.Name = "DocumentID";
            param.Value = DocumentID.ToString();
            paramlist.Add(param);
            return paramlist;
        }

        public override List<SimpleParam> SaveParagraphBD(ParagraphMap pMap)
        {
			var ParagraphID = dbServer.SaveParagraph(pMap);
 			var paramlist = new List<SimpleParam>();
            var param = new SimpleParam();
            param.Name = "ParagraphID";
            param.Value = ParagraphID.ToString();
            paramlist.Add(param);
            return paramlist;
        }

        public override List<SentenceMap> ReadParagraphDB(long pg_id)
        {
            var ParagraphID = 1;
            var sentlist = dbServer.ReadParagraphDB(ParagraphID);
            return sentlist;
        }

		public override void DelParagraph(long ct_id, long doc_id, long pg_id)
		{
			dbServer.DelParagraphDB(pg_id);
            var cont = GetContainerByID(ct_id);
            var doc = cont.GetDocuments().Where(x => x.DocumentID == doc_id).FirstOrDefault();
			doc.RemoveParagraph(pg_id);
		}

        public override void DelDocument(long ct_id, long doc_id)
        {
            dbServer.DelDocumentDB(doc_id);
            var cont = GetContainerByID(ct_id);
            cont.RemoveDocument(doc_id);
        }

        public override void DelContainer(long ct_id)
        {
            dbServer.DelContainerDB(ct_id);
            var cont = GetContainerByID(ct_id);
            containers.Remove(cont);
        }

        public override List<SimpleParam> GetLexema(string word)
        {
            courier.servType = TMorph.Schema.ServType.svSUBD;
            courier.command = TMorph.Schema.ComType.GetWord;
            courier.SendText(word);
            var paramlist = courier.GetParamsList();
            return paramlist;
        }

        public override List<SimpleParam> SaveLexema(string word)
        {
            courier.servType = TMorph.Schema.ServType.svSUBD;
            courier.command = TMorph.Schema.ComType.SaveLex;
            courier.SendText(word);
            var paramlist = courier.GetParamsList();
            return paramlist;
        }

        #endregion
       
        #region Методы работы с GREN

		/// <summary>
		/// Выполнение синтана текста.
		/// </summary>
		public override List<SentenceMap> MorphMakeSyntan(string text)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			courier.SendText(text);
			var sentlistRep = courier.GetSentenceStructList();
			return sentlistRep;
		}

		/// <summary>
		/// Получение списка восстановленных текстов предложений от сервиса.
		/// </summary>
		public override List<string> MorphGetReparedSentsList(List<SentenceMap> sentlist)
		{
			var outlist = new List<string>();
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Repar;
			foreach (var sent in sentlist)
			{
				courier.SendStruct(sent);
				var sents = courier.GetSeparatedSentsList();
				outlist.AddRange(sents);
			}
			return outlist;
		}

		/// <summary>
		/// Разделение текста на предложения с помощью сервиса.
		/// </summary>
		public override List<string> MorphGetSeparatedSentsList(string text)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Separ;
			courier.SendText(text);
			var outlist = courier.GetSeparatedSentsList();
			return outlist;
		}
        #endregion

    } 
}

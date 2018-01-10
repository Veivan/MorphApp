﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Schemas;
using DirectDBconnector;

namespace MorphApp
{
 
    /// <summary>
	/// Реализация IntfInnerStore.
	/// Класс описывает внутреннее хранилище данных клиента MorphApp.
	/// Данные приходят из хранилища в виде DataTable - напрямую из БД.
	/// </summary>
	public class CLInnerStoreDB : IntfInnerStore
	{
        Courier courier = new Courier();
        
        // Работа с БД напрямую
        SagaDBServer dbServer = new SagaDBServer();

        /// <summary>
        /// Заполнение внутреннего хранилища.
        /// </summary>
        public override void Refresh()
        {
            var list = dbServer.GetChildrenContainers(Session.MainStroreID, tpList.tplDBtable);
            this.FillContainers(list);

            var dTable = list.dtable;
            var list_ids = new List<string>();
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                var strID = dTable.Rows[i].Field<long>("ct_id");
                list_ids.Add(strID.ToString());
            }
            list = dbServer.GetDocsInContainerList(list_ids);
            this.FillDocs(list);
            list_ids.Clear();
            dTable = list.dtable;
            for (int i = 0; i < dTable.Rows.Count; i++)
            {
                var strID = dTable.Rows[i].Field<long>("doc_id");
                list_ids.Add(strID.ToString());
            }
            list = dbServer.ReadParagraphsInDocsList(tpList.tplDBtable, list_ids);
            this.FillDocsParagraphs(list);


            /*var list = dbServer.GetChildrenContainers(Session.MainStroreID);
            store.FillContainers(list);

            var dTable = list.list;
            var list_ids = new List<string>();
            for (int i = 0; i < dTable.Count; i++)
            {
                var strID = (dTable[i] as ContainerMap).ContainerID;
                list_ids.Add(strID.ToString());
            }
            list = dbServer.GetDocsInContainerList(list_ids);
            store.FillDocs(list); 
            */
        }

        public override DocumentMap RefreshParagraphs(long contID, long docID)
        {
            var container = GetContainerByID(contID);
            var dMap = container.GetDocumentByID(docID);
            var parags = dMap.GetParagraphs();
            foreach (var paragraph in parags)
            {
                // Чтение данных о структурах предложений и заголовка абзаца из БД
                var sentlist = dbServer.ReadParagraphDB(paragraph.ParagraphID);
                courier.servType = TMorph.Schema.ServType.svMorph;
                courier.command = TMorph.Schema.ComType.Repar;
                int i = -1;
                foreach (var sentstruct in sentlist)
                {
                    courier.SendStruct(sentstruct);
                    var sentlistRep = courier.GetSeparatedSentsList();
                    string phrase = "";
                    if (sentlistRep != null && sentlistRep.Count > 0)
                        phrase = sentlistRep[0];
                    paragraph.RefreshSentProp(i, phrase, sentstruct, true);
                }
            }
            return dMap;
        }
        
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
				// TODO надо переделать поиск контейнеров. Этот вариант поднения двух уровней хранилища.
				var cont = containers.Where(x => x.ContainerID == ct_id).FirstOrDefault();
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
				var ph_id = dTable.Rows[i].Field<long>("ph_id");
				var created_at = dTable.Rows[i].Field<DateTime?>("created_at");
				var ct_id = dTable.Rows[i].Field<long>("ct_id");

				var pMap = new ParagraphMap(pg_id, doc_id, ph_id, created_at);
				var cont = containers.Where(x => x.ContainerID == ct_id).FirstOrDefault();
				var doc = cont.GetDocuments().Where(x => x.DocumentID == doc_id).FirstOrDefault();
				doc.AddParagraph(pMap);
			}
		}

    } 
}
﻿using System;
using System.Collections.Generic;
using System.Data;
using Schemas;

namespace DirectDBconnector
{
    public class SagaDBServer : IDataDealer
    {
        SQLiteConnector dbConnector = SQLiteConnector.Instance;

        #region Унаследованные методы

        public override ComplexValue GetChildrenInContainerList(tpList resulttype, List<string> list_ids)
        {
            ComplexValue rval = new ComplexValue();
            rval.dtable = dbConnector.GetChildrenInContainerList(list_ids);
            if (resulttype == tpList.tblList)
            {
                for (int i = 0; i < rval.dtable.Rows.Count; i++)
                {
                    var ct_id = rval.dtable.Rows[i].Field<long>("ct_id");
                    var parent_id = rval.dtable.Rows[i].Field<long>("parent_id");
                    var name = rval.dtable.Rows[i].Field<string>("name");
                    var created_at = rval.dtable.Rows[i].Field<DateTime?>("created_at");
                    if (created_at == null)
                        created_at = DateTime.Now;
					
					long _order = 0;
					long _fh_id = -1;
					long _predecessor = -1;
					long _successor = -1;
					var cMap = new ContainerMap(name, ct_id, null, parent_id, _order,
						_fh_id, _predecessor, _successor, created_at);

					rval.list.Add(new ContainerNode(cMap));
                }
            }
            return rval;
        }

        public override ComplexValue GetDocsInContainerList(List<string> list_ids)
        {
            ComplexValue rval = new ComplexValue();
            rval.dtable = dbConnector.GetDocsInContainerList(list_ids);
            return rval;
        }

        /// <summary>
        /// Получение плоского списка абзацев
        /// </summary>
        /// <param name="resulttype">тип возвращаемого результата</param>
        /// <param name="list_ids">Список ID документов</param>
        /// <returns>ComplexValue</returns>
        public override ComplexValue ReadParagraphsInDocsList(tpList resulttype, List<string> list_ids = null)
        {
            ComplexValue rval = new ComplexValue();
            if (resulttype == tpList.tplDBtable)
                rval.dtable = dbConnector.ReadParagraphsInDocs(list_ids);
            else
                rval.list.AddRange(dbConnector.ReadParagraphsInDocsList(list_ids));
            return rval;
        }

        public override ComplexValue ReadPhrasesInParagraphsList(tpList resulttype, List<string> list_ids = null)
        {
            ComplexValue rval = new ComplexValue();
            if (resulttype == tpList.tplDBtable)
                rval.dtable = dbConnector.ReadPhrasesInParagraphs(list_ids);
            else
                rval.list.AddRange(dbConnector.ReadPhrasesInParagraphsList(list_ids));
            return rval;
        }

        public override long SaveParagraph(ParagraphMap pMap)
        {
            var paraOper = new ParagraphOperator(pMap, pMap.ParagraphID == -1 ? OpersDB.odInsert : OpersDB.odUpdate);
            paraOper.Execute();
            return paraOper.ParagraphID;
        }

        public override List<SentenceMap> ReadParagraphDB(long pg_id)
        {
            var pMap = new ParagraphMap(pg_id);
            var paraOper = new ParagraphOperator(pMap, OpersDB.odSelect);
            paraOper.Execute();
            return paraOper.GetSentList();
        }

        public override void DelParagraphDB(long pg_id)
        {
            var pMap = new ParagraphMap(pg_id);
            var paraOper = new ParagraphOperator(pMap, OpersDB.odDelete);
            paraOper.Execute();
        }

        public override long SaveContainerBD(string name, long parentID = -1)
        {
            var id = dbConnector.InsertContainerDB(name, parentID);
            return id;
        }

        public override long SaveDocumentBD(string name, long ct_ID)
        {
            var id = dbConnector.InsertDocumentDB(name, ct_ID);
            return id;
        }

        public override void DelDocumentDB(long doc_id)
        {
            var list_ids = new List<string>();
            list_ids.Add(doc_id.ToString());

            var res = dbConnector.DeleteDocumentList(list_ids);
            if (res < 0)
            {
                throw new Exception(string.Format("Ошибка удаления документа ID = {0}", doc_id));
            }
        }

        public override void DelContainerDB(long c_id)
        {
            var res = dbConnector.DeleteContainer(c_id);
            if (res < 0)
            {
                throw new Exception(string.Format("Ошибка удаления контейнера ID = {0}", c_id));
            }
        }

        #endregion

        #region Собственные методы - для приложения DirectDBA
        /// <summary>
        /// Обновление таблицы в БД.
        /// </summary>
        /// <param name="dTable">набор данных</param>
        /// <param name="tblname">enum нужной таблицы</param>
        /// <returns></returns>
        public void UpdateDataTable(DataTable dTable, dbTables tblname)
        {
            dbConnector.dirCmd.UpdateDataTable(dTable, tblname);
        }

        /// <summary>
        /// Получение содержимого таблицы
        /// в виде DataTable
        /// </summary>
        public ComplexValue ReadDataTable(dbTables tblname)
        {
            ComplexValue rval = new ComplexValue();
            rval.dtable = dbConnector.dirCmd.GetDataTable(tblname);
            return rval;
        }
        #endregion

    }
}

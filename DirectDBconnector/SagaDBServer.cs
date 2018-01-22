using System;
using System.Collections.Generic;
using System.Data;
using Schemas;

namespace DirectDBconnector
{
    public class SagaDBServer : IDataDealer
    {
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

        #region Унаследованные методы

		/// <summary>
		/// Получение плоского списка контейнеров
		/// в виде DataTable
		/// </summary>
		/// <param name="resulttype">тип возвращаемого результата</param>
		/// <returns>ComplexValue</returns>
		public override ComplexValue GetChildrenContainers(long parentID, tpList resulttype)
		{
			ComplexValue rval = new ComplexValue();
			if (resulttype == tpList.tplDBtable)
				rval.dtable = dbConnector.GetChildrenContainers(parentID);
			else
				rval.list.AddRange(dbConnector.GetChildrenContainersList(parentID));
			return rval;
		}

		/*public override RetValue GetDocsInContainer(long ct_id)
		{
			RetValue rval = new RetValue();
			return rval;
		}
*/
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

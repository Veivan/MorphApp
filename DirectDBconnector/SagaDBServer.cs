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
		public override ComplexValue ReadContainers()
		{
			ComplexValue rval = new ComplexValue();
			rval.dtable = dbConnector.dirCmd.GetDataTable(dbTables.tblContainers);
			return rval;
		}

		/// <summary>
		/// Получение плоского списка документов
		/// в виде DataTable
		/// </summary>
		public override ComplexValue ReadDocuments()
		{
			ComplexValue rval = new ComplexValue();
			rval.dtable = dbConnector.dirCmd.GetDataTable(dbTables.tblDocuments);
			return rval;
		}

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
				rval.dtable = dbConnector.dirCmd.GetChildrenContainers(parentID);
			else
				rval.list.AddRange(dbConnector.dirCmd.GetChildrenContainersList(parentID));
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
			rval.dtable = dbConnector.dirCmd.GetDocsInContainerList(list_ids);
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
				rval.dtable = dbConnector.dirCmd.ReadParagraphsInDocs(list_ids);
			else
				rval.list.AddRange(dbConnector.dirCmd.ReadParagraphsInDocsList(list_ids));
			return rval;
		}

        public override ComplexValue ReadPhrasesInParagraphsList(tpList resulttype, List<string> list_ids = null)
        {
            ComplexValue rval = new ComplexValue();
            if (resulttype == tpList.tplDBtable)
                rval.dtable = dbConnector.dirCmd.ReadPhrasesInParagraphs(list_ids);
            else
                rval.list.AddRange(dbConnector.dirCmd.ReadPhrasesInParagraphsList(list_ids));
            return rval;
        }

        public override long SaveParagraph(long pg_id, long doc_id, List<SentenceMap> sentlist)
        {
            var paraOper = new ParagraphOperator(pg_id, doc_id, sentlist, pg_id == -1 ? OpersDB.odInsert : OpersDB.odUpdate);
            paraOper.Update();
			return paraOper.ParagraphID;
        }

		public override List<SentenceMap> ReadParagraphDB(long pg_id)
		{
			var paraOper = new ParagraphOperator(pg_id, -1, null, OpersDB.odSelect);
			paraOper.Read();
			return paraOper.GetSentList();
		}

		#endregion

	
		#region Собственные методы
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
		
		#endregion

    }
}

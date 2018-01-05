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
		/// <summary>
		/// Получение плоского списка контейнеров
		/// в виде DataTable
		/// </summary>
		public override RetValue ReadContainers()
		{
			RetValue rval = new RetValue();
			rval.dtable = dbConnector.dirCmd.GetDataTable(dbTables.tblContainers);
			return rval;
		}

		/// <summary>
		/// Получение плоского списка документов
		/// в виде DataTable
		/// </summary>
		public override RetValue ReadDocuments()
		{
			RetValue rval = new RetValue();
			rval.dtable = dbConnector.dirCmd.GetDataTable(dbTables.tblDocuments);
			return rval;
		}

		public override RetValue GetChildrenContainers(long parentID)
		{
			RetValue rval = new RetValue();
			rval.dtable = dbConnector.dirCmd.GetChildrenContainers(parentID);
			return rval;
		}

		/*public override RetValue GetDocsInContainer(long ct_id)
		{
			RetValue rval = new RetValue();
			return rval;
		}
*/
		public override RetValue GetDocsInContainerList(List<string> list_ids)
		{
			RetValue rval = new RetValue();
			rval.dtable = dbConnector.dirCmd.GetDocsInContainerList(list_ids);
			return rval;
		}

		public override long SaveParagraph(long pg_id, List<Schemas.SentenceMap> sentlist)
        {
            var paraOper = new ParagraphOperator(pg_id, sentlist, pg_id == -1 ? OpersDB.odInsert : OpersDB.odUpdate);
            paraOper.Update();
			return paraOper.ParagraphID;
        }

		public override List<Schemas.SentenceMap> ReadParagraphDB(long pg_id)
		{
			var paraOper = new ParagraphOperator(pg_id, null, OpersDB.odSelect);
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

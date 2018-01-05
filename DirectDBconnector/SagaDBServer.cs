using System;
using System.Collections.Generic;
using System.Data;
using Schemas;

namespace DirectDBconnector
{
    public class SagaDBServer : IDataDealer
    {
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

		/// <summary>
		/// Получение списка контейнеров
		/// в виде DataTable
		/// </summary>
		public override RetValue ReadContainers()
		{
			RetValue rval = new RetValue();
			rval.dtable = dbConnector.dirCmd.GetDataTable(dbTables.tblContainers);
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

	}
}

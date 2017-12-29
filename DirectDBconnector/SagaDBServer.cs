using System;
using System.Collections.Generic;
using Schemas;

namespace DirectDBconnector
{
    public class SagaDBServer : IDataDealer
    {
        public long SaveParagraph(long pg_id, List<Schemas.SentenceMap> sentlist)
        {
            var paraOper = new ParagraphOperator(pg_id, sentlist, pg_id == -1 ? OpersDB.odInsert : OpersDB.odUpdate);
            paraOper.Update();
			return paraOper.ParagraphID;
        }

		public List<Schemas.SentenceMap> ReadParagraphDB(long pg_id)
		{
			var paraOper = new ParagraphOperator(pg_id, null, OpersDB.odSelect);
			paraOper.Read();
			return paraOper.GetSentList();
		}
	}
}

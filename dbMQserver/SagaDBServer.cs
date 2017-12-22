using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbMQserver
{
    class SagaDBServer : IdbOperator
    {
        public long SaveParagraph(long pg_id, List<Schemas.SentenceMap> sentlist)
        {
            var paraOper = new ParagraphOperator(pg_id, sentlist);
            paraOper.Update();
            return pg_id;
        }
    }
}

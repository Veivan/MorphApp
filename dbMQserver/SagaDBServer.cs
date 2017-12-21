using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbMQserver
{
    class SagaDBServer : IdbConnector
    {
        public long SaveLex(string word)
        {
            throw new NotImplementedException();
        }

        public long GetWord(string rword)
        {
            throw new NotImplementedException();
        }

        public long SaveParagraph(long pg_id, List<Schemas.SentenceMap> sentlist)
        {
            var paraOper = new ParagraphOperator(pg_id, sentlist);
            paraOper.Update();
            return pg_id;
        }

        public long SavePhrase(long ph_id)
        {
            throw new NotImplementedException();
        }

        public long SavePhraseWords(long ph_id, long lx_id, short sorder)
        {
            throw new NotImplementedException();
        }
    }
}

using System;
using System.Collections.Generic;
using Schemas;
using System.Data.SQLite;

namespace dbMQserver
{
    class ParagraphOperator  
    {
        private List<SentenceMap> sentlist = new List<SentenceMap>();
        private long ParagraphID;

        private ParagraphDBIndicator indicator = new ParagraphDBIndicator();
        SQLiteConnector dbConnector = SQLiteConnector.Instance;

        public ParagraphOperator(long ParagraphID, List<SentenceMap> sentlist)
        {
            this.ParagraphID = ParagraphID;
            this.sentlist.AddRange(sentlist);
            indicator.Fill(ParagraphID);
        }

        internal void Update()
        {
            if (!indicator.CanOperate)
                return;
            switch (indicator.NeedOperate)
            {
                case OpersDB.odInsert:
                    {
                        ParagraphID = dbConnector.InsertParagraphDB();
                        break;
                    }
                case OpersDB.odUpdate:
                    {
                        ParagraphID = dbConnector.InsertParagraphDB();
                        break;
                    }
            }
            TruncateParaContent(sentlist.Count);
            ParagraphID = SaveParagraphDB(pg_id);
            foreach (var sent in sentlist)
            {
                var ph_id = SavePhrase(sent.SentenceID);
                for (short i = 0; i < sent.Capasity; i++)
                {
                    var word = sent.GetWordByOrder(0);
                    var lx_id = SaveLex(word.EntryName.ToLower());
                    SavePhraseWords(ph_id, lx_id, word.order);
                }
            }
        }


    }
}

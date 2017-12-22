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

        public void Update()
        {
            if (!indicator.CanOperate)
                return;
            switch (indicator.NeedOperate)
            {
                case OpersDB.odInsert:
                    {
                        ParagraphID = dbConnector.InsertParagraphDB();
						for (short k = 0; k < sentlist.Count; k++)
						{
							var sent = sentlist[k];
							var ph_id = dbConnector.InsertPhraseDB(ParagraphID, k);
							// Сохранение слов предложения в БД
							for (short i = 0; i < sent.Capasity; i++)
							{
								var word = sent.GetWordByOrder(i);
								var lx_id = dbConnector.SaveLex(word.EntryName.ToLower(), word.ID_PartOfSpeech);
								var c_id = dbConnector.InsertWordDB(ph_id, lx_id, word.order);
								// Сохранение граммем слова в БД
								var grammems = word.GetPairs();
								var keys = grammems.Keys;
								foreach (var key in keys)
  								{
									dbConnector.InsertGrammemDB(c_id, key, grammems[key]);									
								}
							}
							// Сохранение списка синтаксических связей предложения в БД
							var nodes = sent.GetTreeList();

						}
						break;
                    }
                case OpersDB.odUpdate:
                    {
                        ParagraphID = dbConnector.InsertParagraphDB();
                        break;
                    }
            }
            //TruncateParaContent(sentlist.Count);

        }


    }
}

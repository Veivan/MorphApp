using System;
using System.Collections.Generic;
using System.Linq;
using Schemas;
using System.Data.SQLite;

namespace dbMQserver
{
	class ParagraphOperator
	{
		private List<SentenceMap> sentlist = new List<SentenceMap>();
		private long paragraphID = -1;
		public long ParagraphID { get { return paragraphID; } }

		private ParagraphDBIndicator indicator = new ParagraphDBIndicator();
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

		public ParagraphOperator(long ParagraphID, List<SentenceMap> sentlist, OpersDB operDB)
		{
			this.paragraphID = ParagraphID;
			if (sentlist != null)
				this.sentlist.AddRange(sentlist);
			if (operDB != OpersDB.odSelect)
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
						paragraphID = dbConnector.InsertParagraphDB();
						for (short k = 0; k < sentlist.Count; k++)
						{
							var sent = sentlist[k];
							var ph_id = dbConnector.InsertPhraseDB(paragraphID, k);
                            var nodes = sent.GetTreeList();
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
                                // Сохранение списка синтаксических связей предложения в БД
                                InsertNode(nodes, word.order, c_id);
                            }
						}
						break;
					}
				case OpersDB.odUpdate:
					{
						paragraphID = dbConnector.InsertParagraphDB();
						break;
					}
			}
			//TruncateParaContent(sentlist.Count);

		}

        /// <summary>
        /// Определение узла в синтаксическом дереве предложения по порядковому номеру слова в предложении.
        /// Запись в mSyntNodes.
        /// </summary>
        /// <returns></returns>
        private void InsertNode(List<tNode> nodes, int order, long c_id)
        {
            var cnt = nodes.Where(x => x.index == order).Count();
            if (cnt > 0)
            {
                var node = nodes.Where(x => x.index == order).First();
                dbConnector.InsertSyntNodesDB(c_id, node.linktype, node.Level);
            }
        }

		public void Read()
		{
			if (!dbConnector.IsParagraphExists(paragraphID))
				return;
			// Чтение списка предложений
			var list_phID = dbConnector.ReadPhraseDB(paragraphID);
			foreach (var phID in list_phID)
			{
				var sent = new SentenceMap();
				sent.SentenceID = phID;
				// Чтение данных о словах
				var wstructs = dbConnector.ReadPhraseContentDB(phID);
				for (int i = 0; i < wstructs.Count; i++)
				{
					var wstr = wstructs[i];
					// В БД не хранится ID словарной статьи GREN. 
					var word = new WordMap(-1, wstr.sp_id);
					word.EntryName = wstr.lemma;
					// Чтение граммем слова
					var grammems = dbConnector.ReadGrammemsDB(wstr.с_id);
					foreach (var pair in grammems)
					{
						word.AddPair(pair.Key, pair.Value);
					}
					sent.AddWord(i, word);
				}
				// Чтение списка синтаксических связей предложения
                var nodelist = dbConnector.ReadSyntNodesDB(phID);
				for (short i = 0; i < nodelist.Count; i++)
				{
                    var node = nodelist[i];
                    sent.AddNode(i, node.Level, node.linktype);
                }
				sentlist.Add(sent);
			}
		}

		internal List<SentenceMap> GetSentList()
		{
			return sentlist;
		}
	}
}

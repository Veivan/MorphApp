using System;
using System.Collections.Generic;
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
						paragraphID = dbConnector.InsertParagraphDB();
						break;
					}
			}
			//TruncateParaContent(sentlist.Count);

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
				for (int j = 0; j < wstructs.Count; j++)
				{
					var wstr = wstructs[j];
					// В БД не хранится ID словарной статьи GREN. 
					var word = new WordMap(-1, wstr.sp_id);
					word.EntryName = wstr.lemma;
					// Чтение граммем слова
					var grammems = dbConnector.ReadGrammemsDB(wstr.с_id);
					foreach (var pair in grammems)
					{
						word.AddPair(pair.Key, pair.Value);
					}
					sent.AddWord(j, word);
				}
				// Чтение списка синтаксических связей предложения

				sentlist.Add(sent);
			}
		}

		internal List<SentenceMap> GetSentList()
		{
			return sentlist;
		}
	}
}

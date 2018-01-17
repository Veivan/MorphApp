﻿using System;
using System.Collections.Generic;
using System.Linq;
using Schemas;
using System.Data.SQLite;

namespace DirectDBconnector
{
	class ParagraphOperator
	{
		private ParagraphMap pMap;

		private List<long> sent2Del = new List<long>();
		private List<SentenceMap> sentlist = new List<SentenceMap>();
		private long paragraphID = -1;
		private long docID = -1;
		public long ParagraphID { get { return paragraphID; } }

		private OpersDB Operate;

		private ParagraphDBIndicator indicator = new ParagraphDBIndicator();
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

		public ParagraphOperator(ParagraphMap pMap, OpersDB operDB)
		{
			this.pMap = pMap;
			this.paragraphID = pMap.ParagraphID;
			this.docID = pMap.DocumentID;
			var listSmaps = pMap.GetParagraphSents().Select(x => x.sentstruct).OrderBy(x => x.Order).ToList();
			if (listSmaps != null)
                this.sentlist.AddRange(listSmaps);
			this.sent2Del = pMap.GetDeleted();
			if (operDB != OpersDB.odSelect)
				indicator.Fill(ParagraphID);

			this.Operate = operDB;
		}

		public void Execute()
		{
			switch (this.Operate)
			{
				case OpersDB.odSelect:
					Read();
					break;
				case OpersDB.odInsert:
					Insert();
					break;
				case OpersDB.odUpdate:
					Update();
					pMap.ClearDeleted();
					break;
				case OpersDB.odDelete:
					Delete();
					break;
			}
		}

		private void Delete()
		{
			if (!indicator.CanOperate) // TODO Доделать
				return;
			// Удаление абзаца 
			var list_ids = new List<string>(); 
			list_ids.Add(this.ParagraphID.ToString());
			var res = dbConnector.DeleteParagraphList(list_ids);
			if (res < 0)
			{
				throw new Exception(String.Format("Ошибка удаления абзаца ID = {0}", this.ParagraphID.ToString()));
			}
		}

		private void Insert()
		{
			paragraphID = dbConnector.InsertParagraphDB(this.docID);
			for (int k = 0; k < sentlist.Count; k++)
			{
				var sent = sentlist[k];
				var ph_id = dbConnector.InsertPhraseDB(paragraphID, sent.Order);
				var nodes = sent.GetTreeList();
				// Сохранение слов предложения в БД
				for (int i = 0; i < sent.Capasity; i++)
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
		}

		private void Update()
		{
			if (!indicator.CanOperate) // TODO Доделать
				return;

			var list_ids = new List<string>();
			IEnumerator<long> etr = sent2Del.GetEnumerator();
			while (etr.MoveNext())
				list_ids.Add(etr.Current.ToString());
			// Удаление предложений 
			var res = dbConnector.DeletePhrasesListTrans(list_ids);
			if (res < 0)
			{
				string strlist = string.Join(",", list_ids.ToArray());
				throw new Exception(String.Format("Сохранение абзаца.Ошибка удаления предложений с ID :{0}", strlist));
			}
			
			// TODO пока удаляю все предложения и пишу их заново

			list_ids.Clear();
			var list_ph = pMap.GetParagraphSentsIDs();
			etr = list_ph.GetEnumerator();
			while (etr.MoveNext())
				list_ids.Add(etr.Current.ToString());
			// Удаление действительных предложений
			res = dbConnector.DeletePhrasesListTrans(list_ids);
			if (res < 0)
			{
				string strlist = string.Join(",", list_ids.ToArray());
				throw new Exception(String.Format("Сохранение абзаца.Ошибка удаления предложений с ID :{0}", strlist));
			}

			for (int k = 0; k < sentlist.Count; k++)
			{
				var sent = sentlist[k];
				var ph_id = dbConnector.InsertPhraseDB(paragraphID, sent.Order);
				var nodes = sent.GetTreeList();
				// Сохранение слов предложения в БД
				for (int i = 0; i < sent.Capasity; i++)
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

		private void Read()
		{
			if (!dbConnector.IsParagraphExists(paragraphID))
				return;
			// Чтение списка предложений
			var sMapList = dbConnector.ReadPhraseDB(paragraphID);
			foreach (var sent in sMapList)
			{
				var phID = sent.SentenceID;
				// Чтение данных о словах
				var wstructs = dbConnector.ReadPhraseContentDB(phID);
				for (int i = 0; i < wstructs.Count; i++)
				{
					var wstr = wstructs[i];
					// В БД не хранится ID словарной статьи GREN. 
					var word = new WordMap(-1, wstr.sp_id);
					word.EntryName = wstr.lemma;
					word.order = i;
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
				for (int i = 0; i < nodelist.Count; i++)
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

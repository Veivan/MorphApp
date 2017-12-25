﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Data.SQLite;
using Schemas;

namespace dbMQserver
{
	public enum OpersDB { odNone, odSelect, odInsert, odUpdate, odDelete };

	public struct WordStruct
	{
		public int с_id;
		public string lemma;
		public int sp_id;
	}

	/// Singleton
	public sealed class SQLiteConnector
	{
		private static readonly Lazy<SQLiteConnector> instanceHolder =
			new Lazy<SQLiteConnector>(() => new SQLiteConnector());

		private String dbFileName = @"c:\temp\saga.sqlite";
		private SQLiteConnection m_dbConn;
		private SQLiteCommand m_sqlCmd = new SQLiteCommand();

		private SQLiteConnector()
		{
			if (!File.Exists(dbFileName))
				SQLiteConnection.CreateFile(dbFileName);

			try
			{
				m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
				m_dbConn.Open();
				m_sqlCmd.Connection = m_dbConn;
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mSpParts (\n"
						+ "	sp_id integer, speechpart text NOT NULL);";
				m_sqlCmd.ExecuteNonQuery();
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mSiGram (\n"
						+ "	sg_id integer, property text NOT NULL);";
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mLemms (\n"
						+ "	lx_id integer PRIMARY KEY, sp_id integer, lemma text NOT NULL);";
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mParagraphs (\n"
						+ "	pg_id integer PRIMARY KEY, created_at DATETIME DEFAULT CURRENT_TIMESTAMP);";
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mPhrases (\n"
						+ "	ph_id integer PRIMARY KEY, pg_id integer, sorder integer, \n"
						+ " created_at DATETIME DEFAULT CURRENT_TIMESTAMP);";
				m_sqlCmd.ExecuteNonQuery();
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mPhraseContent (\n"
						+ "	с_id integer PRIMARY KEY, ph_id integer, lx_id integer, sorder integer);";
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mGrammems (\n"
					+ "	gr_id integer PRIMARY KEY, с_id integer, sg_id integer, intval integer);";
				m_sqlCmd.ExecuteNonQuery();

			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}

		public static SQLiteConnector Instance
		{
			get { return instanceHolder.Value; }
		}

		/// <summary>
		/// Сохранение леммы в БД.
		/// sqlite некорректно работает с upper, lower и like в utf8. Поэтому в БД надо всё хранить в lower
		/// </summary>
		/// <param name="lemma">лемма</param>
		/// <param name="sp_id">ID части речи</param>
		/// <returns>ID леммы</returns>
		public long SaveLex(string lemma, int sp_id)
		{
			long result = GetWord(lemma, sp_id);
			if (result == -1)
			{
				try
				{
					m_sqlCmd.CommandText = "INSERT INTO mLemms(lx_id, sp_id, lemma) VALUES(NULL, @sp_id, @lemma)";
					m_sqlCmd.Parameters.Clear();
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@sp_id", sp_id));
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@lemma", lemma.ToLower()));
					m_sqlCmd.ExecuteNonQuery();

					m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
					m_sqlCmd.Parameters.Clear();
					result = (long)m_sqlCmd.ExecuteScalar();
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}
			return result;
		}

		/// <summary>
		/// Получение ID леммы из БД.
		/// </summary>
		/// <param name="lemma">лемма</param>
		/// <param name="sp_id">ID части речи</param>
		/// <returns>ID леммы</returns>
		public long GetWord(string lemma, int sp_id)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = "SELECT lx_id FROM mLemms WHERE lemma = @lemma AND sp_id = @sp_id";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@sp_id", sp_id));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@lemma", lemma.ToLower()));
				// Читаем только первую запись
				var resp = m_sqlCmd.ExecuteScalar();
				if (resp != null)
					result = (long)resp;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Проверка, существует в БД абзац pg_id или нет.
		/// </summary>
		/// <param name="pg_id">ID параграфа</param>
		/// <returns>boolean</returns>
		public bool IsParagraphExists(long pg_id)
		{
			var IsParaExists = false;
			if (pg_id != -1)
			{
				m_sqlCmd.CommandText = "SELECT COUNT(*) FROM mParagraphs WHERE pg_id = @pg_id";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@pg_id", pg_id));
				// Читаем только первую запись
				var resp = m_sqlCmd.ExecuteScalar();
				if (resp != null && (long)resp > 0)
					IsParaExists = true;
			}
			return IsParaExists;
		}

		/// <summary>
		/// Вставка в mParagraphs.
		/// </summary>
		/// <returns>ID параграфа</returns>
		public long InsertParagraphDB()
		{
			long pg_id = -1;
			try
			{
				// Нужно только создать ID
				m_sqlCmd.CommandText = "INSERT INTO mParagraphs(pg_id) VALUES(NULL)";
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
				pg_id = (long)m_sqlCmd.ExecuteScalar();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return pg_id;
		}

		/// <summary>
		/// Вставка в mPhrases.
		/// </summary>
		/// <returns>ID предложения</returns>
		public long InsertPhraseDB(long pg_id, short sorder)
		{
			long ph_id = -1;
			try
			{
				m_sqlCmd.CommandText =
					String.Format("INSERT INTO mPhrases(ph_id, pg_id, sorder) VALUES(NULL, {0}, {1})", pg_id, sorder);
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
				ph_id = (long)m_sqlCmd.ExecuteScalar();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return ph_id;
		}

		/// <summary>
		/// Вставка в mPhraseContent.
		/// </summary>
		/// <returns>ID</returns>
		public long InsertWordDB(long ph_id, long lx_id, short sorder)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = "INSERT INTO mPhraseContent(с_id, ph_id, lx_id, sorder) VALUES(NULL, @ph_id, @lx_id, @sorder)";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@ph_id", ph_id));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@lx_id", lx_id));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@sorder", sorder));
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
				result = (long)m_sqlCmd.ExecuteScalar();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Вставка в mGrammems.
		/// </summary>
		/// <returns>ID</returns>
		public long InsertGrammemDB(long с_id, int sg_id, int intval)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText =
					String.Format("INSERT INTO mGrammems(gr_id, с_id, sg_id, intval) VALUES(NULL, {0}, {1}, {2})",
					с_id, sg_id, intval);
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
				result = (long)m_sqlCmd.ExecuteScalar();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Чтение записей из mPhrases, относящихся к абзацу pg_id.
		/// </summary>
		/// <param name="pg_id">ID параграфа</param>
		/// <returns>Список ID предложений</returns>
		public List<long> ReadPhraseDB(long pg_id)
		{
			var listID = new List<long>();
			try
			{
				m_sqlCmd.CommandText = "SELECT ph_id FROM mPhrases WHERE pg_id = @pg_id ORDER BY sorder";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@pg_id", pg_id));

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					listID.Add((long)r["ph_id"]);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return listID;
		}				
		
		/// <summary>
		/// Чтение записей из mPhraseContent, относящихся к предложению ph_id.
		/// </summary>
		/// <param name="ph_id">ID предложения</param>
		/// <returns>Список данных о слове</returns>
		public List<WordStruct> ReadPhraseContentDB(long ph_id)
		{
			var reslist = new List<WordStruct>();
			try
			{
				m_sqlCmd.CommandText = "SELECT P.с_id, B.lemma, B.sp_id FROM mPhraseContent P JOIN mLemms B ON B.lx_id = P.lx_id " +
					"WHERE P.ph_id = @ph_id ORDER BY sorder";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@ph_id", ph_id));

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					var wstruct = new WordStruct();
					wstruct.с_id = r.GetInt32(0); 
					wstruct.lemma = r.GetString(1);
					wstruct.sp_id = r.GetInt32(2);
					reslist.Add(wstruct);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return reslist;
		}

		/// <summary>
		/// Чтение записей из mGrammems, относящихся к слову с_id.
		/// </summary>
		/// <param name="с_id">ID слова</param>
		/// <returns>Список данных о слове</returns>
		public List<KeyValuePair<int, int>> ReadGrammemsDB(long с_id)
		{
			var reslist = new List<KeyValuePair<int, int>>();
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT sg_id, intval FROM mGrammems WHERE с_id = {0}", с_id);

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					var pair = new KeyValuePair<int, int>(r.GetInt32(0), r.GetInt32(1));
					reslist.Add(pair);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
			return reslist;
		}

		/*// <summary>
		/// Удаление записей из mPhrases, относящихся к абзацу pg_id и порядок в предложении у которых больше maxcnt.
		/// </summary>
		private void TruncateParaContent(int maxcnt)
		{
			try
			{
				m_sqlCmd.CommandText = "SELECT ph_id FROM mPhrases WHERE pg_id = @pg_id AND sorder > @maxcnt";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@pg_id", ParagraphID));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@maxcnt", maxcnt - 1));

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					var ph_id = (long)r["ph_id"];
					TruncateWords(ph_id, maxcnt);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		} */

		private void DeleteWords(long ph_id)
		{
			m_sqlCmd.CommandText = "SELECT COUNT(*) FROM mParagraphs WHERE pg_id = @pg_id";
			m_sqlCmd.Parameters.Clear();
			m_sqlCmd.Parameters.Add(new SQLiteParameter("@pg_id", ph_id));
		}

		private void TruncateWords(long ph_id, int maxcnt)
		{
		}



		///
		/// select all rows in the mPhrases table
		/// 
		public void selectAll(string nTable)
		{
			try
			{
				switch (nTable)
				{
					case "mSpParts":
						m_sqlCmd.CommandText = "SELECT sp_id, speechpart FROM mSpParts";
						break;
					case "mSiGram":
						m_sqlCmd.CommandText = "SELECT sg_id, property FROM mSiGram";
						break;
					case "mParagraphs":
						m_sqlCmd.CommandText = "SELECT pg_id, created_at FROM mParagraphs";
						break;
					case "mPhrases":
						m_sqlCmd.CommandText = "SELECT ph_id, pg_id, sorder, created_at FROM mPhrases";
						break;
					case "mPhraseContent":
						m_sqlCmd.CommandText = "SELECT с_id, ph_id, lx_id, sorder FROM mPhraseContent";
						break;
				}
				//m_sqlCmd.CommandText = "SELECT  ph_id, w_id FROM ";

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					switch (nTable)
					{
						case "mSpParts":
							line = r["sp_id"].ToString() + ", " + r["speechpart"];
							break;
						case "mSiGram":
							line = r["sg_id"].ToString() + ", " + r["property"];
							break;
						case "mParagraphs":
							line = r["pg_id"].ToString() + ", " + r["created_at"].ToString();
							break;
						case "mPhrases":
							line = r["ph_id"].ToString() + ", " + r["pg_id"].ToString() + ", " + r["sorder"].ToString();
							break;
						case "mPhraseContent":
							line = r["с_id"].ToString() + ", " + r["ph_id"].ToString() + ", " + r["lx_id"].ToString();
							break;
					}
					//Console.WriteLine(line);
					System.Diagnostics.Debug.WriteLine(line);
				}
				r.Close();

			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		enum GrenPart
		{
			NUM_WORD_CLASS = 2,              // class num_word
			NOUN_ru = 6,                     // class СУЩЕСТВИТЕЛЬНОЕ
			PRONOUN2_ru = 7,                 // class МЕСТОИМ_СУЩ
			PRONOUN_ru = 8,                  // class МЕСТОИМЕНИЕ
			ADJ_ru = 9,                      // class ПРИЛАГАТЕЛЬНОЕ
			NUMBER_CLASS_ru = 10,            // class ЧИСЛИТЕЛЬНОЕ
			INFINITIVE_ru = 11,              // class ИНФИНИТИВ
			VERB_ru = 12,                    // class ГЛАГОЛ
			GERUND_2_ru = 13,                // class ДЕЕПРИЧАСТИЕ
			PREPOS_ru = 14,                  // class ПРЕДЛОГ
			IMPERSONAL_VERB_ru = 15,         // class БЕЗЛИЧ_ГЛАГОЛ
			PARTICLE_ru = 18,                // class ЧАСТИЦА
			CONJ_ru = 19,                    // class СОЮЗ
			ADVERB_ru = 20,                  // class НАРЕЧИЕ
			PUNCTUATION_class = 21,          // class ПУНКТУАТОР
			POSTPOS_ru = 26,                 // class ПОСЛЕЛОГ
			POSESS_PARTICLE = 27,            // class ПРИТЯЖ_ЧАСТИЦА
			MEASURE_UNIT = 28,               // class ЕДИНИЦА_ИЗМЕРЕНИЯ
			COMPOUND_ADJ_PREFIX = 29,        // class ПРЕФИКС_СОСТАВ_ПРИЛ
			COMPOUND_NOUN_PREFIX = 30,       // class ПРЕФИКС_СОСТАВ_СУЩ
			/*		VERB_en = 31,                    // class ENG_VERB
			BEVERB_en = 32,                  // class ENG_BEVERB
			AUXVERB_en = 33,                 // class ENG_AUXVERB
			NOUN_en = 34,                    // class ENG_NOUN
			PRONOUN_en = 35,                 // class ENG_PRONOUN
			ARTICLE_en = 36,                 // class ENG_ARTICLE
			PREP_en = 37,                    // class ENG_PREP
			POSTPOS_en = 38,                 // class ENG_POSTPOS
			CONJ_en = 39,                    // class ENG_CONJ
			ADV_en = 40,                     // class ENG_ADVERB
			ADJ_en = 41,                     // class ENG_ADJECTIVE
			PARTICLE_en = 42,                // class ENG_PARTICLE
			NUMERAL_en = 43,                 // class ENG_NUMERAL
			INTERJECTION_en = 44,            // class ENG_INTERJECTION
			POSSESSION_PARTICLE_en = 45,     // class ENG_POSSESSION
			COMPOUND_PRENOUN_en = 46,        // class ENG_COMPOUND_PRENOUN
			COMPOUND_PREADJ_en = 47,         // class ENG_COMPOUND_PREADJ
			COMPOUND_PREVERB_en = 48,        // class ENG_COMPOUND_PREVERB
			COMPOUND_PREADV_en = 49,         // class ENG_COMPOUND_PREADV
			NUMERAL_fr = 50,                 // class FR_NUMERAL
			ARTICLE_fr = 51,                 // class FR_ARTICLE
			PREP_fr = 52,                    // class FR_PREP
			ADV_fr = 53,                     // class FR_ADVERB
			CONJ_fr = 54,                    // class FR_CONJ
			NOUN_fr = 55,                    // class FR_NOUN
			ADJ_fr = 56,                     // class FR_ADJ
			PRONOUN_fr = 57,                 // class FR_PRONOUN
			VERB_fr = 58,                    // class FR_VERB
			PARTICLE_fr = 59,                // class FR_PARTICLE
			PRONOUN2_fr = 60,                // class FR_PRONOUN2
			NOUN_es = 61,                    // class ES_NOUN
			ROOT_es = 62,                    // class ES_ROOT
			JAP_NOUN = 63,                   // class JAP_NOUN
			JAP_NUMBER = 64,                 // class JAP_NUMBER
			JAP_ADJECTIVE = 65,              // class JAP_ADJECTIVE
			JAP_ADVERB = 66,                 // class JAP_ADVERB
			JAP_CONJ = 67,                   // class JAP_CONJ
			JAP_VERB = 68,                   // class JAP_VERB
			JAP_PRONOUN = 69,                // class JAP_PRONOUN
			JAP_VERB_POSTFIX2 = 72,          // class JAP_VERB_POSTFIX2
			JAP_PARTICLE = 74,               // class JAP_PARTICLE */
			UNKNOWN_ENTRIES_CLASS = 85       // class UnknownEntries
		}

		/// <summary>
		/// Заполнение справочника частей речи.
		/// </summary>
		public void InsertSpeechParts()
		{
			m_sqlCmd.Parameters.Clear();
			m_sqlCmd.Parameters.Add(new SQLiteParameter("@speechpart", ""));

			try
			{
				for (var part = GrenPart.NUM_WORD_CLASS; part <= GrenPart.UNKNOWN_ENTRIES_CLASS; part++)
				{
					long result = -1;
					string ename = Enum.GetName(typeof(GrenPart), part);
					if (ename == null) continue;
					m_sqlCmd.CommandText = "SELECT sp_id FROM mSpParts WHERE speechpart = @speechpart";
					m_sqlCmd.Parameters[0].Value = ename;
					//m_sqlCmd.Parameters. Add(new SQLiteParameter("@speechpart", ename));
					// Читаем только первую запись
					var resp = m_sqlCmd.ExecuteScalar();
					if (resp != null)
						result = (long)resp;
					if (result == -1)
					{
						m_sqlCmd.CommandText =
							String.Format("INSERT INTO mSpParts(sp_id, speechpart) VALUES({0}, @speechpart)", (int)part);
						m_sqlCmd.Parameters[0].Value = ename;
						m_sqlCmd.ExecuteNonQuery();
					}
				}
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// Заполнение справочника грамм. характеристик.
		/// </summary>
		public void InsertGrenProperties()
		{
			m_sqlCmd.Parameters.Clear();
			m_sqlCmd.Parameters.Add(new SQLiteParameter("@property", ""));

			try
			{
				for (var part = Schemas.GrenProperty.CharCasing; part <= Schemas.GrenProperty.MODAL_ru; part++)
				{
					long result = -1;
					string ename = Enum.GetName(typeof(Schemas.GrenProperty), part);
					if (ename == null) continue;
					m_sqlCmd.CommandText = "SELECT sg_id FROM mSiGram WHERE property = @property";
					m_sqlCmd.Parameters[0].Value = ename;
					//m_sqlCmd.Parameters. Add(new SQLiteParameter("@speechpart", ename));
					// Читаем только первую запись
					var resp = m_sqlCmd.ExecuteScalar();
					if (resp != null)
						result = (long)resp;
					if (result == -1)
					{
						m_sqlCmd.CommandText =
							String.Format("INSERT INTO mSiGram(sg_id, property) VALUES({0}, @property)", (int)part);
						m_sqlCmd.Parameters[0].Value = ename;
						m_sqlCmd.ExecuteNonQuery();
					}
				}
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}
	}
}

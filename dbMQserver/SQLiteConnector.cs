using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.IO;
using System.Data.SQLite;

namespace dbMQserver
{
	/// Singleton
	public sealed class SQLiteConnector : IdbConnector
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

				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mPhrases (\n"
						+ "	ph_id integer PRIMARY KEY, created_at DATETIME DEFAULT CURRENT_TIMESTAMP);";
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

		public long SaveLex(string word)
		{
			long result = GetWord(word);
			if (result == -1)
			{
				try
				{
					m_sqlCmd.CommandText = "INSERT INTO mLexems(lx_id, lex) VALUES(NULL, @lex)";
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@lex", word.ToLower()));
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

		public long GetWord(string rword)
		{
			long result = -1;
			try
			{
				//sqlite некорректно работает с upper, lower и like в utf8. Поэтому в БД надо всё хранить в lower
				m_sqlCmd.CommandText = "SELECT lx_id FROM mLexems WHERE lex = @lex";
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@lex", rword.ToLower()));
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

		public long SavePhrase(long ph_id)
		{
			if (ph_id == -1)
				try
				{
					// Нужно только создать ID
					m_sqlCmd.CommandText = "INSERT INTO mPhrases(ph_id) VALUES(NULL)";
					m_sqlCmd.ExecuteNonQuery();

					m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
					ph_id = (long)m_sqlCmd.ExecuteScalar();
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			else
				// Пока тут нечего UPDATE
				try
				{
					// pstmt.setInt(1, ph_id);
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			return ph_id;
		}

		public long SavePhraseContent(long ph_id, long w_id)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = "INSERT INTO mPhraseContent(с_id, ph_id, w_id) VALUES(NULL, @ph_id, @w_id)";
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@ph_id", ph_id));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@w_id", w_id));
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
				}
				//m_sqlCmd.CommandText = "SELECT с_id, ph_id, w_id FROM mPhraseContent";

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					//line = r["с_id"].ToString() + ", " + r["ph_id"].ToString() + ", " + r["w_id"].ToString();
					switch (nTable)
					{
						case "mSpParts":
							line = r["sp_id"].ToString() + ", " + r["speechpart"];
							break;
						case "mSiGram":
							line = r["sg_id"].ToString() + ", " + r["property"];
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

using System;
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

		public DirectSQL dirCmd;

		private SQLiteConnector()
		{
			if (!File.Exists(dbFileName))
				SQLiteConnection.CreateFile(dbFileName);
			try
			{
				m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
				m_dbConn.Open();
				m_sqlCmd.Connection = m_dbConn;
				CreateDictTablesDB();
				CreateOperTablesDB();

				dirCmd = new DirectSQL(m_dbConn);
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
		/// Удаление всех данных из БД кроме справочников
		/// </summary>
		public void EmptyDB()
		{
			try
			{
				m_sqlCmd.CommandText =
					"DROP TABLE IF EXISTS mSyntNodes;\n" +
					"DROP TABLE IF EXISTS mGrammems;\n" +
					"DROP TABLE IF EXISTS mPhraseContent;\n" +
					"DROP TABLE IF EXISTS mPhrases;\n" +
					"DROP TABLE IF EXISTS mParagraphs;\n" +
					"DROP TABLE IF EXISTS mDocuments;\n" +
					"DROP TABLE IF EXISTS mContainers;\n" +
					"DROP TABLE IF EXISTS mLemms;\n VACUUM;";
				m_sqlCmd.ExecuteNonQuery();

				CreateOperTablesDB();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("EmptyDB Error: " + ex.Message);
			}
		}

		/// <summary>
		/// Создание справочников в БД
		/// </summary>
		public void CreateDictTablesDB()
		{
			try
			{
				m_sqlCmd.CommandText =
					"CREATE TABLE IF NOT EXISTS mSpParts (\n"
						+ "	sp_id integer, speechpart text NOT NULL);" +
					" CREATE TABLE IF NOT EXISTS mSiGram (\n"
						+ "	sg_id integer, property text NOT NULL);" +
					"CREATE TABLE IF NOT EXISTS mSiLinks (\n"
						+ "	ln_id integer, linktype text NOT NULL);";
				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("CreateOperTablesDB Error: " + ex.Message);
			}
		}

		/// <summary>
		/// Создание таблиц данных в БД (кроме справочников)
		/// </summary>
		public void CreateOperTablesDB()
		{
			try
			{
				m_sqlCmd.CommandText = 
					"CREATE TABLE IF NOT EXISTS mLemms (\n"
						+ "	lx_id integer PRIMARY KEY, sp_id integer, lemma text NOT NULL);" +
					" CREATE TABLE IF NOT EXISTS mContainers (\n"
						+ "	ct_id integer PRIMARY KEY, created_at DATETIME DEFAULT CURRENT_TIMESTAMP, name text);" +
					" CREATE TABLE IF NOT EXISTS mDocuments (doc_id integer PRIMARY KEY, \n"
						+ "	ct_id integer, created_at DATETIME DEFAULT CURRENT_TIMESTAMP, name text);" +
					" CREATE TABLE IF NOT EXISTS mParagraphs (pg_id integer PRIMARY KEY, \n"
						+ "doc_id integer, created_at DATETIME DEFAULT CURRENT_TIMESTAMP, ph_id integer);" +
					"CREATE TABLE IF NOT EXISTS mPhrases (\n"
						+ "	ph_id integer PRIMARY KEY, pg_id integer, sorder integer, \n"
						+ " created_at DATETIME DEFAULT CURRENT_TIMESTAMP);" +
					"CREATE TABLE IF NOT EXISTS mPhraseContent (\n"
						+ "	с_id integer PRIMARY KEY, ph_id integer, lx_id integer, sorder integer);" +
					"CREATE TABLE IF NOT EXISTS mGrammems (\n" + 
						" gr_id integer PRIMARY KEY, с_id integer, sg_id integer, intval integer);" +
					"CREATE TABLE IF NOT EXISTS mSyntNodes (\n" +
						" sn_id integer PRIMARY KEY, с_id integer, ln_id integer, level integer);";
				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("CreateOperTablesDB Error: " + ex.Message);
			}
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
				m_sqlCmd.CommandText = "SELECT * FROM mParagraphs WHERE pg_id = @pg_id";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@pg_id", pg_id));
				var executeScalar = m_sqlCmd.ExecuteScalar();
				var resp = executeScalar == null ? 0 : 1;
				IsParaExists = resp > 0;
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
				Console.WriteLine("Error InsertGrammemDB: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Вставка в mSyntNodes.
		/// </summary>
		/// <returns>ID</returns>
		public long InsertSyntNodesDB(long с_id, int ln_id, int level)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText =
					String.Format("INSERT INTO mSyntNodes(sn_id, с_id, ln_id, level) VALUES(NULL, {0}, {1}, {2})",
					с_id, ln_id, level);
				m_sqlCmd.ExecuteNonQuery();

				m_sqlCmd.CommandText = "SELECT last_insert_rowid()";
				result = (long)m_sqlCmd.ExecuteScalar();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error InsertSyntNodesDB: " + ex.Message);
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

		/// <summary>
		/// Чтение записей из mSyntNodes, относящихся к предложению ph_id.
		/// </summary>
		/// <param name="ph_id">ID предложения</param>
		/// <returns>Список синт. связей предложения</returns>
		public List<tNode> ReadSyntNodesDB(long ph_id)
		{
			var reslist = new List<tNode>();
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT A.ln_id, A.level, B.sorder FROM mSyntNodes A " +
					"JOIN mPhraseContent B ON B.с_id = A.с_id WHERE B.ph_id = {0} ORDER BY A.sn_id", ph_id);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				short i = 0;
				while (r.Read())
				{
					var node = new tNode();
					node.ID = i;
					node.Level = r.GetInt32(1);
					node.index = r.GetInt32(2);
					node.linktype = r.GetInt32(0);
					reslist.Add(node);
					i++;
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
					case "mSiLinks":
						m_sqlCmd.CommandText = "SELECT ln_id, linktype FROM mSiLinks";
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
					case "mGrammems":
						m_sqlCmd.CommandText = "SELECT gr_id, с_id, sg_id, intval FROM mGrammems";
						break;
					case "mSyntNodes":
						m_sqlCmd.CommandText = "SELECT sn_id, с_id, ln_id, level FROM mSyntNodes";
						break;
				}

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
						case "mSiLinks":
							line = r["ln_id"].ToString() + ", " + r["linktype"];
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
						case "mGrammems":
							line = r["gr_id"].ToString() + ", " + r["с_id"].ToString() + ", " + r["sg_id"].ToString() + ", " + r["intval"].ToString();
							break;
						case "mSyntNodes":
							line = r["sn_id"].ToString() + ", " + r["с_id"].ToString() + ", " + r["ln_id"].ToString() + ", " + r["level"].ToString();
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

		/// <summary>
		/// Заполнение справочника типов синт. связей.
		/// </summary>
		public void InsertGrenLinks()
		{
			m_sqlCmd.Parameters.Clear();
			m_sqlCmd.Parameters.Add(new SQLiteParameter("@linktype", ""));

			try
			{
				for (var link = GrenLink.OBJECT_link; link <= Schemas.GrenLink.SECOND_VERB_link; link++)
				{
					long result = -1;
					string ename = Enum.GetName(typeof(GrenLink), link);
					if (ename == null) continue;
					m_sqlCmd.CommandText = "SELECT ln_id FROM mSiLinks WHERE linktype = @linktype";
					m_sqlCmd.Parameters[0].Value = ename;
					// Читаем только первую запись
					var resp = m_sqlCmd.ExecuteScalar();
					if (resp != null)
						result = (long)resp;
					if (result == -1)
					{
						m_sqlCmd.CommandText =
							String.Format("INSERT INTO mSiLinks(ln_id, linktype) VALUES({0}, @linktype)", (int)link);
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

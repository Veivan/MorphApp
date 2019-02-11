using System;
using System.Collections.Generic;
using System.Linq;

using System.Data;
using System.IO;
using System.Data.SQLite;
using Schemas;
using Schemas.BlockPlatform;

namespace DirectDBconnector
{
	public enum OpersDB { odNone, odSelect, odInsert, odUpdate, odDelete };

	public struct WordStruct
	{
		public long с_id;
		public string lemma;
		public int sp_id;
		public int rcind;
		public string realWord;
	}

	public struct TermStruct
	{
		public long tс_id;
		public long tm_id;
		public int order;
		public int rcind;
		public long lem_id;
	}

	/// Singleton
	public sealed class SQLiteConnector
	{
		private static readonly Lazy<SQLiteConnector> instanceHolder =
			new Lazy<SQLiteConnector>(() => new SQLiteConnector());

		private SQLiteConnection m_dbConn;
		private SQLiteCommand m_sqlCmd = new SQLiteCommand();

		public DirectSQL dirCmd;

		private SQLiteConnector()
		{
			String dbFileName = Properties.DirectDBC.Default.dbFileName;
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
					"DROP TABLE IF EXISTS mUndefContent;\n" +
					"DROP TABLE IF EXISTS mUndefs;\n" +
					"DROP TABLE IF EXISTS mTerminContent;\n" +
					"DROP TABLE IF EXISTS mTermins;\n" +
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
					"CREATE TABLE IF NOT EXISTS mRealWord (\n"
						+ "	rw_id integer PRIMARY KEY, wform text NOT NULL);\n" +
					"CREATE TABLE IF NOT EXISTS mLemms (\n"
						+ "	lx_id integer PRIMARY KEY, sp_id integer, lemma text NOT NULL);\n" +
					" CREATE TABLE IF NOT EXISTS mContainers (\n"
						+ "	ct_id integer PRIMARY KEY, created_at DATETIME DEFAULT CURRENT_TIMESTAMP, name text, parent_id integer);\n" +
					" CREATE TABLE IF NOT EXISTS mDocuments (doc_id integer PRIMARY KEY, \n"
						+ "	ct_id integer, created_at DATETIME DEFAULT CURRENT_TIMESTAMP, name text);\n" +
					" CREATE TABLE IF NOT EXISTS mParagraphs (pg_id integer PRIMARY KEY, \n"
						+ "doc_id integer, created_at DATETIME DEFAULT CURRENT_TIMESTAMP);\n" +
					"CREATE TABLE IF NOT EXISTS mPhrases (\n"
						+ "	ph_id integer PRIMARY KEY, pg_id integer, sorder integer, \n"
						+ " created_at DATETIME DEFAULT CURRENT_TIMESTAMP);\n" +
					"CREATE TABLE IF NOT EXISTS mPhraseContent (\n"
						+ "	с_id integer PRIMARY KEY, ph_id integer, lx_id integer, rcind integer, sorder integer, rw_id integer);\n" +
					"CREATE TABLE IF NOT EXISTS mGrammems (\n"
						+ " gr_id integer PRIMARY KEY, с_id integer, sg_id integer, intval integer);\n" +
					"CREATE TABLE IF NOT EXISTS mSyntNodes (\n"
						+ " sn_id integer PRIMARY KEY, с_id integer, ln_id integer, level integer, pс_id integer);\n" +
					"CREATE TABLE IF NOT EXISTS mTermins (\n"
						+ "	tm_id integer PRIMARY KEY);\n" +
					"CREATE TABLE IF NOT EXISTS mTerminContent (\n"
						+ "	tc_id integer PRIMARY KEY, tm_id integer, sorder integer, rcind integer, lem_id integer);\n" +
					"CREATE TABLE IF NOT EXISTS mUndefs (\n"
						+ "	mu_id integer PRIMARY KEY, alalemma text NOT NULL);\n" +
					"CREATE TABLE IF NOT EXISTS mUndefContent (\n"
						+ "	uv_id integer PRIMARY KEY, mu_id integer, rw_id integer);\n" +

					"CREATE TABLE IF NOT EXISTS mBlockTypes (\n"
						+ "	bt_id integer PRIMARY KEY, namekey text, nameui text);\n" +
					"CREATE TABLE IF NOT EXISTS mAttrTypes (\n"
						+ "	mt_id integer PRIMARY KEY, name text, size integer);\n" +
					"CREATE TABLE IF NOT EXISTS mAttributes (\n"
						+ "	ma_id integer PRIMARY KEY, namekey text, nameui text, mt_id integer, bt_id integer, sorder integer, mandatory integer);\n" +
					"CREATE TABLE IF NOT EXISTS mBlocks (\n"
						+ "	b_id integer PRIMARY KEY, bt_id integer, created_at DATETIME DEFAULT CURRENT_TIMESTAMP, \n"
						+ " parent integer, treeorder integer, fh_id integer, predecessor integer, successor integer, deleted integer DEFAULT 0);\n" +
					"CREATE TABLE IF NOT EXISTS mFactHeap (\n"
						+ "	fh_id integer PRIMARY KEY, blockdata blob);\n";


				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("CreateOperTablesDB Error: " + ex.Message);
			}
		}

		#region Методы работы с контейнерами /// TODO удалить в дальнейшем
		/// <summary>
		/// Вставка в mContainers.
		/// </summary>
		/// <param name="name">Имя контейнера</param>
		/// <param name="parent_id">ID родительского контейнера</param>
		/// <returns>ID контейнера</returns>
		public long InsertContainerDB(string name, long parent_id = -1)
		{
			long ct_id = -1;
			try
			{
				m_sqlCmd.CommandText = String.Format("INSERT INTO mContainers(ct_id, name, parent_id) VALUES(NULL, '{0}', {1})", name, parent_id);
				m_sqlCmd.ExecuteNonQuery();
				ct_id = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("InsertContainerDB Error: " + ex.Message);
			}
			return ct_id;
		}

		/// <summary>
		/// Чтения дочерних контейнеров множества родительских контейнеров.
		/// </summary>
		/// <param name="list_ids">список ID родительских контейнеров</param>
		/// <returns>DataTable</returns>
		public DataTable GetChildrenInContainerList(List<string> list_ids)
		{
			DataTable dTable = new DataTable();
			string result = string.Join(",", list_ids.ToArray());
			if (String.IsNullOrEmpty(result))
				return dTable;

			var stmnt = string.Format("SELECT ct_id, created_at, name, parent_id FROM mContainers WHERE parent_id IN ({0})", result);
			try
			{
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(stmnt, m_dbConn);
				adapter.Fill(dTable);
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return dTable;
		}

		/// <summary>
		/// Удаление контейнера.
		/// Контейнеры можно удалять только по одному, проверяя отсутствие дочерних контейнеров.
		/// Документы внутри контейнера удаляются пачкой внутри транзакции.
		/// </summary>
		/// <param name="ct_id">ID удаляемого контейнера</param>
		/// <returns>int</returns>
		public int DeleteContainer(long ct_id)
		{
			var result = -1;
			SQLiteTransaction transaction = null;
			try
			{
				transaction = m_dbConn.BeginTransaction();
				var list_ids = new List<string>();
				//Формирование списка документов для удаления
				m_sqlCmd.CommandText = string.Format("SELECT doc_id FROM mDocuments WHERE ct_id = {0}", ct_id);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					list_ids.Add(r["doc_id"].ToString());
				}
				r.Close();
				// Удаление документов
				DeleteDocumentListInner(list_ids);
				//Удаление контейнера
				m_sqlCmd.CommandText = string.Format("DELETE FROM mContainers WHERE ct_id = {0}", ct_id);
				m_sqlCmd.ExecuteNonQuery();
				result = 0;
				transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				transaction.Rollback();
				//MessageBox.Show("Error: " + ex.Message);
			}
			return result;
		}

		#endregion

		#region Методы работы с документами
		/// <summary>
		/// Вставка в mDocuments.
		/// </summary>
		/// <param name="name">Имя документа</param>
		/// <param name="ct_id">ID контейнера</param>
		/// <returns>ID документа</returns>
		public long InsertDocumentDB(string name, long ct_id)
		{
			long doc_id = -1;
			try
			{
				m_sqlCmd.CommandText = String.Format("INSERT INTO mDocuments(doc_id, ct_id, name) VALUES(NULL, {0}, '{1}')",
					ct_id, name);
				m_sqlCmd.ExecuteNonQuery();
				doc_id = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("InsertDocumentDB Error: " + ex.Message);
			}
			return doc_id;
		}

		/// <summary>
		/// Чтения документов из выбранного контейнера.
		/// </summary>
		/// <param name="ct_id">ID контейнера</param>
		/// <returns>DataTable</returns>
		public DataTable GetDocsInContainer(long ct_id)
		{
			var stmnt = String.Format("SELECT doc_id, ct_id, created_at, name, parent_id FROM mDocuments WHERE ct_id = {0}", ct_id);
			DataTable dTable = new DataTable();
			try
			{
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(stmnt, m_dbConn);
				adapter.Fill(dTable);
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return dTable;
		}

		/// <summary>
		/// Чтения документов из множества контейнеров.
		/// </summary>
		/// <param name="list_ids">список ID контейнеров</param>
		/// <returns>DataTable</returns>
		public DataTable GetDocsInContainerList(List<string> list_ids)
		{
			/*StringBuilder builder = new StringBuilder();
			foreach (var id in list_ids)
			{
				builder.Append(id.ToString()).Append(",");
			}
			string result = builder.ToString().TrimEnd(',');*/

			DataTable dTable = new DataTable();
			string result = string.Join(",", list_ids.ToArray());
			if (String.IsNullOrEmpty(result))
				return dTable;

			var stmnt = string.Format("SELECT doc_id, ct_id, created_at, name FROM mDocuments WHERE ct_id IN ({0})", result);
			try
			{
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(stmnt, m_dbConn);
				adapter.Fill(dTable);
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return dTable;
		}

		/// <summary>
		/// Удаление документов без транзакции.
		/// Используется при удалении контейнера и списка документов (внутри их транзакций).
		/// </summary>
		/// <param name="list_ids">Перечень удаляемых ID</param>
		/// <returns></returns>
		private void DeleteDocumentListInner(List<string> list_ids)
		{
			if (list_ids.Count == 0)
				return;

			var list_pg = new List<string>();
			var list_ph = new List<string>();
			//Формирование списка абзацев для удаления
			string strlist = string.Join(",", list_ids.ToArray());
			m_sqlCmd.CommandText = string.Format("SELECT pg_id FROM mParagraphs WHERE doc_id IN ({0})", strlist);
			SQLiteDataReader r = m_sqlCmd.ExecuteReader();
			while (r.Read())
			{
				list_pg.Add(r["pg_id"].ToString());
			}
			r.Close();
			//Формирование списка фраз для удаления
			var strPGlist = string.Join(",", list_pg.ToArray());
			m_sqlCmd.CommandText = string.Format("SELECT ph_id FROM mPhrases WHERE pg_id IN ({0})", strPGlist);
			r = m_sqlCmd.ExecuteReader();
			while (r.Read())
			{
				list_ph.Add(r["ph_id"].ToString());
			}
			r.Close();
			//Удаление фраз 
			DeletePhrasesListInner(list_ph);
			//Удаление абзацев
			m_sqlCmd.CommandText = string.Format("DELETE FROM mParagraphs WHERE pg_id IN ({0})", strPGlist);
			m_sqlCmd.ExecuteNonQuery();
			//Удаление документов
			m_sqlCmd.CommandText = string.Format("DELETE FROM mDocuments WHERE doc_id IN ({0})", strlist);
			m_sqlCmd.ExecuteNonQuery();
		}

		/// <summary>
		/// Удаление документов.
		/// </summary>
		/// <param name="list_ids">Перечень удаляемых ID</param>
		/// <returns>int</returns>
		public int DeleteDocumentList(List<string> list_ids)
		{
			if (list_ids.Count == 0)
				return 0;

			var result = -1;
			SQLiteTransaction transaction = null;
			try
			{
				transaction = m_dbConn.BeginTransaction();
				DeleteDocumentListInner(list_ids);
				result = 0;
				transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				transaction.Rollback();
				//MessageBox.Show("Error: " + ex.Message);
			}
			return result;
		}
		#endregion

		#region Методы работы с абзацами
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
		public long InsertParagraphDB(long doc_id)
		{
			long pg_id = -1;
			try
			{
				m_sqlCmd.CommandText = String.Format("INSERT INTO mParagraphs(pg_id, doc_id) VALUES(NULL, {0})", doc_id);
				m_sqlCmd.ExecuteNonQuery();
				pg_id = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return pg_id;
		}

		/// <summary>
		/// Чтения абзацев из множества документов.
		/// Если list_ids == null, то выбираются все записи.
		/// </summary>
		/// <param name="list_ids">список ID документов</param>
		/// <returns>DataTable</returns>
		public DataTable ReadParagraphsInDocs(List<string> list_ids)
		{
			DataTable dTable = new DataTable();
			var stmnt = "SELECT P.pg_id, P.doc_id, P.created_at, D.ct_id FROM mParagraphs P LEFT JOIN mDocuments D ON D.doc_id = P.doc_id";
			if (list_ids != null)
			{
				string result = string.Join(",", list_ids.ToArray());
				if (!String.IsNullOrEmpty(result))
					stmnt = stmnt + String.Format(" WHERE P.doc_id IN ({0})", result);
			}
			try
			{
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(stmnt, m_dbConn);
				adapter.Fill(dTable);
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return dTable;
		}

		/// <summary>
		/// Чтения абзацев из множества документов.
		/// Если list_ids == null, то выбираются все записи.
		/// </summary>
		/// <param name="list_ids">список ID документов</param>
		/// <returns>List of ParagraphMap</returns>
		public List<ParagraphMap> ReadParagraphsInDocsList(List<string> list_ids)
		{
			var reslist = new List<ParagraphMap>();
			var stmnt = "SELECT P.pg_id, P.doc_id, P.created_at, D.ct_id FROM mParagraphs P JOIN mDocuments D ON D.doc_id = P.doc_id";
			if (list_ids != null)
			{
				string result = string.Join(",", list_ids.ToArray());
				if (!String.IsNullOrEmpty(result))
					stmnt = stmnt + String.Format(" WHERE P.doc_id IN ({0})", result);
			}
			try
			{
				m_sqlCmd.CommandText = stmnt;
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					var dt = DateTime.Now;
					var created = r["created_at"].ToString();
					if (!String.IsNullOrEmpty(created))
						dt = DateTime.Parse(created);
					reslist.Add(new ParagraphMap(r.GetInt64(0), r.GetInt64(1), dt, r.GetInt64(3)));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return reslist;
		}

		/// <summary>
		/// Удаление абзацев.
		/// </summary>
		/// <param name="list_ids">Перечень удаляемых ID</param>
		/// <returns>int</returns>
		public int DeleteParagraphList(List<string> list_ids)
		{
			if (list_ids.Count == 0)
				return 0;
			string strlist = string.Join(",", list_ids.ToArray());

			var list_ph = new List<string>();
			IEnumerator<string> etr = list_ids.GetEnumerator();

			var result = -1;
			SQLiteTransaction transaction = null;
			try
			{
				transaction = m_dbConn.BeginTransaction();
				while (etr.MoveNext())
				{
					var pg_id = etr.Current;
					list_ph.Clear();

					m_sqlCmd.CommandText = String.Format("SELECT ph_id FROM mPhrases WHERE pg_id = {0}", pg_id);
					SQLiteDataReader r = m_sqlCmd.ExecuteReader();
					while (r.Read())
					{
						list_ph.Add(r["ph_id"].ToString());
					}
					r.Close();
					//Удаление фраз абзаца
					DeletePhrasesListInner(list_ph);
					// Удаление фраз
					m_sqlCmd.CommandText = String.Format("DELETE FROM mParagraphs WHERE pg_id = ({0})", pg_id);
					m_sqlCmd.ExecuteNonQuery();
				}
				result = 0;
				transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				transaction.Rollback();
				//MessageBox.Show("Error: " + ex.Message);
			}
			return result;
		}
		#endregion

		#region Методы работы с предложениями
		/// <summary>
		/// Вставка в mPhrases.
		/// </summary>
		/// <returns>ID предложения</returns>
		public long InsertPhraseDB(long pg_id, int sorder)
		{
			long ph_id = -1;
			try
			{
				m_sqlCmd.CommandText =
					String.Format("INSERT INTO mPhrases(ph_id, pg_id, sorder) VALUES(NULL, {0}, {1})", pg_id, sorder);
				m_sqlCmd.ExecuteNonQuery();
				ph_id = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return ph_id;
		}

		/// <summary>
		/// Чтение записей из mPhrases, относящихся к абзацу pg_id.
		/// </summary>
		/// <param name="pg_id">ID параграфа</param>
		/// <returns>Список SentenceMap</returns>
		public List<SentenceMap> ReadPhraseDB(long pg_id)
		{
			var reslist = new List<SentenceMap>();
			try
			{
				m_sqlCmd.CommandText = "SELECT ph_id, sorder, created_at FROM mPhrases WHERE pg_id = @pg_id ORDER BY sorder";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@pg_id", pg_id));

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					var dt = DateTime.Now;
					var created = r["created_at"].ToString();
					if (!String.IsNullOrEmpty(created))
						dt = DateTime.Parse(created);
					var sent = new SentenceMap(r.GetInt64(0), pg_id, r.GetInt16(1), dt);
					reslist.Add(sent);
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
		/// Чтения предложений из множества абзацев.
		/// Если list_ids == null, то выбираются все записи.
		/// </summary>
		/// <param name="list_ids">список ID абзацев</param>
		/// <returns>DataTable</returns>
		public DataTable ReadPhrasesInParagraphs(List<string> list_ids)
		{
			DataTable dTable = new DataTable();
			var stmnt = "SELECT P.ph_id, P.pg_id, P.sorder, P.created_at FROM mPhrases P ";
			if (list_ids != null)
			{
				string result = string.Join(",", list_ids.ToArray());
				if (!String.IsNullOrEmpty(result))
					stmnt = stmnt + String.Format(" WHERE P.pg_id IN ({0})", result);
			}
			try
			{
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(stmnt, m_dbConn);
				adapter.Fill(dTable);
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return dTable;
		}

		/// <summary>
		/// Чтения абзацев из множества документов.
		/// Если list_ids == null, то выбираются все записи.
		/// </summary>
		/// <param name="list_ids">список ID документов</param>
		/// <returns>List of ParagraphMap</returns>
		public List<SentenceMap> ReadPhrasesInParagraphsList(List<string> list_ids)
		{
			var reslist = new List<SentenceMap>();
			var stmnt = "SELECT P.ph_id, P.pg_id, P.sorder, P.created_at FROM mPhrases P ";
			if (list_ids != null)
			{
				string result = string.Join(",", list_ids.ToArray());
				if (!String.IsNullOrEmpty(result))
					stmnt = stmnt + String.Format(" WHERE P.pg_id IN ({0})", result);
			}
			try
			{
				m_sqlCmd.CommandText = stmnt;
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					var dt = DateTime.Now;
					var created = r["created_at"].ToString();
					if (!String.IsNullOrEmpty(created))
						dt = DateTime.Parse(created);
					reslist.Add(new SentenceMap(r.GetInt64(0), r.GetInt64(1), r.GetInt32(2), dt));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return reslist;
		}

		/// <summary>
		/// Удаление абзацев.
		/// </summary>
		/// <param name="list_ids">Перечень удаляемых ID</param>
		/// <returns>int</returns>
		public int DeletePhrasesListTrans(List<string> list_ids)
		{
			if (list_ids.Count == 0)
				return 0;
			var result = -1;
			SQLiteTransaction transaction = null;
			try
			{
				transaction = m_dbConn.BeginTransaction();
				DeletePhrasesListInner(list_ids);
				transaction.Commit();
				result = 0;
			}
			catch (SQLiteException ex)
			{
				transaction.Rollback();
				//MessageBox.Show("Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Удаление предложений без транзакции.
		/// Используется при удалении абзаца (внутри его транзакции).
		/// </summary>
		/// <param name="list_ids">Перечень удаляемых ID</param>
		/// <returns></returns>
		private void DeletePhrasesListInner(List<string> list_ids)
		{
			if (list_ids.Count == 0)
				return;
			string strlist = string.Join(",", list_ids.ToArray());

			//Удаление синтаксических связей
			DeleteSyntNodesOfPhrase(list_ids);
			//Удаление граммем
			DeleteGrammemsOfPhrase(list_ids);
			//Удаление состава фраз
			DeleteContentOfPhrase(list_ids);
			// Удаление фраз
			m_sqlCmd.CommandText = String.Format("DELETE FROM mPhrases WHERE ph_id IN ({0})", strlist);
			m_sqlCmd.ExecuteNonQuery();
		}
		#endregion

		#region Методы работы с содержимым предложений
		/// <summary>
		/// Вставка в mPhraseContent.
		/// </summary>
		/// <returns>ID</returns>
		public long InsertWordDB(long ph_id, long lx_id, int sorder, int rcind, long rw_id)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText =
					"INSERT INTO mPhraseContent(с_id, ph_id, lx_id, sorder, rcind, rw_id) VALUES(NULL, @ph_id, @lx_id, @sorder, @rcind, @rw_id)";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@ph_id", ph_id));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@lx_id", lx_id));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@sorder", sorder));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@rcind", rcind));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@rw_id", rw_id));
				m_sqlCmd.ExecuteNonQuery();
				result = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
			return result;
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
				m_sqlCmd.CommandText = "SELECT P.с_id, B.lemma, B.sp_id, P.rcind, R.wform FROM mPhraseContent P " +
					"JOIN mLemms B ON B.lx_id = P.lx_id " +
					"JOIN mRealWord R ON R.rw_id = P.rw_id " +
					"WHERE P.ph_id = @ph_id ORDER BY sorder";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@ph_id", ph_id));

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					var wstruct = new WordStruct();
					wstruct.с_id = r.GetInt64(0);
					wstruct.lemma = r.GetString(1);
					wstruct.sp_id = r.GetInt32(2);
					wstruct.rcind = r.GetInt32(3);
					wstruct.realWord = r.GetString(4);
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
		/// Удаление из mPhraseContent записей, относящихся к предложениям из списка.
		/// </summary>
		/// <param name="list_ids">Список ID предложений</param>
		/// <returns></returns>
		private void DeleteContentOfPhrase(List<string> list_ids)
		{
			string strlist = string.Join(",", list_ids.ToArray());
			m_sqlCmd.CommandText = String.Format("DELETE FROM mPhraseContent WHERE ph_id IN ({0})", strlist);
			m_sqlCmd.ExecuteNonQuery();
		}

		#endregion

		#region Методы работы с граммемами
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
				result = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error InsertGrammemDB: " + ex.Message);
			}
			return result;
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
		/// Удаление из mGrammems записей, относящихся к предложениям из списка.
		/// </summary>
		/// <param name="list_ids">Список ID предложений</param>
		/// <returns></returns>
		private void DeleteGrammemsOfPhrase(List<string> list_ids)
		{
			string strlist = string.Join(",", list_ids.ToArray());
			m_sqlCmd.CommandText = "DELETE FROM mGrammems WHERE с_id IN (SELECT N.с_id FROM mGrammems N JOIN mPhraseContent C ON C.с_id = N.с_id " +
				String.Format("WHERE C.ph_id IN ({0}) )", strlist);
			m_sqlCmd.ExecuteNonQuery();
		}

		#endregion

		#region Методы работы с синтаксическими узлами
		/// <summary>
		/// Вставка в mSyntNodes.
		/// </summary>
		/// <returns>ID</returns>
		public long InsertSyntNodesDB(long с_id, int ln_id, int level, long pс_id)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText =
					string.Format("INSERT INTO mSyntNodes(sn_id, с_id, ln_id, level, pс_id) VALUES(NULL, {0}, {1}, {2}, {3})",
					с_id, ln_id, level, pс_id);
				m_sqlCmd.ExecuteNonQuery();
				result = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error InsertSyntNodesDB: " + ex.Message);
			}
			return result;
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
				m_sqlCmd.CommandText = String.Format("SELECT A.ln_id, A.level, B.sorder, C.sorder FROM mSyntNodes A " +
					"JOIN mPhraseContent B ON B.с_id = A.с_id " +
					"LEFT JOIN mPhraseContent C ON C.с_id = A.pс_id " +
					"WHERE B.ph_id = {0} ORDER BY A.sn_id", ph_id);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				int i = 0;
				while (r.Read())
				{
					var node = new tNode();
					node.ID = i;
					node.Level = r.GetInt32(1);
					node.index = r.GetInt32(2);
					node.linktype = r.GetInt32(0);
					node.parentind = r[3] as int? ?? -1; ;
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

		/// <summary>
		/// Удаление из mSyntNodes записей, относящихся к предложениям из списка.
		/// </summary>
		/// <param name="list_ids">Список ID предложений</param>
		/// <returns></returns>
		private void DeleteSyntNodesOfPhrase(List<string> list_ids)
		{
			string strlist = string.Join(",", list_ids.ToArray());
			m_sqlCmd.CommandText = "DELETE FROM mSyntNodes WHERE с_id IN (SELECT N.с_id FROM mSyntNodes N JOIN mPhraseContent C ON C.с_id = N.с_id " +
				String.Format("WHERE C.ph_id IN ({0}) )", strlist);
			m_sqlCmd.ExecuteNonQuery();
		}

		#endregion

		#region Методы работы с леммами
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
					result = m_dbConn.LastInsertRowId;
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

		#endregion

		#region Методы работы с формами слов
		/// <summary>
		/// Сохранение формы слова в БД.
		/// </summary>
		/// <param name="realWord">форма слова</param>
		/// <returns>ID формы слова</returns>
		public long SaveRealWord(string realWord)
		{
			long result = GetRealWord(realWord);
			if (result == -1)
			{
				try
				{
					m_sqlCmd.CommandText = "INSERT INTO mRealWord(rw_id, wform) VALUES(NULL, @wform)";
					m_sqlCmd.Parameters.Clear();
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@wform", realWord));
					m_sqlCmd.ExecuteNonQuery();
					result = m_dbConn.LastInsertRowId;
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}
			return result;
		}

		private long GetRealWord(string realWord)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = "SELECT rw_id FROM mRealWord WHERE wform = @wform";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@wform", realWord));
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
		#endregion

		#region Методы работы с терминами
		/// <summary>
		/// Сохранение содержимого термина в БД.
		/// Реализация сохраняет только однословные термины.
		/// </summary>
		/// <param name="termlist">список структур TermStruct</param>
		/// <returns>ID термина</returns>
		public long SaveTermin(List<TermStruct> termlist)
		{
			var term0 = termlist[0];
			long tm_id = FindTermin(termlist);
			if (tm_id == -1)
			{
				try
				{
					m_sqlCmd.CommandText = "INSERT INTO mTermins(tm_id) VALUES(NULL)";
					m_sqlCmd.ExecuteNonQuery();
					tm_id = m_dbConn.LastInsertRowId;

					m_sqlCmd.CommandText = String.Format("INSERT INTO mTerminContent(tc_id, tm_id, sorder, rcind, lem_id) " +
						"VALUES(NULL, {0}, {1}, {2}, {3})", tm_id, term0.order, term0.rcind, term0.lem_id);
					m_sqlCmd.ExecuteNonQuery();
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("SaveTermin Error: " + ex.Message);
				}
			}
			return tm_id;
		}

		/// <summary>
		/// Поиск термина в БД по содержимому.
		/// Реализация ищет только однословные термины.
		/// </summary>
		/// <param name="termlist">список структур TermStruct</param>
		/// <returns>ID термина</returns>
		private long FindTermin(List<TermStruct> termlist)
		{
			var term0 = termlist[0];
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT tm_id FROM mTerminContent WHERE " +
					"sorder = {0} AND rcind = {1} AND lem_id = {2}",
					term0.order, term0.rcind, term0.lem_id);
				// Читаем только первую запись
				var resp = m_sqlCmd.ExecuteScalar();
				if (resp != null)
					result = (long)resp;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("FindTermin Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Сохранение "неопрелённого" слова в БД.
		/// </summary>
		/// <param name="termlist">список структур TermStruct</param>
		/// <returns>ID термина</returns>
		public long SaveUndefWord(string rword, long rw_id)
		{
			long mu_id = FindUndef(rw_id);
			if (mu_id == -1)
			{
				try
				{
					m_sqlCmd.CommandText = String.Format("INSERT INTO mUndefs(mu_id, alalemma) VALUES(NULL, '{0}')", rword.ToLower());
					m_sqlCmd.ExecuteNonQuery();
					mu_id = m_dbConn.LastInsertRowId;

					m_sqlCmd.CommandText = String.Format("INSERT INTO mUndefContent(uv_id, mu_id, rw_id) " +
						"VALUES(NULL, {0}, {1})", mu_id, rw_id);
					m_sqlCmd.ExecuteNonQuery();
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("SaveUndefWord Error: " + ex.Message);
				}
			}
			return mu_id;
		}

		/// <summary>
		/// Поиск "неопрелённого" слова в БД по содержимому.
		/// </summary>
		/// <param name="rw_id">ID слова в mRealWord</param>
		/// <returns>ID "неопрелённого" слова</returns>
		private long FindUndef(long rw_id)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT mu_id FROM mUndefContent WHERE rw_id = {0}", rw_id);
				// Читаем только первую запись
				var resp = m_sqlCmd.ExecuteScalar();
				if (resp != null)
					result = (long)resp;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("FindUndef Error: " + ex.Message);
			}
			return result;
		}
		#endregion

		#region Методы служебные
		public void DropColumn()
		{
			//SQLiteTransaction transaction = null;
			try
			{
				//transaction = m_dbConn.BeginTransaction();

				var stmt =
					"BEGIN TRANSACTION;\n" +
					"CREATE TABLE IF NOT EXISTS mAttributes_back (ma_id integer PRIMARY KEY, \n" +
						"namekey text, nameui text, mt_id integer, bt_id integer, sorder integer, mandatory integer);\n" +
					"INSERT INTO mAttributes_back(ma_id, namekey, nameui, mt_id, bt_id, sorder, mandatory) \n" +
					"SELECT ma_id, namekey, nameui, mt_id, bt_id, sorder, mandatory FROM mAttributes;\n" +
					"DROP TABLE mAttributes;\n" +
					"ALTER TABLE mAttributes_back RENAME TO mAttributes;\n" +
					"COMMIT;\n";
				m_sqlCmd.CommandText = stmt;
				m_sqlCmd.ExecuteNonQuery();

				//transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				//transaction.Rollback();
				Console.WriteLine("DropColumn Error: " + ex.Message);
			}
		}

		public void AddColumn()
		{
			try
			{
				var stmt =
					"ALTER TABLE mBlocks ADD COLUMN deleted integer DEFAULT 0;";

				/*					"ALTER TABLE mAttributes ADD COLUMN namekey text;\n"
									+ "ALTER TABLE mAttributes ADD COLUMN nameui text;\n"
									+ "UPDATE mAttributes SET namekey = name;"; */
				m_sqlCmd.CommandText = stmt;
				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("AddColumn Error: " + ex.Message);
			}
		}

		public void RenameColumn()
		{
			try
			{
				var stmt =
					"BEGIN TRANSACTION;\n" +
					"ALTER TABLE mBlockTypes RENAME TO _mBlockTypes_old;\n" +
					"CREATE TABLE mBlockTypes (bt_id integer PRIMARY KEY, namekey text, nameui text);\n" +
					"INSERT INTO mBlockTypes(bt_id, namekey, nameui) SELECT bt_id, name, name FROM _mBlockTypes_old;\n" +
					"DROP TABLE _mBlockTypes_old;\n" +
					"COMMIT;\n";
				m_sqlCmd.CommandText = stmt;
				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("RenameColumn Error: " + ex.Message);
			}
		}

		public void PerformOperator()
		{
			try
			{
				m_sqlCmd.CommandText = "DROP TABLE IF EXISTS mLemms;\n VACUUM;";

				/*				m_sqlCmd.CommandText = string.Format("UPDATE mBlocks SET created_at = @date WHERE b_id = {0}", 8);
								m_sqlCmd.Parameters.Clear();
								var dt = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
								//m_sqlCmd.Parameters.Add(new SQLiteParameter("@date", "2018-08-20 18:02:25"));
								m_sqlCmd.Parameters.Add(new SQLiteParameter("@date", dt));*/

				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("PerformOperator Error: " + ex.Message);
				throw ex;
			}
		}

		///
		/// select all rows in the mPhrases table
		/// 
		public void selectAll(dbTables tblname)
		{
			try
			{
				m_sqlCmd.CommandText = TableSelector.GetSelectStatement(tblname);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					switch (tblname)
					{
						case dbTables.tblParts:
							line = r["sp_id"].ToString() + ", " + r["speechpart"];
							break;
						case dbTables.tblSiGram:
							line = r["sg_id"].ToString() + ", " + r["property"];
							break;
						case dbTables.tblSiLinks:
							line = r["ln_id"].ToString() + ", " + r["linktype"];
							break;
						case dbTables.tblLemms:
							line = r["lx_id"].ToString() + ", " + r["sp_id"].ToString() + ", " + r["lemma"];
							break;
						case dbTables.tblContainers:
							line = r["ct_id"].ToString() + ", " + r["created_at"].ToString() + ", " + r["name"] + ", " + r["parent_id"].ToString();
							break;
						case dbTables.tblDocuments:
							line = r["doc_id"].ToString() + ", " + r["ct_id"].ToString() + ", " + r["created_at"].ToString() + ", " + r["name"];
							break;
						case dbTables.tblParagraphs:
							line = r["pg_id"].ToString() + ", " + r["doc_id"].ToString() + ", " + r["created_at"].ToString();
							break;
						case dbTables.tblSents:
							line = r["ph_id"].ToString() + ", " + r["pg_id"].ToString() + ", " + r["sorder"].ToString();
							break;
						case dbTables.tblPhraseContent:
							line = r["с_id"].ToString() + ", " + r["ph_id"].ToString() + ", " + r["lx_id"].ToString() + ", " + r["rcind"].ToString() + ", " + r["rw_id"].ToString() + ", " + r["sorder"].ToString();
							break;
						case dbTables.tblGrammems:
							line = r["gr_id"].ToString() + ", " + r["с_id"].ToString() + ", " + r["sg_id"].ToString() + ", " + r["intval"].ToString();
							break;
						case dbTables.tblSyntNodes:
							line = r["sn_id"].ToString() + ", " + r["с_id"].ToString() + ", " + r["ln_id"].ToString() + ", " + r["level"].ToString() + ", " + r["pс_id"].ToString();
							break;
						case dbTables.mBlockTypes:
							//line = r["bt_id"].ToString() + ", " + r["name"] ;
							line = r["bt_id"].ToString() + ", " + r["namekey"] + ", " + r["nameui"];
							break;
						case dbTables.mAttributes:
							line = r["ma_id"].ToString() + ", " + r["namekey"] + ", " + r["nameui"] + ", " + r["mt_id"].ToString() + ", " + r["bt_id"].ToString()
								+ ", " + r["sorder"].ToString() + ", " + r["mandatory"].ToString();
							break;
						case dbTables.mBlocks:
							line = r["b_id"].ToString() + ", " + r["bt_id"].ToString() + ", " + r["created_at"].ToString() + ", " + r["parent"].ToString() + ", " + r["treeorder"].ToString()
								+ ", " + r["fh_id"].ToString() + ", " + r["predecessor"].ToString() + ", " + r["successor"].ToString();
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

		#endregion

		#region Методы работы с справочниками GREN
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
		#endregion

		#region Методы работы с Типами блоков
		public BlockType dbCreateBlockType(string nameKey, string nameUI)
		{
			BlockType result = null;
			try
			{
				m_sqlCmd.CommandText = String.Format("INSERT INTO mBlockTypes(bt_id, namekey, nameui) VALUES(NULL, '{0}', '{1}')", nameKey, nameUI);
				m_sqlCmd.ExecuteNonQuery();
				result = new BlockType(m_dbConn.LastInsertRowId, nameKey, nameUI);
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbCreateBlockType Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Получение адреса типа блока по ключу.
		/// </summary>
		/// <returns>long</returns>
		public long dbGetSystemType(string namekey)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = "SELECT bt_id FROM mBlockTypes WHERE LOWER(namekey) = LOWER(@namekey)";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@namekey", namekey));
				var executeScalar = m_sqlCmd.ExecuteScalar();
				if (executeScalar != null)
					result = (long)executeScalar;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetSystemType Error: " + ex.Message);
			}
			return result;
		}

		public BlockType dbGetBlockTypeByNameKey(string nameKey)
		{
			BlockType result = null;
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT bt_id, namekey, nameui FROM mBlockTypes WHERE LOWER(namekey) = @nameKey", nameKey);
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@nameKey", nameKey.ToLower()));
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				// Читаем только первую запись
				if (r.Read())
				{
					result = new BlockType(r.GetInt32(0), r.GetString(1), r.GetString(2));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetBlockTypeByNameKey Error: " + ex.Message);
			}
			return result;
		}

		public BlockType dbGetBlockTypeByAddr(long addr)
		{
			BlockType result = null;
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT bt_id, namekey, nameui FROM mBlockTypes WHERE bt_id = {0}", addr);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				// Читаем только первую запись
				if (r.Read())
				{
					result = new BlockType(r.GetInt32(0), r.GetString(1), r.GetString(2));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetBlockTypeByAddr Error: " + ex.Message);
			}
			return result;
		}

		public List<BlockType> dbGetAllBlockTypes()
		{
			var reslist = new List<BlockType>();
			try
			{
				m_sqlCmd.CommandText = "SELECT bt_id, namekey, nameui FROM mBlockTypes ORDER BY bt_id";
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					reslist.Add(new BlockType(r.GetInt32(0), r.GetString(1), r.GetString(2)));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetAllBlockTypes Error: " + ex.Message);
			}
			return reslist;
		}

		public void dbBlockTypeChangeStrings(BlockType blocktype)
		{
			try
			{
				m_sqlCmd.CommandText = string.Format("UPDATE mBlockTypes SET namekey = @namekey, nameui = @nameui WHERE bt_id = {0}", blocktype.BlockTypeID);
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@namekey", blocktype.NameKey));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@nameui", blocktype.NameUI));
				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbBlockTypeChangeStrings Error: " + ex.Message);
				throw ex;
			}
		}


		#endregion

		#region Функции для работы с атрибутами типов блоков

		public List<AttrType> GetAllAttrTypes()
		{
			var attrtypes = new List<AttrType>();
			try
			{
				m_sqlCmd.CommandText = "SELECT mt_id, name FROM mAttrTypes ORDER BY mt_id;";
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					var attrtype = new AttrType(r.GetInt64(0), r.GetString(1));
					attrtypes.Add(attrtype);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("GetAllAttrTypes Error: " + ex.Message);
			}
			return attrtypes;
		}

		public long dbCreateAttribute(string nameKey, string nameUI, long AttrType, long BlockType, int sorder, bool mandatory = false)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = string.Format(
					"INSERT INTO mAttributes(ma_id, namekey, nameUI, mt_id, bt_id, sorder, mandatory) VALUES(NULL, '{0}', '{1}', {2}, {3}, {4}, {5})",
					nameKey, nameUI, AttrType, BlockType, sorder, mandatory == false ? 0 : 1);
				m_sqlCmd.ExecuteNonQuery();
				result = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbCreateAttribute Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Получение списка типов атрибутов блока.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>список типов атрибутов</returns>
		public List<int> dbGetAttrTypesList(long addr)
		{
			var reslist = new List<int>();
			try
			{
				m_sqlCmd.CommandText = string.Format("SELECT mt_id FROM mAttributes A JOIN mBlocks B ON B.bt_id = A.bt_id WHERE B.b_id = {0} ORDER BY A.sorder", addr);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					reslist.Add(r.GetInt32(0));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetAttrTypesList Error: " + ex.Message);
			}
			return reslist;
		}

		/// <summary>
		/// Получение списка типов атрибутов блока.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>список типов атрибутов</returns>
		public List<int> dbGetAttrTypesListByType(long blockType)
		{
			var reslist = new List<int>();
			try
			{
				using (var local_sqlCmd = new SQLiteCommand())
				{
					local_sqlCmd.CommandText = string.Format("SELECT mt_id FROM mAttributes A WHERE  A.bt_id = {0} ORDER BY A.sorder", blockType);
					local_sqlCmd.Connection = m_dbConn;
					SQLiteDataReader r = local_sqlCmd.ExecuteReader();
					while (r.Read())
					{
						reslist.Add(r.GetInt32(0));
					}
					r.Close();
				}
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetAttrTypesListByType Error: " + ex.Message);
			}
			return reslist;
		}

		/// <summary>
		/// Получение списка ключей атрибутов типа блока.
		/// </summary>
		/// <param name="addr">адрес типа блока</param>
		/// <returns>список ключей атрибутов типа блока</returns>
		public List<string> dbGetFildsNameKey(long blockType)
		{
			var reslist = new List<string>();
			try
			{
				m_sqlCmd.CommandText = string.Format("SELECT namekey FROM mAttributes WHERE bt_id = {0} ORDER BY sorder", blockType);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					reslist.Add(r.GetString(0));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetFildsNameKey Error: " + ex.Message);
			}
			return reslist;
		}

		/// <summary>
		/// Получение списка атрибутов типа блока.
		/// </summary>
		/// <param name="addr">адрес типа блока</param>
		/// <returns>список атрибутов типа блока</returns>
		public AttrsCollection dbGetAttrsCollection(long blockType)
		{
			var attrs = new AttrsCollection();
			try
			{
				using (var local_sqlCmd = new SQLiteCommand())
				{
					local_sqlCmd.CommandText =
					string.Format("SELECT A.ma_id, A.namekey, A.nameui, A.mt_id, A.bt_id, A.sorder, B.namekey, B.nameui FROM mAttributes A " +
					" JOIN mBlockTypes B ON B.bt_id = A.bt_id WHERE A.bt_id = {0} ORDER BY A.sorder", blockType);
					local_sqlCmd.Connection = m_dbConn;
					SQLiteDataReader r = local_sqlCmd.ExecuteReader();
					while (r.Read())
					{
						var bt = new BlockType(r.GetInt64(4), r.GetString(6), r.GetString(7));
						var attr = new BlockAttribute(r.GetInt64(0), r.GetString(1), r.GetString(2), r.GetInt32(3), bt);
						attr.Order = r.GetInt32(5);
						attrs.AddElement(attr);
					}
					r.Close();
				}
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetAttrsCollection Error: " + ex.Message);
			}
			return attrs;
		}

		public void AttributeUpdate(BlockAttribute attr)
		{
			try
			{
				m_sqlCmd.CommandText = string.Format(
					"UPDATE mAttributes SET namekey = '{0}', nameui = '{1}', mt_id = {2}, bt_id = {3}, sorder = {4}, mandatory = {5} WHERE ma_id = {6}",
					attr.NameKey, attr.NameUI, (int)attr.AttrType, attr.BlockType.BlockTypeID, attr.Order, attr.Mandatory == false ? 0 : 1, attr.AttrID);
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbAttrTypeUpdate Error: " + ex.Message);
				throw ex;
			}
		}

		#endregion

		#region Функции для работы с Блоками
		public long dbCreateBlock(long BlockType, long parent, int treeorder)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = string.Format("INSERT INTO mBlocks(b_id, bt_id, parent, treeorder) VALUES(NULL, '{0}', {1}, {2})",
					BlockType, parent, treeorder);
				m_sqlCmd.ExecuteNonQuery();
				result = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbCreateBlock Error: " + ex.Message);
			}
			return result;
		}

		public BlockBase dbGetBlock(long addr)
		{
			BlockBase result = null;
			try
			{
				m_sqlCmd.CommandText = string.Format("SELECT B.b_id, B.bt_id, T.namekey, B.parent, B.treeorder, B.fh_id, B.predecessor, B.successor, T.nameui, B.created_at " +
					"FROM mBlocks B JOIN mBlockTypes T ON T.bt_id = B.bt_id WHERE b_id = {0}", addr);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					var fh_id = r[5] as long? ?? 0;
					var predecessor = r[6] as long? ?? 0;
					var successor = r[7] as long? ?? 0;
					var bt = new BlockType(r.GetInt64(1), r.GetString(2), r.GetString(8));
					result = new BlockBase(addr, bt, r.GetInt64(3), r.GetInt64(4), fh_id, predecessor, successor, null, r.GetDateTime(9));
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetBlock Error: " + ex.Message);
			}
			return result;
		}

		public int dbGetOrder(long addr)
		{
			int result = -1;
			try
			{
				m_sqlCmd.CommandText = String.Format("SELECT treeorder FROM mBlocks WHERE b_id = {0}", addr);
				var executeScalar = m_sqlCmd.ExecuteScalar();
				if (executeScalar != null)
					result = Convert.ToInt32(executeScalar);
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetOrder Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Указание/отмена у блока состояниея "Удалён".
		/// </summary>
		/// <param name="addr">ID - адрес блока</param>
		/// <param name="mark">1 - удалён, 0 - нет</param>
		/// <returns></returns>
		public void MarkBlock4Delete(long addr, int mark)
		{
			m_sqlCmd.CommandText = string.Format("UPDATE mBlocks SET deleted = {0} WHERE b_id = {1}", mark, addr);
			m_sqlCmd.ExecuteNonQuery();
		}

		/// <summary>
		/// Присвоение фактического значения блоку.
		/// </summary>
		/// <param name="addr">ID - адрес блока</param>
		/// <returns>long</returns>
		public long dbSetFactData(long addr, byte[] blob, bool makeVersion)
		{
			long result = -1;
			long fh_id = -1;
			SQLiteTransaction transaction = null;
			//if (makeVersion) // надо делать новый блок и новые фактические данные
			try
			{
				transaction = m_dbConn.BeginTransaction();
				// Ищем адрес фактических данных
				m_sqlCmd.CommandText = string.Format("SELECT fh_id FROM mBlocks WHERE b_id = {0}", addr);
				// Читаем только первую запись
				var resp = m_sqlCmd.ExecuteScalar();
				if (resp == null) return result; // Record not found
				if (resp != DBNull.Value)
				{
					fh_id = (long)resp;
					m_sqlCmd.CommandText = "UPDATE mFactHeap SET blockdata = @blob WHERE fh_id = @fh_id";
					m_sqlCmd.Parameters.Clear();
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@blob", blob));
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@fh_id", fh_id));
					int rows = m_sqlCmd.ExecuteNonQuery();
				}
				else
				{
					m_sqlCmd.CommandText = "INSERT INTO mFactHeap(fh_id, blockdata) VALUES(NULL, @blob)";
					m_sqlCmd.Parameters.Clear();
					m_sqlCmd.Parameters.Add(new SQLiteParameter("@blob", blob));
					m_sqlCmd.ExecuteNonQuery();
					fh_id = m_dbConn.LastInsertRowId;

					// обновить адрес факт.данных в блоке
					m_sqlCmd.CommandText = string.Format("UPDATE mBlocks SET fh_id = {0} WHERE b_id = {1}", fh_id, addr);
					m_sqlCmd.ExecuteNonQuery();
				}
				result = addr;
				transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				result = -1;
				transaction.Rollback();
				Console.WriteLine("dbSetFactData Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Получение фактических данных блока.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>фактические данные</returns>
		public byte[] dbGetFactData(long addr)
		{
			byte[] result = null;
			m_sqlCmd.CommandText = string.Format("SELECT blockdata FROM mFactHeap F INNER JOIN mBlocks B ON B.fh_id = F.fh_id WHERE B.b_id = {0}", addr);
			try
			{
				var executeScalar = m_sqlCmd.ExecuteScalar();
				if (executeScalar != null && executeScalar != DBNull.Value)
					result = (byte[])executeScalar;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetFactData Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Чтения дочерних блоков множества родительских блоков.
		/// Выбираются только прямые наследники (без ссылок)
		/// </summary>
		/// <param name="list_ids">список ID родительских блоков</param>
		/// <returns>DataTable</returns>
		public List<BlockBase> dbGetChildren(List<string> list_ids)
		{
			var result = new List<BlockBase>();
			string ids = string.Join(",", list_ids.ToArray());
			if (String.IsNullOrEmpty(ids))
				return null;
			m_sqlCmd.CommandText = string.Format("SELECT B.b_id, B.bt_id, T.namekey, B.parent, B.treeorder, B.fh_id, B.predecessor, B.successor,  T.nameui, B.created_at, " +
				" F.blockdata FROM mBlocks B JOIN mBlockTypes T ON T.bt_id = B.bt_id LEFT JOIN mFactHeap F ON F.fh_id = B.fh_id " +
				" WHERE B.deleted = 0 AND B.parent IN ({0})", ids);
			try
			{
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					var bt_id = r.GetInt64(1);
					var fh_id = r[5] as long? ?? 0;
					var predecessor = r[6] as long? ?? 0;
					var successor = r[7] as long? ?? 0;
					var order = r[4] as long? ?? 0;
					var DateCreate = r[9] as DateTime? ?? DateTime.Now;
					var bt = new BlockType(bt_id, r.GetString(2), r.GetString(8));
					var attrs = dbGetAttrsCollection(bt_id);
					var block = new BlockBase(r.GetInt64(0), bt, r.GetInt64(3), order, fh_id, predecessor, successor, attrs, DateCreate);

					if (fh_id > 0)
					{
						// Getting attrtypes
						var idtplist = dbGetAttrTypesListByType(bt_id);
						var tplist = new List<enAttrTypes>();
						foreach (var idtp in idtplist)
							tplist.Add((enAttrTypes)idtp);

						// Reading BLOB
						var bytearr = GetBytes(r, 10);
						block.PerformBlob(tplist, bytearr);
					}
					result.Add(block);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbGetChildren Error: " + ex.Message);
			}
			return result;
		}

		/// <summary>
		/// Поиск блока по типу (если задан) и значениям атрибутов.
		/// Все значения атрибутов должны совпадать.
		/// </summary>
		/// <param name="blockType">тип блока</param>
		/// <param name="args">справочник аргументов поиска</param>
		/// <returns>список блоков</returns>
		public List<BlockBase> dbSearchBlocks(long blockType, Dictionary<string, object> args)
		{
			var result = new List<BlockBase>();
			var attrs = dbGetAttrsCollection(blockType);
			var tplist = attrs.GetAttrTypesList();
			try
			{
				m_sqlCmd.CommandText = string.Format("SELECT B.b_id, B.bt_id, T.namekey, B.parent, B.treeorder, " +
				"B.fh_id, B.predecessor, B.successor, T.nameui, B.created_at, F.blockdata " +
				"FROM mBlocks B JOIN mBlockTypes T ON T.bt_id = B.bt_id AND T.bt_id = {0} " +
				"INNER JOIN mFactHeap F ON F.fh_id = B.fh_id ", blockType);
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				Blob blob = null;
				while (r.Read())
				{
					var addr = r.GetInt64(0);
					// Reading BLOB
					var bytearr = GetBytes(r, 10);
					blob = new Blob(tplist, bytearr);
					var IsMatch = false;
					foreach (var arg in args)
					{
						var attrOrd = -1;
						IsMatch = false;
						try
						{
							attrOrd = attrs.GetOrdByNameKey(arg.Key);
							var curval = blob.GetAttrValue(attrOrd);
							IsMatch = curval != null && curval.Equals(arg.Value);
						}
						catch { }

						if (IsMatch == false)
							break;						
					}
					if (IsMatch == false)
						continue;

					var fh_id = r[5] as long? ?? 0;
					var predecessor = r[6] as long? ?? 0;
					var successor = r[7] as long? ?? 0;
					var bt = new BlockType(r.GetInt64(1), r.GetString(2), r.GetString(8));
					var block = new BlockBase(addr, bt, r.GetInt64(3), r.GetInt64(4), fh_id, predecessor, successor, null, r.GetDateTime(9));
					//					block.Blob = blob;
					block.PerformBlob(tplist, bytearr);
					result.Add(block);
				}
				r.Close();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbSearchBlocks Error: " + ex.Message);
			}

			return result;
		}

		static byte[] GetBytes(SQLiteDataReader reader, int col)
		{
			const int CHUNK_SIZE = 2 * 1024;
			byte[] buffer = new byte[CHUNK_SIZE];
			long bytesRead;
			long fieldOffset = 0;
			using (MemoryStream stream = new MemoryStream())
			{
				while ((bytesRead = reader.GetBytes(col, fieldOffset, buffer, 0, buffer.Length)) > 0)
				{
					stream.Write(buffer, 0, (int)bytesRead);
					fieldOffset += bytesRead;
				}
				return stream.ToArray();
			}
		}
		#endregion

		#region Функции для работы с Фактическими значениями
		public long dbInsertFactData(byte[] blob)
		{
			long result = -1;
			try
			{
				m_sqlCmd.CommandText = "INSERT INTO mFactHeap(fh_id, blockdata) VALUES(NULL, @blob)";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@blob", blob));
				m_sqlCmd.ExecuteNonQuery();
				result = m_dbConn.LastInsertRowId;
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("dbInsertFactData Error: " + ex.Message);
			}
			return result;
		}
		#endregion

		#region Функции для работы со Справочниками
		/// <summary>
		/// Создание справочника.
		/// </summary>
		/// <param name="name">Наименование справочника</param>
		/// <param name="blockType">ID - адрес типа блока - элемента справочника</param>
		/// <returns>long</returns>
		public long dbCreateDictionary(string name, int blockType)
		{
			long result = -1;
			SQLiteTransaction transaction = null;
			try
			{
				transaction = m_dbConn.BeginTransaction();
				var typeOfDict = dbGetSystemType("Dictionary");
				m_sqlCmd.CommandText = string.Format("INSERT INTO mBlocks(b_id, bt_id, parent, treeorder) VALUES(NULL, '{0}', {1}, {2})",
					typeOfDict, -1, 0);
				m_sqlCmd.ExecuteNonQuery();
				result = m_dbConn.LastInsertRowId;

				// Записать в атрибуты 
				// - ResolvedType 
				// - Name

				var list = new List<AttrFactData>();
				var attr0 = new AttrFactData(enAttrTypes.mnlong, blockType);
				var attr1 = new AttrFactData(enAttrTypes.mntxt, name);
				list.Add(attr0);
				list.Add(attr1);
				Blob blobpar = new Blob(list);
				var bdata = blobpar.Data;

				/*var bdata = BitConverter.GetBytes(blockType);
				if (BitConverter.IsLittleEndian)
					Array.Reverse(bdata); */

				m_sqlCmd.CommandText = "INSERT INTO mFactHeap(fh_id, blockdata) VALUES(NULL, @blob)";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@blob", bdata));
				m_sqlCmd.ExecuteNonQuery();
				var fh_id = m_dbConn.LastInsertRowId;

				// обновить адрес факт.данных в блоке
				m_sqlCmd.CommandText = "UPDATE mBlocks SET fh_id = @fh_id WHERE b_id = @b_id";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@b_id", result));
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@fh_id", fh_id));
				m_sqlCmd.ExecuteNonQuery();

				transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				result = -1;
				transaction.Rollback();
				Console.WriteLine("dbCreateDictionary Error: " + ex.Message);
			}
			return result;
		}

		#endregion


		/// <summary>
		/// !!!! Функция нигде не используется!!!
		/// Оставлена как пример - что делать с блоком после изменения фактических данных.
		/// 
		/// 
		/// Изменение элементов в Справочнике.
		/// </summary>
		/// <param name="addr">адрес Справочника</param>
		/// <param name="blob">новые фактические данные справочника - адреса элементов в виде массива байт</param>
		/// <remarks>
		/// При изменении (добавлении/удалении) элементов в Справочник будет создан новый блок-последователь существующего.
		/// Адрес нового блока заменит адрес блока в Справочнике.
		/// </remarks>
		/// <returns></returns>
		public void dbDictPerfomElements_unused(long addr, byte[] blob)
		{
			SQLiteTransaction transaction = null;
			try
			{
				transaction = m_dbConn.BeginTransaction();
				// Создание новых фактических данных блока
				m_sqlCmd.CommandText = "INSERT INTO mFactHeap(fh_id, blockdata) VALUES(NULL, @blob)";
				m_sqlCmd.Parameters.Clear();
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@blob", blob));
				m_sqlCmd.ExecuteNonQuery();
				var fh_id = m_dbConn.LastInsertRowId;

				// Получение адреса блока-справочника
				long b_id = -1;
				m_sqlCmd.CommandText = String.Format("SELECT b_id FROM mDicts WHERE md_id = {0}", addr);
				var executeScalar = m_sqlCmd.ExecuteScalar();
				if (executeScalar != null)
					b_id = (long)executeScalar;

				// Создать новый блок на основе существующего
				m_sqlCmd.CommandText = string.Format(
					"INSERT INTO mBlocks SELECT b_id = NULL, bt_id, created_at, parent, treeorder, fh_id, " +
						"predecessor, successor = NULL FROM mBlocks WHERE b_id = {0}", b_id);
				m_sqlCmd.ExecuteNonQuery();
				var newaddr = m_dbConn.LastInsertRowId;
				// Обновить данные нового блока
				m_sqlCmd.CommandText = string.Format("UPDATE mBlocks SET created_at = @date, fh_id = {0}, predecessor = {1} " +
					"WHERE b_id = {2}", fh_id, b_id, newaddr);
				m_sqlCmd.Parameters.Clear();
				var dt = String.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
				m_sqlCmd.Parameters.Add(new SQLiteParameter("@date", dt));
				m_sqlCmd.ExecuteNonQuery();
				// Обновить данные блока-родителя
				m_sqlCmd.CommandText = string.Format("UPDATE mBlocks SET successor = {0} WHERE b_id = {1}",
					newaddr, b_id);
				m_sqlCmd.ExecuteNonQuery();
				// Обновить адрес блока в справочнике
				m_sqlCmd.CommandText = string.Format("UPDATE mDicts SET b_id = {0} WHERE md_id = {1}", newaddr, addr);
				m_sqlCmd.ExecuteNonQuery();
				transaction.Commit();
			}
			catch (SQLiteException ex)
			{
				transaction.Rollback();
				Console.WriteLine("dbDictPerfomElements Error: " + ex.Message);
				throw ex;
			}
		}


	}
}

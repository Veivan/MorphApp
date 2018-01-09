using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using System.Data.SQLite;
using Schemas;

namespace DirectDBconnector
{
	/// <summary>
	/// Класс возвращает DataTable из БД
	/// </summary>
	public class DirectSQL
	{
		private SQLiteConnection m_dbConn;
		private SQLiteCommand m_sqlCmd = new SQLiteCommand();
		private TableSelector tableSelector = new TableSelector();

		public DirectSQL(SQLiteConnection _m_dbConn)
		{
			m_dbConn = _m_dbConn;
			m_sqlCmd.Connection = m_dbConn;
		}

		/// <summary>
		/// Чтения таблицы из БД.
		/// </summary>
		/// <param name="tblname">enum нужной таблицы</param>
		/// <returns>DataTable</returns>
		public DataTable GetDataTable(dbTables tblname)
		{
			var stmnt = tableSelector.GetSelectStatement(tblname);
			if (String.IsNullOrEmpty(stmnt))
				return null;
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
		/// Обновление таблицы в БД.
		/// </summary>
		/// <param name="dTable">набор данных</param>
		/// <param name="tblname">enum нужной таблицы</param>
		/// <returns></returns>
		public void UpdateDataTable(DataTable dTable, dbTables tblname)
		{
			if (dTable == null) return;
			var stmnt = tableSelector.GetSelectStatement(tblname);
			if (String.IsNullOrEmpty(stmnt))
				return;
			m_sqlCmd.CommandText = stmnt;
			var dataAdapter = new SQLiteDataAdapter(m_sqlCmd);
			var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
			dataAdapter.Update(dTable);
		}

		/// <summary>
		/// Чтения контейнеров выбранного родителя из БД.
		/// </summary>
		/// <param name="parentID">ID родителя</param>
		/// <returns>DataTable</returns>
		public DataTable GetChildrenContainers(long parentID)
		{
			var stmnt = String.Format("SELECT ct_id, created_at, name, parent_id FROM mContainers WHERE parent_id = {0}", parentID);
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
		/// Чтения контейнеров выбранного родителя из БД.
		/// </summary>
		/// <param name="parentID">ID родителя</param>
		/// <returns>Коллекцию ContainerMap</returns>
		public List<ContainerMap> GetChildrenContainersList(long parentID)
		{
			var reslist = new List<ContainerMap>();
			try
			{
				var stmnt = String.Format("SELECT ct_id, created_at, name, parent_id FROM mContainers WHERE parent_id = {0}", parentID);
				m_sqlCmd.CommandText = stmnt;
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					var dt = DateTime.Now;
					var created = r["created_at"].ToString();
					if (!String.IsNullOrEmpty(created))
						dt = DateTime.Parse(created);
					reslist.Add(new ContainerMap(r.GetInt64(0), r["name"].ToString(), dt, r.GetInt64(3)));
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

			var stmnt = String.Format("SELECT doc_id, ct_id, created_at, name FROM mDocuments WHERE ct_id IN ({0})", result);
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
        /// Чтения документов из выбранного контейнера.
        /// </summary>
        /// <param name="ct_id">ID контейнера</param>
        /// <returns>Коллекцию DocumentMap</returns>
        public List<DocumentMap> GetDocumentsL(long ct_id = -1)
        {
            var reslist = new List<DocumentMap>();
            try
            {
                var stmnt = String.Format("SELECT doc_id, ct_id, created_at, name, parent_id FROM mDocuments WHERE ct_id = {0}", ct_id);
                m_sqlCmd.CommandText = stmnt;
                SQLiteDataReader r = m_sqlCmd.ExecuteReader();
                while (r.Read())
                {
                    var dt = DateTime.Now;
                    var created = r["created_at"].ToString();
                    if (!String.IsNullOrEmpty(created))
                        dt = DateTime.Parse(created);
                    reslist.Add(new DocumentMap(r.GetInt64(0), r.GetInt64(1), r["name"].ToString(), dt));
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
		/// Чтения абзацев из множества документов.
		/// Если list_ids == null, то выбираются все записи.
		/// </summary>
		/// <param name="list_ids">список ID документов</param>
		/// <returns>DataTable</returns>
		public DataTable ReadParagraphsInDocs(List<string> list_ids)
		{
			DataTable dTable = new DataTable();
			var stmnt = "SELECT P.pg_id, P.doc_id, P.created_at, P.ph_id, D.ct_id FROM mParagraphs P JOIN mDocuments D ON D.doc_id = P.doc_id";
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
			var stmnt = "SELECT P.pg_id, P.doc_id, P.created_at, P.ph_id, D.ct_id FROM mParagraphs P JOIN mDocuments D ON D.doc_id = P.doc_id";
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
					reslist.Add(new ParagraphMap(r.GetInt64(0), r.GetInt64(1), r.GetInt64(2), dt));
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

	}
}

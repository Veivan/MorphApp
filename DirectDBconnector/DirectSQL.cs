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
	/// Класс используется в DirectDBA для работы с БД
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
  




	}
}

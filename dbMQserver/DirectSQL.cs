using System;
using System.Data;
using System.Collections.Generic;

using System.Data.SQLite;
using Schemas;

namespace dbMQserver
{
	/// <summary>
	/// Класс возвращает DataTable из БД
	/// </summary>
	public class DirectSQL
	{
		private SQLiteConnection m_dbConn;
		private SQLiteCommand m_sqlCmd = new SQLiteCommand();

		public DirectSQL(SQLiteConnection _m_dbConn)
		{
			m_dbConn = _m_dbConn;
			m_sqlCmd.Connection = m_dbConn;
		}

		public DataTable GetDocumentsT()
		{
			DataTable dTable = new DataTable();

			try
			{
				var sqlQuery = "SELECT * FROM mSpParts";
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
				adapter.Fill(dTable);
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return dTable;
		}

		public List<DocumentMap> GetDocumentsL()
		{
			var reslist = new List<DocumentMap>();
			try
			{
				m_sqlCmd.CommandText = "SELECT sp_id, speechpart FROM mSpParts";
				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				while (r.Read())
				{
					reslist.Add(new DocumentMap(r.GetInt64(0), 0, r["speechpart"].ToString(), DateTime.Now));
				}
			}
			catch (SQLiteException ex)
			{
				//MessageBox.Show("Error: " + ex.Message);
			}
			return reslist;
		}
	
	}
}

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.ComponentModel;

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


        #region Функции для Документов
        public DataTable GetDocumentsT()
		{
			DataTable dTable = new DataTable();

			try
			{
				SQLiteDataAdapter adapter = new SQLiteDataAdapter(""/*comSelDocuments*/, m_dbConn);
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
                m_sqlCmd.CommandText = ""; //comSelDocuments;
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

		public void UpdateDocumentsT(DataTable dTable)
		{
			if (dTable == null) return;
            m_sqlCmd.CommandText = "";// comSelDocuments;
			var dataAdapter = new SQLiteDataAdapter(m_sqlCmd);
			var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
			dataAdapter.Update(dTable);
		}

		public void UpdateDocuments(object dTable)
		{
			if (dTable == null) return;
            m_sqlCmd.CommandText = "";// comSelDocuments;
			var dataAdapter = new SQLiteDataAdapter(m_sqlCmd);
			var commandBuilder = new SQLiteCommandBuilder(dataAdapter);
			if (dTable is DataTable)
			{
				dataAdapter.Update(dTable as DataTable);
			}
			else
			{
				var realDT = MakeDocumentsTable(dTable);
				dataAdapter.Update(realDT);
			}
		}

		private DataTable MakeDocumentsTable(object datasource)
		{
			// Create sample Customers table.
			DataTable table = new DataTable();
			//table.TableName = "mDocuments";

			// Create two columns, ID and Name.
			DataColumn idColumn = table.Columns.Add("doc_id", typeof(long));
			table.Columns.Add("ct_id", typeof(long));
			table.Columns.Add("created_at", typeof(string));
			table.Columns.Add("name", typeof(string));

			// Set the ID column as the primary key column.
			table.PrimaryKey = new DataColumn[] { idColumn };

			foreach (var rec in (BindingList<DocumentMap>)datasource)
			{
				table.Rows.Add(new object[] { rec.DocumentID, rec.ContainerID, rec.Created_at, rec.Name });
			}
			table.AcceptChanges();
			return table;
		}
        #endregion
    }
}

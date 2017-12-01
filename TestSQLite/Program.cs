using System;
using System.Text;

using System.IO;
using System.Data.SQLite;
using System.Data;

namespace TestSQLite
{
	class Program
	{
		static void Main(string[] args)
		{
			String dbFileName = "sample.sqlite";
			SQLiteConnection m_dbConn;
			SQLiteCommand m_sqlCmd = new SQLiteCommand();

			if (!File.Exists(dbFileName))
				SQLiteConnection.CreateFile(dbFileName);

			try
			{
				m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
				m_dbConn.Open();
				m_sqlCmd.Connection = m_dbConn;

				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS Catalog (id INTEGER PRIMARY KEY AUTOINCREMENT, author TEXT, book TEXT)";
				m_sqlCmd.ExecuteNonQuery();

				//lbStatusText.Text = "Connected";
				//writeDataToDb(m_dbConn);
				//btAdd_Click(m_dbConn);
				btReadAll_Click();
			}
			catch (SQLiteException ex)
			{
				//lbStatusText.Text = "Disconnected";
				Console.WriteLine("Error: " + ex.Message);
			}

		}

		private static void btReadAll_Click()
		{
			String dbFileName = "sample.sqlite";
			SQLiteConnection m_dbConn;

			if (!File.Exists(dbFileName))
				SQLiteConnection.CreateFile(dbFileName);

			try
			{
				m_dbConn = new SQLiteConnection("Data Source=" + dbFileName + ";Version=3;");
				m_dbConn.Open();
				DataTable dTable = new DataTable();
				String sqlQuery;

				if (m_dbConn.State != ConnectionState.Open)
				{
					Console.WriteLine("Open connection with database");
					return;
				}

				try
				{
					sqlQuery = "SELECT * FROM Catalog";
					var m_sqlCmd = new SQLiteCommand(sqlQuery, m_dbConn);

					SQLiteDataReader r = m_sqlCmd.ExecuteReader();
					string line = String.Empty;
					while (r.Read())
					{ 
						line = r["id"].ToString() + ", "
							 + r["author"] + ", "
							 + r["book"];
						Console.WriteLine(line);
					}
					r.Close();
					/*SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
					adapter.Fill(dTable);

					if (dTable.Rows.Count > 0)
					{
							dgvViewer.Rows.Clear();

							for (int i = 0; i < dTable.Rows.Count; i++)
								dgvViewer.Rows.Add(dTable.Rows[i].ItemArray); 
					}
					else
						Console.WriteLine("Database is empty");*/
				}
				catch (SQLiteException ex)
				{
					Console.WriteLine("Error: " + ex.Message);
				}
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}

		private static void btAdd_Click(SQLiteConnection m_dbConn)
		{
			SQLiteCommand m_sqlCmd = new SQLiteCommand();
			m_sqlCmd.Connection = m_dbConn;
			if (m_dbConn.State != ConnectionState.Open)
			{
				Console.WriteLine("Open connection with database");
				return;
			}

			try
			{
				m_sqlCmd.CommandText = "INSERT INTO Catalog ('author', 'book') values ('" +
					"Иван Ветров" + "' , '" +
					"ЫЙД" + "')";

				m_sqlCmd.ExecuteNonQuery();
			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}

		private static void writeDataToDb(SQLiteConnection m_dbConn)
		{
			String query = "INSERT INTO Test ('message') values ('Hello message')";
			var m_sqlCmd = new SQLiteCommand(query, m_dbConn);
			m_sqlCmd.ExecuteNonQuery();
		}

	}
}

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

		private String dbFileName = "saga.sqlite";
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
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mLexems (\n"
						+ "	lx_id integer PRIMARY KEY, lex text NOT NULL);";
				m_sqlCmd.ExecuteNonQuery();
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mDictionary (\n"
						+ "	w_id integer PRIMARY KEY, lx_id integer, rword text NOT NULL);";
				m_sqlCmd.ExecuteNonQuery();
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mPhrases (\n"
						+ "	ph_id integer PRIMARY KEY, created_at DATETIME DEFAULT CURRENT_TIMESTAMP);";
				m_sqlCmd.ExecuteNonQuery();
				m_sqlCmd.CommandText = "CREATE TABLE IF NOT EXISTS mPhraseContent (\n"
						+ "	с_id integer PRIMARY KEY, ph_id integer, w_id integer);";
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
		public void selectAll()
		{
			try
			{
				//m_sqlCmd.CommandText = "SELECT с_id, ph_id, w_id FROM mPhraseContent";
				m_sqlCmd.CommandText = "SELECT lx_id, lex FROM mLexems";

				SQLiteDataReader r = m_sqlCmd.ExecuteReader();
				string line = String.Empty;
				while (r.Read())
				{
					//line = r["с_id"].ToString() + ", " + r["ph_id"].ToString() + ", " + r["w_id"].ToString();
					line = r["lx_id"].ToString() + ", " + r["lex"];
					Console.WriteLine(line);
				}
				r.Close();

			}
			catch (SQLiteException ex)
			{
				Console.WriteLine("Error: " + ex.Message);
			}
		}
	}
}

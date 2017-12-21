using System;
using System.Collections.Generic;
using Schemas;
using System.Data.SQLite;

namespace dbMQserver
{
    class ParagraphOperator  
    {
        private List<SentenceMap> sentlist = new List<SentenceMap>();
        private long ParagraphID;

        private ParagraphDBIndicator indicator = new ParagraphDBIndicator();
        SQLiteConnector dbConnector = SQLiteConnector.Instance;

        public ParagraphOperator(long ParagraphID, List<SentenceMap> sentlist)
        {
            this.ParagraphID = ParagraphID;
            this.sentlist.AddRange(sentlist);
            indicator.Fill(ParagraphID);
        }

        internal void Update()
        {
            if (ParagraphID == -1)
            {
                TruncateParaContent(sentlist.Count);
            }
            ParagraphID = SaveParagraphDB(pg_id);
            foreach (var sent in sentlist)
            {
                var ph_id = SavePhrase(sent.SentenceID);
                for (short i = 0; i < sent.Capasity; i++)
                {
                    var word = sent.GetWordByOrder(0);
                    var lx_id = SaveLex(word.EntryName.ToLower());
                    SavePhraseWords(ph_id, lx_id, word.order);
                }
            }
        }

        /// <summary>
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
        }

        private long SaveParagraphDB(long pg_id)
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
            if (pg_id == -1 || !IsParaExists)
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

    }
}

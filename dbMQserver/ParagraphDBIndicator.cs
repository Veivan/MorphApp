using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace dbMQserver
{
    /// <summary>
    /// Класс служит для определения необходимости и возможности обновления абзаца в БД.
    /// </summary>
    class ParagraphDBIndicator
    {
        internal OpersDB NeedOperate { get; private set; }
        internal bool CanOperate { get; private set; }

        internal void Fill(long ParagraphID)
        {
            if (ParagraphID == -1)
                NeedOperate = OpersDB.odInsert;
            else
            { 
                // проверить существование записи
                SQLiteConnector dbConnector = SQLiteConnector.Instance;
            }
        }
    }
}

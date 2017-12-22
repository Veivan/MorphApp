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
        public OpersDB NeedOperate { get; private set; }
        public bool CanOperate { get; private set; }

        SQLiteConnector dbConnector = SQLiteConnector.Instance;

        public void Fill(long ParagraphID)
        {
            CanOperate = true; // TODO Сделать проверку на блокировку
            NeedOperate = OpersDB.odNone;
            if (ParagraphID == -1)
                NeedOperate = OpersDB.odInsert;
            else
            { 
                // проверить существование записи
                if (dbConnector.IsParagraphExists(ParagraphID))
                    NeedOperate = OpersDB.odUpdate;
                else
                    NeedOperate = OpersDB.odInsert;
            }
        }
    }
}

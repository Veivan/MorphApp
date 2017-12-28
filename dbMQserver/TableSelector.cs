using System;
using Schemas;

namespace dbMQserver
{
    class TableSelector
    {
        const string comSelContainers = "SELECT ct_id, created_at, name FROM mContainers";
        const string comSelDocuments = "SELECT doc_id, ct_id, created_at, name FROM mDocuments";

        public string GetSelectStatement(dbTables tblname)
        {
            switch (tblname)
            {
                case dbTables.tblContainers: return comSelContainers;
                case dbTables.tblDocuments: return comSelDocuments;
                default: return "";
            }
        }
    }
}

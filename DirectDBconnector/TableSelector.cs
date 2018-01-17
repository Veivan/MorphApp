using System;
using Schemas;

namespace DirectDBconnector
{
    class TableSelector
    {
		const string comSelContainers = "SELECT ct_id, created_at, name, parent_id FROM mContainers";
		const string comSelDocuments = "SELECT doc_id, ct_id, created_at, name FROM mDocuments";
        const string comSelParagraphs = "SELECT pg_id, doc_id, created_at FROM mParagraphs";
        const string comSelSents = "SELECT ph_id, pg_id, sorder, created_at FROM mPhrases";
        const string comSelPhraseContent = "SELECT с_id, ph_id, lx_id, sorder FROM mPhraseContent";
		const string comSelLemms = "SELECT lx_id, sp_id, lemma FROM mLemms";
		const string comSelGrammems = "SELECT gr_id, с_id, sg_id, intval FROM mGrammems";

        public string GetSelectStatement(dbTables tblname)
        {
            switch (tblname)
            {
                case dbTables.tblContainers: return comSelContainers;
				case dbTables.tblDocuments: return comSelDocuments;
				case dbTables.tblParagraphs: return comSelParagraphs;
                case dbTables.tblSents: return comSelSents;
                case dbTables.tblPhraseContent: return comSelPhraseContent;
				case dbTables.tblLemms: return comSelLemms;
				case dbTables.tbGrammems: return comSelGrammems;
				default: return "";
            }
        }
    }
}

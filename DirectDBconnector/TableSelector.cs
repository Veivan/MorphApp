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
        const string comSelPhraseContent = "SELECT с_id, ph_id, lx_id, sorder, rcind, rw_id FROM mPhraseContent";
		const string comSelLemms = "SELECT lx_id, sp_id, lemma FROM mLemms";
        const string comSelGrammems = "SELECT gr_id, с_id, sg_id, intval FROM mGrammems";
        const string comSelSyntNodes = "SELECT sn_id, с_id, ln_id, level, pс_id FROM mSyntNodes";
        const string comSelRealWord = "SELECT rw_id, wform FROM mRealWord";
        const string comSelTermContent = "SELECT tc_id, tm_id, lx_id, sorder FROM mTerminContent";

        public static string GetSelectStatement(dbTables tblname)
        {
            switch (tblname)
            {
                case dbTables.tblContainers: return comSelContainers;
				case dbTables.tblDocuments: return comSelDocuments;
				case dbTables.tblParagraphs: return comSelParagraphs;
                case dbTables.tblSents: return comSelSents;
                case dbTables.tblPhraseContent: return comSelPhraseContent;
				case dbTables.tblLemms: return comSelLemms;
                case dbTables.tblGrammems: return comSelGrammems;
                case dbTables.tblSyntNodes: return comSelSyntNodes;
                case dbTables.tblRealWord: return comSelRealWord;
                case dbTables.tblTermContent: return comSelTermContent;
                default: return "";
            }
        }
    }
}

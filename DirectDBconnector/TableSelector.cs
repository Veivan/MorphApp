﻿using System;
using Schemas;

namespace DirectDBconnector
{
    class TableSelector
    {
		const string comSelParts = "SELECT sp_id, speechpart FROM mSpParts";

		const string comSelContainers = "SELECT ct_id, created_at, name, parent_id FROM mContainers";
		const string comSelDocuments = "SELECT doc_id, ct_id, created_at, name FROM mDocuments";
        const string comSelParagraphs = "SELECT pg_id, doc_id, created_at FROM mParagraphs";
        const string comSelSents = "SELECT ph_id, pg_id, sorder, created_at FROM mPhrases";
        const string comSelPhraseContent = "SELECT с_id, ph_id, lx_id, sorder, rcind, rw_id FROM mPhraseContent";
		const string comSelLemms = "SELECT lx_id, sp_id, lemma FROM mLemms";
        const string comSelGrammems = "SELECT gr_id, с_id, sg_id, intval FROM mGrammems";
        const string comSelSyntNodes = "SELECT sn_id, с_id, ln_id, level, pс_id FROM mSyntNodes";
        const string comSelRealWord = "SELECT rw_id, wform FROM mRealWord";
        const string comSelTermContent = "SELECT tc_id, tm_id, sorder, rcind, lem_id FROM mTerminContent";
        const string comSelUndefContent = "SELECT uv_id, mu_id, rw_id FROM mUndefContent";

		const string comSelmBlockTypes = "SELECT bt_id, namekey, nameui FROM mBlockTypes";
		const string comSelmAttrTypes = "SELECT mt_id, name FROM mAttrTypes";
		const string comSelmAttributes = "SELECT ma_id, namekey, nameui, mt_id, bt_id, sorder, mandatory FROM mAttributes";
		const string comSelmBlocks = "SELECT b_id, bt_id, created_at, parent, treeorder, fh_id, predecessor, successor, deleted FROM mBlocks";
		const string comSelmFactHeap = "SELECT fh_id, blockdata FROM mFactHeap";

		public static string GetSelectStatement(dbTables tblname)
        {
            switch (tblname)
            {
				case dbTables.tblParts: return comSelParts;

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
                case dbTables.tblUndefContent: return comSelUndefContent;

				case dbTables.mBlockTypes: return comSelmBlockTypes;
				case dbTables.mAttrTypes: return comSelmAttrTypes;
				case dbTables.mAttributes: return comSelmAttributes;
				case dbTables.mBlocks: return comSelmBlocks;
				case dbTables.mFactHeap: return comSelmFactHeap;
				default: return "";
            }
        }
    }
}

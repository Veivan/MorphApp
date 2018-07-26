using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
	public enum GrenPart
	{
		NUM_WORD_CLASS = 2,              // class num_word
		NOUN_ru = 6,                     // class СУЩЕСТВИТЕЛЬНОЕ
		PRONOUN2_ru = 7,                 // class МЕСТОИМ_СУЩ
		PRONOUN_ru = 8,                  // class МЕСТОИМЕНИЕ
		ADJ_ru = 9,                      // class ПРИЛАГАТЕЛЬНОЕ
		NUMBER_CLASS_ru = 10,            // class ЧИСЛИТЕЛЬНОЕ
		INFINITIVE_ru = 11,              // class ИНФИНИТИВ
		VERB_ru = 12,                    // class ГЛАГОЛ
		GERUND_2_ru = 13,                // class ДЕЕПРИЧАСТИЕ
		PREPOS_ru = 14,                  // class ПРЕДЛОГ
		IMPERSONAL_VERB_ru = 15,         // class БЕЗЛИЧ_ГЛАГОЛ
		PARTICLE_ru = 18,                // class ЧАСТИЦА
		CONJ_ru = 19,                    // class СОЮЗ
		ADVERB_ru = 20,                  // class НАРЕЧИЕ
		PUNCTUATION_class = 21,          // class ПУНКТУАТОР
		POSTPOS_ru = 26,                 // class ПОСЛЕЛОГ
		POSESS_PARTICLE = 27,            // class ПРИТЯЖ_ЧАСТИЦА
		MEASURE_UNIT = 28,               // class ЕДИНИЦА_ИЗМЕРЕНИЯ
		COMPOUND_ADJ_PREFIX = 29,        // class ПРЕФИКС_СОСТАВ_ПРИЛ
		COMPOUND_NOUN_PREFIX = 30,       // class ПРЕФИКС_СОСТАВ_СУЩ
		/*		VERB_en = 31,                    // class ENG_VERB
		BEVERB_en = 32,                  // class ENG_BEVERB
		AUXVERB_en = 33,                 // class ENG_AUXVERB
		NOUN_en = 34,                    // class ENG_NOUN
		PRONOUN_en = 35,                 // class ENG_PRONOUN
		ARTICLE_en = 36,                 // class ENG_ARTICLE
		PREP_en = 37,                    // class ENG_PREP
		POSTPOS_en = 38,                 // class ENG_POSTPOS
		CONJ_en = 39,                    // class ENG_CONJ
		ADV_en = 40,                     // class ENG_ADVERB
		ADJ_en = 41,                     // class ENG_ADJECTIVE
		PARTICLE_en = 42,                // class ENG_PARTICLE
		NUMERAL_en = 43,                 // class ENG_NUMERAL
		INTERJECTION_en = 44,            // class ENG_INTERJECTION
		POSSESSION_PARTICLE_en = 45,     // class ENG_POSSESSION
		COMPOUND_PRENOUN_en = 46,        // class ENG_COMPOUND_PRENOUN
		COMPOUND_PREADJ_en = 47,         // class ENG_COMPOUND_PREADJ
		COMPOUND_PREVERB_en = 48,        // class ENG_COMPOUND_PREVERB
		COMPOUND_PREADV_en = 49,         // class ENG_COMPOUND_PREADV
		NUMERAL_fr = 50,                 // class FR_NUMERAL
		ARTICLE_fr = 51,                 // class FR_ARTICLE
		PREP_fr = 52,                    // class FR_PREP
		ADV_fr = 53,                     // class FR_ADVERB
		CONJ_fr = 54,                    // class FR_CONJ
		NOUN_fr = 55,                    // class FR_NOUN
		ADJ_fr = 56,                     // class FR_ADJ
		PRONOUN_fr = 57,                 // class FR_PRONOUN
		VERB_fr = 58,                    // class FR_VERB
		PARTICLE_fr = 59,                // class FR_PARTICLE
		PRONOUN2_fr = 60,                // class FR_PRONOUN2
		NOUN_es = 61,                    // class ES_NOUN
		ROOT_es = 62,                    // class ES_ROOT
		JAP_NOUN = 63,                   // class JAP_NOUN
		JAP_NUMBER = 64,                 // class JAP_NUMBER
		JAP_ADJECTIVE = 65,              // class JAP_ADJECTIVE
		JAP_ADVERB = 66,                 // class JAP_ADVERB
		JAP_CONJ = 67,                   // class JAP_CONJ
		JAP_VERB = 68,                   // class JAP_VERB
		JAP_PRONOUN = 69,                // class JAP_PRONOUN
		JAP_VERB_POSTFIX2 = 72,          // class JAP_VERB_POSTFIX2
		JAP_PARTICLE = 74,               // class JAP_PARTICLE */
		UNKNOWN_ENTRIES_CLASS = 85       // class UnknownEntries
	}

	public enum GrenProperty
	{
		CharCasing = 4,                  // enum CharCasing
		PERSON_ru = 27,                  // enum ЛИЦО
		NUMBER_ru = 28,                  // enum ЧИСЛО
		GENDER_ru = 29,                  // enum РОД
		TRANSITIVENESS_ru = 30,          // enum ПЕРЕХОДНОСТЬ
		PARTICIPLE_ru = 31,              // enum ПРИЧАСТИЕ
		PASSIVE_PARTICIPLE_ru = 32,      // enum СТРАД
		ASPECT_ru = 33,                  // enum ВИД
		VERB_FORM_ru = 35,               // enum НАКЛОНЕНИЕ
		TENSE_ru = 36,                   // enum ВРЕМЯ
		SHORTNESS_ru = 37,               // enum КРАТКИЙ
		CASE_ru = 39,                    // enum ПАДЕЖ
		FORM_ru = 40,                    // enum ОДУШ
		COUNTABILITY_ru = 41,            // enum ПЕРЕЧИСЛИМОСТЬ
		COMPAR_FORM_ru = 42,             // enum СТЕПЕНЬ
		CASE_GERUND_ru = 43,             // enum ПадежВал
		MODAL_ru = 44                    // enum МОДАЛЬНЫЙ
	}

	public enum GrenLink
	{
		OBJECT_link = 0,
		ATTRIBUTE_link = 1,
		CONDITION_link = 2,
		CONSEQUENCE_link = 3,
		SUBJECT_link = 4,
		RHEMA_link = 5,
		COVERB_link = 6,
		NUMBER2OBJ_link = 12,
		TO_VERB_link = 16,
		TO_INF_link = 17,
		TO_PERFECT = 18,
		TO_UNPERFECT = 19,
		TO_NOUN_link = 20,
		TO_ADJ_link = 21,
		TO_ADV_link = 22,
		TO_RETVERB = 23,
		TO_GERUND_2_link = 24,
		WOUT_RETVERB = 25,
		TO_ENGLISH_link = 26,
		TO_RUSSIAN_link = 27,
		TO_FRENCH_link = 28,
		SYNONYM_link = 29,
		SEX_SYNONYM_link = 30,
		CLASS_link = 31,
		MEMBER_link = 32,
		TO_SPANISH_link = 33,
		TO_GERMAN_link = 34,
		TO_CHINESE_link = 35,
		TO_POLAND_link = 36,
		TO_ITALIAN_link = 37,
		TO_PORTUGUAL_link = 38,
		ACTION_link = 39,
		ACTOR_link = 40,
		TOOL_link = 41,
		RESULT_link = 42,
		TO_JAPANESE_link = 43,
		TO_KANA_link = 44,
		TO_KANJI_link = 45,
		ANTONYM_link = 46,
		EXPLANATION_link = 47,
		WWW_link = 48,
		ACCENT_link = 49,
		YO_link = 50,
		TO_DIMINUITIVE_link = 51,
		TO_RUDE_link = 52,
		TO_BIGGER_link = 53,
		TO_NEUTRAL_link = 54,
		TO_SCOLARLY = 55,
		TO_SAMPLE_link = 56,
		USAGE_TAG_link = 57,
		PROPERTY_link = 58,
		TO_CYRIJI_link = 59,
		HABITANT_link = 60,
		CHILD_link = 61,
		PARENT_link = 62,
		UNIT_link = 63,
		SET_link = 64,
		TO_WEAKENED_link = 65,
		VERBMODE_BASIC_link = 66,
		NEGATION_PARTICLE_link = 67,
		NEXT_COLLOCATION_ITEM_link = 68,
		SUBORDINATE_CLAUSE_link = 69,
		RIGHT_GENITIVE_OBJECT_link = 70,
		ADV_PARTICIPLE_link = 71,
		POSTFIX_PARTICLE_link = 72,
		INFINITIVE_link = 73,
		NEXT_ADJECTIVE_link = 74,
		NEXT_NOUN_link = 75,
		THEMA_link = 76,
		RIGHT_AUX2INFINITIVE_link = 77,
		RIGHT_AUX2PARTICIPLE = 78,
		RIGHT_AUX2ADJ = 79,
		RIGHT_LOGIC_ITEM_link = 80,
		RIGHT_COMPARISON_Y_link = 81,
		RIGHT_NOUN_link = 82,
		RIGHT_NAME_link = 83,
		ADJ_PARTICIPLE_link = 84,
		PUNCTUATION_link = 85,
		IMPERATIVE_SUBJECT_link = 86,
		IMPERATIVE_VERB2AUX_link = 87,
		AUX2IMPERATIVE_VERB = 88,
		PREFIX_PARTICLE_link = 89,
		PREFIX_CONJUNCTION_link = 90,
		LOGICAL_CONJUNCTION_link = 91,
		NEXT_CLAUSE_link = 92,
		LEFT_AUX_VERB_link = 93,
		BEG_INTRO_link = 94,
		RIGHT_PREPOSITION_link = 95,
		WH_SUBJECT_link = 96,
		IMPERATIVE_PARTICLE_link = 97,
		GERUND_link = 98,
		PREPOS_ADJUNCT_link = 99,
		DIRECT_OBJ_INTENTION_link = 100,
		COPULA_link = 101,
		DETAILS_link = 102,
		SENTENCE_CLOSER_link = 103,
		OPINION_link = 104,
		APPEAL_link = 105,
		TERM_link = 106,
		SPEECH_link = 107,
		QUESTION_link = 108,
		POLITENESS_link = 109,
		SEPARATE_ATTR_link = 110,
		POSSESSION_POSTFIX_link = 111,
		COMPOUND_PREFIX_link = 112,
		UNKNOWN_SLOT_link = 113,
		SECOND_VERB_link = 114
	}

    public enum dbTables
    {
        tblContainers,
        tblDocuments,
        tblParagraphs,
        tblSents,
        tblPhraseContent,
        tblLemms,
        tblGrammems,
        tblSyntNodes,
        tblRealWord,
        tblTermContent,
        tblUndefContent,

		mBlockTypes,
		mAttrTypes,
		mAttributes,
		mBlocks,
		mFactHeap,
		mDicts,

		tblParts,
		tblSiGram,
		tblSiLinks
	}

	/// <summary>
	/// Тип возвращаемого значения методов чтения из БД.
	/// </summary>
	public enum tpList {tplDBtable,	tblList};

	/// <summary>
	/// enstAll - выбор всех предложений абзаца.
	/// enstNotActualHead - выбор неактуальных предложений заголовка абзаца.
	/// enstNotActualBody - выбор неактуальных предложений содержимого абзаца.
	/// enstHeader - выбор заголовка абзаца.
    /// enstBody - выбор предложений содержимого абзаца.
	/// </summary>
	public enum SentTypes { enstAll, enstNotActualHead, enstNotActualBody, enstHeader, enstBody };

	/// <summary>
	/// Тип узла в клиентском дереве отображения объектов хранилища.
	/// </summary>
	public enum clNodeType { clnContainer, clnDocument, clnParagraph };

	/// <summary>
	/// Тип операции в клиентском дереве.
	/// </summary>
	public enum treeOper { toAdd, toEdit, toDelete };
	
	/// <summary>
	/// Типы атрибутов блоков.
	/// </summary>
	public enum enAttrTypes {mntxt=1, mnint=2, mnreal=3, mnbool=4, mnblob=5, mnlink=6, mnarr=7};

	/// <summary>
	/// Структура используется в обмене сообщениями с серверами.
	/// </summary>
	public struct SimpleParam
	{
		public string Name;
		public string Value;
	}

	/// <summary>
	/// Структура используется для работы с фактическими данными атрибутов блока.
	/// </summary>
	public struct AttrFactData
	{
		public enAttrTypes Type;
		public object Value;
	}

}

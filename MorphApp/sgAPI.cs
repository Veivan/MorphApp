using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SolarixGrammarEngineNET;

namespace Gren
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

    //public const int CharCasing = 4;                  // enum CharCasing
    public enum enCharCasing
    {
        // Coordiname CharCasing states:
        DECAPITALIZED_CASED = 0,         // CharCasing : Lower
        FIRST_CAPITALIZED_CASED = 1,     // CharCasing : FirstCapitalized
        ALL_CAPITALIZED_CASED = 2,       // CharCasing : Upper
        EACH_LEXEM_CAPITALIZED_CASED = 3 // CharCasing : EachLexemCapitalized
    }

    //public const int PERSON_ru = 27;                  // enum ЛИЦО
    public enum enPERSON_ru
    {
        // Coordiname ЛИЦО states:
        PERSON_1_ru = 0,                 // ЛИЦО : 1
        PERSON_2_ru = 1,                 // ЛИЦО : 2
        PERSON_3_ru = 2                  // ЛИЦО : 3
    }

    //public const int NUMBER_ru = 28;                  // enum ЧИСЛО
    public enum enNUMBER_ru
    {
        // Coordiname ЧИСЛО states:
        SINGULAR_NUMBER_ru = 0,          // ЧИСЛО : ЕД
        PLURAL_NUMBER_ru = 1             // ЧИСЛО : МН
    }

    //public const int GENDER_ru = 29;                  // enum РОД
    public enum enGENDER_ru
    {
        // Coordiname РОД states:
        MASCULINE_GENDER_ru = 0,         // РОД : МУЖ
        FEMININE_GENDER_ru = 1,          // РОД : ЖЕН
        NEUTRAL_GENDER_ru = 2            // РОД : СР
    }

    //public const int TRANSITIVENESS_ru = 30;          // enum ПЕРЕХОДНОСТЬ
    public enum enTRANSITIVENESS_ru
    {
        // Coordiname ПЕРЕХОДНОСТЬ states:
        NONTRANSITIVE_VERB_ru = 0,      // ПЕРЕХОДНОСТЬ : НЕПЕРЕХОДНЫЙ
        TRANSITIVE_VERB_ru = 1         // ПЕРЕХОДНОСТЬ : ПЕРЕХОДНЫЙ
    }

    //public const int PARTICIPLE_ru = 31;              // enum ПРИЧАСТИЕ

    //public const int PASSIVE_PARTICIPLE_ru = 32;      // enum СТРАД

    //public const int ASPECT_ru = 33;                  // enum ВИД
    public enum enASPECT_ru
    {
        // Coordiname ВИД states:
        PERFECT_ru = 0,                  // ВИД : СОВЕРШ
        IMPERFECT_ru = 1                 // ВИД : НЕСОВЕРШ
    }

    //public const int VERB_FORM_ru = 35;               // enum НАКЛОНЕНИЕ
    public enum enVERB_FORM_ru
    {
        // Coordiname НАКЛОНЕНИЕ states:
        VB_INF_ru = 0,                   // НАКЛОНЕНИЕ : ИЗЪЯВ
        VB_ORDER_ru = 1                  // НАКЛОНЕНИЕ : ПОБУД
    }

    //public const int TENSE_ru = 36;                   // enum ВРЕМЯ
    public enum enTENSE_ru
    {
        // Coordiname ВРЕМЯ states:
        PAST_ru = 0,                     // ВРЕМЯ : ПРОШЕДШЕЕ
        PRESENT_ru = 1,                  // ВРЕМЯ : НАСТОЯЩЕЕ
        FUTURE_ru = 2                    // ВРЕМЯ : БУДУЩЕЕ
    }

    //public const int SHORTNESS_ru = 37;               // enum КРАТКИЙ

    //public const int CASE_ru = 39;                    // enum ПАДЕЖ
    public enum enCASE_ru
    {
        // Coordiname ПАДЕЖ states:
        NOMINATIVE_CASE_ru = 0,          // ПАДЕЖ : ИМ
        VOCATIVE_CASE_ru = 1,            // ЗВАТ
        GENITIVE_CASE_ru = 2,            // ПАДЕЖ : РОД
        PARTITIVE_CASE_ru = 3,           // ПАРТ
        INSTRUMENTAL_CASE_ru = 5,        // ПАДЕЖ : ТВОР
        ACCUSATIVE_CASE_ru = 6,          // ПАДЕЖ : ВИН
        DATIVE_CASE_ru = 7,              // ПАДЕЖ : ДАТ
        PREPOSITIVE_CASE_ru = 8,         // ПАДЕЖ : ПРЕДЛ
        LOCATIVE_CASE_ru = 10           // МЕСТ
    }

    //public const int FORM_ru = 40;                    // enum ОДУШ
    public enum enFORM_ru
    {
        // Coordiname ОДУШ states:
        ANIMATIVE_FORM_ru = 0,           // ОДУШ : ОДУШ
        INANIMATIVE_FORM_ru = 1          // ОДУШ : НЕОДУШ
    }

    //public const int COUNTABILITY_ru = 41;            // enum ПЕРЕЧИСЛИМОСТЬ
    public enum enCOUNTABILITY_ru
    {
        // Coordiname ПЕРЕЧИСЛИМОСТЬ states:
        COUNTABLE_ru = 0,               // ПЕРЕЧИСЛИМОСТЬ : ДА
        UNCOUNTABLE_ru = 1              // ПЕРЕЧИСЛИМОСТЬ : НЕТ
    }

    //public const int COMPAR_FORM_ru = 42;             // enum СТЕПЕНЬ
    public enum enCOMPAR_FORM_ru
    {
        // Coordiname СТЕПЕНЬ states:
        ATTRIBUTIVE_FORM_ru = 0,         // СТЕПЕНЬ : АТРИБ
        COMPARATIVE_FORM_ru = 1,         // СТЕПЕНЬ : СРАВН
        SUPERLATIVE_FORM_ru = 2,         // СТЕПЕНЬ : ПРЕВОСХ
        LIGHT_COMPAR_FORM_RU = 3,        // СТЕПЕНЬ : КОМПАРАТИВ2
        ABBREV_FORM_ru = 4               // СТЕПЕНЬ : СОКРАЩ
    }

    /*             
                public const int VERBMODE_TENSE = 45;             // enum СГД_Время
               // Coordiname СГД_Время states:

                public const int VERBMODE_DIRECTION = 46;         // enum СГД_Направление
               // Coordiname СГД_Направление states:

                public const int NUMERAL_CATEGORY = 47;           // enum КАТЕГОРИЯ_ЧИСЛ
               // Coordiname КАТЕГОРИЯ_ЧИСЛ states:
                public const int CARDINAL = 0;                    // КАТЕГОРИЯ_ЧИСЛ : КОЛИЧ
                public const int COLLECTION = 1;                  // КАТЕГОРИЯ_ЧИСЛ : СОБИР

                public const int ADV_SEMANTICS = 48;              // enum ОБСТ_ВАЛ
               // Coordiname ОБСТ_ВАЛ states:
                public const int S_LOCATION = 0;                  // ОБСТ_ВАЛ : МЕСТО
                public const int S_DIRECTION = 1;                 // ОБСТ_ВАЛ : НАПРАВЛЕНИЕ
                public const int S_DIRECTION_FROM = 2;            // ОБСТ_ВАЛ : НАПРАВЛЕНИЕ_ОТКУДА
                public const int S_FINAL_POINT = 3;               // ОБСТ_ВАЛ : КОНЕЧНАЯ_ТОЧКА
                public const int S_DISTANCE = 4;                  // ОБСТ_ВАЛ : РАССТОЯНИЕ
                public const int S_VELOCITY = 5;                  // ОБСТ_ВАЛ : СКОРОСТЬ
                public const int S_MOMENT = 6;                    // ОБСТ_ВАЛ : МОМЕНТ_ВРЕМЕНИ
                public const int S_DURATION = 7;                  // ОБСТ_ВАЛ : ДЛИТЕЛЬНОСТЬ
                public const int S_TIME_DIVISIBILITY = 8;         // ОБСТ_ВАЛ : КРАТНОСТЬ
                public const int S_ANALOG = 9;                    // ОБСТ_ВАЛ : СОПОСТАВЛЕНИЕ
                public const int S_FACTOR = 10;                   // ОБСТ_ВАЛ : МНОЖИТЕЛЬ

                public const int ADJ_TYPE = 49;                   // enum ТИП_ПРИЛ
               // Coordiname ТИП_ПРИЛ states:
                public const int ADJ_POSSESSIVE = 0;              // ТИП_ПРИЛ : ПРИТЯЖ
                public const int ADJ_ORDINAL = 1;                 // ТИП_ПРИЛ : ПОРЯДК

                public const int PRONOUN_TYPE_ru = 51;            // enum ТИП_МЕСТОИМЕНИЯ
               // Coordiname ТИП_МЕСТОИМЕНИЯ states:

                public const int VERB_TYPE_ru = 52;               // enum ТИП_ГЛАГОЛА
               // Coordiname ТИП_ГЛАГОЛА states:
                public const int COPULA_VERB_ru = 2;              // ТИП_ГЛАГОЛА : СВЯЗОЧН

                public const int PARTICLE_TYPE = 53;              // enum ТИП_ЧАСТИЦЫ
               // Coordiname ТИП_ЧАСТИЦЫ states:
                public const int PREFIX_PARTICLE = 0;             // ТИП_ЧАСТИЦЫ : ПРЕФИКС
                public const int POSTFIX_PARTICLE = 1;            // ТИП_ЧАСТИЦЫ : ПОСТФИКС
 
                */
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

    class sgAPI
    {
        public static GrenLink GetLinkType(int cnst)
        {
            if (!Enum.IsDefined(typeof(GrenLink), cnst))
            {
                throw new InvalidOperationException("Enum out of range.");
            }

            return (GrenLink)cnst;
        }

        public static string GetLinkTypeStr(int cnst)
        {
            string result = "";
            if (Enum.IsDefined(typeof(GrenLink), cnst))
            {
                result = ((GrenLink)cnst).ToString();
            }

            return result;
        }


        public static Type GetTypeClassByID(GrenPart part)
        {

            Type result;
            switch (part)
            {
                case GrenPart.NUM_WORD_CLASS:  // class num_word ???
                    result = typeof(NOUN_ru);
                    break;
                case GrenPart.NOUN_ru:
                    result = typeof(NOUN_ru); // class СУЩЕСТВИТЕЛЬНОЕ
                    break;
                case GrenPart.PRONOUN2_ru:
                    result = typeof(PRONOUN2_ru); // class МЕСТОИМ_СУЩ
                    break;
                case GrenPart.PRONOUN_ru:
                    result = typeof(PRONOUN_ru); // class МЕСТОИМЕНИЕ
                    break;

                case GrenPart.VERB_ru:
                    result = typeof(VERB_ru); // class ГЛАГОЛ
                    break;
                /*
                         ADJ_ru = 9,                      // class ПРИЛАГАТЕЛЬНОЕ
                         NUMBER_CLASS_ru = 10,            // class ЧИСЛИТЕЛЬНОЕ
                         INFINITIVE_ru = 11,              // class ИНФИНИТИВ
                          = 12,                    // class ГЛАГОЛ
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

                                     */
                default:
                    result = null;
                    break;
            }

             return result;
        }

    }

    /// <summary>
    /// Базовый класс для хранения ID характеристик слова в виде словаря.
    /// </summary>
    class HasDict
    {
        private List<int> _dimentions = new List<int>();

        public int[] dimentions
        {
            get
            {
                return _dimentions.ToArray();
            }
        }

        protected void SetDims(int[] dims)
        {
            _dimentions.AddRange(dims);
        }

        protected Dictionary<int, int> pairs = new Dictionary<int, int>();

        /// <summary>
        /// Добавление ID характеристик слова в словарь.
        /// </summary>
        public void AddPair(int Key, int Value)
        {
            if (!pairs.ContainsKey(Key))
                pairs.Add(Key, Value);
        }

        /// <summary>
        /// Получение словаря ID характеристик.
        /// </summary>
        public Dictionary<int, int> GetPairs()
        {
            return pairs;
        }

        /// <summary>
        /// Получение словаря характеристик в виде символов.
        /// </summary>
        public Dictionary<string, string> GetPairsNames(int[] dimentions)
        {
            Dictionary<string, string> pnames = new Dictionary<string, string>();
            foreach (var dim in dimentions)
            {
                if (this.pairs.ContainsKey(dim))
                {
                    var Key = ((GrenProperty)dim).ToString();
                    var Value = "";
                    switch (dim)
                    {
                        case GrammarEngineAPI.CharCasing:
                            Value = ((enCharCasing)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.PERSON_ru:
                            Value = ((enPERSON_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.NUMBER_ru:
                            Value = ((enNUMBER_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.GENDER_ru:
                            Value = ((enGENDER_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.TRANSITIVENESS_ru:
                            Value = ((enTRANSITIVENESS_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.PARTICIPLE_ru:
                        case GrammarEngineAPI.PASSIVE_PARTICIPLE_ru:
                            break;
                        case GrammarEngineAPI.ASPECT_ru:
                            Value = ((enASPECT_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.VERB_FORM_ru:
                            Value = ((enVERB_FORM_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.TENSE_ru:
                            Value = ((enTENSE_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.SHORTNESS_ru:
                            break;
                        case GrammarEngineAPI.CASE_ru:
                            Value = ((enCASE_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.FORM_ru:
                            Value = ((enFORM_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.COUNTABILITY_ru:
                            Value = ((enCOUNTABILITY_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.COMPAR_FORM_ru:
                            Value = ((enCOMPAR_FORM_ru)this.pairs[dim]).ToString();
                            break;
                        case GrammarEngineAPI.CASE_GERUND_ru:
                        case GrammarEngineAPI.MODAL_ru:
                            break;
                    }
                    pnames.Add(Key, Value);
                }
            }
            return pnames;
        }

    }

    /// <summary>
    /// СУЩЕСТВИТЕЛЬНОЕ 
    /// </summary>
    class NOUN_ru : HasDict
    {
        private int[] dims = { 
				/*/ МОДАЛЬНЫЙ   // модальность - для слов, могущий принимать участие в сочетаниях типа "желание учиться" или "возможность получить"
				GrammarEngineAPI.MODAL_ru, 
				// CharCasing  // Слова, начинающиеся с заглавной буквы или целиком в верхнем регистре
				GrammarEngineAPI.CharCasing, */

				GrammarEngineAPI.COUNTABILITY_ru,   //ПЕРЕЧИСЛИМОСТЬ
				GrammarEngineAPI.FORM_ru,           //ОДУШ
				GrammarEngineAPI.GENDER_ru,         //РОД

				GrammarEngineAPI.NUMBER_ru,         //ЧИСЛО
				GrammarEngineAPI.CASE_ru            //ПАДЕЖ
			};

        public NOUN_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// МЕСТОИМ_СУЩ 
    /// </summary>
    class PRONOUN2_ru : HasDict
    {
        private int[] dims = { 
				GrammarEngineAPI.GENDER_ru, // РОД
				GrammarEngineAPI.CASE_ru    //ПАДЕЖ
			};

        public PRONOUN2_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// МЕСТОИМЕНИЕ 
    /// </summary>
    class PRONOUN_ru : HasDict
    {
        private int[] dims = { 
				GrammarEngineAPI.PERSON_ru, //ЛИЦО
				GrammarEngineAPI.NUMBER_ru, //ЧИСЛО
				GrammarEngineAPI.GENDER_ru, //РОД
				GrammarEngineAPI.CASE_ru    //ПАДЕЖ              
			};

        public PRONOUN_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// Базовый класс для прилагательных, причастий, числительных.
    /// </summary>
    class ADJ : HasDict
    {
        private int[] dims = { 
				GrammarEngineAPI.GENDER_ru, //РОД
				GrammarEngineAPI.NUMBER_ru, //ЧИСЛО
				GrammarEngineAPI.CASE_ru,   //ПАДЕЖ
				GrammarEngineAPI.FORM_ru    //ОДУШ
            };

        public ADJ()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ПРИЛАГАТЕЛЬНОЕ 
    /// </summary>
    class ADJ_ru : ADJ
    {
        private int[] dims = { 
				GrammarEngineAPI.PARTICIPLE_ru,         //ПРИЧАСТИЕ
				GrammarEngineAPI.PASSIVE_PARTICIPLE_ru, //СТРАД
				GrammarEngineAPI.TRANSITIVENESS_ru,     //ПЕРЕХОДНОСТЬ
				GrammarEngineAPI.CASE_GERUND_ru,        //падежная валентность
                GrammarEngineAPI.ASPECT_ru,             //ВИД
				GrammarEngineAPI.MODAL_ru,              //МОДАЛЬНЫЙ
				GrammarEngineAPI.TENSE_ru,              //ВРЕМЯ			
                GrammarEngineAPI.COMPAR_FORM_ru,        //СТЕПЕНЬ
				GrammarEngineAPI.SHORTNESS_ru           //КРАТКИЙ
            };
        public ADJ_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ЧИСЛИТЕЛЬНОЕ 
    /// </summary>
    class NUMBER_CLASS_ru : ADJ
    {
    }

    /// <summary>
    /// ИНФИНИТИВ 
    /// </summary>
    class INFINITIVE_ru : HasDict
    {
        private int[] dims = { 
                GrammarEngineAPI.ASPECT_ru,             //ВИД
				GrammarEngineAPI.TRANSITIVENESS_ru,     //ПЕРЕХОДНОСТЬ
				GrammarEngineAPI.CASE_GERUND_ru,        //падежная валентность
				GrammarEngineAPI.MODAL_ru               //МОДАЛЬНЫЙ
            };
        public INFINITIVE_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ГЛАГОЛ, ПРЕДИКАТ 
    /// </summary>
    class VERB_ru : HasDict
    {
        private int[] dims = { 
				GrammarEngineAPI.TRANSITIVENESS_ru,     //ПЕРЕХОДНОСТЬ
				GrammarEngineAPI.CASE_GERUND_ru,        //падежная валентность
                GrammarEngineAPI.ASPECT_ru,             //ВИД              
				GrammarEngineAPI.MODAL_ru,              //МОДАЛЬНЫЙ
				GrammarEngineAPI.TENSE_ru,              //ВРЕМЯ			
				GrammarEngineAPI.PERSON_ru,             //ЛИЦО
				GrammarEngineAPI.GENDER_ru,             //РОД
				GrammarEngineAPI.NUMBER_ru,             //ЧИСЛО
                GrammarEngineAPI.VERB_FORM_ru           //НАКЛОНЕНИЕ
                // ??? ЗАЛОГ
            };
        public VERB_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ДЕЕПРИЧАСТИЕ 
    /// </summary>
    class GERUND_2_ru : HasDict
    {
        private int[] dims = { 
                GrammarEngineAPI.ASPECT_ru,             //ВИД        
				GrammarEngineAPI.TRANSITIVENESS_ru,     //ПЕРЕХОДНОСТЬ
 				GrammarEngineAPI.CASE_GERUND_ru,        //падежная валентность
  				GrammarEngineAPI.MODAL_ru               //МОДАЛЬНЫЙ
            };
        public GERUND_2_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ПРЕДЛОГ 
    /// </summary>
    class PREPOS_ru : HasDict
    {
        private int[] dims = { 
				GrammarEngineAPI.CASE_ru    //ПАДЕЖ
            };
        public PREPOS_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// БЕЗЛИЧ_ГЛАГОЛ 
    /// </summary>
    class IMPERSONAL_VERB_ru : HasDict
    {
        private int[] dims = { 
                GrammarEngineAPI.ASPECT_ru,             //ВИД        
				GrammarEngineAPI.TRANSITIVENESS_ru,     //ПЕРЕХОДНОСТЬ
 				GrammarEngineAPI.CASE_GERUND_ru,        //падежная валентность
				GrammarEngineAPI.TENSE_ru,              //ВРЕМЯ			
  				GrammarEngineAPI.MODAL_ru               //МОДАЛЬНЫЙ
            };
        public IMPERSONAL_VERB_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ЧАСТИЦА 
    /// </summary>
    class PARTICLE_ru : HasDict
    {
    }

    /// <summary>
    /// СОЮЗ 
    /// </summary>
    class CONJ_ru : HasDict
    {
    }

    /// <summary>
    /// НАРЕЧИЕ 
    /// </summary>
    class ADVERB_ru : HasDict
    {
        private int[] dims = { 
                GrammarEngineAPI.COMPAR_FORM_ru        //СТЕПЕНЬ
            };
        public ADVERB_ru()
        {
            this.SetDims(dims);
        }
    }

    /// <summary>
    /// ПУНКТУАТОР 
    /// </summary>
    class PUNCTUATION_class : HasDict
    {
    }

}

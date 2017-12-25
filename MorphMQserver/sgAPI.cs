using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SolarixGrammarEngineNET;
using Schemas;

namespace Gren
{
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
				case GrenPart.ADJ_ru:
					result = typeof(ADJ_ru); // class ПРИЛАГАТЕЛЬНОЕ
					break;
				case GrenPart.NUMBER_CLASS_ru:
					result = typeof(NUMBER_CLASS_ru); // class ЧИСЛИТЕЛЬНОЕ
					break;
				case GrenPart.INFINITIVE_ru:
					result = typeof(INFINITIVE_ru); // class ИНФИНИТИВ
					break;
				case GrenPart.VERB_ru:
					result = typeof(VERB_ru); // class ГЛАГОЛ
					break;
				case GrenPart.GERUND_2_ru:
					result = typeof(GERUND_2_ru); // class ДЕЕПРИЧАСТИЕ
					break;
				case GrenPart.PREPOS_ru:
					result = typeof(PREPOS_ru); // class ПРЕДЛОГ
					break;
				case GrenPart.IMPERSONAL_VERB_ru:
					result = typeof(IMPERSONAL_VERB_ru); // class БЕЗЛИЧ_ГЛАГОЛ
					break;
				case GrenPart.PARTICLE_ru:
					result = typeof(PARTICLE_ru); // class ЧАСТИЦА
					break;
				case GrenPart.CONJ_ru:
					result = typeof(CONJ_ru); // class СОЮЗ
					break;
				case GrenPart.ADVERB_ru:
					result = typeof(ADVERB_ru); // class НАРЕЧИЕ
					break;
				case GrenPart.PUNCTUATION_class:
					result = typeof(PUNCTUATION_class); // class ПУНКТУАТОР
					break;
				/*
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
	/// СУЩЕСТВИТЕЛЬНОЕ 
	/// </summary>
    class NOUN_ru : HasDict
	{
		private int[] dims = { 
				/*/ МОДАЛЬНЫЙ   // модальность - для слов, могущий принимать участие в сочетаниях типа "желание учиться" или "возможность получить"
				GrammarEngineAPI.MODAL_ru, 
				// CharCasing  // Слова, начинающиеся с заглавной буквы или целиком в верхнем регистре */
				GrammarEngineAPI.CharCasing, 

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

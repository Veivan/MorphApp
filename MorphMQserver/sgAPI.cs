﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MorphMQserver
{
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
			string result = "" + cnst;
			if (Enum.IsDefined(typeof(GrenLink), cnst))
			{
				result = ((GrenLink)cnst).ToString();
			}

			return result;
		}

	}
}

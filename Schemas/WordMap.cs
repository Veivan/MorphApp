﻿using System;
using System.Collections.Generic;
using TMorph.Schema;

namespace Schemas
{
	/// <summary>
	/// Класс хранит грамматическое представление одного слова.
	/// </summary>
	public class WordMap
	{
		public HasDict xPart = null;
		public string EntryName = "";
	    public short order; // Порядок слова в предложении

		/// <summary>
		/// ID словарной статьи.
		/// </summary>
		private int id_entry;		
		public int ID_Entry
		{
			get
			{
				return id_entry;
			}
		}
		
		/// <summary>
		/// ID части речи.
		/// </summary>
		private int id_partofspeech;
		public int ID_PartOfSpeech
		{
			get
			{
				return id_partofspeech;
			}
		}

		public WordMap(int _id_entry, int _id_partofspeech)
		{
			id_entry = _id_entry;
			id_partofspeech = _id_partofspeech;
		}

		private Dictionary<int, int> pairs = new Dictionary<int, int>();

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

		public int GetPropertyValue(GrenProperty property)
		{
			if (pairs.ContainsKey((int)property))
				return pairs[(int)property];
			else
				return -1;
		}

        /// <summary>
        /// Получение из сообщения списка структур WordMap, сортированных в порядке слов в предложении.
        /// </summary>
        public static SortedList<short, WordMap> GetWordsFromMessage(Message message)
        {
            if (message.SentencesLength == 0)
                return null;
            SortedList<short, WordMap> outlist = null;
            var sent = message.Sentences(0);
            if (sent.HasValue)
            {
                outlist = new SortedList<short, WordMap>();
                var sentval = sent.Value;
                // Чтение слов
                for (int i = 0; i < sentval.WordsLength; i++)
                {
                    var word = BuildFromLexema(sentval.Words(i));
                    outlist.Add(word.order, word);
                }
            }
            return outlist;
        }
        
        /// <summary>
        /// Получение структуры WordMap из структуры Lexema.
        /// </summary>
        private static WordMap BuildFromLexema(Lexema? lexema)
        {
            WordMap word = null;
            if (lexema.HasValue)
            {
                var lexval = lexema.Value;
                word = new WordMap(lexval.IdEntry, lexval.IdPartofspeech);
                word.EntryName = lexval.EntryName;
                // Чтение граммем
                for (int i = 0; i < lexval.GrammemsLength; i++)
                {
                    var grammema = lexval.Grammems(i);
                    if (grammema.HasValue)
                    {
                        word.AddPair(grammema.Value.Key, grammema.Value.Value);
                    }
                }
            }
            return word;
        }
        
    }
}

using System;
using System.Collections.Generic;

namespace Schemas
{
	/// <summary>
	/// Класс хранит грамматическое представление одного слова.
	/// </summary>
	public class WordMap
	{
		public HasDict xPart = null;
		public string EntryName = "";

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

	}
}

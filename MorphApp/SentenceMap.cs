using System;
using System.Collections;
using System.Collections.Generic;

namespace MorphApp
{
	/// <summary>
	/// Класс хранит информацию о предложении.
	/// </summary>
	class SentenceMap
	{
		private SortedList<int, WordMap> words = new SortedList<int, WordMap>();

		public int Capasity
		{
			get
			{
				return words.Count;
			}
		}

		public void AddWord(int order, WordMap word)
		{
			if (!words.ContainsKey(order) && word != null)
				words.Add(order, word);
		}

		public WordMap GetWordByOrder(int order)
		{
			if (words.ContainsKey(order))
				return words[order];
			else
				return null;
		}

	}
}

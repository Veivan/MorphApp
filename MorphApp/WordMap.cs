using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MorphApp
{
	/// <summary>
	/// Класс хранит грамматическое представление одного слова.
	/// </summary>
	class WordMap
	{
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

	}
}

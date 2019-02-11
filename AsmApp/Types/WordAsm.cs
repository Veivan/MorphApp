using System;
using System.Collections.Generic;
using Schemas;
using Schemas.BlockPlatform;

namespace AsmApp.Types
{
	/// <summary>
	/// Класс хранит грамматическое представление одного слова.
	/// </summary>
	public class WordAsm : AssemblyBase
	{

		#region Privates		
		private AssemblyBase srcAsm; // Сборка, из которой было сформировано Слово
		private int id_entry;
		private int id_partofspeech;
		private Dictionary<int, int> pairs = new Dictionary<int, int>();
		#endregion

		#region Properties
		public HasDict xPart = null;
		public string EntryName = "";
		public string RealWord = "";
		public int order; // Порядок слова в предложении
		public int rcind; // характеристика словарной статьи (нашлось слово или нет)

		/// <summary>
		/// ID словарной статьи.
		/// </summary>
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
		public int ID_PartOfSpeech
		{
			get
			{
				return id_partofspeech;
			}
		}


		#endregion

		#region Constructors
		public WordAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
		}

		public WordAsm(int _id_entry, int _id_partofspeech) : base(Session.Instance().GetBlockTypeByNameKey(Session.wordTypeName))
		{
			id_entry = _id_entry;
			id_partofspeech = _id_partofspeech;
		}
		#endregion

		#region Methods
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

		public void Add2SaveSet()
		{
			var store = Session.Instance().Store;
			foreach (var pair in pairs) ;// where !sent.IsActual
//				pair.Add2SaveSet();
		}

		#endregion
	}
}

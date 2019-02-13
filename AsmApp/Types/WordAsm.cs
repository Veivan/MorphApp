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

		private long lx_id; // Ссылка на сборку лексемы
		#endregion

		#region Properties
		public HasDict xPart = null;
		public string EntryName = "";
		public string RealWord = "";
		public int order; // Порядок слова в предложении

		/// <summary>
		/// характеристика словарной статьи (=0 слово нашлось; =1 EntryName == "???"; =2 EntryName == "number_")
		/// </summary>
		public int rcind; 

		/// <summary>
		/// ID словарной статьи (из справочника GREN). 
		/// Может меняться при перекомпиляции справочника, поэтому в БД не сохраняется.
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
			// Сохранение лексемы в БД
			var asmlex = store.GetLexema(id_partofspeech, EntryName, true);
			lx_id = asmlex.BlockID;
			// Сохранение граммем в БД
			var gramlist = new List<long>();

			srcAsm.SetValue("Grammems", gramlist);
			foreach (var pair in pairs)
//				var asmgram = store.GetLexema(id_partofspeech, EntryName, true);
			;
			//				pair.Add2SaveSet();

			store.Add2SaveSet(this.Revert2Asm());
		}

		/// <summary>
		/// Обратное преобразование из объекта программы в AssemblyBase.
		/// Происходит заполнение полей исходной сборки srcAsm актуальными значениями.
		/// </summary>
		private AssemblyBase Revert2Asm()
		{
			if (srcAsm == null)
				srcAsm = new AssemblyBase(-1, Session.Instance().GetBlockTypeByNameKey(Session.wordTypeName));

			srcAsm.SetValue("lx_id", lx_id);
			srcAsm.SetValue("rcind", rcind);
			srcAsm.SetValue("order", order);
			srcAsm.SetValue("RealWord", RealWord);
			return srcAsm;
		}

		#endregion
	}
}

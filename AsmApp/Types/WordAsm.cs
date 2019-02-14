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
		private Dictionary<long, long> pairs = new Dictionary<long, long>();

		private long lx_id; // Ссылка на сборку лексемы
		#endregion

		#region Properties
		public HasDict xPart = null;
		public string EntryName = "";
		public string RealWord = "";

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
		public void AddPair(long Key, long Value)
		{
			if (!pairs.ContainsKey(Key))
				pairs.Add(Key, Value);
		}

		/// <summary>
		/// Получение словаря ID характеристик.
		/// </summary>
		public Dictionary<long, long> GetPairs()
		{
			return pairs;
		}

		public long GetPropertyValue(GrenProperty property)
		{
			if (pairs.ContainsKey((long)property))
				return pairs[(long)property];
			else
				return -1;
		}

		public override void Save()
		{
			// Сохранение лексемы в БД
			var asmlex = new LexemaAsm(id_partofspeech, EntryName);
			asmlex.Save();
			lx_id = asmlex.BlockID;
			// Сохранение граммем в БД
			var gramlist = new List<long>();
			foreach (var pair in pairs)
			{
				var asmgramm = new GrammemAsm(pair.Key, pair.Value);
				asmgramm.Save();
				gramlist.Add(asmgramm.BlockID);
			}

			this.SetValue("lx_id", lx_id);
			this.SetValue("rcind", rcind);
//			this.SetValue("order", order);
			this.SetValue("RealWord", RealWord);
			this.SetValue("Grammems", gramlist);

			base.Save();
		}

		#endregion
	}
}

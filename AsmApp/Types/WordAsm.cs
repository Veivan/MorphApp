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

//		private Dictionary<long, long> pairs = new Dictionary<long, long>();
		
		/// <summary>
		/// Хранилище грамматическиз характеристик слова в предложении.
		/// </summary>
		private List<GrammemAsm> grammems = new List<GrammemAsm>();

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
			var store = Session.Instance().Store;
			this.srcAsm = srcAsm;
			this.Order = srcAsm.Order;
			this.lx_id = Convert.ToInt64(srcAsm.GetValue("lx_id"));
			var asmlex = store.GetAssembly(lx_id);
			this.id_partofspeech = Convert.ToInt32(asmlex.GetValue("GrenPart"));
			this.EntryName = (string)asmlex.GetValue("Lemma");

			this.rcind = Convert.ToInt32(srcAsm.GetValue("rcind"));
			//wasm.xPart = wmap.xPart;
			this.RealWord = (string)srcAsm.GetValue("RealWord");
			var gramIDs = (List<long>)srcAsm.GetValue("Grammems");
			int i = 0;
			if (gramIDs != null)
				foreach (var ID in gramIDs)
				{
					var asm = store.GetAssembly(ID);
					var gram = new GrammemAsm(asm);
					grammems.Add(gram);
				}
		}

		public WordAsm(int _id_entry, int _id_partofspeech) : base(Session.Instance().GetBlockTypeByNameKey(Session.wordTypeName))
		{
			id_entry = _id_entry;
			id_partofspeech = _id_partofspeech;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Добавление характеристики слова в словарь.
		/// </summary>
		internal void AddGrammem(GrammemAsm asmgramm)
		{
			grammems.Add(asmgramm);
		}

		public override void Save()
		{
			// Сохранение лексемы в БД
			var asmlex = new LexemaAsm(id_partofspeech, EntryName);
			asmlex.Save();
			lx_id = asmlex.BlockID;

			//Сохранение слова для получения ID
			if (this.IsVirtual)
				base.Save();

			// Сохранение граммем в БД
			var gramlist = new List<long>();
			foreach (var asmgramm in grammems)
			{
				asmgramm.ParentID = this.BlockID;
				asmgramm.Save();
				gramlist.Add(asmgramm.BlockID);
			}

			this.SetValue("lx_id", lx_id);
			this.SetValue("rcind", rcind);
			this.SetValue("RealWord", RealWord);
			this.SetValue("Grammems", gramlist);

			base.Save();
		}

		#endregion
	}
}

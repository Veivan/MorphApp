using Schemas;
using Schemas.BlockPlatform;

namespace AsmApp.Types
{
	/// <summary>
	/// Класс хранит одну лемму :
	/// обозначение части речи и значение.
	/// </summary>
	public class LexemaAsm : AssemblyBase
	{
		#region Privates		
		private AssemblyBase srcAsm; // Сборка, из которой был сформирован объект
		private int grenPart;
		private string lemma;
		#endregion

		#region Constructors
		/// <summary>
		/// Конструктор объекта из данных БД.
		/// </summary>
		public LexemaAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
			/// TODO здесь надо заполнять внутренние поля
		}

		/// <summary>
		/// Конструктор нового объекта.
		/// </summary>
		/// <param name="_grenPart">ID части речи (значение константы GrenPart из ConstantEnums)</param>
		/// <param name="_lemma">Лемма (в нижнем регистре)</param>
		/// <returns></returns>
		public LexemaAsm(int _grenPart, string _lemma) : base(Session.Instance().GetBlockTypeByNameKey(Session.lexTypeName))
		{
			grenPart = _grenPart;
			lemma = _lemma;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Идентификатор части речи -  знчение члена перечисления Schemas.GrenPart.
		/// </summary>
		public int GrenPart
		{
			get
			{
				return grenPart;
			}
		}

		/// <summary>
		/// Лемма.
		/// </summary>
		public string Lemma
		{
			get
			{
				return lemma;
			}
		}
		#endregion

		#region Methods
		public override void Save()
		{
			var store = Session.Instance().Store;
			srcAsm = store.GetLexema(grenPart, lemma, true);
		}
		#endregion
	}
}
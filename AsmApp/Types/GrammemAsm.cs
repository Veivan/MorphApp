using Schemas;
using Schemas.BlockPlatform;

namespace AsmApp.Types
{
	/// <summary>
	/// Класс хранит грамматическую характеристику одного слова :
	/// тип и значение.
	/// </summary>
	public class GrammemAsm : AssemblyBase
	{
		#region Privates		
		private AssemblyBase srcAsm; // Сборка, из которой был сформирован объект
		private int grenProperty;
		private int value;
		#endregion

		#region Constructors
		public GrammemAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
		}
		public GrammemAsm(int _grenProperty, int _value) : base(Session.Instance().GetBlockTypeByNameKey(Session.grammTypeName))
		{
			grenProperty = _grenProperty;
			value = _value;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Идентификатор грамматической характеристики -  знчение члена перечисления Schemas.GrenProperty.
		/// </summary>
		public int GrenProperty
		{
			get
			{
				return grenProperty;
			}
		}

		/// <summary>
		/// Значение грамматической характеристики.
		/// </summary>
		public int Value
		{
			get
			{
				return value;
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Обратное преобразование из объекта программы в AssemblyBase.
		/// Происходит заполнение полей исходной сборки srcAsm актуальными значениями.
		/// </summary>
		public AssemblyBase Revert2Asm()
		{
			srcAsm.SetValue("GrenProperty", grenProperty);
			srcAsm.SetValue("Value", value);
			return srcAsm;
		}
		#endregion

	}
}
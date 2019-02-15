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
		private long grenProperty;
		private long value;
		#endregion

		#region Constructors
		public GrammemAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
			this.grenProperty = (long)srcAsm.GetValue("GrenProperty");
			this.value = (long)srcAsm.GetValue("Value");
		}

		public GrammemAsm(long _grenProperty, long _value) : base(Session.Instance().GetBlockTypeByNameKey(Session.grammTypeName))
		{
			grenProperty = _grenProperty;
			value = _value;
		}
		#endregion

		#region Properties
		/// <summary>
		/// Идентификатор грамматической характеристики -  знчение члена перечисления Schemas.GrenProperty.
		/// </summary>
		public long GrenProperty
		{
			get
			{
				return grenProperty;
			}
		}

		/// <summary>
		/// Значение грамматической характеристики.
		/// </summary>
		public long Value
		{
			get
			{
				return value;
			}
		}
		#endregion

		#region Methods

		public override void Save()
		{
			this.SetValue("GrenProperty", grenProperty);
			this.SetValue("Value", value);
			base.Save();
		}
		#endregion

	}
}
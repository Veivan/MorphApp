using Schemas;
using Schemas.BlockPlatform;


namespace AsmApp.Types
{
	/// <summary>
	/// Класс хранит описание синтаксической связи между двумя словами в предложении.
	/// </summary>
	public class SyntNodeAsm : AssemblyBase
	{
		#region Privates		
		private AssemblyBase srcAsm; // Сборка, из которой был сформирован объект
		private long c_id; // Идентификатор слова в предложении (ребёнка в синт.связи)– ссылка на объект типа SingleWord
		private long GrenLink; // ID синт.связи (значение константы GrenLink из ConstantEnums)
		private long level; // Уровень вложенности
		private long pс_id; // Идентификатор слова в предложении (родителя в синт.связи)– ссылка на объект типа SingleWord
		#endregion

		#region Constructors
		public SyntNodeAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
		}
		public SyntNodeAsm(long _c_id, long _GrenLink, long _level, long _pc_id ) : base(Session.Instance().GetBlockTypeByNameKey(Session.syntNodeTypeName))
		{
			c_id = _c_id;
			GrenLink = _GrenLink;
			level = _level;
			pс_id = _pc_id;
		}
		#endregion

		#region Methods

		public override void Save()
		{
			this.SetValue("c_id", c_id);
			this.SetValue("GrenLink", GrenLink);
			this.SetValue("level", level);
			this.SetValue("pс_id", pс_id);
			base.Save();
		}
		#endregion
	}
}

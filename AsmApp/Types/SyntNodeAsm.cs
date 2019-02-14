using System;
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
		private long c_id;  
		private long grenLink; 
		private long level; 
		private long pс_id;  
		private long childOrder;  
		private long parentOrder;
		#endregion

		#region Properties
		/// <summary>
		/// Уровень вложенности
		/// </summary>
		public long Level { get { return level; } }

		/// <summary>
		/// ID синт.связи (значение константы GrenLink из ConstantEnums)
		/// </summary>
		public long GrenLink { get { return grenLink; } }
		
		/// <summary>
		/// Порядок слова в предложении (ребёнка в синт.связи)
		/// </summary>
		public long ChildOrder { get {	return childOrder; } }

		/// <summary>
		/// Порядок слова в предложении (родителя в синт.связи)
		/// </summary>
		public long ParentOrder { get { return parentOrder; } }

		/// <summary>
		/// Идентификатор слова в предложении (ребёнка в синт.связи)– ссылка на объект типа SingleWord
		/// </summary>
		public long CID { get { return c_id; } set { c_id = value; } }

		/// <summary>
		/// Идентификатор слова в предложении (родителя в синт.связи)– ссылка на объект типа SingleWord
		/// </summary>
		public long PCID { get { return pс_id; } set { pс_id = value; } }
		#endregion

		#region Constructors
		public SyntNodeAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
			c_id = Convert.ToInt64(srcAsm.GetValue("c_id"));
			grenLink = Convert.ToInt64(srcAsm.GetValue("GrenLink"));
			level = Convert.ToInt64(srcAsm.GetValue("level"));
			pс_id = Convert.ToInt64(srcAsm.GetValue("pс_id"));
		}

		public SyntNodeAsm(long _c_id, long _GrenLink, long _level, long _pc_id, long _ChildOrder, long _ParentOrder) 
			: base(Session.Instance().GetBlockTypeByNameKey(Session.syntNodeTypeName))
		{
			c_id = _c_id;
			grenLink = _GrenLink;
			level = _level;
			pс_id = _pc_id;
			childOrder = _ChildOrder;
			parentOrder = _ParentOrder;
		}
		#endregion

		#region Methods

		public override void Save()
		{
			this.SetValue("c_id", c_id);
			this.SetValue("GrenLink", grenLink);
			this.SetValue("level", level);
			this.SetValue("pс_id", pс_id);
			this.SetValue("ChildOrder", childOrder);
			this.SetValue("ParentOrder", parentOrder);
			base.Save();
		}
		#endregion
	}
}

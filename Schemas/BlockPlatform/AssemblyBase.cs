using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Базовый класс для сборок.
	/// </summary>
	public class AssemblyBase : BlockBase
	{
		private AssemblyBase templAsm;

		protected List<AssemblyBase> children = new List<AssemblyBase>();

		#region Properties
		/// <summary>
		/// Стартовый блок сборки.
		/// </summary>
		public long RootBlock_id { get { return this.BlockID; }  set { this.BlockID = value; } }

		/// <summary>
		/// Сборка - образец/шаблон, на основе которой была сделана текущая сборка.
		/// </summary>
		public AssemblyBase Following
		{
			get
			{
				return templAsm;
			}

			set
			{
				templAsm = value;
			}
		}

		public List<AssemblyBase> Children
		{
			get
			{
				var ret = new List<AssemblyBase>(children);
				return ret;
			}
		}

		public List<AssemblyBase> AllChildren
		{
			get
			{
				var ret = GetAllNodes(children);
				return ret.ToList();
			}
		}

		/// <summary>
		/// Наименование сборки
		/// </summary>
		public string Name
		{
			get
			{
				if (IsMainDataContainer)
					return (string)GetValue("Name", Session.MainStoreName);
				if (IsDictsStore)
					return (string)GetValue("Name", Session.DictsStoreName);
				
				if (IsDataContainer)
					return (string)GetValue("Name", Session.DefaultContainerName);
				else
					return (string)GetValue("Name");
			}
		}

		/// <summary>
		/// Признак того, что содержимое было изменено в процессе редактирования и отличается от содержавшегося в БД.
		/// </summary>
		public bool IsDirty { get; set; }
		#endregion

		#region Характеристики

		public bool IsMainDataContainer	{ get; set;	}

		public bool IsDictsStore { get; set; }

		public bool IsDataContainer
		{
			get
			{
				return BlockType.NameKey == "DataContainer";
			}
		}

		public bool IsVirtual { get { return this.BlockID == -1; } }
		#endregion

		#region Конструкторы

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		public AssemblyBase(BlockType blockType) :
				base(-1, blockType, -1, 0, -1, -1, -1, null, DateTime.Now)
		{
			IsDirty = true;
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		/// <param name="id">адрес стартового блока сборки</param>
		public AssemblyBase(BlockAddress id, BlockType blockType, BlockAddress ParentContID = -1) :
			base(id, blockType, ParentContID, 0, -1, -1, -1, null, DateTime.Now)
		{
			IsDirty = false;
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="templAsm">Сборка - образец/шаблон, на основе которой создаётся текущая сборка.</param>
		/// <param name="id">адрес стартового блока сборки</param>
		/// <param name="mode">признак, определяющий, сохранять ссылку на шаблон\образец или нет.</param>
		public AssemblyBase(AssemblyBase templAsm, BlockAddress id, FOLLOWMODE mode) :
			base(id, templAsm.BlockType, -1, 0, -1, -1, -1, null, DateTime.Now)
		{
			if (mode == FOLLOWMODE.Follow)
				this.templAsm = templAsm;
			CopyChildrenRequrs(this, templAsm.Children);
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="block">стартовый блок сборки.</param>
		public AssemblyBase(BlockBase block) : base(block)
		{
		}

		/// <summary>
		/// Конструктор - копировщик.
		/// </summary>
		/// <param name="assembly">Исходная сборка.</param>
		public AssemblyBase(AssemblyBase assembly) :
			base(assembly.BlockID, assembly.BlockType, assembly.ParentID, assembly.Order, assembly.FactID,
				assembly.PredecessorID, assembly.SuccessorID, assembly.UserAttrs, assembly.Created)
		{
		}

		#endregion

		#region Public methods
		public void AddChild(AssemblyBase asm)
		{
			var cont = children.Where(x => x.BlockID == asm.BlockID).FirstOrDefault();
			if (cont == null)
				this.children.Add(asm);
		}

		public virtual void Save()
		{
			var store = Session.Instance().Store;
			store.Add2SaveSet(this);
			store.Save();
		}

		#endregion
	
		#region Вспомогательные функции
		/// <summary>
		/// Получение плоского списка из дерева рекурсивно.
		/// </summary>
		private IEnumerable<AssemblyBase> GetAllNodes(IEnumerable<AssemblyBase> data)
		{
			var result = new List<AssemblyBase>();

			if (data != null)
			{
				foreach (var item in data)
				{
					result.Add(item);
					result.AddRange(GetAllNodes(item.children));
				}
			}
			return result;
		}

		/// <summary>
		/// Рекурсивное копирование дочерних сборок из источника в целевую сборку.
		/// </summary>
		/// <param name="asm">целевая сборка</param>
		/// <param name="src_children">перечень дочерних сборок сборки-источника</param>
		private void CopyChildrenRequrs(AssemblyBase asm, List<AssemblyBase> src_children)
		{
			foreach (var src_child in src_children)
			{
				var newchild = new AssemblyBase(src_child.BlockType);
				newchild.ParentID = asm.RootBlock_id;
				newchild.Order = src_child.Order;
				asm.children.Add(newchild);
				CopyChildrenRequrs(newchild, src_child.children);
			}
		}

		#endregion
	}
}

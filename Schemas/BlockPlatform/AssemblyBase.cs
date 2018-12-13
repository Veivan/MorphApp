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
		private BlockAddress _parentAsmId = 0;
		private long _treeorder = 0;
		//private string name;

		protected List<AssemblyBase> children = new List<AssemblyBase>();

		#region Свойства
		/// <summary>
		/// Стартовый блок сборки.
		/// </summary>
		public long RootBlock_id { get { return this.BlockID; } }

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
		/// Адрес сборки, являющейся Родителем для текущий сборки.
		/// </summary>
		public BlockAddress ParentAssemblyID
		{
			get
			{
				return _parentAsmId;
			}

			set
			{
				_parentAsmId = value;
			}
		}

		/// <summary>
		/// Поддержка дерева. Порядок сборки в дереве.
		/// </summary>
		public long Treeorder
		{
			get
			{
				return _treeorder;
			}

			set
			{
				_treeorder = value;
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
				if (IsDataContainer)
					return (string)GetValue("Name", Session.DefaulContainerName);
				else
					return (string)GetValue("Name");
			}
		}

		#endregion

		#region Характеристики

		public bool IsMainDataContainer	{ get; set;	}

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
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		/// <param name="id">адрес стартового блока сборки</param>
		public AssemblyBase(BlockAddress id, BlockType blockType, BlockAddress ParentContID = -1) :
			base(id, blockType, ParentContID, 0, -1, -1, -1, null, DateTime.Now)
		{
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
		public AssemblyBase(BlockBase block) :
			base(block.BlockID, block.BlockType, block.ParentID, block.Order, block.FactID,
				block.PredecessorID, block.SuccessorID, block.UserAttrs, block.Created)
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

		public void AddChild(AssemblyBase asm)
		{
			var cont = children.Where(x => x.BlockID == asm.BlockID).FirstOrDefault();
			if (cont == null)
				this.children.Add(asm);
		}

		public void Save()
		{
			var store = Session.Instance().Store;
			store.Save(this);
		}

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
				newchild._parentAsmId = asm.RootBlock_id;
				newchild._treeorder = src_child._treeorder;
				asm.children.Add(newchild);
				CopyChildrenRequrs(newchild, src_child.children);
			}
		}

		#endregion
	}
}

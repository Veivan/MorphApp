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
	public class AssemblyBase
	{
		private BlockAddress _rootBlock_id;
		private BlockType _blockType;
		private AttrsCollection _SysAttrs;
		private AttrsCollection _UserAttrs;
		private AssemblyBase templAsm;
		private AssemblyBase ParentAsm;
		private List<AssemblyBase> children = new List<AssemblyBase>();

		/// <summary>
		/// Тип сборки.
		/// </summary>
		public BlockType BlockType
		{
			get
			{
				return _blockType;
			}
		}

		/// <summary>
		/// Коллекция системных атрибутов.
		/// </summary>
		public AttrsCollection SysAttrs
		{
			get
			{
				return _SysAttrs;
			}
			/// TODO Сделать системный атрибут "ссылка на источник"
		}

		/// <summary>
		/// Коллекция пользовательских атрибутов.
		/// </summary>
		public AttrsCollection UserAttrs
		{
			get
			{
				return _UserAttrs;
			}
		}

		/// <summary>
		/// Стартовый блок сборки.
		/// </summary>
		public long RootBlock_id
		{
			get
			{
				return _rootBlock_id;
			}

			set
			{
				_rootBlock_id = value;
			}
		}

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

		public AssemblyBase ParentAssembly
		{
			get
			{
				return ParentAsm;
			}

			set
			{
				ParentAsm = value;
			}
		}

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

		public AssemblyBase(BlockType blockType)
		{
			this._blockType = blockType;
		}

		public AssemblyBase(BlockType type, long id)
		{
			this._blockType = type;
			this._rootBlock_id = id;
		}

		public AssemblyBase(AssemblyBase templAsm, long id, FOLLOWMODE mode)
		{
			this._rootBlock_id = id;
			if (mode == FOLLOWMODE.Follow)
				this.templAsm = templAsm;
			CopyChildrenRequrs(this, templAsm.Children);
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
				newchild.ParentAsm = asm;
				asm.children.Add(newchild);
				CopyChildrenRequrs(newchild, src_child.children);
			}
		}
	}
}

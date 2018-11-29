﻿using System;
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
		private AttrsCollection _SysAttrs;
		private AttrsCollection _UserAttrs;
		private AssemblyBase templAsm;
		private BlockAddress _parentAsmId = 0;
		private long _treeorder = 0;

		protected List<AssemblyBase> children = new List<AssemblyBase>();

		#region Свойства
		/// <summary>
		/// Стартовый блок сборки.
		/// </summary>
		public long RootBlock_id { get { return this.BlockID;} }

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
		#endregion

		#region Конструкторы


		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		public AssemblyBase(BlockType blockType) :
			base(-1, blockType, -1, 0, -1, -1, -1, DateTime.Now)
		{
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		/// <param name="id">адрес стартового блока сборки</param>
		public AssemblyBase(BlockType blockType, BlockAddress id) :
			base(id, blockType, -1, 0, -1, -1, -1, DateTime.Now)
		{
		}

		/// <summary>
		/// Конструктор.
		/// </summary>
		/// <param name="templAsm">Сборка - образец/шаблон, на основе которой создаётся текущая сборка.</param>
		/// <param name="id">адрес стартового блока сборки</param>
		/// <param name="mode">признак, определяющий, сохранять ссылку на шаблон\образец или нет.</param>
		public AssemblyBase(AssemblyBase templAsm, BlockAddress id, FOLLOWMODE mode) :
			base(id, templAsm.BlockType, -1, 0, -1, -1, -1, DateTime.Now)
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
			base(block.BlockID, block.BlockType, block.ParentID, block.Order, -1, -1, -1, DateTime.Now)
		{
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
				newchild._parentAsmId = asm.RootBlock_id;
				newchild._treeorder = src_child._treeorder;
				asm.children.Add(newchild);
				CopyChildrenRequrs(newchild, src_child.children);
			}
		}
		#endregion
	}
}

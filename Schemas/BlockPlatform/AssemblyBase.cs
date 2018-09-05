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
		/// <summary>
		/// Стартовый блок сборки.
		/// </summary>
		private BlockAddress _rootBlock_id;
		private BlockType _blockType;
		private AttrsCollection _SysAttrs;
		private AttrsCollection _UserAttrs;

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

		public AssemblyBase(BlockType blockType)
		{
			this._blockType = blockType;
		}
	}
}

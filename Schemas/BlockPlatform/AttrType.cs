using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет тип атрибута блока.
	/// </summary>
	class AttrType
	{
		private BlockAddress _mt_id = -1;
		private string _name = "";
		
		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress AttrTypeID { get { return _mt_id; } }

		/// <summary>
		/// Наименование типа
		/// </summary>
		public string Name { get { return _name; }  }

		/// <summary>
		/// Конструктор
		/// </summary>
		public AttrType(BlockAddress mt_id, string name)
		{
			_mt_id = mt_id;
			_name = name;
			/*	if (created_at == null)
					_created_at = DateTime.Now;
				else
					_created_at = (DateTime)created_at;
			} */
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет тип блока.
	/// </summary>
	class BlockType
	{
		private BlockAddress _bt_id = -1;
		private string _name = "";
		
		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress BlockTypeID { get { return _bt_id; } }

		/// <summary>
		/// Наименование типа
		/// </summary>
		public string Name { get { return _name; } set { _name = value; } }
		
		/// <summary>
		/// Конструктор
		/// </summary>
		public BlockType(BlockAddress bt_id, string name)
		{
			_bt_id = bt_id;
			_name = name;
		/*	if (created_at == null)
				_created_at = DateTime.Now;
			else
				_created_at = (DateTime)created_at;
		} */
	}
}

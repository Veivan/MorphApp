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
	public class BlockType
	{
		private BlockAddress _bt_id = -1;
		private string _nameKey = "";
		private string _nameUI = "";

		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress BlockTypeID { get { return _bt_id; } }

		/// <summary>
		/// Ключ типа
		/// </summary>
		public string NameKey { get { return _nameKey; } set { _nameKey = value; } }

		/// <summary>
		/// Наименование типа
		/// </summary>
		public string NameUI { get { return _nameUI; } set { _nameUI = value; } }
		/// <summary>
		/// Конструктор
		/// </summary>
		public BlockType(BlockAddress bt_id, string nameKey, string nameUI)
		{
			_bt_id = bt_id;
			_nameKey = nameKey;
			_nameUI = nameUI;

			/*	if (created_at == null)
					_created_at = DateTime.Now;
				else
					_created_at = (DateTime)created_at;
			} */
		}
	}
}

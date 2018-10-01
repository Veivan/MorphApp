using System;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет атрибут блока.
	/// </summary>
	public class BlockAttribute
	{
		private BlockAddress ma_id;
		private string nameKey = "";
		private string nameUI = "";
		private enAttrTypes mt;
		private BlockType bt;
		private bool mandatory = false;
		private int sorder = 0;

		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress AttrID { get { return ma_id; } }

		/// <summary>
		/// Ключ атрибута
		/// </summary>
		public string NameKey { get { return nameKey; } set { nameKey = value; } }

		/// <summary>
		/// Наименование атрибута
		/// </summary>
		public string NameUI { get { return nameUI; } set { nameUI = value; } }

		/// <summary>
		/// Тип атрибута.
		/// </summary>
		public enAttrTypes AttrType { get { return mt; } set { mt = value; } }

		/// <summary>
		/// Тип блока.
		/// </summary>
		public BlockType BlockType { get { return bt; } set { bt = value; } }

		/// <summary>
		/// Порядок следования в типе блока.
		/// </summary>
		public int Order { get { return sorder; } set { sorder = value; } }

		/// <summary>
		/// Обязательный к заполнению.
		/// </summary>
		public bool Mandatory { get { return mandatory; } set { mandatory = value; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public BlockAttribute(BlockAddress _ma_id, string _nameKey, string _nameUI, int _mt, BlockType _bt)
		{
			ma_id = _ma_id;
			nameKey = _nameKey;
			nameUI = _nameUI;
			mt = (enAttrTypes)_mt;
			bt = _bt;
			/*	if (created_at == null)
					_created_at = DateTime.Now;
				else
					_created_at = (DateTime)created_at;
			} */
		}
	}
}

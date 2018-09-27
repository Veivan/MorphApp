using System;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет атрибут блока.
	/// </summary>
	public class Attribute
	{
		private BlockAddress _ma_id;
		private string _nameKey = "";
		private string _nameUI = "";
		private enAttrTypes _mt;
		private BlockType _bt;
		private bool _mandatory = false;
		private int sorder = 0;

		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress AttrID { get { return _ma_id; } }

		/// <summary>
		/// Ключ атрибута
		/// </summary>
		public string NameKey { get { return _nameKey; } }

		/// <summary>
		/// Наименование атрибута
		/// </summary>
		public string NameUI { get { return _nameUI; } }

		/// <summary>
		/// Тип атрибута.
		/// </summary>
		public enAttrTypes AttrType { get { return _mt; } }

		/// <summary>
		/// Тип блока.
		/// </summary>
		public BlockType BlockType { get { return _bt; } }

		/// <summary>
		/// Порядок следования в типе блока.
		/// </summary>
		public int Order { get { return sorder; } set { sorder = value; } }

		/// <summary>
		/// Обязательный к заполнению.
		/// </summary>
		public bool Mandatory { get { return _mandatory; } set { _mandatory = value; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public Attribute(BlockAddress ma_id, string nameKey, string nameUI, int mt, BlockType bt)
		{
			_ma_id = ma_id;
			_nameKey = nameKey;
			_nameUI = nameUI;
			_mt = (enAttrTypes)mt;
			_bt = bt;
			/*	if (created_at == null)
					_created_at = DateTime.Now;
				else
					_created_at = (DateTime)created_at;
			} */
		}
	}
}

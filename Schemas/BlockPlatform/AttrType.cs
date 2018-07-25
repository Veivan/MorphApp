using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using DBAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет тип атрибута блока.
	/// </summary>
	public class AttrType
	{
		private DBAddress _mt_id = -1;
		private string _name = "";
		private enAttrTypes _type;

		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public DBAddress AttrTypeID { get { return _mt_id; } }

		/// <summary>
		/// Наименование типа
		/// </summary>
		public string Name { get { return _name; }  }

		public enAttrTypes Type	{ get {	return _type; }	}

		/// <summary>
		/// Конструктор
		/// </summary>
		public AttrType(DBAddress mt_id, string name)
		{
			_mt_id = mt_id;
			_name = name;
			_type = (enAttrTypes)mt_id;

			/*	if (created_at == null)
					_created_at = DateTime.Now;
				else
					_created_at = (DateTime)created_at;
			} */
		}
	}
}

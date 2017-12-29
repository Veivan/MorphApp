using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
	/// <summary>
	/// Класс хранит представление одного контейнера.
	/// </summary>
	public class ContainerMap
	{
		private long _ct_id = -1;
		private DateTime _created_at;
		private string _name = "";
		private long _parent_id = -1;

		/// <summary>
		/// Идентификатор контейнера в БД.
		/// </summary>
		public long ContainerID { get { return _ct_id; } set { _ct_id = value; } }
		/// <summary>
		/// Дата создания.
		/// </summary>
		public DateTime Created { get { return _created_at; } set { _created_at = value; } }
		/// <summary>
		/// Наименование документа
		/// </summary>
		public string Name { get { return _name; } set { _name = value; } }
		/// <summary>
		/// Идентификатор родительского контейнера в БД.
		/// </summary>
		public long ParentID { get { return _parent_id; } set { _parent_id = value; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public ContainerMap(long ct_id, string name, DateTime? created_at, long parent_id)
        {
			_ct_id = ct_id;
			_parent_id = parent_id;
			_name = name;
			if (created_at == null)	
				_created_at = DateTime.Now;
			else
				_created_at = (DateTime)created_at;
		}
	}
}

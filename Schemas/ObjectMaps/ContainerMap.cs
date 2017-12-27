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
		/// Конструктор
		/// </summary>
		public ContainerMap(long ct_id, string name, DateTime created_at)
        {
			_ct_id = ct_id;
			_name = name;
			//if (created_at == null)	created_at = DateTime.Now;
			_created_at = created_at;
		}
	}
}

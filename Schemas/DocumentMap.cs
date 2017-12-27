using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
	/// <summary>
	/// Класс хранит представление одного документа.
	/// </summary>
	public class DocumentMap
	{
		private long _doc_id = -1;
		private long _ct_id = -1;
		private DateTime _created_at;
		private string _name = "";

		/// <summary>
		/// Идентификатор документа в БД.
		/// </summary>
		public long DocumentID { get { return _doc_id; } set { _doc_id = value; } }
		/// <summary>
		/// Контейнер размещения — ссылка на mContainers.
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
		public DocumentMap(long doc_id, long ct_id, string name, DateTime created_at)
        {
			_doc_id = doc_id;
			_ct_id = ct_id;
			_name = name;
			//if (created_at == null)	created_at = DateTime.Now;
			_created_at = created_at;
		}

	}
}

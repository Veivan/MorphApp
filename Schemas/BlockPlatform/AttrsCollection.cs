using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет коллекцию атрибутов типа блока.
	/// </summary>
	public class AttrsCollection
	{
		private List<Attribute> attrs = new List<Attribute>();

	
		public List<Attribute> Attrs
			{
				get
				{
					var ret = new List<Attribute>(attrs);
					return ret;
				}

			}

		/// <summary>
		/// Получение атрибута по индексу.
		/// </summary>
		public Attribute this[int index]
		{
			get
			{
				return Attrs[index];
			}
		}

		/// <summary>
		/// Получение атрибута по ключу.
		/// </summary>
		public Attribute this[string nameKey]
		{
			get
			{
				return attrs.Where(o => o.NameKey == nameKey).FirstOrDefault();
			}
		}

		public void AddElement(Attribute attr)
		{
			if (!attrs.Contains(attr))
				attrs.Add(attr);
		}

		public int GetOrdByNameKey(string attrnamekey)
		{
			var attr = attrs.Where(o => o.NameKey == attrnamekey).FirstOrDefault();
			if (attr == null)
				throw new Exception(string.Format("У объекта нет атрибута '{0}'!", attrnamekey));
			var ord = attr.Order;
			return ord;
		}

		public List<enAttrTypes> GetAttrTypesList()
		{
			return attrs.Select(o => o.AttrType).ToList();
		}
	}
}

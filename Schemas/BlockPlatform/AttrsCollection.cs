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
		/// Получение атрибута по имени.
		/// </summary>
		public Attribute this[string name]
		{
			get
			{
				return attrs.Where(o => o.Name == name).FirstOrDefault();
			}
		}

		public void AddElement(Attribute attr)
		{
			if (!Attrs.Contains(attr))
				Attrs.Add(attr);
		}

		public int GetOrdByName(string attrname)
		{
			var attr = attrs.Where(o => o.Name == attrname).FirstOrDefault();
			if (attr == null)
				throw new Exception(string.Format("У объекта нет атрибута '{0}'!", attrname));
			var ord = attr.Order;
			return ord;
		}

		public List<enAttrTypes> GetAttrTypesList()
		{
			return attrs.Select(o => o.AttrType).ToList();
		}
	}
}

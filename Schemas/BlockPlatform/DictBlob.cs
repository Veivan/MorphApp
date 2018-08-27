using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemas
{
	/// <summary>
	/// Класс предназначен для работы с фактическими данными блока-справочника
	/// </summary>
	public class DictBlob : Blob
	{
		// Порядковые номера атрибутов в справочнике
		const int nordResolvedType = 0;
		const int nordArr = 1;

		/// <summary>
		/// Конструктор - создание Blob-справочника из двоичных данных (После чтения из БД)
		/// </summary>
		/// <param name="data">массив байт</param>
		public DictBlob(byte[] data) : base(data)
		{
			var attrTypes = new List<enAttrTypes>();
			attrTypes.Add(enAttrTypes.mnint);	// атрибут "ResolvedType"
			attrTypes.Add(enAttrTypes.mnarr);	// перечень элементов
			_factdata = new List<AttrFactData>();
			if (data == null)
			{
				_factdata.Add(new AttrFactData(enAttrTypes.mnint, 0));
				_factdata.Add(new AttrFactData(enAttrTypes.mnarr, null));
			}
			else
				ParseBlob(attrTypes);
		}

		/// <summary>
		/// Получение первого INT из хранящихся в byte[]
		/// </summary>
		/// <returns>Значение атрибута "ResolvedType" - тип объектов, содержащихся в справочнике</returns>
		/// <remarks>Используется в чтении данных справочников </remarks>
		public int GetDictResolvedTypeFromBytes()
		{
			return (int)ValueList[nordResolvedType].Value;
		}

		/// <summary>
		/// Получение перечня ссылок (адресов) на элементы справочника из хранящихся в byte[]
		/// </summary>
		/// <returns>массив ссылок (адресов) на элементы справочника</returns>
		/// <remarks>Используется в чтении данных справочников </remarks>
		public long[] GetDictContentFromBytes()
		{
			var attrval = GetAttrValue(nordArr);
			if (attrval == null)
				return null;

			return ((List<long>)attrval).ToArray();
		}

		/// <summary>
		/// Добавление массива адресов элементов в справочник
		/// </summary>
		/// <param name="subaddr">массив адресов элементов</param>
		public void AddElements(long[] subaddr)
		{
			if (subaddr == null || subaddr.Count() == 0)
				return;
			var oldval = GetDictContentFromBytes();
			var list = new List<long>();
			if (oldval != null)
				list.AddRange(oldval);
			list.AddRange(subaddr);
			var noDupes = list.Distinct().ToList();
			this.SetAttrValue(nordArr, noDupes);
		}

		/// <summary>
		/// Удаление массива адресов элементов из справочника
		/// </summary>
		/// <param name="subaddr">массив адресов элементов</param>
		public void RemoveElements(long[] subaddr)
		{
			var oldval = GetDictContentFromBytes();
			var list = new List<long>();
			list.AddRange(oldval);
			list.RemoveAll(o => subaddr.ToList().Contains(o));
			this.SetAttrValue(nordArr, list);
		}
	}
}

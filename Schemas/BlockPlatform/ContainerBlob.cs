using System;
using System.Collections.Generic;
using System.Linq;

namespace Schemas
{
	/// <summary>
	/// Класс предназначен для работы с фактическими данными блока-Контейнера
	/// </summary>
	public class ContainerBlob : Blob
	{
		// Порядковые номера атрибутов в справочнике
		const int nordName = 0;
		const int nordContent = 1;

		/// <summary>
		/// Конструктор - создание Blob-справочника из двоичных данных (После чтения из БД)
		/// </summary>
		/// <param name="data">массив байт</param>
		public ContainerBlob(byte[] data) : base(data)
		{
			var attrTypes = new List<enAttrTypes>();
			attrTypes.Add(enAttrTypes.mntxt);  // атрибут "Name"
			attrTypes.Add(enAttrTypes.mnarr);   // перечень содержащихся в Контейнере объектов
			_factdata = new List<AttrFactData>();
			if (data == null)
			{
				_factdata.Add(new AttrFactData(enAttrTypes.mntxt, Session.DefaulContainerName));
				_factdata.Add(new AttrFactData(enAttrTypes.mnarr, 0));
			}
			else
				ParseBlob(attrTypes);
		}
		/// <summary>
		/// Получение первого String из хранящихся в byte[]
		/// </summary>
		/// <returns>Значение атрибута "Name" - наименование Контейнера</returns>
		/// <remarks>Используется в чтении данных Контейнера </remarks>
		public string GetContainerNameFromBytes()
		{
			return (string)ValueList[nordName].Value;
		}

		/// <summary>
		/// Получение перечня ссылок (адресов) на содержащиеся в Контейнере объекты из хранящихся в byte[]
		/// </summary>
		/// <returns>массив ссылок (адресов) на содержащиеся в Контейнере объекты</returns>
		/// <remarks>Используется в чтении данных Контейнера </remarks>
		public long[] GetContainerContentFromBytes()
		{
			var attrval = GetAttrValue(nordContent);
			if (attrval == null)
				return null;
			return ((List<long>)attrval).ToArray();
		}

		/// <summary>
		/// Добавление массива адресов объектов в Контейнер
		/// </summary>
		/// <param name="subaddr">массив адресов объектов</param>
		public void AddElements(long[] subaddr)
		{
			if (subaddr == null || subaddr.Count() == 0)
				return;
			var oldval = GetContainerContentFromBytes();
			var list = new List<long>();
			if (oldval != null)
				list.AddRange(oldval);
			list.AddRange(subaddr);
			var noDupes = list.Distinct().ToList();
			this.SetAttrValue(nordContent, noDupes);
		}

		/// <summary>
		/// Удаление массива адресов объектов из Контейнера
		/// </summary>
		/// <param name="subaddr">массив адресов объектов</param>
		public void RemoveElements(long[] subaddr)
		{
			var oldval = GetContainerContentFromBytes();
			if (oldval == null)
				return;
			var list = new List<long>();
			list.AddRange(oldval);
			list.RemoveAll(o => subaddr.ToList().Contains(o));
			this.SetAttrValue(nordContent, list);
		}
	}
}

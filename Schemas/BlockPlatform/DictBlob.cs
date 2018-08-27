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
			ParseBlob(attrTypes);
		}

		/// <summary>
		/// Получение первого INT из хранящихся в byte[]
		/// </summary>
		/// <returns>Значение атрибута "ResolvedType" - тип объектов, содержащихся в справочнике</returns>
		/// <remarks>Используется в чтении данных справочников </remarks>
		public int GetDictResolvedTypeFromBytes()
		{
			return (int)this._factdata[0].Value;
		}

		/// <summary>
		/// Получение перечня ссылок (адресов) на элементы справочника из хранящихся в byte[]
		/// </summary>
		/// <returns>массив ссылок (адресов) на элементы справочника</returns>
		/// <remarks>Используется в чтении данных справочников </remarks>
		public long[] GetDictContentFromBytes()
		{
			return ((List<long>)this._factdata[1].Value).ToArray();
		}

		/// <summary>
		/// Создание нового блоба из существующего
		/// путём добавления массива адресов элементов
		/// </summary>
		/// <param name="subaddr">массив адресов добавляемых элементов</param>
		public void AddElements(long[] subaddr)
		{
			throw new NotImplementedException();
		}
	}
}

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
		public DictBlob (byte[] data) : base(null, data)
		{
			this._bytedata = new byte[data.Length];
			int idx = 0;
			for (int i = 0; i < _bytedata.Length; i++)
				_bytedata[idx++] = data[i];
		}

		/// <summary>
		/// Получение первого INT из хранящихся в byte[]
		/// </summary>
		/// <param name="byteArray">byte array</param>
		/// <returns>Значение атрибута "ResolvedType" - тип объектов, содержащихся в справочнике</returns>
		/// <remarks>Используется в чтении данных справочников </remarks>
		public int GetDictResolvedTypeFromBytes()
		{
			const int intlen = 4; // количество байт для хранения Int
			int offset = 0; // Текущая позиция чтения в _bytedata
			var arrbt = new byte[intlen];
			for (int i = 0; i < intlen; i++)
				arrbt[i] = _bytedata[offset++];
			if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
			var result = BitConverter.ToInt32(arrbt, 0);
			return result;
		}

		/// <summary>
		/// Получение перечня ссылок (адресов) на элементы справочника из хранящихся в byte[]
		/// </summary>
		/// <param name="byteArray">byte array</param>
		/// <returns>перечня ссылок (адресов) на элементы справочника</returns>
		/// <remarks>Используется в чтении данных справочников </remarks>
		public long[] GetDictContentFromBytes()
		{
			const int intlen = 4; // количество байт для хранения Int
			const int lnglen = 8; // количество байт для хранения адреса
			int offset = intlen; // Текущая позиция чтения в byteArray. 
								 //Перемещение курсора на начало второго атрибута справочника - его содержимого.
			if (offset == _bytedata.Length) // Пустой справочник
				return null;

			var arrbt = new byte[intlen];
			// Определяем длину списка ссылок
			arrbt = new byte[intlen];
			for (int i = 0; i < intlen; i++)
				arrbt[i] = _bytedata[offset++];
			if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
			var listlen = BitConverter.ToInt32(arrbt, 0);
			// Чтение списка
			var lst = new List<long>();
			arrbt = new byte[lnglen];
			for (int i = 0; i < listlen; i++)
			{
				for (int j = 0; j < lnglen; j++)
					arrbt[j] = _bytedata[offset++];
				if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
				lst.Add(BitConverter.ToInt64(arrbt, 0));
			}
			return lst.ToArray();
		}

	}
}

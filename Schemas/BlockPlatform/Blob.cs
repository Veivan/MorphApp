using System;
using System.Collections.Generic;

namespace Schemas
{
	/// <summary>
	/// Класс предназначен для работы с фактическими данными блока
	/// </summary>
	/// <remarks>После чтения из БД создаётся новый Blob из данных БД. При этом данные парсятся и заносятся в справочник. 
	/// Получить фактические данные атрибутов можно в виде ValueList.
	/// Перед записью фактических данных в БД надо создать Blob из справочника - сформировать массив байт.
	/// После этого он доступен по свойству Data. 
	/// </remarks>
	public class Blob
	{
		//private List<enAttrTypes> _attrTypes; // список типов атрибутов
		private byte[] _bytedata; // данные атрибутов в двоичном виде
		private List<AttrFactData> _factdata; // хранилище фактических данных атрибутов
		
		/// <summary>
		/// Конструктор - создание Blob из двоичных данных (После чтения из БД)
		/// </summary>
		/// <param name="attrTypes">Список типов атрибутов блоков </param>
		public Blob(List<enAttrTypes> attrTypes, byte[] data)
		{
			//_attrTypes = new List<enAttrTypes>(attrTypes);
			_bytedata = new byte[data.Length];
			int idx = 0;
			for (int i = 0; i < _bytedata.Length; i++)
				_bytedata[idx++] = data[i];
			ParseBlob(attrTypes);
		}

		/// <summary>
		/// Конструктор - создание Blob из фактических данных (Перед записью в БД)
		/// </summary>
		/// <param name="attrTypes">Список типов атрибутов блоков </param>
		public Blob(List<AttrFactData> attrs)
		{
			_factdata = new List<AttrFactData>(attrs);
			MakeBlob();
		}

		public byte[] Data { get { return _bytedata;} }

		public List<AttrFactData> ValueList { get { return _factdata;	} }
		
		/// <summary>
		/// Создание массива байт из фактических значений атрибутов
		/// </summary>
		private void MakeBlob()
		{
			foreach (var rec in _factdata)
			{
				switch (rec.Type)
				{
					case enAttrTypes.mntxt:
						byte[] arrmntxt = new byte[3];
						AddData(arrmntxt);
						break;
					case enAttrTypes.mnint:
						int number = (int)rec.Value;
						byte[] arrmnint = BitConverter.GetBytes(number);
						if (BitConverter.IsLittleEndian)
							Array.Reverse(arrmnint);
						AddData(arrmnint);
						break;
					case enAttrTypes.mnreal:
						break;
					case enAttrTypes.mnbool:
						break;
					case enAttrTypes.mnblob:
						break;
					case enAttrTypes.mnlink:
						break;
					case enAttrTypes.mnarr:
						break;
				}
			}
		}

		/// <summary>
		/// Разбор Блоба из БД на значения атрибутов и заполнение Словаря
		/// </summary>
		private void ParseBlob(List<enAttrTypes> _attrTypes)
		{
			throw new NotImplementedException();
			/*byte[] bytes = { 0, 0, 0, 25 };

// If the system architecture is little-endian (that is, little end first),
// reverse the byte array.
if (BitConverter.IsLittleEndian)
    Array.Reverse(bytes);

int i = BitConverter.ToInt32(bytes, 0);
Console.WriteLine("int: {0}", i);
// Output: int: 25*/
		}

		private void AddData(byte[] addon)
		{
			if (addon == null) return;
			var selflen = _bytedata == null ? 0 : _bytedata.Length;
			byte[] result = new byte[selflen + addon.Length];
			int idx = 0;
			for (int i = 0; i < selflen; i++)
				result[idx++] = _bytedata[i];
			for (int j = 0; j < addon.Length; j++)
				result[idx++] = addon[j];
			_bytedata = result;
		}
	}
}

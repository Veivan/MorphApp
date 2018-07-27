using System;
using System.Collections.Generic;
using System.Text;

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
			_factdata = new List<AttrFactData>();
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

		public byte[] Data { get { return _bytedata; } }

		public List<AttrFactData> ValueList { get { return _factdata; } }

		/// <summary>
		/// Создание массива байт из фактических значений атрибутов
		/// </summary>
		private void MakeBlob()
		{
			byte[] arrbt;
			foreach (var rec in _factdata)
			{
				switch (rec.Type)
				{
					case enAttrTypes.mntxt:
					case enAttrTypes.mnrtf:
						var txt = (string)rec.Value;
						arrbt = Encoding.UTF8.GetBytes(txt);
						var lenbt = BitConverter.GetBytes(arrbt.Length);
						AddData(lenbt);
						AddData(arrbt);
						break;
					case enAttrTypes.mnlink:
					case enAttrTypes.mnint:
						var number = (int)rec.Value;
						arrbt = BitConverter.GetBytes(number);
						AddData(arrbt);
						break;
					case enAttrTypes.mnreal:
						var fnumber = (float)rec.Value;
						arrbt = BitConverter.GetBytes(fnumber);
						AddData(arrbt);
						break;
					case enAttrTypes.mnbool:
						var bflag = (bool)rec.Value;
						arrbt = BitConverter.GetBytes(bflag);
						AddData(arrbt);
						break;
					case enAttrTypes.mnblob:
						break;
					case enAttrTypes.mnarr:
						var lst = (List<int>)rec.Value;
						arrbt = BitConverter.GetBytes(lst.Count);
						AddData(arrbt);
						foreach (var addr in lst)
						{
							arrbt = BitConverter.GetBytes(addr);
							AddData(arrbt);
						}
						break;
				}
			}
		}

		/// <summary>
		/// Разбор Блоба из БД на значения атрибутов и заполнение Словаря
		/// </summary>
		private void ParseBlob(List<enAttrTypes> _attrTypes)
		{
			if (_bytedata == null || _bytedata.Length == 0) return;

			byte[] arrbt;
			AttrFactData attrfactdata;
			int shift = 0;
			int offset = 0; // Текущая позиция чтения в _bytedata
			object value = null;

			foreach (var type in _attrTypes)
			{
				switch (type)
				{
					case enAttrTypes.mntxt:
					case enAttrTypes.mnrtf:
						shift = 4;
						// Определяем длину текста
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						offset += shift;
						shift = BitConverter.ToInt32(arrbt, 0);
						// Чтение текста
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						value = Encoding.UTF8.GetString(arrbt);
						break;
					case enAttrTypes.mnlink:
					case enAttrTypes.mnint:
						shift = 4;
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						value = BitConverter.ToInt32(arrbt, 0);
						break;
					case enAttrTypes.mnreal:
						shift = 4;
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						value = BitConverter.ToSingle(arrbt, 0);
						break;
					case enAttrTypes.mnbool:
						shift = 1;
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						value = BitConverter.ToBoolean(arrbt, 0);
						break;
					case enAttrTypes.mnblob:
						break;
					case enAttrTypes.mnarr:
						shift = 4;
						// Определяем длину списка ссылок
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						offset += shift;
						shift = BitConverter.ToInt32(arrbt, 0);
						// Чтение списка
						var lst = new List<int>();
						arrbt = new byte[shift];
						for (int i = 0; i < shift; i++)
							arrbt[i] = _bytedata[offset + i];
						if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
						value = Encoding.UTF8.GetString(arrbt);

						/*var lst = (List<int>)rec.Value;
						arrbt = BitConverter.GetBytes(lst.Count);
						AddData(arrbt);
						foreach (var addr in lst)
						{
							arrbt = BitConverter.GetBytes(addr);
							AddData(arrbt);
						}*/
						break;
				}
				attrfactdata = new AttrFactData(type, value);
				_factdata.Add(attrfactdata);
				offset += shift;
			}
		}

		private void AddData(byte[] addon)
		{
			if (addon == null) return;
			if (BitConverter.IsLittleEndian)
				Array.Reverse(addon);
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

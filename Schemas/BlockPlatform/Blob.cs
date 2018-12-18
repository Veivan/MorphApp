using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using System.Linq;

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
		protected byte[] _bytedata; // данные атрибутов в двоичном виде
		protected List<AttrFactData> _factdata; // хранилище фактических данных атрибутов

		#region Constructors
		/// <summary>
		/// Конструктор - копировщик.
		/// Доступен только в наслелнике - DictBlob. Возможно и не нужен после ликвидации DictBlob.
		/// </summary>
		/// <param name="data">массив байт</param>
		/// <param name="fieldsCount">Количество атрибутов блоба</param>
		protected Blob(byte[] data, int fieldsCount)
		{
			int idx = 0;
			if (data == null)
			{
				this._bytedata = new byte[fieldsCount];
				for (int i = 0; i < _bytedata.Length; i++)
					_bytedata[idx++] = 1;
			}
			else
			{
				this._bytedata = new byte[data.Length];
				for (int i = 0; i < _bytedata.Length; i++)
					_bytedata[idx++] = data[i];
			}
		}

		/// <summary>
		/// Конструктор - создание Blob из двоичных данных (После чтения из БД)
		/// </summary>
		/// <param name="attrTypes">Список типов атрибутов блоков</param>
		/// <param name="data">массив байт</param>
		public Blob(List<enAttrTypes> attrTypes, byte[] data) : this (data, attrTypes.Count)
		{
			_factdata = new List<AttrFactData>();
			if (data == null)
			{
				foreach (var type in attrTypes)
				{
					var attrfactdata = new AttrFactData(type, null);
					_factdata.Add(attrfactdata);
				}
			}
			else
				ParseBlob(attrTypes);
		}

		/// <summary>
		/// Конструктор - создание Blob из фактических данных (Перед записью в БД)
		/// </summary>
		/// <param name="attrs">Список фактических данных</param>
		public Blob(List<AttrFactData> attrs)
		{
			_factdata = new List<AttrFactData>(attrs);
			MakeBlob();
		}
		#endregion

		public byte[] Data { get { return _bytedata; } }

		public List<AttrFactData> ValueList { get { return _factdata; } }
		
		/// <summary>
		/// Получение значения атрибута
		/// </summary>
		/// <param name="order">Порядок следования атрибута в списке атрибутов</param>
		public object GetAttrValue(int order)
		{
			if (order >= this._factdata.Count)
				throw new Exception(string.Format("Отсутствует атрибут с номером {0}!", order));
			return _factdata[order].Value;
		}

		/// <summary>
		/// Присвоение нового значения атрибуту
		/// </summary>
		/// <param name="order">Порядок следования атрибута в списке атрибутов</param>
		/// <param name="value">Новое значение атрибута</param>
		public void SetAttrValue(int order, object value)
		{
			if (order >= this._factdata.Count)
				throw new Exception(string.Format("Нет атрибута с номером {0} !", order));
			_factdata[order] = new AttrFactData(_factdata[order].Type, value);
			_bytedata = null;
			MakeBlob();
		}

		/// <summary>
		/// Создание массива байт из фактических значений атрибутов
		/// </summary>
		private void MakeBlob()
		{
			byte[] arrbt;
			foreach (var rec in _factdata)
			{
				// Добавление флага, определяещего состояние IsNull последующего поля
				// IsNull = 1 (true), иначе = 0 (false)
				if (rec.Value == null)
				{
					AddData(new byte[] { 1 });
					continue;
				}
				else
					AddData(new byte[] { 0 });

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
					case enAttrTypes.mnlong:
						var number = (long)rec.Value;
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
						var img = (Image)rec.Value;
						arrbt = TMorph.Common.Utils.ImageToByte(img);
						//arrbt = TMorph.Common.Utils.BytesFromImage(img);						
						var arrlen = BitConverter.GetBytes(arrbt.Length);
						AddData(arrlen);
						AddData(arrbt);
						break;
					case enAttrTypes.mnarr:
						var lst = (List<long>)rec.Value;
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
		protected void ParseBlob(List<enAttrTypes> _attrTypes)
		{
			if (_bytedata == null || _bytedata.Length == 0 || _attrTypes == null) return;

			const int intlen = 4; // количество байт для хранения Int
			const int boolen = 1; // количество байт для хранения bool
			const int lnglen = 8; // количество байт для хранения адреса
			byte[] arrbt;
			AttrFactData attrfactdata;
			int offset = 0; // Текущая позиция чтения в _bytedata
			object value = null;

			foreach (var type in _attrTypes)
			{
				value = null;
				// Читаем значение IsNull
				arrbt = new byte[boolen];
				for (int i = 0; i < boolen; i++)
					arrbt[i] = _bytedata[offset++];
				if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
				var IsNull = BitConverter.ToBoolean(arrbt, 0);

				if (!IsNull)
				{
					switch (type)
					{
						case enAttrTypes.mntxt:
						case enAttrTypes.mnrtf:
							// Определяем длину текста
							arrbt = new byte[intlen];
							for (int i = 0; i < intlen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
							var txtlen = BitConverter.ToInt32(arrbt, 0);
							// Чтение текста
							arrbt = new byte[txtlen];
							for (int i = 0; i < txtlen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
							value = Encoding.UTF8.GetString(arrbt);
							break;
						case enAttrTypes.mnlink:
						case enAttrTypes.mnlong:
							arrbt = new byte[lnglen];
							for (int i = 0; i < lnglen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
							value = BitConverter.ToInt64(arrbt, 0);
							break;
						case enAttrTypes.mnreal:
							arrbt = new byte[lnglen];
							for (int i = 0; i < lnglen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
							value = BitConverter.ToSingle(arrbt, 0);
							break;
						case enAttrTypes.mnbool:
							arrbt = new byte[boolen];
							for (int i = 0; i < boolen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
							value = BitConverter.ToBoolean(arrbt, 0);
							break;
						case enAttrTypes.mnblob:
							// Определяем длину массива байт картинки
							arrbt = new byte[intlen];
							for (int i = 0; i < intlen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);
							var imglen = BitConverter.ToInt32(arrbt, 0);
							// Чтение массива байт картинки
							arrbt = new byte[imglen];
							for (int i = 0; i < imglen; i++)
								arrbt[i] = _bytedata[offset++];
							if (BitConverter.IsLittleEndian) Array.Reverse(arrbt);

							//value = TMorph.Common.Utils.GetImageFromByteArray(arrbt);

							using (var ms = new MemoryStream(arrbt))
							{
								value = Image.FromStream(ms, true);
							}
							break;
						case enAttrTypes.mnarr:
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
							value = lst;
							break;
					}
				}
				attrfactdata = new AttrFactData(type, value);
				_factdata.Add(attrfactdata);
			}
		}

		/// <summary>
		/// Добавление массива байт к внутреннему массиву _bytedata
		/// </summary>
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

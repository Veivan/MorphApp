using System.Collections.Generic;
using Schemas;
using Schemas.BlockPlatform;

namespace DirectDBconnector
{

	/// <summary>
	/// Класс предназначен для работы с фактическими данными блока
	/// </summary>
	public class BlobConvertor
	{
		private List<enAttrTypes> _attrTypes;
		
		/// <summary>
		/// Конструктор
		/// </summary>
		/// <param name="attrTypes">Список типов атрибутов блоков </param>
		public BlobConvertor(List<enAttrTypes> attrTypes)
		{
			_attrTypes = new List<enAttrTypes>(attrTypes);
		}

		/// <summary>
		/// Формирование Blob из справочника значений
		/// </summary>
		/// <param name="attrTypes">Список ID типов атрибутов блоков </param>
		/// <returns>Blob - массив байт</returns>
		public Blob BlobSetData(Dictionary<AttrType, object> attrs)
		{
			Blob result = null;
			foreach (var rec in attrs)
			{

			}

			//enAttrTypes
			return result;
		}
		
	}
}

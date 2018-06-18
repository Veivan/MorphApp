using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
	/// <summary>
	/// Абстрактный класс, обобщающий методы работы с блоками.
	/// Представляет API для работы с блоками.
	/// </summary>
	public abstract class IBlockDealer
	{
		#region Функции для работы с Типами блоков

		/// <summary>
		/// Создание нового типа блоков.
		/// </summary>
		/// <param name="name">Наименование нового типа блоков</param>
		/// <returns>адрес добавленного объекта </returns>
		public abstract int CreateBlockType(string name);

		/// <summary>
		/// Получение адреса типа блока по наименованию.
		/// </summary>
		/// <param name="name">Наименование нового типа блоков</param>
		/// <returns>адрес объекта </returns>
		public abstract int GetBlockTypeByName(string name);

		/// <summary>
		/// Получение наименования типа блока по адресу.
		/// </summary>
		/// <param name="addr">адрес типа блоков</param>
		/// <returns>наименования объекта </returns>
		public abstract string GetBlockTypeByAddr(int addr);
		#endregion

		#region Функции для работы с атрибутами типов блоков

		/// <summary>
		/// Создание нового атрибута типа блоков.
		/// </summary>
		/// <param name="name">Наименование атрибута</param>
		/// <param name="AttrType">адрес типа атрибута (объект справочника "Типы атрибутов")</param>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <param name="sorder">Порядок следования в типе блока.Можно задать явно.Можно задать = 0, 
		///		тогда функция должна определить последний максимальный номер атрибута и
		///		новому атрибуту присвоить последний максимальный номер +1</param>
		/// <param name="mandatory">Обязательный к заполнению(true), иначе false(default)</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract int CreateAttribute(string name, int AttrType, int BlockType, int sorder, bool mandatory = false);

		#endregion

		#region Функции для работы со Блоками

		/// <summary>
		/// Создание нового блока.
		/// </summary>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <param name="parent">Поддержка дерева.Родитель.Это ссылка на блок, являющийся Родителем для блока.</param>
		/// <param name="treeorder">Поддержка дерева. Последовательность блока в списке детей Родителя (порядок блока в дереве).
		///		Можно задать явно.Можно задать = 0, тогда функция должна определить последний максимальный номер атрибута и
		///		присвоить новому атрибуту последний максимальный номер +1</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract int CreateBlock(int BlockType, int parent, int treeorder);

		/// <summary>
		/// Присвоение блоку адреса родительского блока в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="parent">адрес родительского блока</param>
		/// <returns></returns>
		public abstract void SetParent(int addr, int parent);

		/// <summary>
		/// Получение адреса родительского блока в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>адрес родительского блока</returns>
		public abstract int GetParent(int addr);

		/// <summary>
		/// Присвоение блоку порядка следования среди блоков одного уровня(сиблингов) в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="order">порядок следования</param>
		/// <returns></returns>
		public abstract void SetOrder(int addr, int order);

		/// <summary>
		/// Получение порядка следования блока среди блоков одного уровня(сиблингов) в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>порядок следования</returns>
		public abstract int GetOrder(int addr);

		#endregion

		#region Функции для работы со Справочниками

		/// <summary>
		/// Создание нового атрибута типа блоков.
		/// </summary>
		/// <param name="name">Наименование атрибута</param>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract int CreateDictionary(string name, int BlockType);

		#endregion

	}
}

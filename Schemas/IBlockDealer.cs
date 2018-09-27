using System;
using System.Collections.Generic;

using BlockAddress = System.Int64;
using Schemas.BlockPlatform;

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
		/// <param name="nameKey">Ключ</param>
		/// <param name="nameUI">Видимое наименование</param>
		/// <returns>объект типа BlockType </returns>
		public abstract BlockType CreateBlockType(string nameKey, string nameUI);

		/// <summary>
		/// Получение типа блока по ключу.
		/// </summary>
		/// <param name="nameKey">Ключ типа блоков</param>
		/// <returns>объект типа BlockType</returns>
		public abstract BlockType GetBlockTypeByNameKey(string nameKey);

		/// <summary>
		/// Получение типа блока по адресу.
		/// </summary>
		/// <param name="addr">адрес типа блоков</param>
		/// <returns>объект типа BlockType</returns>
		public abstract BlockType GetBlockTypeByAddr(BlockAddress addr);

		/// <summary>
		/// Получение всех типов блоков.
		/// </summary>
		/// <returns>список всех типов блоков</returns>
		public abstract List<BlockType> GetAllBlockTypes();

		#endregion

		#region Функции для работы с атрибутами типов блоков

		/// <summary>
		/// Создание нового атрибута типа блоков.
		/// </summary>
		/// <param name="nameKey">Ключ атрибута</param>
		/// <param name="nameUI">Видимое наименование атрибута</param>
		/// <param name="AttrType">адрес типа атрибута (объект справочника "Типы атрибутов")</param>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <param name="sorder">Порядок следования в типе блока.Можно задать явно.Можно задать = 0, 
		///		тогда функция должна определить последний максимальный номер атрибута и
		///		новому атрибуту присвоить последний максимальный номер +1</param>
		/// <param name="mandatory">Обязательный к заполнению(true), иначе false(default)</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract BlockAddress CreateAttribute(string nameKey, string nameUI, BlockAddress AttrType, BlockAddress BlockType, int sorder, bool mandatory = false);

		/// <summary>
		/// Получение списка ключей атрибутов типа блока.
		/// </summary>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <returns>список ключей атрибутов</returns>
		public abstract List<string> GetFildsNameKey(BlockAddress BlockType);

		/// <summary>
		/// Получение коллекции атрибутов типа блока.
		/// </summary>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <returns>коллекция имён атрибутов</returns>
		public abstract AttrsCollection GetAttrsCollection(BlockAddress BlockType);

		#endregion

		#region Функции для работы с Блоками

		/// <summary>
		/// Создание нового блока.
		/// </summary>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <param name="parent">Поддержка дерева.Родитель.Это ссылка на блок, являющийся Родителем для блока.</param>
		/// <param name="treeorder">Поддержка дерева. Последовательность блока в списке детей Родителя (порядок блока в дереве).
		///		Можно задать явно.Можно задать = 0, тогда функция должна определить последний максимальный номер атрибута и
		///		присвоить новому атрибуту последний максимальный номер +1</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract BlockAddress CreateBlock(BlockAddress BlockType, BlockAddress parent, int treeorder);

		/// <summary>
		/// Получение Блока по адресу.
		/// </summary>
		/// <param name="addr">адрес Блока</param>
		/// <returns>Блок</returns>
		public abstract BlockBase GetBlock(BlockAddress addr);

		/// <summary>
		/// Присвоение блоку адреса родительского блока в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="parent">адрес родительского блока</param>
		/// <returns></returns>
		public abstract void SetParent(BlockAddress addr, BlockAddress parent);

		/// <summary>
		/// Получение адреса родительского блока в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>адрес родительского блока</returns>
		public abstract BlockAddress GetParent(BlockAddress addr);

		/// <summary>
		/// Присвоение блоку порядка следования среди блоков одного уровня(сиблингов) в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="order">порядок следования</param>
		/// <returns></returns>
		public abstract void SetOrder(BlockAddress addr, int order);

		/// <summary>
		/// Получение порядка следования блока среди блоков одного уровня(сиблингов) в дереве.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>порядок следования</returns>
		public abstract int GetOrder(BlockAddress addr);

		/// <summary>
		/// Присвоение фактических данных блоку.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="data">фактические данные</param>
		/// <param name="MakeVersion">флаг, указывающий создавать новую версию блока или нет</param>
		/// <returns>Функция возвращает адрес блока — (Если MakeVersion = false, то = addr, если MakeVersion = true, то адрес новой версии).</returns>
		public abstract BlockAddress SetFactData(BlockAddress addr, Blob data, bool MakeVersion = false);

		/// <summary>
		/// Получение фактических данных блока.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <returns>фактические данные</returns>
		public abstract Blob GetFactData(BlockAddress addr);

		/// <summary>
		/// Присвоение значения атрибуту блока по номеру атрибута.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="ord">порядковый номер атрибута</param>
		/// <param name="value">новое значение атрибута</param>
		/// <returns>значение атрибута блока</returns>
		public abstract void AttrSetValue(BlockAddress addr, int ord, object value);

		/// <summary>
		/// Получение значения атрибута блока по номеру атрибута.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="ord">порядковый номер атрибута</param>
		/// <returns>значение атрибута блока</returns>
		public abstract T AttrGetValue<T>(BlockAddress addr, int ord);

		/// <summary>
		/// Присвоение значения атрибуту блока по имени атрибута.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="attrname">имя атрибута</param>
		/// <param name="value">новое значение атрибута</param>
		/// <returns>значение атрибута блока</returns>
		public abstract void AttrSetValue(BlockAddress addr, string attrname, object value);

		/// <summary>
		/// Получение значения атрибута блока по имени атрибута.
		/// </summary>
		/// <param name="addr">адрес блока</param>
		/// <param name="attrname">имя атрибута</param>
		/// <returns>значение атрибута блока</returns>
		public abstract T AttrGetValue<T>(BlockAddress addr, string attrname);

		#endregion

		#region Функции для работы со Справочниками

		/// <summary>
		/// Создание нового Справочника.
		/// </summary>
		/// <param name="name">Наименование Справочника</param>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract BlockAddress CreateDictionary(string name, int BlockType);

		/// <summary>
		/// Получение адреса Справочника по наименованию.
		/// </summary>
		/// <param name="name">Наименование Справочника</param>
		/// <returns>адрес объекта </returns>
		public abstract BlockAddress GetDictionaryByName(string name);

		/// <summary>
		/// Получение наименования Справочника по адресу.
		/// </summary>
		/// <param name="addr">адрес Справочника</param>
		/// <returns>наименования объекта </returns>
		public abstract string GetDictName(BlockAddress addr);

		/// <summary>
		/// Получение типа блока, который может содержаться в справочнике, по адресу.
		/// </summary>
		/// <param name="addr">адрес Справочника</param>
		/// <returns>адрес типа блока</returns>
		public abstract long GetDictType(BlockAddress addr);

		/// <summary>
		/// Получение адресов блоков, содержащихся в справочнике.
		/// </summary>
		/// <param name="addr">адрес Справочника</param>
		/// <returns>массив адресов блоков </returns>
		public abstract BlockAddress[] GetDictContent(BlockAddress addr);

		/// <summary>
		/// Добавление элементов(блоков) в справочник.
		/// </summary>
		/// <param name="addr">адрес Справочника</param>
		/// <param name="subaddr">массив адресов добавляемых блоков</param>
		/// <returns></returns>
		public abstract void DictAddBlocks(BlockAddress addr, BlockAddress[] subaddr);

		/// <summary>
		/// Удаление элементов(блоков) из справочника.
		/// </summary>
		/// <param name="addr">адрес Справочника</param>
		/// <param name="subaddr">массив адресов удаляемого блоков</param>
		/// <returns></returns>
		public abstract void DictRemoveBlocks(BlockAddress addr, BlockAddress[] subaddr);

		#endregion

	}
}

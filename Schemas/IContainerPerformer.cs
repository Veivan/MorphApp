using System;
using System.Collections.Generic;

using BlockAddress = System.Int64;

namespace Schemas
{
	/// <summary>
	/// Абстрактный класс, обобщающий методы работы с контейнерами : 
	/// создание, редактирование и различные выборки для визуализации.
	/// Представляет API для работы с контейнерами.
	/// </summary>
	public abstract class IContainerPerformer
	{
		/// <summary>
		/// Создание нового Контейнера.
		/// </summary>
		/// <param name="name">Наименование Контейнера</param>
		/// <param name="parent">Поддержка дерева.Родитель.Это ссылка на блок, являющийся Родителем для блока.</param>
		/// <param name="treeorder">Поддержка дерева. Последовательность блока в списке детей Родителя (порядок блока в дереве).
		///		Можно задать явно.Можно задать = 0, тогда функция должна определить последний максимальный номер атрибута и
		///		присвоить новому атрибуту последний максимальный номер +1</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract BlockAddress CreateContainer(string name, BlockAddress parent, int treeorder);

		/// <summary>
		/// Чтения дочерних контейнеров множества родительских контейнеров.
		/// </summary>
		/// <param name="resulttype">тип возвращаемого результата</param>
		/// <param name="list_ids">список ID родительских контейнеров</param>
		/// <returns>ComplexValue</returns>
		public abstract ComplexValue GetChildrenInContainerList(tpList resulttype, List<string> list_ids);
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
	/// <summary>
	/// Абстрактный класс описывает внутренне хранилищем данных клиента SAGA.
	/// </summary>
	public abstract class IntfInnerStore
	{

		public List<ContainerNode> containers = new List<ContainerNode>();

		/// <summary>
		/// Заполнение Хранилище данными о контейнерах.
		/// </summary>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillContainers(ComplexValue list);

		/// <summary>
		/// Заполнение Хранилище данными о документах.
		/// </summary>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillDocs(ComplexValue list);

	}
}

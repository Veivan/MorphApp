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
		/// Заполнение Хранилища данными о контейнерах.
		/// </summary>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillContainers(ComplexValue list);

		/// <summary>
		/// Заполнение Хранилища данными о документах.
		/// </summary>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillDocs(ComplexValue list);

		/// <summary>
		/// Заполнение Документа данными о его абзацах.
		/// </summary>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillDocsParagraphs(ComplexValue list);
	}
}

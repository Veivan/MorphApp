using System;
using System.Collections.Generic;
using Schemas;

namespace MorphApp
{
	/// <summary>
	/// Реализация IntfInnerStore.
	/// Класс описывает внутреннее хранилище данных клиента MorphApp.
	/// </summary>
	public class CLInnerStore : IntfInnerStore
	{
		public override void FillContainers(ComplexValue list)
		{
			var dTable = list.list;
			containers.Clear();
			foreach (ContainerMap item in dTable)
			{
				var cont = new ContainerNode(item);
				containers.Add(cont);
			}
		}

		public override void FillDocs(ComplexValue list)
		{
			throw new NotImplementedException();
		}
	}
}

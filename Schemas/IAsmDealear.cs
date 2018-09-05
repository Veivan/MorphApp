using Schemas.BlockPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schemas
{
	/// <summary>
	/// Абстрактный класс, обобщающий методы работы со сборками.
	/// Представляет API для работы со сборками.
	/// </summary>
	public abstract class IAsmDealear
	{
		/// <summary>
		/// Создание сборки.
		/// </summary>
		/// <param name="name">Наименование нового типа блоков</param>
		/// <returns>адрес добавленного объекта </returns>
		public abstract AssemblyBase CreateAssembly(BlockType type);
	
	}
}

using Schemas.BlockPlatform;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsmAddress = System.Int64;

namespace Schemas
{
	/// <summary>
	/// Абстрактный класс, обобщающий методы работы со сборками.
	/// Представляет API для работы со сборками.
	/// </summary>
	public abstract class IAsmDealear
	{
		/// <summary>
		/// Создание пустой сборки заданного типа.
		/// </summary>
		/// <param name="type">тип блока</param>
		/// <returns>сборка</returns>
		public abstract AssemblyBase CreateAssembly(BlockType type);


		/// <summary>
		/// Создание сборки по образцу / шаблону.
		/// При создании новой сборки копируется структура (дочерние сборки) из образца / шаблона.
		/// </summary>
		/// <param name="templAsm">сборка - образец или шаблон</param>
		/// <param name="mode">признак, определяющий, сохранять ссылку на шаблон\образец или нет</param>
		/// <returns>сборка</returns>
		public abstract AssemblyBase CreateAssembly(AssemblyBase templAsm, FOLLOWMODE mode = FOLLOWMODE.Forget);

		/// <summary>
		/// Получение сборки по адресу из хранилища.
		/// </summary>
		/// <param name="templAsm">сборка - образец или шаблон</param>
		/// <param name="mode">признак, определяющий, учитывать ссылку на образцовую сборку или нет</param>
		/// <returns>сборка</returns>
		public abstract AssemblyBase GetAssembly(AsmAddress Addr, FOLLOWMODE mode = FOLLOWMODE.Forget);

	}
}

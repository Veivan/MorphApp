using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Schemas;
using Schemas.BlockPlatform;
using DirectDBconnector;
using BlockAddress = System.Int64;

namespace LogicProcessor
{
	/// <summary>
	/// Класс, обобщающий методы работы с хранилищем данных.
	/// Представляет API хранилища сборок, контейнеров и справочников.
	/// </summary>
	public class StoreServer
	{

		public BlockDBServer DBserver = new BlockDBServer();

		#region API для работы со сборками
		/// <summary>
		/// Создание пустой сборки заданного типа.
		/// </summary>
		/// <param name="type">тип блока</param>
		/// <returns>сборка</returns>
		public AssemblyBase CreateAssembly(BlockType type)
		{
			var id = DBserver.CreateBlock(type.BlockTypeID, -1, 0);
			var asm = new AssemblyBase(type, id);
			return asm;
		}

		/// <summary>
		/// Создание сборки по образцу / шаблону.
		/// При создании новой сборки копируется структура (дочерние сборки) из образца / шаблона.
		/// </summary>
		/// <param name="templAsm">сборка - образец или шаблон</param>
		/// <param name="mode">признак, определяющий, сохранять ссылку на шаблон\образец или нет</param>
		/// <returns>сборка</returns>
		public AssemblyBase CreateAssembly(AssemblyBase templAsm, FOLLOWMODE mode = FOLLOWMODE.Forget)
		{
			var id = DBserver.CreateBlock(templAsm.BlockType.BlockTypeID, -1, 0);
			var asm = new AssemblyBase(templAsm, id, mode);
			CreateChildrenRequrs(asm.Children);
			return asm;
		}

		/// <summary>
		/// Получение сборки по адресу из хранилища.
		/// </summary>
		/// <param name="templAsm">сборка - образец или шаблон</param>
		/// <param name="mode">признак, определяющий, учитывать ссылку на образцовую сборку или нет</param>
		/// <returns>сборка</returns>
		public AssemblyBase GetAssembly(long Addr, FOLLOWMODE mode = FOLLOWMODE.Forget)
		{
			var block = DBserver.GetBlock(Addr);
			var asm = new AssemblyBase(block);
			return asm;
		}

		/// <summary>
		/// Рекурсивное сохранение дочерних сборок в БД.
		/// </summary>
		/// <param name="src_children">перечень дочерних сборок</param>
		private void CreateChildrenRequrs(List<AssemblyBase> src_children)
		{
			foreach (var child in src_children)
			{
				var chid = DBserver.CreateBlock(child.BlockType.BlockTypeID, child.ParentAssemblyID, (int)child.Treeorder);
				child.RootBlock_id = chid;
				CreateChildrenRequrs(child.Children);
			}
		}
		#endregion

		#region API для работы с контейнерами : создание, редактирование и различные выборки для визуализации.


		/// <summary>
		/// Создание нового Контейнера.
		/// </summary>
		/// <param name="name">Наименование Контейнера</param>
		/// <param name="parent">Поддержка дерева.Родитель.Это ссылка на блок, являющийся Родителем для блока.</param>
		/// <param name="treeorder">Поддержка дерева. Последовательность блока в списке детей Родителя (порядок блока в дереве).
		///		Можно задать явно.Можно задать = 0, тогда функция должна определить последний максимальный номер атрибута и
		///		присвоить новому атрибуту последний максимальный номер +1</param>
		/// <returns>адрес добавленного объекта</returns>
		public BlockAddress CreateContainer(string name, BlockAddress parent, int treeorder)
		{
			var typeOfDict = DBserver.GetBlockTypeByNameKey(Session.containerTypeName);
			BlockAddress id = DBserver.CreateBlock(typeOfDict.BlockTypeID, parent, treeorder);
			DBserver.AttrSetValue(id, "Name", name);
			return id;
		}

		/// <summary>
		/// Чтения дочерних контейнеров множества родительских контейнеров.
		/// </summary>
		/// <param name="resulttype">тип возвращаемого результата</param>
		/// <param name="list_ids">список ID родительских контейнеров</param>
		/// <returns>ComplexValue</returns>
		public ComplexValue GetChildrenInContainerList(tpList resulttype, List<string> list_ids)
		{
			ComplexValue rval = DBserver.GetChildren(resulttype, list_ids);
			return rval;
		}

	#endregion
}
}

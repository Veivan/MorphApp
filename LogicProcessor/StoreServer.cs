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
	/// Реализация интерфейса, обобщающего методы работы с хранилищем данных.
	/// Представляет API хранилища сборок, контейнеров и справочников.
	/// </summary>
	public class StoreServer : IAsmDealer
	{

		private BlockDBServer DBserver = new BlockDBServer();

		public StoreServer()
		{
			Session.Instance().Init(this, DBserver);
		}

		#region API для работы со сборками

		public override AssemblyBase CreateAssembly(BlockType type, BlockAddress ParentContID = -1)
		{
			var id = DBserver.CreateBlock(type.BlockTypeID, ParentContID, 0);
			var asm = new AssemblyBase(id, type);
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
		/// <param name="Addr">Адрес сборки</param>
		/// <param name="FillChildren">признак, определяющий, заполгять сборку данными о детях или нет</param>
		/// <returns>сборка</returns>
		public AssemblyBase GetAssembly(long Addr, bool FillChildren = true)
		{
			var block = DBserver.GetBlock(Addr);
			var asm = new AssemblyBase(block);
			if (FillChildren)
				this.AsmFillChildren(asm);
			return asm;
		}

		/// <summary>
		/// Заполнение сборки данными о детях.
		/// </summary>
		/// <param name="parentAsm">сборка</param>
		/// <returns></returns>
		public void AsmFillChildren(AssemblyBase parentAsm)
		{
			var list_ids = new List<string> { parentAsm.BlockID.ToString() };
			var listCh = this.GetChildren(list_ids);
			foreach (var child in listCh)
				parentAsm.AddChild(child);
		}

		/// <summary>
		/// Рекурсивное сохранение дочерних сборок в БД.
		/// </summary>
		/// <param name="src_children">перечень дочерних сборок</param>
		private void CreateChildrenRequrs(List<AssemblyBase> src_children)
		{
			foreach (var child in src_children)
			{
				var chid = DBserver.CreateBlock(child.BlockType.BlockTypeID, child.ParentAssemblyID, (int)child.Order);
				//child.RootBlock_id = chid;
				CreateChildrenRequrs(child.Children);
			}
		}

		/// <summary>
		/// Получение дочерних сборок множества родительских сборок.
		/// Выбираются только прямые наследники (без ссылок)
		/// </summary>
		/// <param name="list_ids">список ID родительских сборок</param>
		/// <returns>DataTable</returns>
		public List<AssemblyBase> GetChildren(List<string> list_ids)
		{
			var listBlocks = DBserver.GetChildren(list_ids);
			var listAsm = new List<AssemblyBase>();
			foreach (var block in listBlocks)
			{
				var asm = new AssemblyBase(block);
				listAsm.Add(asm);
			}
			return listAsm;
		}

		public override void Add2SaveSet(AssemblyBase asm)
		{
			var saveSet = SetKeeper.Instance();
			saveSet.Add(asm);
		}

		public override void Save()
		{
			var saveSet = SetKeeper.Instance();
			if(saveSet.IsDirty)
			{
				// В DBServer надо сделать функции с групповым входным параметром, а внутри их - транзакции
				var list = saveSet.GetSet();
				saveSet.Clear();
			}

/*			if (asm.IsDeleted == 1)
				DBserver.Delete(asm.BlockID);
			DBserver.SetFactData(asm.BlockID, asm.Blob);*/
		}

		#endregion

		#region API для работы с контейнерами : создание, редактирование и различные выборки для визуализации.

		/*		public AssemblyBase CreateContainer(string name, BlockAddress ParentContID)
				{
					var asm = this.CreateAssembly(Session.Instance().GetBlockTypeDataContainer(), ParentContID);
					asm.SetValue("Name", name);
					return asm;
				}

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
					var typeOfDict = Session.Instance().GetBlockTypeByNameKey(Session.containerTypeName);
					BlockAddress id = DBserver.CreateBlock(typeOfDict.BlockTypeID, parent, treeorder);
					DBserver.AttrSetValue(id, "Name", name);
					return id;
				} */
		#endregion

		#region Методы работы с GREN
		public override List<SentenceMap> MorphMakeSyntan(string text)
		{
			//throw new NotImplementedException();
			return null;
		}

		public override List<string> MorphGetSeparatedSentsList(string text)
		{
			//throw new NotImplementedException();
			return null;
		}
		#endregion

	}
}

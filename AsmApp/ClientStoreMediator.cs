using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Schemas;
using Schemas.BlockPlatform;
using LogicProcessor;

using BlockAddress = System.Int64;


namespace AsmApp
{
	/// <summary>
	/// Класс поддерживает взаимодействие клиента с хранилищем данных SAGA.
	/// </summary>
	public class ClientStoreMediator
	{
		// Список контейнеров для клиента
		public List<AssemblyBase> containers = new List<AssemblyBase>();

		StoreServer store = new StoreServer();

		#region Методы работы с хранилищем данных
		/// <summary>
		/// Заполнение внутреннего хранилища.
		/// </summary>
		public void Refresh()
		{
			containers.Clear();

			var MainStore = new AssemblyBase(Session.MainStoreID, Session.Instance().GetBlockTypeByNameKey(Session.containerTypeName));
			MainStore.IsMainDataContainer = true;
			containers.Add(MainStore);

			var list_ids = new List<string>();
			list_ids.Add(Session.MainStoreID.ToString());
			var list = store.GetChildren(list_ids);
			this.FillChildren(MainStore, list);
		}

		public void FillChildren(AssemblyBase in_parentCont, List<BlockBase> list)
		{
			foreach (var container in list)
			{
				in_parentCont.AddChild(new AssemblyBase(container));
			}
		}

		public AssemblyBase CreateContainer(string name, BlockAddress ParentContID)
		{
			var asm = store.CreateContainer(name, ParentContID);
			asm.Save();
			return asm;
		}

		public AssemblyBase CreateDocument(string name, BlockAddress ParentID)
		{
			var asm = store.CreateAssembly(Session.Instance().GetBlockTypeByNameKey(Session.documentTypeName), ParentID);
			asm.SetValue("Name", name);
			asm.Save();
			return asm;
		}

		public void RefreshContainer(AssemblyBase cont)
		{
			var list = store.GetChildren(new List<string>() { cont.BlockID.ToString() });
			this.FillChildren(cont, list);
		}

		/// <summary>
		/// Формирование содержимого внутреннего объекта ParagraphMap.
		/// </summary>
		/// <param name="pAsm">объект ParagraphMap</param>
		/// <param name="input">текстовое содержание абзаца</param>
		/// <returns></returns>
		public void UpdateParagraph(AssemblyBase pAsm, string input, bool IsHeader)
		{
			var ihash = input.GetHashCode();
			int currenthash = pAsm.GetHashCode(IsHeader);
			var range = IsHeader ? SentTypes.enstNotActualHead : SentTypes.enstNotActualBody;
			var sentlist = pAsm.GetParagraphSents(range);
			if (ihash == currenthash && sentlist.Count == 0)
				return;

			var sents = store.MorphGetSeparatedSentsList(input);
			pAsm.RefreshParagraph(new ArrayList(sents), IsHeader);

			sentlist = pAsm.GetParagraphSents(range);
			if (sentlist.Count == 0)
				return;
			// Выполнение синтана для неактуальных предложений.
			foreach (var sent in sentlist)
			{
				var sentlistRep = store.MorphMakeSyntan(sent.sentence);
				if (sentlistRep.Count > 0)
					pAsm.UpdateSentStruct(sent.order, sentlistRep[0]);
			}
		}
		#endregion

		#region Оставшиеся (ненужные?) методы работы с хранилищем

		/*// <summary>
		/// Поиск контейнера в хранилище по его ID.
		/// </summary>
		/// <param name="ContainerID">ID контейнера</param>
		/// <returns>ContainerNode</returns>
		public ContainerNode GetContainerByID(long ContainerID)
		{
			var result = RecursGetContainerByID(containers, ContainerID);
			return result;
		}
		*/
		private ContainerNode RecursGetContainerByID(List<ContainerNode> containers, long ContainerID)
		{
			var result = containers.Where(x => x.ContainerID == ContainerID).FirstOrDefault();
			if (result == null)
				foreach (var cont in containers)
				{
					var children = cont.Children();
					result = RecursGetContainerByID(cont.Children(), ContainerID);
					if (result != null)
						break;
				}
			return result;
		}
		#endregion
	
	}
}

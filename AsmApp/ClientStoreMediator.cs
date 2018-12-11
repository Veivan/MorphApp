using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

using Schemas;
using Schemas.BlockPlatform;
using LogicProcessor;

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

			var MainStore = new AssemblyBase(Session.Instance().GetBlockTypeByNameKey(Session.containerTypeName));
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

		#region Собственные непереопределяемые методы работы с хранилищем

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

		/*// <summary>
		/// Формирование заголовков абзацев.
		/// </summary>
		public abstract DocumentMap RefreshParagraphs(long contID, long docID);

		/// <summary>
		/// Заполнение детей и документов самого абзаца и детей на один уровень вниз данными из БД.
		/// </summary>
		public abstract void RefreshContainer(long contID);

		/// <summary>
		/// Заполнение дочерних контейнеров.
		/// </summary>
		/// <param name="parentCont">родительский контейнер</param>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillChildren(ContainerNode parentCont, ComplexValue list);

		/// <summary>
		/// Заполнение Хранилища данными о документах.
		/// </summary>
		/// <param name="cont">родительский контейнер</param>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillDocs(ContainerNode cont, ComplexValue list);

		/// <summary>
		/// Заполнение Документа данными о его абзацах.
		/// </summary>
		/// <param name="list">Набор данных или список</param>
		/// <returns></returns>
		public abstract void FillDocsParagraphs(ComplexValue list);

		/// <summary>
		/// Сохранение контейнера в БД.
		/// </summary>
		/// <param name="name">имя контейнера</param>
		/// <param name="parentID">ID родительского контейнера</param>
		/// <returns>List of SimpleParam</returns>
		public abstract List<SimpleParam> SaveContainerBD(string name, long parentID = -1);

		/// <summary>
		/// Сохранение документа в БД.
		/// </summary>
		/// <param name="name">Имя документа</param>
		/// <param name="ct_id">ID контейнера</param>
		/// <returns>List of SimpleParam</returns>
		public abstract List<SimpleParam> SaveDocumentBD(string name, long ct_ID);

		/// <summary>
		/// Сохранение абзаца в БД.
		/// </summary>
		/// <param name="pMap">ParagraphMap</param>
		/// <returns>List of SimpleParam</returns>
		public abstract List<SimpleParam> SaveParagraphBD(ParagraphMap pMap);

		/// <summary>
		/// Чтение одного абзаца из БД.
		/// </summary>
		/// <param name="pg_id">ID абзаца</param>
		/// <returns>List of SentenceMap</returns>
		public abstract List<SentenceMap> ReadParagraphDB(long pg_id);

		/// <summary>
		/// Удаление одного абзаца из БД.
		/// </summary>
		/// <param name="pg_id">ID абзаца</param>
		/// <returns></returns>
		public abstract void DelParagraph(long ct_id, long doc_id, long pg_id);

		/// <summary>
		/// Удаление одного документа из БД.
		/// </summary>
		/// <param name="doc_id">ID документа</param>
		/// <returns></returns>
		public abstract void DelDocument(long ct_id, long doc_id);

		/// <summary>
		/// Удаление одного контейнера из БД.
		/// </summary>
		/// <param name="c_id">ID контейнера</param>
		/// <returns></returns>
		public abstract void DelContainer(long ct_id);

		/// <summary>
		/// Поиск слова в БД.
		/// </summary>
		/// <param name="word">слово</param>
		/// <returns>List of SimpleParam</returns>
		public abstract List<SimpleParam> GetLexema(string word);

		/// <summary>   
		/// Сохранение слова в БД.
		/// </summary>
		/// <param name="word">слово</param>
		/// <returns>List of SimpleParam</returns>
		public abstract List<SimpleParam> SaveLexema(string word);
		*/
		#endregion
	}
}

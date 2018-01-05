using System;
using System.Collections.Generic;

namespace Schemas
{
	/// <summary>
	/// Абстрактный класс, обобщающий методы работы с хранилищем данных.
	/// Представляет API хранилища
	/// </summary>
	public abstract class IDataDealer
	{
		/// <summary>
		/// Получение плоского списка контейнеров
		/// </summary>
		/// <returns>RetValue</returns>
		public abstract ComplexValue ReadContainers();

		/// <summary>
		/// Получение плоского списка документов
		/// </summary>
		/// <returns>RetValue</returns>
		public abstract ComplexValue ReadDocuments();

		/// <summary>
		/// Получение контейнеров выбранного родителя.
		/// </summary>
		/// <param name="parentID">ID родителя</param>
		/// <param name="resulttype">тип возвращаемого результата</param>
		/// <returns>RetValue</returns>
		public abstract ComplexValue GetChildrenContainers(long parentID, tpList resulttype);

		/// <summary>
		/// Получение документов из выбранного контейнера.
		/// </summary>
		/// <param name="ct_id">ID контейнера</param>
		/// <returns>RetValue</returns>
		//public abstract RetValue GetDocsInContainer(long ct_id);

		/// <summary>
		/// Чтения документов из множества контейнеров.
		/// </summary>
		/// <param name="list_ids">список ID контейнеров</param>
		/// <returns>RetValue</returns>
		public abstract ComplexValue GetDocsInContainerList(List<string> list_ids);

		/// <summary>
		/// Сохранение абзаца в DB. Если pg_id == -1, то Insert, иначе - Update.
		/// </summary>
		/// <param name="pg_id">ID абзаца</param>
		/// <param name="sentlist">упорядоченный список структур предложений</param>
		/// <returns>ID абзаца</returns>
		public abstract long SaveParagraph(long pg_id, List<SentenceMap> sentlist);

		/// <summary>
		/// Чтение абзаца из DB.
		/// </summary>
		/// <param name="pg_id">ID абзаца</param>
		/// <returns>упорядоченный список структур предложений</returns>
		public abstract List<SentenceMap> ReadParagraphDB(long pg_id);
	
		/**
		 * Сохранение лексемы в DB. Функция проверяет, нет ли уже слова в словаре.
		 * Если слово отсутствует, то происходит добавление.
		 * 
		 * @return ID лексемы
		 */
		//long SaveLex(String word);

		/**
		 * Поиск слова в словаре. Затем надо искать его по словоформе в справчнике
		 * лексем.
		 * 
		 * @return ID лексемы
		 */
		//long GetWord(String rword);


		/**
		 * Сохранение фразы в DB. Если ph_id == -1, то Insert, иначе - Update
		 * 
		 * @param ph_id
		 * @return ID фразы
		 */
		//long SavePhrase(long ph_id);

		/**
		 * Сохранение состава фразы в DB. 
		 * 
		 * @param ph_id  - ID фразы
		 * @param lx_id  - ID лексемы
		 * @param sorder - порядок слова в предложении
		 * 
		 * @return ID записи
		 */
		//long SavePhraseWords(long ph_id, long lx_id, short sorder);

	}
}

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
        /// Чтения дочерних контейнеров множества родительских контейнеров.
        /// </summary>
		/// <param name="resulttype">тип возвращаемого результата</param>
        /// <param name="list_ids">список ID родительских контейнеров</param>
        /// <returns>ComplexValue</returns>
        public abstract ComplexValue GetChildrenInContainerList(tpList resulttype, List<string> list_ids);

        /// <summary>
        /// Чтения документов из множества контейнеров.
        /// </summary>
        /// <param name="list_ids">список ID контейнеров</param>
        /// <returns>ComplexValue</returns>
        public abstract ComplexValue GetDocsInContainerList(List<string> list_ids);

		/// <summary>
		/// Получение плоского списка абзацев
		/// </summary>
        /// <param name="list_ids">список ID документов</param>
		/// <returns>ComplexValue</returns>
		public abstract ComplexValue ReadParagraphsInDocsList(tpList resulttype, List<string> list_ids = null);

        /// <summary>
        /// Получение плоского списка предложений
        /// </summary>
        /// <param name="list_ids">список ID абзацев</param>
        /// <returns>ComplexValue</returns>
        public abstract ComplexValue ReadPhrasesInParagraphsList(tpList resulttype, List<string> list_ids = null);
        
        /// <summary>
		/// Сохранение абзаца в DB. 
		/// </summary>
		/// <param name="pMap">Структура абзаца</param>
		/// <returns>ID абзаца</returns>
		public abstract long SaveParagraph(ParagraphMap pMap);

		/// <summary>
		/// Чтение абзаца из DB.
		/// </summary>
		/// <param name="pg_id">ID абзаца</param>
		/// <returns>упорядоченный список структур предложений</returns>
		public abstract List<SentenceMap> ReadParagraphDB(long pg_id);

		/// <summary>
		/// Удаление одного абзаца из БД.
		/// </summary>
		/// <param name="pg_id">ID абзаца</param>
		/// <returns></returns>
		public abstract void DelParagraphDB(long pg_id);

		/// <summary>
		/// Сохранение контейнера в БД.
		/// </summary>
		/// <param name="name">имя контейнера</param>
		/// <param name="parentID">ID родительского контейнера</param>
		/// <returns>List of SimpleParam</returns>
		public abstract long SaveContainerBD(string name, long parentID = -1);

        /// <summary>
        /// Сохранение документа в БД.
        /// </summary>
        /// <param name="name">Имя документа</param>
        /// <param name="ct_id">ID контейнера</param>
        /// <returns>List of SimpleParam</returns>
        public abstract long SaveDocumentBD(string name, long ct_ID);

        /// <summary>
        /// Удаление одного документа из БД.
        /// </summary>
        /// <param name="doc_id">ID документа</param>
        /// <returns></returns>
        public abstract void DelDocumentDB(long doc_id);

        /// <summary>
        /// Удаление одного контейнера из БД.
        /// </summary>
        /// <param name="c_id">ID контейнера</param>
        /// <returns></returns>
        public abstract void DelContainerDB(long c_id);

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

﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

namespace Schemas
{
    /// <summary>
    /// Абстрактный класс описывает внутренне хранилищем данных клиента SAGA.
    /// </summary>
    public abstract class IntfInnerStore
    {

        public List<ContainerNode> containers = new List<ContainerNode>();

        #region Собственные непереопределяемые методы работы с хранилищем
       
        /// <summary>
        /// Поиск контейнера в хранилище по его ID.
        /// </summary>
        /// <param name="ContainerID">ID контейнера</param>
        /// <returns>ContainerNode</returns>
        public ContainerNode GetContainerByID(long ContainerID)
        {
            var result = RecursGetContainerByID(containers, ContainerID);
            return result;
        }

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

        /// <summary>
        /// Поиск параграфа в хранилище.
        /// </summary>
        /// <param name="contID">ID контейнера</param>
        /// <param name="docID">ID документа</param>
        /// <param name="parID">ID параграфа</param>
        /// <returns>pMap</returns>
        public ParagraphMap GetParagraph(long contID, long docID, long parID)
        {
            var container = GetContainerByID(contID);
            if (container == null) return null;
            var dMap = container.GetDocumentByID(docID);
            if (dMap == null) return null;
            return dMap.GetParagraph(parID);          
        }

        /// <summary>
        /// Формирование содержимого внутреннего объекта ParagraphMap.
        /// </summary>
        /// <param name="pMap">объект ParagraphMap</param>
        /// <param name="input">текстовое содержание абзаца</param>
        /// <returns></returns>
        public void UpdateParagraph(ParagraphMap pMap, string input, bool IsHeader) 
		{
			var ihash = input.GetHashCode();
			int currenthash = pMap.GetHashCode(IsHeader);
			var range = IsHeader ? SentTypes.enstNotActualHead : SentTypes.enstNotActualBody;
			var sentlist = pMap.GetParagraphSents(range);
			if (ihash == currenthash && sentlist.Count == 0)
				return;

			var sents = this.MorphGetSeparatedSentsList(input);
			pMap.RefreshParagraph(new ArrayList(sents), IsHeader);

			sentlist = pMap.GetParagraphSents(range);
			if (sentlist.Count == 0)
				return;
			// Выполнение синтана для неактуальных предложений.
			foreach (var sent in sentlist)
			{
				var sentlistRep = this.MorphMakeSyntan(sent.sentence);
				if (sentlistRep.Count > 0)
					pMap.UpdateSentStruct(sent.order, sentlistRep[0]);
			}		
		}

        public bool CanRemoveContainer(long contID)
        {
            var container = GetContainerByID(contID);
            if (container == null) return false;
            return (container.Children().Count == 0);
        }

        #endregion

        #region Методы работы с БД
        /// <summary>
        /// Заполнение внутреннего хранилища.
        /// </summary>
        public abstract void Refresh();

        /// <summary>
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
        
        #endregion

        #region Методы работы с GREN
		
		/// <summary>
		/// Выполнение синтана текста.
		/// </summary>
		public abstract List<SentenceMap> MorphMakeSyntan(string text);

			/// <summary>
		/// Получение списка восстановленных текстов предложений от сервиса.
		/// </summary>
		public abstract List<string> MorphGetReparedSentsList(List<SentenceMap> sentlist);

		/// <summary>
		/// Разделение текста на предложения с помощью сервиса.
		/// </summary>
		public abstract List<string> MorphGetSeparatedSentsList(string text);

		#endregion
    }
}


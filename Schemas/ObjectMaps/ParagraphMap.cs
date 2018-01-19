using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Schemas
{
    public class SentProps
    {
        public int order;
        public string sentence;
        public int hash;
        public bool IsActual;
        public SentenceMap sentstruct;
    }

    /// <summary>
    /// Класс представляет хранилище предложений одного абзаца.
    /// </summary>
    public class ParagraphMap
    {
        private long _pg_id = -1;
        private long _doc_id = -1;
        private long _ct_id = -1;
        private DateTime _created_at;
		
		/// <summary>
		/// Список предназначен для хранения предложений абзаца.
		/// </summary>
		private List<SentProps> innerPara = new List<SentProps>();

        /// <summary>
        /// Идентификатор абзаца в БД.
        /// </summary>
        public long ParagraphID { get { return _pg_id; } set { _pg_id = value; } }

        /// <summary>
        /// Идентификатор документа, к которобу относится абзац.
        /// Ссылка на абзаца в БД.
        /// </summary>
        public long DocumentID { get { return _doc_id; } set { _doc_id = value; } }

        /// <summary>
        /// Возвращает предложение абзаца по порядку order.
        /// Предложения заголовка имеет order < 0;
        /// </summary>
        public string GetSentenseByOrder(int order)
        {
            SentProps sprop = innerPara.Where(x => x.order == order).FirstOrDefault();
            return sprop == null ? "" : sprop.sentence;
        }

        /// <summary>
        /// В списке хранятся ID предложений абзаца, которые нужно удалить при сохранении в БД.
        /// </summary>
        private List<long> sents2Del = new List<long>();

        public List<long> GetDeleted() { return sents2Del; }

        /// <summary>
        /// Ссхранение списка ID предложений абзаца, которые надо удалить при сохранении.
        /// </summary>
        private void SetDeleted(List<SentProps> versionPara, bool IsHeader)
        {
            var listBefore = innerPara
                .Where(x => x.sentstruct != null && x.sentstruct.SentenceID != -1)
                .Select(x => x.sentstruct.SentenceID).ToArray();
            var listAfter = versionPara
                .Where(x => x.sentstruct != null && x.sentstruct.SentenceID != -1)
                .Select(x => x.sentstruct.SentenceID)
                .ToArray();
            sents2Del.AddRange(listBefore.Except(listAfter));
        }

        public void ClearDeleted()
        {
            this.sents2Del.Clear();
        }

        /// <summary>
        /// Конструктор
        /// </summary>
        public ParagraphMap(long pg_id = -1, long doc_id = -1, DateTime? created_at = null, long ct_id = -1)
        {
            _pg_id = pg_id;
            _doc_id = doc_id;
            _ct_id = ct_id;
            if (created_at == null)
                _created_at = DateTime.Now;
            else
                _created_at = (DateTime)created_at;
        }

		/// <summary>
		/// Конструктор - копировщик
		/// </summary>
		public ParagraphMap(ParagraphMap pMap)
		{
			this._pg_id = pMap._pg_id;
			this._doc_id = pMap._doc_id;
			this._ct_id = pMap._ct_id;
			this._created_at = pMap._created_at;
			var innerPara = pMap.GetParagraphSents();
			foreach (var sentprop in pMap.innerPara)
			{
				this.AddSentStruct(sentprop.order, sentprop.sentstruct);
				this.innerPara[sentprop.order].hash = sentprop.hash;
				this.innerPara[sentprop.order].IsActual = sentprop.IsActual;
			}
		}
		
		private static bool Belongs2Header(SentProps p)
        {
            return p.order < 0;
        }

        private static bool Belongs2Body(SentProps p)
        {
            return p.order > -1;
        }

        /// <summary>
        /// Добавление предложений абзаца в хранилище.
        /// При добавлении вычисляется хэш предложения и по жэшу происходит поиск существующего предложения в хранилище.
        /// Если предложение есть, то структура переносится в новый список,
        /// если нет, то добавляется новая структура с признаком "Неактуальная".
        /// Для "Неактуальных" надо делать синтаксический анализ.
        /// По окончании новый список заменяет предыдущее содержание хранилища.
        /// </summary>
        public void RefreshParagraph(ArrayList input, bool IsHeader)
        {
            List<SentProps> versionPara = new List<SentProps>();
            int i = 0;
            if (IsHeader) i = -1 * input.Count;
            foreach (var sent in input)
            {
                SentProps newsprops;
                var ihash = sent.GetHashCode();
                var sentprops = innerPara.Where(x => x.hash.Equals(ihash) && (Belongs2Header(x) == IsHeader)).ToList();
                if (sentprops.Count == 0)
                {
                    newsprops = new SentProps();
                    newsprops.sentence = sent as string;
                    newsprops.hash = ihash;
                    newsprops.IsActual = false;
                }
                else
                {
                    newsprops = sentprops[0];
                    newsprops.IsActual = true;
                }
                newsprops.order = i;
                versionPara.Add(newsprops);
                i++;
            }

            SetDeleted(versionPara, IsHeader);
            if (IsHeader)
                innerPara.RemoveAll(Belongs2Header);
            else
                innerPara.RemoveAll(Belongs2Body);
            innerPara.AddRange(versionPara);
        }

        /// <summary>
        /// Получение списка структур предложений абзаца.
        /// </summary>
		/// <param name="sttype">диапазон выбираемых предложений</param>
		/// <returns>List of SentProps</returns>
		public List<SentProps> GetParagraphSents(SentTypes sttype = SentTypes.enstAll)
        {
            List<SentProps> versionPara = new List<SentProps>();
            switch (sttype)
            {
                case SentTypes.enstAll:
                    versionPara.AddRange(innerPara);
                    break;
				case SentTypes.enstNotActualHead:
					versionPara.AddRange(innerPara.Where(x => x.order < 0 && !x.IsActual)
						.OrderBy(x => x.order)
						.ToList());
					break;
				case SentTypes.enstNotActualBody:
					versionPara.AddRange(innerPara.Where(x => x.order > -1 && !x.IsActual)
						.OrderBy(x => x.order)
						.ToList());
					break;
				case SentTypes.enstHeader:
                    versionPara.AddRange(innerPara.Where(x => x.order < 0)
                        .OrderBy(x => x.order)
                        .ToList());
                    break;
                case SentTypes.enstBody:
                    versionPara.AddRange(innerPara.Where(x => x.order > -1)
                        .OrderBy(x => x.order)
                        .ToList());
                    break;
            }
            return versionPara;
        }

        /// <summary>
        /// Получение списка ID предложений абзаца.
        /// </summary>
        public List<long> GetParagraphSentsIDs()
        {
            List<long> list_ids = new List<long>();
            list_ids.AddRange(innerPara.Select(x => x.sentstruct.SentenceID).ToList());
            return list_ids;
        }

        /// <summary>
        /// Запиcь в хранилище предложения новой структуры синтана этого предложения.
        /// </summary>
        public void UpdateSentStruct(int order, SentenceMap sentstruct)
        {
            var sent = innerPara.Where(x => x.order == order).FirstOrDefault();
            if (sent.sentstruct == null)
            {
                sentstruct.ParagraphID = this.ParagraphID;
                sentstruct.Order = order;
                sent.sentstruct = new SentenceMap(sentstruct);
            }
            sent.IsActual = true;
        }

        /// <summary>
        /// Запиcь в хранилище предложения новой структуры синтана этого предложения.
        /// </summary>
        public void AddSentStruct(int order, SentenceMap sentstruct)
        {
            var sprop = new SentProps();
            sprop.order = order;
            sentstruct.ParagraphID = this.ParagraphID;
            sentstruct.Order = order;
            sprop.sentstruct = new SentenceMap(sentstruct);
            innerPara.Add(sprop);
        }

        /// <summary>
        /// Обновление элемента хранения предложения.
        /// </summary>
        public void RefreshSentProp(string sentence, SentenceMap sentstruct, bool IsActual)
        {
            int order = sentstruct.Order;
            SentProps sprop = innerPara.Where(x => x.order == order).FirstOrDefault();
            if (sprop == null)
            {
                sprop = new SentProps();
                sprop.order = order;
                innerPara.Add(sprop);
            }

            if (sprop.sentstruct == null)
                sprop.sentstruct = new SentenceMap(sentstruct);
            sprop.sentence = sentence;
            sprop.hash = sentence.GetHashCode();
            sprop.IsActual = true;
        }


        public string GetHeader()
        {
            return String.Join(" ", this.GetParagraphSents(SentTypes.enstHeader)
                                .Select(x => x.sentence).ToList());
        }

		public string GetBody()
		{
			return String.Join(" ", this.GetParagraphSents(SentTypes.enstBody)
								.Select(x => x.sentence).ToList());
		}

		public int GetHashCode(bool IsHeader)
		{
			string str;
			if (IsHeader)
				str = GetHeader();
			else
				str = GetBody();
			return str.GetHashCode();
		}
	}
}

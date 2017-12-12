using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MorphApp
{
    struct SentProps
    {
        public int order;
        public string sentence;
        public int hash;
        public bool IsActual;
    }

    /// <summary>
    /// Класс представляет хранилище предложений одного абзаца.
    /// </summary>
    class Paragraph
    {
        /// <summary>
        /// Список предназначен для хранения хэшей предложений абзаца.
        /// </summary>
        private List<SentProps> innerPara = new List<SentProps>();

        /// <summary>
        /// Добавление предложений абзаца в хранилище.
        /// При добавлении вычисляется хэш предложения и по жэшу происходит поиск существующего предложения в хранилище.
        /// Если предложение есть, то структура переносится в новый список,
        /// если нет , то добавляется новая структура с признаком "Неактальная".
        /// Для "Неактуальных" надо делать синтаксический анализ.
        /// По окончании новый список заменяет предыдущее содержание хранилища.
        /// </summary>
        public void AddParagraph(ArrayList input)
        {
            List<SentProps> versionPara = new List<SentProps>();

            int i = 0;
            foreach (var sent in input)
            {
                SentProps newsprops;
                var ihash = sent.GetHashCode();
                var sentex = innerPara.Where(x => x.hash.Equals(ihash)).ToList();
                if (sentex.Count == 0)
                {
                    newsprops = new SentProps();
                    newsprops.sentence = sent as string;
                    newsprops.hash = ihash;
                    newsprops.IsActual = false;
                }
                else
                {
                    newsprops = sentex[0];
                    newsprops.IsActual = true;
                }
                newsprops.order = i;
                versionPara.Add(newsprops);
             }

            innerPara.Clear();
            innerPara.AddRange(versionPara);
        }

        public List<SentProps> GetParagraph()
        {
            List<SentProps> versionPara = new List<SentProps>();
            versionPara.AddRange(innerPara);
            return versionPara;
        }
    }
}

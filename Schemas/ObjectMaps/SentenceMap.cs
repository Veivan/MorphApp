using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMorph.Schema;
using FlatBuffers;

namespace Schemas
{
    public struct tNode
    {
        public int ID; // Порядок добавления в дерево, для сортировки в виде плоского списка
        public int Level; // Уровень вложенности, для отображения
        public int index; // порядковый номер в предложении
        public int linktype; // тип взаимосвязи с родителем
    }

    /// <summary>
    /// Класс хранит информацию о предложении.
    /// </summary>
    public class SentenceMap
    {
        private long _phID = -1;
        private long _pg_id = -1;
		private int _order = -1;
        private DateTime _created_at;

        /// <summary>
        /// Идентификатор предложения в БД.
        /// </summary>
        public long SentenceID { get { return _phID; } set { _phID = value; } }

		/// <summary>
		/// Порядок предложения в абзаце.
		/// У заголовка _order = -1;
		/// </summary>
		public int Order { get { return _order; } set { _order = value; } }
		
		/// <summary>
        /// Хранилище структур слов предложения.
        /// </summary>
        private SortedList<int, WordMap> words = new SortedList<int, WordMap>();

        /// <summary>
        /// Хранилище синтаксических связей предложения.
        /// </summary>
        private List<tNode> treeList = new List<tNode>();

        /// <summary>
        /// Конструктор по умолчанию
        /// </summary>
        public SentenceMap()
        {
        }

		/// <summary>
		/// Конструктор
		/// </summary>
		public SentenceMap(long ph_id = -1, long pg_id = -1, int order = -1, DateTime? created_at = null)
        {
            _phID = ph_id;
			_pg_id = pg_id;
            _order = order;
			if (created_at == null)
				_created_at = DateTime.Now;
			else
				_created_at = (DateTime)created_at;
		}

        /// <summary>
        /// Конструктор - копировщик.
        /// </summary>
        public SentenceMap(SentenceMap map)
        {
            this.SentenceID = map.SentenceID;
            foreach (var w in map.words)
                this.words.Add(w.Key, w.Value);
            this.treeList.AddRange(map.treeList);
        }

        /// <summary>
        /// Количество слов предложения.
        /// </summary>
        public int Capasity
        {
            get
            {
                return words.Count;
            }
        }

        /// <summary>
        /// Добавление слова и синт.узла во внутренние хранилища.
        /// </summary>
        /// <param name="order">Порядок следования слова в предложении</param>
        /// <param name="word">Структура слова</param>
        /// <param name="Level">Уровень вложенности синтаксического узла</param>
        /// <param name="linktype">тип взаимосвязи с родителем</param>
        /// <returns></returns>
        public void AddWord(int order, WordMap word, int Level = -1, int linktype = -1)
        {
            if (words.ContainsKey(order) || word == null)
                return;
            words.Add(order, word);
            if (Level > -1 && linktype > -1)
            {
                this.AddNode(order, Level, linktype);
            }
        }

        /// <summary>
        /// Добавление синт.узла во внутреннее хранилище.
        /// </summary>
        /// <param name="order">Порядок следования слова в предложении</param>
        /// <param name="Level">Уровень вложенности синтаксического узла</param>
        /// <param name="linktype">тип взаимосвязи с родителем</param>
        /// <returns></returns>
        public void AddNode(int order, int Level, int linktype)
        {
            int maxID = 0;
            if (treeList.Count > 0)
                maxID = (int)(treeList.OrderByDescending(x => x.Level).First().ID + 1);
            var node = new tNode();
            node.ID = maxID;
            node.Level = Level;
            node.index = order;
            node.linktype = linktype;
            treeList.Add(node);
        }

        /// <summary>
        /// Количество слов предложения.
        /// </summary>
        public WordMap GetWordByOrder(int order)
        {
            if (words.ContainsKey(order))
                return words[order];
            else
                return null;
        }

        /// <summary>
        /// Получение списка синтаксических связей предложения.
        /// </summary>
        public List<tNode> GetTreeList()
        {
            var newlist = treeList.OrderBy(x => x.ID)
                //.ThenBy(x => x.orderlvl)
                .ToList();
            return newlist;
        }

        public static List<SentenceMap> BuildFromMessage(Message message)
        {
            List<SentenceMap> sentlist = new List<SentenceMap>();
            if (message.SentencesLength == 0)
                return sentlist;
            for (int i = 0; i < message.SentencesLength; i++)
            {
                SentenceMap resSent = null;
                var sent = message.Sentences(i);
                if (sent.HasValue)
                {
                    var sentval = sent.Value;
                    resSent = new SentenceMap();
                    resSent._phID = sentval.SentenceID;
                    var wlist = WordMap.GetWordsFromMessSentence(sentval);
                    foreach (var w in wlist)
                        resSent.words.Add(w.Key, w.Value);
                    var nlist = GetNodesFromMessSentence(sentval);
                    foreach (var node in nlist)
                        resSent.treeList.Add(node.Value);
                    sentlist.Add(resSent);
                }
            }
            return sentlist;
        }

        /// <summary>
        /// Получение из сообщения списка структур tNode, сортированных в порядке слов в предложении.
        /// </summary>
        private static SortedList<int, tNode> GetNodesFromMessSentence(Sentence sent)
        {
            var outlist = new SortedList<int, tNode>();
            // Чтение узлов
            for (int i = 0; i < sent.NodesLength; i++)
            {
                if (sent.Nodes(i).HasValue)
                {
                    var node = BuildFromNode(sent.Nodes(i));
                    outlist.Add(node.ID, node);
                }
            }
            return outlist;
        }

        /// <summary>
        /// Получение структуры tNode из структуры Node.
        /// </summary>
        private static tNode BuildFromNode(Node? innode)
        {
            var innodeval = innode.Value;
            var node = new tNode();
            node.ID = innodeval.ID;
            node.Level = innodeval.Level;
            node.index = innodeval.Index;
            node.linktype = innodeval.Linktype;
            return node;
        }

        /// <summary>
        /// Формирование объекта синт.структуры предложения для записи в сообщение.
        /// </summary>
        public static VectorOffset BuildSentOffsetFromSentStructList(FlatBufferBuilder builder, List<SentenceMap> sentlist)
        {
            VectorOffset sentscol = default(VectorOffset);
            var sents = new Offset<Sentence>[sentlist.Count];
            for (int i = 0; i < sentlist.Count; i++)
            {
                sents[i] = BuildSingleFlatSent(builder, sentlist[i], i);
            }
            sentscol = Message.CreateSentencesVector(builder, sents);
            return sentscol;
        }

        private static Offset<Sentence> BuildSingleFlatSent(FlatBufferBuilder builder, SentenceMap sentence, int order)
        {
            // Чтение слов
            var words = new Offset<Lexema>[sentence.Capasity];
            for (int i = 0; i < sentence.Capasity; i++)
            {
                var word = sentence.GetWordByOrder(i);
                var EntryName = builder.CreateString(word.EntryName);

                // Чтение граммем
                var pairs = word.GetPairs();
                var grammems = new Offset<Grammema>[pairs.Count];
                int j = 0;
                foreach (var pair in pairs)
                {
                    grammems[j] = Grammema.CreateGrammema(builder, pair.Key, pair.Value);
                    j++;
                }
                var gramsCol = Lexema.CreateGrammemsVector(builder, grammems);
                words[i] = Lexema.CreateLexema(builder, i, EntryName, word.ID_Entry, word.ID_PartOfSpeech, gramsCol);
            }
            var wordsCol = Sentence.CreateWordsVector(builder, words);

            // Чтение узлов
            var treelist = sentence.GetTreeList();
            var nodes = new Offset<Node>[treelist.Count];

            for (int i = 0; i < treelist.Count; i++)
            {
                nodes[i] = Node.CreateNode(builder, treelist[i].ID, treelist[i].Level,
                    treelist[i].index, treelist[i].linktype);
            }
            var nodesCol = Sentence.CreateNodesVector(builder, nodes);

            var sentVal = builder.CreateString("");
            return Sentence.CreateSentence(builder, order, nodesCol, wordsCol, sentVal);
        }
    }
}

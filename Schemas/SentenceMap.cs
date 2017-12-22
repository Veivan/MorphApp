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
		public short ID; // Порядок добавления в дерево, для сортировки в виде плоского списка
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
		/// <summary>
		/// Идентификатор предложения в БД.
		/// </summary>
		public long SentenceID { get { return _phID; } set { _phID = value; } }

		/// <summary>
		/// Хранилище структур слов предложения.
		/// </summary>
		private SortedList<int, WordMap> words = new SortedList<int, WordMap>();

		/// <summary>
		/// Хранилище синтаксических связей предложения.
		/// </summary>
		private List<tNode> treeList = new List<tNode>();

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

		public void AddWord(int order, WordMap word, int Level = -1, int linktype = -1)
		{
			if (words.ContainsKey(order) || word == null)
				return;
			words.Add(order, word);
			if (Level > -1 && linktype > -1)
			{
				short maxID = 0;
				if (treeList.Count > 0)
					maxID = (short)(treeList.OrderByDescending(x => x.Level).First().ID + 1);
				var node = new tNode();
				node.ID = maxID;
				node.Level = Level;
				node.index = order;
				node.linktype = linktype;
				treeList.Add(node);
			}
		}

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
		private static SortedList<short, tNode> GetNodesFromMessSentence(Sentence sent)
		{
			SortedList<short, tNode> outlist = null;
			outlist = new SortedList<short, tNode>();
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
			for (short i = 0; i < sentlist.Count; i++)
			{
				sents[i] = BuildSingleFlatSent(builder, sentlist[i], i);
			}
			sentscol = Message.CreateSentencesVector(builder, sents);
			return sentscol;
		}

		private static Offset<Sentence> BuildSingleFlatSent(FlatBufferBuilder builder, SentenceMap sentence, short order)
		{
			// Чтение слов
			var words = new Offset<Lexema>[sentence.Capasity];
			for (short i = 0; i < sentence.Capasity; i++)
			{
				var word = sentence.GetWordByOrder(i);
				var EntryName = builder.CreateString(word.EntryName);

				// Чтение граммем
				var pairs = word.GetPairs();
				var grammems = new Offset<Grammema>[pairs.Count];
				short j = 0;
				foreach (var pair in pairs)
				{
					grammems[j] = Grammema.CreateGrammema(builder, (short)pair.Key, (short)pair.Value);
					j++;
				}
				var gramsCol = Lexema.CreateGrammemsVector(builder, grammems);
				words[i] = Lexema.CreateLexema(builder, i, EntryName, word.ID_Entry, (short)word.ID_PartOfSpeech, gramsCol);
			}
			var wordsCol = Sentence.CreateWordsVector(builder, words);

			// Чтение узлов
			var treelist = sentence.GetTreeList();
			var nodes = new Offset<Node>[treelist.Count];

			for (short i = 0; i < treelist.Count; i++)
			{
				nodes[i] = Node.CreateNode(builder, treelist[i].ID, (short)treelist[i].Level,
					(short)treelist[i].index, (short)treelist[i].linktype);
			}
			var nodesCol = Sentence.CreateNodesVector(builder, nodes);

			var sentVal = builder.CreateString("");
			return Sentence.CreateSentence(builder, order, nodesCol, wordsCol, sentVal);
		}
	}
}

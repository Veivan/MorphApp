using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MorphMQserver
{
	struct tNode
	{
		public int Level;
		public int orderlvl;
		public int index;
		public int linktype;
	}
	
	/// <summary>
	/// Класс хранит информацию о предложении.
	/// </summary>
	class SentenceMap
	{
		private SortedList<int, WordMap> words = new SortedList<int, WordMap>();

		private List<tNode> treeList = new List<tNode>();

		public int Capasity
		{
			get
			{
				return words.Count;
			}
		}

		public void AddWord(int order, WordMap word, int Level = -1, int orderlvl = -1, int linktype = -1)
		{
			if (words.ContainsKey(order) || word == null)
				return;
			words.Add(order, word);
			if (Level > -1 && orderlvl > -1)
			{
				var node = new tNode();
				node.Level = Level;
				node.orderlvl = orderlvl;
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

		public List<tNode> GetTreeList()
		{
			var newlist = treeList.OrderBy(x => x.Level).ThenBy(x => x.orderlvl).ToList();
			return newlist;
		}

	}
}

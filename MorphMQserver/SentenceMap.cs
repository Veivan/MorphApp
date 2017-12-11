using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MorphMQserver
{
	struct tNode
	{
		public int ID; // Порядок добавления в дерево, для сортировки в виде плоского списка
		public int Level; // Уровень вложенности, для отображения
		public int index; // порядковый номер в предложении
		public int linktype; // тип взаимосвязи с родителем
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

		public void AddWord(int order, WordMap word, int Level = -1, int linktype = -1)
		{
			if (words.ContainsKey(order) || word == null)
				return;
			words.Add(order, word);
			//if (Level > -1 && orderlvl > -1)
			var maxID = 0;
			if (treeList.Count > 0)
				maxID = treeList.OrderByDescending(x => x.Level).First().Level + 1;
			//if (lastNode == null) maxID = 0; else maxID++;
			{
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

		public List<tNode> GetTreeList()
		{
			var newlist = treeList.OrderBy(x => x.ID)
				//.ThenBy(x => x.orderlvl)
				.ToList();
			return newlist;
		}

	}
}

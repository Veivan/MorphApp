using System.Collections.Generic;
using System.Linq;

using Schemas;
using Schemas.BlockPlatform;

namespace AsmApp.Types
{

	public struct tNode
	{
		public int ID;          // Порядок добавления в дерево, для сортировки в виде плоского списка
		public int Level;       // Уровень вложенности, для отображения
		public int index;       // порядковый номер слова (ребёнка в синт.связи) в предложении
		public int linktype;    // тип взаимосвязи с родителем
		public int parentind;   // порядковый номер слова (родителя в синт.связи) в предложении
	}

	/// <summary>
	/// Класс хранит информацию о предложении.
	/// </summary>
	public class SentenceAsm : AssemblyBase
	{
		#region Privates
		// Сборка, из которой было сформировано Предложение
		private AssemblyBase srcAsm;
		/// <summary>
		/// Хранилище структур слов предложения.
		/// </summary>
		private SortedList<int, WordAsm> words = new SortedList<int, WordAsm>();
		#endregion

		/// <summary>
		/// Хранилище синтаксических связей предложения.
		/// </summary>
		private List<tNode> treeList = new List<tNode>();

		#region Properties


		/// <summary>
		/// Хранилище синтаксических связей предложения.
		/// </summary>
		public int Position { get; set; }

		public string sentence;
		public int hash;
		public bool IsActual;

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

		#endregion

		#region Constructors

		public SentenceAsm() : base(Session.Instance().GetBlockTypeByNameKey(Session.sentenceTypeName))
		{
		}

		public SentenceAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
		}
		#endregion

		#region Methods
		/// <summary>
		/// Добавление слова и синт.узла во внутренние хранилища.
		/// </summary>
		/// <param name="order">Порядок следования слова в предложении</param>
		/// <param name="word">Структура слова</param>
		/// <returns></returns>
		public void AddWord(int order, WordAsm word)
		{
			if (!words.ContainsKey(order) && word != null)
				words.Add(order, word);
		}

		/// <summary>
		/// Добавление синт.узла во внутреннее хранилище.
		/// </summary>
		/// <param name="order">порядковый номер слова (ребёнка в синт.связи) в предложении</param>
		/// <param name="Level">Уровень вложенности синтаксического узла</param>
		/// <param name="linktype">тип взаимосвязи с родителем</param>
		/// <param name="parentind">порядковый номер слова (родителя в синт.связи) в предложении</param>
		/// <returns></returns>
		public void AddNode(int order, int Level, int linktype, int parentind)
		{
			if (Level == -1)
				return;
			int maxID = 0;
			if (treeList.Count > 0)
				maxID = (int)(treeList.OrderByDescending(x => x.ID).First().ID + 1);
			var node = new tNode();
			node.ID = maxID;
			node.Level = Level;
			node.index = order;
			node.linktype = linktype;
			node.parentind = parentind;
			treeList.Add(node);
		}

		/// <summary>
		/// Получение слова предложения по его порядку следования.
		/// </summary>
		public WordAsm GetWordByOrder(int order)
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

		#endregion

	}
}

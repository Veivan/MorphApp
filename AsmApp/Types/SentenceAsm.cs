using System.Collections.Generic;
using System.Linq;

using Schemas;
using Schemas.BlockPlatform;

namespace AsmApp.Types
{
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

		/// <summary>
		/// Хранилище синтаксических связей предложения.
		/// </summary>
		private List<SyntNodeAsm> treeList = new List<SyntNodeAsm>();
		#endregion

		#region Properties

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
			var store = Session.Instance().Store;
			var wordIDs = (List<long>)srcAsm.GetValue("Words");
			foreach (var ID in wordIDs)
			{
				var asm = store.GetAssembly(ID);
				var word = new WordAsm(asm);
				words.Add(word.order, word);
			}
			var syntIDs = (List<long>)srcAsm.GetValue("SyntNodes");
			foreach (var ID in syntIDs)
			{
				var asm = store.GetAssembly(ID);
				var syntNode = new SyntNodeAsm(asm);
				treeList.Add(syntNode);
			}
		}
		#endregion

		#region Methods
		/// <summary>
		/// Добавление слова во внутренние хранилища.
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
		/// <param name="node">объект типа SyntNodeAsm</param>
		/// <returns></returns>
		public void AddNode(SyntNodeAsm node)
		{
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
		public List<SyntNodeAsm> GetTreeList()
		{
			var newlist = new List<SyntNodeAsm>(treeList);
			return newlist;
		}

		public override void Save()
		{
			var wordlist = new List<long>();
			// Сохранение слов в БД
			foreach (var word in words) { 
				word.Value.Save();
				wordlist.Add(word.Value.BlockID);
			}
			this.SetValue("Words", wordlist);

			// Сохранение списка синтаксических связей предложения в БД
			var syntNodes = new List<long>();
			foreach (var node in treeList)
			{
				var word = words.Where(o => o.Value.order == node.ChildOrder).Select(o => o.Value).FirstOrDefault();
				if (word != null)
					node.CID = word.BlockID;
				var pword = words.Where(o => o.Value.order == node.ParentOrder).Select(o => o.Value).FirstOrDefault();
				if (pword != null)
					node.PCID = pword.BlockID;
				node.Save();
				syntNodes.Add(node.BlockID);
			}
			this.SetValue("SyntNodes", syntNodes);
			base.Save();
		}

		#endregion

	}
}

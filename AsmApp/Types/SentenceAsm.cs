using System.Collections.Generic;
using System.Linq;
using System.Text;

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

		private string sentence;

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

		public int hash;
		public bool IsActual;

		/// <summary>
		/// Количество слов предложения.
		/// </summary>
		public int Capasity	{ get { return words.Count;	} }

		/// <summary>
		/// Текст предложения.
		/// </summary>
		public string Text { get { return sentence; } set { sentence = value; } }
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
			int i = 0;
			if(wordIDs != null)
				foreach (var ID in wordIDs)
				{
					var asm = store.GetAssembly(ID);
					var word = new WordAsm(asm);
					word.Order = i++;
					words.Add((int)word.Order, word);
				}
			var syntIDs = (List<long>)srcAsm.GetValue("SyntNodes");
			if (syntIDs != null)
				foreach (var ID in syntIDs)
				{
					var asm = store.GetAssembly(ID);
					var syntNode = new SyntNodeAsm(asm);
					treeList.Add(syntNode);
				}

			this.sentence = RestorePhrase();
		}
		#endregion

		#region Public Methods
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
			//Сохранение предложения для получения ID
			if (this.IsVirtual)
				base.Save();
			
			// Сохранение слов в БД
			foreach (var word in words) {
				word.Value.ParentAssemblyID = this.RootBlock_id;
				word.Value.Save();
				wordlist.Add(word.Value.BlockID);
			}
			this.SetValue("Words", wordlist);


			// Сохранение списка синтаксических связей предложения в БД
			var syntNodes = new List<long>();
			foreach (var node in treeList)
			{
				var word = words.Where(o => o.Value.Order == node.ChildOrder).Select(o => o.Value).FirstOrDefault();
				if (word != null)
					node.CID = word.BlockID;
				var pword = words.Where(o => o.Value.Order == node.ParentOrder).Select(o => o.Value).FirstOrDefault();
				if (pword != null)
					node.PCID = pword.BlockID;
				node.ParentAssemblyID = this.BlockID;
				node.Save();
				syntNodes.Add(node.BlockID);
			}
			this.SetValue("SyntNodes", syntNodes);
			base.Save();
		}
		#endregion

		#region Private Methods
		/// <summary>
		/// Восстановление предложения по формам слов, хранящимся в структурах WordAsm
		/// </summary>
		private string RestorePhrase()
		{
			var sb = new StringBuilder();
			for (int i = 0; i < Capasity; i++)
			{
				var word = GetWordByOrder(i);
				if (i > 0 && i < Capasity && word.ID_PartOfSpeech != (int)GrenPart.PUNCTUATION_class)
					sb.Append(" ");
				sb.Append(word.RealWord);
			}
			return sb.ToString();
		}
		#endregion

	}
}

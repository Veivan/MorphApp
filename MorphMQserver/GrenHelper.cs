using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SolarixGrammarEngineNET;
using Schemas;

namespace MorphMQserver
{

	public class GrenHelper
	{
		private bool IsReady = false;
		private IntPtr hEngine = IntPtr.Zero;

		public void Init()
		{
			var dict = Properties.MorphMQsrvr.Default.DictPath;
			// Загружаем грамматический словарь в ленивом режиме, то есть словарные статьи нам сейчас не нужны сразу все в оперативной памяти.
			// IntPtr hEngine = GrammarEngine.sol_CreateGrammarEngineExW(dict_path, GrammarEngine.SOL_GREN_LAZY_LEXICON);

			// http://www.solarix.ru/api/ru/sol_CreateGrammarEngine.shtml
			hEngine = GrammarEngine.sol_CreateGrammarEngineW(dict);
			if (hEngine == IntPtr.Zero)
			{
				MessageBox.Show("Could not load the dictionary");
			}
			else
				IsReady = true;				
		}

		public string GetDictVersion()
		{
			string result = "Ошибка загрузки словаря.";
			if (IsReady)
			{
				Int32 r = GrammarEngine.sol_DictionaryVersion(hEngine);
				if (r != -1)
					result = r.ToString();
			}
			return result;
		}

		public string MakeNRestoreSentence(string phrase)
		{
			string result = "Ошибка загрузки словаря.";
			if (!IsReady)
				return result;
			var sb = new StringBuilder();
			var sent = new SentenceMap();
			// http://www.solarix.ru/api/ru/sol_Tokenize.shtml
			string[] tokens = GrammarEngine.sol_TokenizeFX(hEngine, phrase, GrammarEngineAPI.RUSSIAN_LANGUAGE);
			// http://www.solarix.ru/api/ru/sol_CountStrings.shtml
			int ntok = tokens.Length;
			sb.Append("Токенов : " + ntok + "\r\n");
			sb.Append(new String('-', 32) + "\r\n");

			int iorder = 0;
			WordMap wmap = null;
			foreach (var token in tokens)
			{
				// http://www.solarix.ru/api/ru/sol_ProjectWord.shtml
				IntPtr hProjs = GrammarEngine.sol_ProjectWord(hEngine, token, 0);
				int nprojs = GrammarEngine.sol_CountProjections(hProjs);
				for (int i = 0; i < nprojs; i++)
				{
					int id_entry = GrammarEngine.sol_GetIEntry(hProjs, i);
					int id_partofspeech = GrammarEngine.sol_GetEntryClass(hEngine, id_entry);

					wmap = new WordMap(id_entry, id_partofspeech);
					wmap.EntryName = GrammarEngine.sol_GetEntryNameFX(hEngine, id_entry);

					// Определение типа класса по ID части речи
					var xType = Gren.sgAPI.GetTypeClassByID((GrenPart)id_partofspeech);
					if (xType != null)
					{
						// Создание класса части речи
						var xPart = Activator.CreateInstance(xType) as HasDict;
						wmap.xPart = xPart;
						//Перебор пар характеристик, относящихся к данной части речи
						foreach (var CoordID in xPart.dimensions)
						{
							int state = GrammarEngine.sol_GetProjCoordState(hEngine, hProjs, i, CoordID);
							wmap.AddPair(CoordID, state);
						}
					}

					/* 
					 * //Перебор всех пар характеристик
					// http://www.solarix.ru/api/ru/sol_GetNodePairsCount.shtml
					int PairsCount = GrammarEngine.sol_GetNodePairsCount(hNode);
					for (int i = 0; i < PairsCount; i++)
					{
						// http://www.solarix.ru/api/ru/sol_GetNodePairCoord.shtml
						int Coord = GrammarEngine.sol_GetNodePairCoord(hNode, i);
						// http://www.solarix.ru/api/ru/sol_GetNodePairState.shtml
						int State = GrammarEngine.sol_GetNodePairState(hNode, i);
					}*/

					sent.AddWord(iorder++, wmap);
				}
			}

			sb.Append(RestoreSentenceOnePass(sent));
			result = sb.ToString();
			return result;
		}

		/// <summary>
		/// Восстановление предложения из грамматических характеристик каждого слова
		/// с использованием обобщённой функции sol_GenerateWordformsFX.
		/// </summary>
		public string RestoreSentenceOnePass(SentenceMap sentence)
		{
			if (sentence == null)
				return "";

			// Перечень характеристик, нужных для восстановления слова по словоформе
			var props = new GrenProperty[] { GrenProperty.NUMBER_ru, GrenProperty.CASE_ru ,
					GrenProperty.GENDER_ru, GrenProperty.TENSE_ru, GrenProperty.PERSON_ru,
					GrenProperty.FORM_ru, GrenProperty.SHORTNESS_ru, GrenProperty.COMPAR_FORM_ru
				};
			var sb = new StringBuilder();
			var coords = new ArrayList();
			var states = new ArrayList();
			for (int i = 0; i < sentence.Capasity; i++)
			{
				var wmap = sentence.GetWordByOrder(i);
				coords.Clear();
				states.Clear();

				foreach (var prop in props)
				{
					var val = wmap.GetPropertyValue(prop);
					if (val > -1)
					{
						coords.Add((int)prop);
						states.Add((int)val);
					}
				}
			
				// Проверка ID_Entry (В БД не хранится ID_Entry)
				int entry_id = GrammarEngine.sol_FindEntry( hEngine, wmap.EntryName, wmap.ID_PartOfSpeech, GrammarEngineAPI.RUSSIAN_LANGUAGE );
				//if( entry_id==-1 )
				string word = "";
				ArrayList fx = GrammarEngine.sol_GenerateWordformsFX(hEngine, entry_id, coords, states);
				//ArrayList fx = GrammarEngine.sol_GenerateWordformsFX(hEngine, wmap.ID_Entry, coords, states);
				if (fx != null && fx.Count > 0)
				{
					word = (fx[0] as String).ToLower();
				}

				if (word != "")
				{
					if (i == 0)
					{
						word = char.ToUpper(word[0]) + word.Substring(1);
					}
					if (i > 0 && i < sentence.Capasity && wmap.ID_PartOfSpeech != GrammarEngineAPI.PUNCTUATION_class)
						sb.Append(" ");
					sb.Append(word);
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Пример: Восстановление предложения из грамматических характеристик каждого слова
		/// с использованием функций частей речи.
		/// </summary>
		/*
		private string RestoreSentence(SentenceMap sentence)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < sentence.Capasity; i++)
			{
				var wmap = sentence.GetWordByOrder(i);
				string word = wmap.EntryName;
				var tok_buf = new StringBuilder(GrammarEngine.sol_MaxLexemLen(hEngine));
				int rc_res = -1;
				if (wmap.xPart.CanMorph)
				{
					switch (wmap.xPart.MorphAs)
					{
						case Gren.GrenPart.NOUN_ru:
							rc_res = GrammarEngine.sol_GetNounForm(
										hEngine,
										wmap.ID_Entry,
										wmap.GetPropertyValue(Gren.GrenProperty.NUMBER_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.CASE_ru),
										tok_buf
										);
							break;
						case Gren.GrenPart.VERB_ru:
							rc_res = GrammarEngine.sol_GetVerbForm(
										hEngine,
										wmap.ID_Entry,
										wmap.GetPropertyValue(Gren.GrenProperty.NUMBER_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.GENDER_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.TENSE_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.PERSON_ru),
										tok_buf
									    );
							break;
						case Gren.GrenPart.ADJ_ru:
							int rc_subj = GrammarEngine.sol_GetAdjectiveForm(
										hEngine,
										wmap.ID_Entry,
										wmap.GetPropertyValue(Gren.GrenProperty.NUMBER_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.GENDER_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.CASE_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.FORM_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.SHORTNESS_ru),
										wmap.GetPropertyValue(Gren.GrenProperty.COMPAR_FORM_ru),
										tok_buf
										);
							break;
					}
					word = tok_buf.ToString();
				}
				sb.Append(word);
				sb.Append(" ");
			}

			return sb.ToString();
		} */

        /// <summary>
        /// Разбивка текста на предложении.
        /// </summary>
        public List<string> SeparateIt(string text)
        {
            if (!IsReady)
                return null; //"Ошибка загрузки словаря.";

            var outlist = new List<string>();

            // эту строку будет делить на предложения с помощью сегментатора
            // http://www.solarix.ru/api/ru/sol_CreateSentenceBrokerMem.shtml
            IntPtr hSegmenter = GrammarEngine.sol_CreateSentenceBrokerMemW(hEngine, text, GrammarEngineAPI.RUSSIAN_LANGUAGE);

            while (GrammarEngine.sol_FetchSentence(hSegmenter) >= 0)
            {
                // извлекаем очередное предложение...
                string sentence = GrammarEngine.sol_GetFetchedSentenceFX(hSegmenter);
                if (sentence.Length > 0)
                    outlist.Add(sentence);
            }

            // закончили извлекать предложения из текста - удаляем объект сегментатора
            // http://www.solarix.ru/api/ru/sol_DeleteSentenceBroker.shtml
            GrammarEngine.sol_DeleteSentenceBroker(hSegmenter);

            return outlist;
        }
        
        /// <summary>
		/// Получение грамматических характеристик каждого слова в предложении.
		/// </summary>
		public string GetMorphInfo(string phrase)
		{
			string result = "Ошибка загрузки словаря.";
			if (!IsReady)
				return result;

			var sb = new StringBuilder();

			// http://www.solarix.ru/api/ru/sol_Tokenize.shtml
			string[] tokens = GrammarEngine.sol_TokenizeFX(hEngine, phrase, GrammarEngineAPI.RUSSIAN_LANGUAGE);

			// http://www.solarix.ru/api/ru/sol_CountStrings.shtml
			int ntok = tokens.Length;

			sb.Append("Токенов : " + ntok + "\r\n");
			sb.Append(new String('-', 32) + "\r\n");

			WordMap wmap = null;
			foreach (var token in tokens)
			{
				// http://www.solarix.ru/api/ru/sol_ProjectWord.shtml
				IntPtr hProjs = GrammarEngine.sol_ProjectWord(hEngine, token, 0);
				int nprojs = GrammarEngine.sol_CountProjections(hProjs);
				for (int i = 0; i < nprojs; ++i)
				{
					int id_entry = GrammarEngine.sol_GetIEntry(hProjs, i);
					int id_partofspeech = GrammarEngine.sol_GetEntryClass(hEngine, id_entry);

					wmap = new WordMap(id_entry, id_partofspeech);
					wmap.EntryName = GrammarEngine.sol_GetEntryNameFX(hEngine, id_entry);

					StringBuilder PartOfSpeechName = new StringBuilder(GrammarEngine.sol_MaxLexemLen(hEngine));
					GrammarEngine.sol_GetClassName(hEngine, id_partofspeech, PartOfSpeechName);

					sb.Append(wmap.EntryName + "\r\n");
					sb.Append(PartOfSpeechName + "\r\n");

					// Определение типа класса по ID части речи
					var xType = Gren.sgAPI.GetTypeClassByID((GrenPart)id_partofspeech);
					if (xType != null)
					{
						// Создание класса части речи
						var xPart = Activator.CreateInstance(xType) as HasDict;
						wmap.xPart = xPart;
						foreach (var CoordID in xPart.dimensions)
						{
							int state = GrammarEngine.sol_GetProjCoordState(hEngine, hProjs, i, CoordID);
							wmap.AddPair(CoordID, state);
						}

						var pairs = wmap.GetPairs();
						ICollection<int> keys = pairs.Keys;
						StringBuilder propname = new StringBuilder(GrammarEngine.sol_MaxLexemLen(hEngine));
						StringBuilder propvalname = new StringBuilder(GrammarEngine.sol_MaxLexemLen(hEngine));
						foreach (int j in keys)
						{
							GrammarEngine.sol_GetCoordName(hEngine, j, propname);
							GrammarEngine.sol_GetCoordStateName(hEngine, j, pairs[j], propvalname);
							sb.Append(String.Format("ID -> {0}  Name -> {1} \r\n", propname, propvalname));
						}
					}
					sb.Append(new String('-', 32) + "\r\n\r\n");
				}
				GrammarEngine.sol_DeleteProjections(hProjs);
			}

			result = sb.ToString();
			return result;
		}

		public string TokenizeIt(string phrase)
		{
			string result = "Ошибка загрузки словаря.";
			if (!IsReady)
				return result;

			DateTime t0 = DateTime.Now;
			int sentence_count = 0;
			int word_count = 0;
			string text = phrase;

			// эту строку будет делить на предложения с помощью сегментатора
			// http://www.solarix.ru/api/ru/sol_CreateSentenceBrokerMem.shtml
			IntPtr hSegmenter = GrammarEngine.sol_CreateSentenceBrokerMemW(hEngine, text, GrammarEngineAPI.RUSSIAN_LANGUAGE);

			StringBuilder sb = new StringBuilder();
			while (GrammarEngine.sol_FetchSentence(hSegmenter) >= 0)
			{
				// извлекаем очередное предложение...
				string sentence = GrammarEngine.sol_GetFetchedSentenceFX(hSegmenter);

				if (sentence.Length > 0)
				{
					sentence_count++;

					// разбиваем его на слова с помощью токенизатора
					// http://www.solarix.ru/api/ru/sol_Tokenize.shtml
					string[] tokens = GrammarEngine.sol_TokenizeFX(hEngine, sentence, SolarixGrammarEngineNET.GrammarEngineAPI.RUSSIAN_LANGUAGE);
					word_count += tokens.Length;
					foreach (string token in tokens)
					{
						// каждое слово печатаем на отдельной строке
						sb.Append(token + "\r\n");
					}
					// это разделитель слов в разных предложениях
					sb.Append(new String('-', 32) + "\r\n");
				}
			}

			// закончили извлекать предложения из текста - удаляем объект сегментатора
			// http://www.solarix.ru/api/ru/sol_DeleteSentenceBroker.shtml
			GrammarEngine.sol_DeleteSentenceBroker(hSegmenter);

			DateTime t1 = DateTime.Now;
			double elapsed_sec = (t1 - t0).TotalSeconds;
			MessageBox.Show(String.Format(" {0} sentences, {1} words DONE.\n Elapsed time: {0} sec", sentence_count, word_count, elapsed_sec));

			return sb.ToString();
		}

        /// <summary>
        /// Выполнение синтаксического анализа предложения.
        /// Получение структуры SentenceMap из предложении.
        /// </summary>
        public SentenceMap GetSynInfoMap(string phrase)
        {
            if (!IsReady)
                return null; //"Ошибка загрузки словаря."

            // 60000 - это ограничение на время разбора в мсек.
            // 20 - это beam size в ходе перебора вариантов.
            IntPtr hPack = GrammarEngine.sol_SyntaxAnalysis(hEngine, phrase, 0, 0, (60000 | (20 << 22)), GrammarEngineAPI.RUSSIAN_LANGUAGE);

            // Выберем граф с минимальным количеством корневых узлов
            // http://www.solarix.ru/api/ru/sol_CountGrafs.shtml
            int ngrafs = GrammarEngine.sol_CountGrafs(hPack);

            int imin_graf = -1, minv = 2000000;
            for (int i = 0; i < ngrafs; i++)
            {
                // http://www.solarix.ru/api/ru/sol_CountRoots.shtml
                int nroots = GrammarEngine.sol_CountRoots(hPack, i);
                if (nroots < minv)
                {
                    minv = nroots;
                    imin_graf = i;
                }
            }

            Int32 rCount = GrammarEngine.sol_CountRoots(hPack, imin_graf);
            var sent = new SentenceMap();
            int Level = 0;
            // Сохранение графа в памяти
            for (int j = 1; j < rCount - 1; j++)
            {
                // http://www.solarix.ru/api/ru/sol_GetRoot.shtml
                IntPtr hNode = GrammarEngine.sol_GetRoot(hPack, imin_graf, j);
                SaveNodeReq(hNode, sent, Level, IntPtr.Zero, j);
            }
            return sent;
        }
        
        public string GetSynInfo(string phrase)
		{
			string result = "Ошибка загрузки словаря.";
			if (!IsReady)
				return result;

            SentenceMap sent = GetSynInfoMap(phrase);

			StringBuilder sb = new StringBuilder();
			sb.Append(RestoreSentenceOnePass(sent));
			sb.Append("\r\n");

			sb.Append(PrintGraf(sent));

			/*/ Распечатаем этот граф
			StringBuilder sb = new StringBuilder();
			result = "";
			for (int j = 1; j < rCount-1; j++)
			{
				// http://www.solarix.ru/api/ru/sol_GetRoot.shtml
				IntPtr hNode = GrammarEngine.sol_GetRoot(hPack, imin_graf, j);
				PrintNode(hNode, sb, IntPtr.Zero, j);
				sb.Append(" ");
			} */

			result = sb.ToString();
			return result;
		}

		private void SaveNodeReq(IntPtr hNode, SentenceMap sent, int Level, IntPtr hParentNode, int LeafIndex)
		{
			WordMap wmap = null;
			int Position = GrammarEngine.sol_GetNodePosition(hNode);
			if (Position > -1)
			{
				int id_entry = GrammarEngine.sol_GetNodeIEntry(hEngine, hNode);
				int id_partofspeech = GrammarEngine.sol_GetEntryClass(hEngine, id_entry);
				wmap = new WordMap(id_entry, id_partofspeech);
				wmap.EntryName = GrammarEngine.sol_GetEntryNameFX(hEngine, id_entry);
				if (wmap.EntryName == "???")
					wmap.EntryName = "дубль";
				// Определение типа класса по ID части речи
				var xType = Gren.sgAPI.GetTypeClassByID((GrenPart)id_partofspeech);
				if (xType != null)
				{
					// Создание класса части речи
					var xPart = Activator.CreateInstance(xType) as HasDict;
					wmap.xPart = xPart;
					//Перебор пар характеристик, относящихся к данной части речи
					foreach (var CoordID in xPart.dimensions)
					{
						int state = GrammarEngine.sol_GetNodeCoordState(hNode, CoordID);
						wmap.AddPair(CoordID, state);
					}

					int linktype = -1;
					if (hParentNode != IntPtr.Zero)
					{
						linktype = GrammarEngine.sol_GetLeafLinkType(hParentNode, LeafIndex);
					}
					sent.AddWord(Position, wmap, Level, linktype);
				}
				Int32 n_leaf = GrammarEngine.sol_CountLeafs(hNode);				
				for (int ileaf = 0; ileaf < n_leaf; ileaf++)
				{
					IntPtr hLeaf = GrammarEngine.sol_GetLeaf(hNode, ileaf);
					SaveNodeReq(hLeaf, sent, Level + 1, hNode, ileaf);
				}
			}
		}

		private string PrintGraf(SentenceMap sent)
		{
			StringBuilder sb = new StringBuilder();

			var ordlist = sent.GetTreeList();
			foreach (tNode x in ordlist)
			{
				sb.Append(new String('\t', x.Level) + 
					String.Format("{0} Level {1} link {2} \r\n",
						sent.GetWordByOrder(x.index).EntryName, x.Level, Gren.sgAPI.GetLinkTypeStr(x.linktype)));
			}

			return sb.ToString();
		}

		private string PrintNode(IntPtr hNode, StringBuilder sbit, IntPtr hParentNode, int LeafIndex)
		{
			Int32 n_leaf = GrammarEngine.sol_CountLeafs(hNode);
			StringBuilder sb = new StringBuilder();
			// http://www.solarix.ru/api/ru/sol_GetNodeContents.shtml
			//GrammarEngine.sol_GetNodeContents(hNode, sb);

			string Content = GrammarEngine.sol_GetNodeContentsFX(hNode);
			sb.Append(Content);

			int Position = GrammarEngine.sol_GetNodePosition(hNode);
			if (Position > -1)
			{
				sb.Append("\t Position: " + Position.ToString());
				// http://www.solarix.ru/api/ru/sol_GetNodeIEntry.shtml
				int ient = GrammarEngine.sol_GetNodeIEntry(hEngine, hNode);
				string NameFX = GrammarEngine.sol_GetEntryNameFX(hEngine, ient);
				sb.Append("\t");
				sb.Append(NameFX);


				// Определение части речи
				// http://www.solarix.ru/api/ru/sol_GetClassName.shtml
				int iclass = GrammarEngine.sol_GetEntryClass(hEngine, ient);
				StringBuilder buffer = new StringBuilder(SolarixGrammarEngineNET.GrammarEngine.sol_MaxLexemLen(hEngine));
				//SolarixGrammarEngineNET.GrammarEngine.sol_GetClassName(hEngine, SolarixGrammarEngineNET.GrammarEngineAPI.VERB_ru, buffer);
				SolarixGrammarEngineNET.GrammarEngine.sol_GetClassName(hEngine, iclass, buffer);
				sb.Append("\t");
				sb.Append(buffer.ToString());


				/*/ http://www.solarix.ru/api/ru/sol_GetNodePairsCount.shtml
				int PairsCount = GrammarEngine.sol_GetNodePairsCount(hNode);
				for (int i = 0; i < PairsCount; i++)
				{
					// http://www.solarix.ru/api/ru/sol_GetNodePairCoord.shtml
					int Coord = GrammarEngine.sol_GetNodePairCoord(hNode, i);
					// http://www.solarix.ru/api/ru/sol_GetNodePairState.shtml
					int State = GrammarEngine.sol_GetNodePairState(hNode, i);
					sb.Append("\t");
					sb.Append(Coord.ToString());
					sb.Append("\t");
					sb.Append(State.ToString());
				} */

				/*/ http://www.solarix.ru/api/ru/sol_GetNodeVersionCount.shtml
				int VersionCount = GrammarEngine.sol_GetNodeVersionCount(hEngine, hNode);
				sb.Append("\t Число версий: " + VersionCount.ToString()); */


				if (hParentNode != IntPtr.Zero)
				{
					int ss = GrammarEngine.sol_GetLeafLinkType(hParentNode, LeafIndex);
					string x = Gren.sgAPI.GetLinkTypeStr(ss);
					sb.Append("\t");
					sb.Append(x);
					sb.Append("\t");
				}
			}
			sbit.Append(sb);
			//Console.Write(sb.ToString());

			if (n_leaf > 0)
			{
				//Console.Write("( ");
				sbit.Append("\r\n\t( ");

				for (int ileaf = 0; ileaf < n_leaf; ++ileaf)
				{
					IntPtr hLeaf = GrammarEngine.sol_GetLeaf(hNode, ileaf);
					PrintNode(hLeaf, sbit, hNode, ileaf);
					sbit.Append(" ");
					//Console.Write(" ");
				}

				sbit.Append(" )");
				//Console.Write(")");
			}

			return sb.ToString();
		}
	}
}

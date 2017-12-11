﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

using SolarixGrammarEngineNET;

namespace MorphMQserver
{

	class GrenHelper
	{
		const string dict = @"d:\Work\Framework\GrammarEngine\src\bin-windows64\dictionary.xml";

		private bool IsReady = false;
		private IntPtr hEngine = IntPtr.Zero;

		public void Init()
		{
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
					var xType = Gren.sgAPI.GetTypeClassByID((Gren.GrenPart)id_partofspeech);
					if (xType != null)
					{
						// Создание класса части речи
						var xPart = Activator.CreateInstance(xType) as Gren.HasDict;
						wmap.xPart = xPart;
						foreach (var CoordID in xPart.dimensions)
						{
							int state = GrammarEngine.sol_GetProjCoordState(hEngine, hProjs, i, CoordID);
							wmap.AddPair(CoordID, state);
						}
					}
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
		private string RestoreSentenceOnePass(SentenceMap sentence)
		{
			var sb = new StringBuilder();
			for (int i = 0; i < sentence.Capasity; i++)
			{
				var wmap = sentence.GetWordByOrder(i);
				var coords = new ArrayList();
				var states = new ArrayList();

				var props = new Gren.GrenProperty[] { Gren.GrenProperty.NUMBER_ru, Gren.GrenProperty.CASE_ru ,
					Gren.GrenProperty.GENDER_ru, Gren.GrenProperty.TENSE_ru, Gren.GrenProperty.PERSON_ru,
					Gren.GrenProperty.FORM_ru, Gren.GrenProperty.SHORTNESS_ru, Gren.GrenProperty.COMPAR_FORM_ru
				};

				foreach (var prop in props)
				{
					var val = wmap.GetPropertyValue(prop);
					if (val > -1)
					{
						coords.Add((int)prop);
						states.Add((int)val);
					}
				}

				string word = "";
				ArrayList fx = GrammarEngine.sol_GenerateWordformsFX(hEngine, wmap.ID_Entry, coords, states);
				if (fx != null && fx.Count > 0)
				{
					word = (fx[0] as String).ToLower();
					if (i == 0)
					{
						word = char.ToUpper(word[0]) + word.Substring(1);
					}
					if (i != sentence.Capasity-1)
						sb.Append(" ");
					sb.Append(word);
				}
			}

			return sb.ToString();
		}

		/// <summary>
		/// Восстановление предложения из грамматических характеристик каждого слова
		/// с использованием функций частей речи.
		/// </summary>
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
					var xType = Gren.sgAPI.GetTypeClassByID((Gren.GrenPart)id_partofspeech);
					if (xType != null)
					{
						// Создание класса части речи
						var xPart = Activator.CreateInstance(xType) as Gren.HasDict;
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

		public string GetSynInfo(string phrase)
		{
			string result = "Ошибка загрузки словаря.";
			if (!IsReady)
				return result;

			// 60000 - это ограничение на время разбора в мсек.
			// 20 - это beam size в ходе перебора вариантов.
			IntPtr hPack = GrammarEngine.sol_SyntaxAnalysis(hEngine, phrase, 0, 0, (60000 | (20 << 22)), GrammarEngineAPI.RUSSIAN_LANGUAGE);

			// Выберем граф с минимальным количеством корневых узлов
			// http://www.solarix.ru/api/ru/sol_CountGrafs.shtml
			int ngrafs = GrammarEngine.sol_CountGrafs(hPack);
			//MessageBox.Show("Всего графов : " + ngrafs.ToString());
			Console.Write("Наиболее достоверный вариант разбора:\n");
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

			// Распечатаем этот граф
			StringBuilder sb = new StringBuilder();
			result = "";
			//Console.Write("Наиболее достоверный вариант разбора:\n");
			Int32 rCount = GrammarEngine.sol_CountRoots(hPack, imin_graf);
			for (int j = 0; j < rCount; j++)
			{
				// http://www.solarix.ru/api/ru/sol_GetRoot.shtml
				IntPtr hNode = GrammarEngine.sol_GetRoot(hPack, imin_graf, j);
				PrintNode(hNode, sb, IntPtr.Zero, j);
				sb.Append(" ");
				//Console.Write(" ");
			}
			result = sb.ToString();
			return result;
		}

		// мама мыла раму
		//   string sent = "Пила злобно лежит на дубовом столе.";

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


				// http://www.solarix.ru/api/ru/sol_GetNodePairsCount.shtml
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

				}

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

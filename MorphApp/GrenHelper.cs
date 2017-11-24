using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SolarixGrammarEngineNET;

namespace MorphApp
{

	class GrenHelper
	{
		private bool IsReady = false;
		private IntPtr hEngine = IntPtr.Zero;

		public void Init(string dict)
		{
			// http://www.solarix.ru/api/ru/sol_CreateGrammarEngine.shtml
			hEngine = GrammarEngine.sol_CreateGrammarEngineW(dict);
			if (hEngine == IntPtr.Zero)
			{
				throw new Exception("Could not load the dictionary");
			}
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

		public string GetMorphInfo(string phrase)
		{
			string result = "Ошибка загрузки словаря.";
			if (!IsReady)
				return result;

			var sb = new StringBuilder();

			// http://www.solarix.ru/api/ru/sol_Tokenize.shtml
			IntPtr hTokens = GrammarEngine.sol_TokenizeW(hEngine, phrase, GrammarEngineAPI.RUSSIAN_LANGUAGE);
			// http://www.solarix.ru/api/ru/sol_CountStrings.shtml
			int ntok = GrammarEngine.sol_CountStrings(hTokens);
			sb.Append("Токенов : " + ntok + "\r\n");
			sb.Append("qq ");

			result = sb.ToString();
			return result;
		}

		internal string GetSynInfo(string phrase)
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
				PrintNode(hNode, sb, j);
				sb.Append(" ");
				//Console.Write(" ");
			}
			result = sb.ToString();
			return result;
		}

		// мама мыла раму
		//   string sent = "пила злобно лежит на дубовом столе";

		static string PrintNode(IntPtr hNode, StringBuilder sbit, int LeafIndex)
		{
			Int32 n_leaf = GrammarEngine.sol_CountLeafs(hNode);
			StringBuilder sb = new StringBuilder();
			GrammarEngine.sol_GetNodeContents(hNode, sb);
			int ss = GrammarEngine.sol_GetLeafLinkType(hNode, LeafIndex);
			string x = sgAPI.GetLinkTypeStr(ss);
			sb.Append(x);


			sbit.Append(sb);
			//Console.Write(sb.ToString());

			if (n_leaf > 0)
			{
				//Console.Write("( ");
				sbit.Append("\r\n\t( ");

				for (int ileaf = 0; ileaf < n_leaf; ++ileaf)
				{
					IntPtr hLeaf = GrammarEngine.sol_GetLeaf(hNode, ileaf);
					PrintNode(hLeaf, sbit, ileaf);
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

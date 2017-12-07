using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

using SolarixGrammarEngineNET;

namespace MorphApp
{

    class GrenHelperTest
    {
        const string dict = @"c:\Work\Framework\GrammarEngine\src\bin-windows64\dictionary.xml";

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

            var noun = new Gren.ADJ();
            foreach (var dim in noun.GetDimensions())
            {
                noun.AddPair(dim, 1);
            }

            result = "";
            var pairs = noun.GetPairs();
            ICollection<int> keys = pairs.Keys;
            foreach (int j in keys)
                result += String.Format("ID -> {0}  Name -> {1} \r\n", j, pairs[j]);

            var pairsn = noun.GetPairsNames();
            var keysn = pairsn.Keys;
            foreach (var j in keysn)
                result += String.Format("ID -> {0}  Name -> {1} \r\n", j, pairsn[j]);

            /*var sb = new StringBuilder();

            // http://www.solarix.ru/api/ru/sol_Tokenize.shtml
            IntPtr hTokens = GrammarEngine.sol_TokenizeW(hEngine, phrase, GrammarEngineAPI.RUSSIAN_LANGUAGE);
            // http://www.solarix.ru/api/ru/sol_CountStrings.shtml
            int ntok = GrammarEngine.sol_CountStrings(hTokens);
            sb.Append("Токенов : " + ntok + "\r\n");

            result = sb.ToString();*/
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

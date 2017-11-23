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
    }
}

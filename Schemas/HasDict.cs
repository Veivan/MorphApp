using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
    public enum GrenProperty
    {
        CharCasing = 4,                  // enum CharCasing
        PERSON_ru = 27,                  // enum ЛИЦО
        NUMBER_ru = 28,                  // enum ЧИСЛО
        GENDER_ru = 29,                  // enum РОД
        TRANSITIVENESS_ru = 30,          // enum ПЕРЕХОДНОСТЬ
        PARTICIPLE_ru = 31,              // enum ПРИЧАСТИЕ
        PASSIVE_PARTICIPLE_ru = 32,      // enum СТРАД
        ASPECT_ru = 33,                  // enum ВИД
        VERB_FORM_ru = 35,               // enum НАКЛОНЕНИЕ
        TENSE_ru = 36,                   // enum ВРЕМЯ
        SHORTNESS_ru = 37,               // enum КРАТКИЙ
        CASE_ru = 39,                    // enum ПАДЕЖ
        FORM_ru = 40,                    // enum ОДУШ
        COUNTABILITY_ru = 41,            // enum ПЕРЕЧИСЛИМОСТЬ
        COMPAR_FORM_ru = 42,             // enum СТЕПЕНЬ
        CASE_GERUND_ru = 43,             // enum ПадежВал
        MODAL_ru = 44                    // enum МОДАЛЬНЫЙ
    }

    /// <summary>
    /// Базовый класс для хранения ID характеристик слова в виде словаря.
    /// </summary>
    public class HasDict
    {
        private List<int> _dimensions = new List<int>();

        public int[] dimensions
        {
            get
            {
                return _dimensions.ToArray();
            }
        }

        protected void SetDims(int[] dims)
        {
            _dimensions.AddRange(dims);
        }
    }
}

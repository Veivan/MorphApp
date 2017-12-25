using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Schemas
{
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

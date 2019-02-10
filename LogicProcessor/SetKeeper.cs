using System;
using System.Collections.Generic;
using Schemas.BlockPlatform;

namespace LogicProcessor
{
	/// <summary>
	/// Класс-синглтон. 
	/// Создаётся один раз при создании сервера БД
	/// </summary>
	public sealed class SetKeeper
	{
		private List<AssemblyBase> saveset = new List<AssemblyBase>();
		static object _sunc = new object();
		static SetKeeper _setKeeper;

		/// <summary>
		/// Конструктор
		/// </summary>
		private SetKeeper()
		{
		}

		public static SetKeeper Instance()
		{
			//чтобы не лочить каждое обращение, так как null будет только 1 раз
			if (_setKeeper == null)
			{
				lock (_sunc)
				{
					//теперь ещё раз проверяем, чтобы не создать несколько объектов, 
					//остальные потоки после lock уже не создадут новые объекты
					if (_setKeeper == null)
					{
						_setKeeper = new SetKeeper();
					}
				}
			}

			return _setKeeper;
		}

		public void Add(AssemblyBase asm)
		{
			saveset.Add(asm);
		}

		public void Clear()
		{
			saveset.Clear();
		}

		public List<AssemblyBase> GetSet()
		{
			return new List<AssemblyBase>(saveset);
		}

		public bool IsDirty
		{
			get
			{
				return saveset.Count > 0;
			}
		}
	}
}

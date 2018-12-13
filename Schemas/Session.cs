using System;
using System.Linq;

using Schemas.BlockPlatform;
using System.Collections.Generic;

namespace Schemas
{

	/// <summary>
	/// Класс-синглтон. 
	/// Создаётся один раз при создании сервера БД
	/// </summary>
	public sealed class Session
	{
		static object _sunc = new object();
		static Session _session;

		public static long MainStoreID = 1;
		public static string MainStoreName = "Хранилище";
		public static string DefaulContainerName = "Контейнер";
		public static string containerTypeName = "DataContainer";

		private List<BlockType> typeList;
		public IBlockDealer DBserver;
		public IAsmDealer Store;

		/// <summary>
		/// Конструктор
		/// </summary>
		private Session()
		{
		}

		public static Session Instance()
		{
			//чтобы не лочить каждое обращение, так как null будет только 1 раз
			if (_session == null)
			{
				lock (_sunc)
				{
					//теперь ещё раз проверяем, чтобы не создать несколько объектов, 
					//остальные потоки после lock уже не создадут новые объекты
					if (_session == null)
					{
						_session = new Session();
					}
				}
			}

			return _session;
		}

		public void Init(IAsmDealer _Store, IBlockDealer _DBserver)
		{
			Store = _Store;
			DBserver = _DBserver;
			typeList = _DBserver.GetAllBlockTypes();
		}

		public AttrsCollection GetUserAttrs(BlockType _bt)
		{
			return DBserver.GetAttrsCollection(_bt.BlockTypeID);
		}

		/// <summary>
		/// Возвращает тип блока из кэша
		/// </summary>
		public BlockType GetBlockTypeByNameKey(string NameKey)
		{
			return typeList.Where(o => o.NameKey == NameKey).FirstOrDefault();
		}

		/// <summary>
		/// Возвращает тип блока-Контейнера из кэша
		/// </summary>
		public BlockType GetBlockTypeDataContainer()
		{
			return typeList.Where(o => o.NameKey == containerTypeName).FirstOrDefault();
		}

	}
}
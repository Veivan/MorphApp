using System;

namespace Schemas
{
	public class Session
	{
		public static long MainStoreID = 1;
		public static string MainStoreName = "Хранилище";
		public static string DefaulContainerName = "Контейнер";
		public static string containerTypeName = "DataContainer";

		/// <summary>
		/// Возвращает тип блока - Контейнер
		/// </summary>
		/*	public BlockPlatform.BlockType ContainerType
			{
				get
				{
					BlockDBServer lowStore = new BlockDBServer();
					return name;
				}
			} */
	}
}

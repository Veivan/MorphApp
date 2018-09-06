using System;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет Блок.
	/// </summary>
	public class BlockBase
	{
		private BlockAddress _b_id;
		private BlockType _bt;
		private BlockAddress _parent = 0;
		private long _treeorder = 0;
		private BlockAddress _fh_id = 0;
		private BlockAddress _predecessor = 0;
		private BlockAddress _successor = 0;

		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress BlockID { get { return _b_id; } }

		/// <summary>
		/// Тип блока.
		/// </summary>
		public BlockType BlockType { get { return _bt; } }

		/// <summary>
		/// Адрес блока, являющегося Родителем для текущего блока.
		/// </summary>
		public BlockAddress ParentID { get { return _parent; } }

		/// <summary>
		/// Порядок следования блока в дереве.
		/// </summary>
		public long Order { get { return _treeorder; } }

		/// <summary>
		/// Адрес фактических данных блока.
		/// </summary>
		public BlockAddress FactID { get { return _fh_id; } }

		/// <summary>
		/// Поддержка версионности. Предшественник.
		/// </summary>
		public BlockAddress PredecessorID { get { return _predecessor; } }

		/// <summary>
		/// Поддержка версионности. Последователь.
		/// </summary>
		public BlockAddress SuccessorID { get { return _successor; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public BlockBase(BlockAddress b_id, BlockAddress bt, string btname, BlockAddress parent, long order, BlockAddress fh_id, BlockAddress predecessor, BlockAddress successor)
		{
			_b_id = b_id;
			_bt = new BlockType(bt, btname);
			_parent = parent;
			_treeorder = order;
			_fh_id = fh_id;
			_predecessor = predecessor;
			_successor = successor;
			/*	if (created_at == null)
					_created_at = DateTime.Now;
				else
					_created_at = (DateTime)created_at;
			} */
		}
	}
}

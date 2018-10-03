using System;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	/// <summary>
	/// Класс представляет Блок.
	/// </summary>
	public class BlockBase
	{
		protected BlockAddress b_id;
		protected BlockType bt;
		protected DateTime created_at;
		protected BlockAddress parent = 0;
		protected long treeorder = 0;
		protected BlockAddress fh_id = 0;
		protected BlockAddress predecessor = 0;
		protected BlockAddress successor = 0;

		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress BlockID { get { return b_id; } }

		/// <summary>
		/// Тип блока.
		/// </summary>
		public BlockType BlockType { get { return bt; } }

		/// <summary>
		/// Адрес блока, являющегося Родителем для текущего блока.
		/// </summary>
		public BlockAddress ParentID { get { return parent; } }

		/// <summary>
		/// Порядок следования блока в дереве.
		/// </summary>
		public long Order { get { return treeorder; } }

		/// <summary>
		/// Адрес фактических данных блока.
		/// </summary>
		public BlockAddress FactID { get { return fh_id; } }

		/// <summary>
		/// Поддержка версионности. Предшественник.
		/// </summary>
		public BlockAddress PredecessorID { get { return predecessor; } }

		/// <summary>
		/// Поддержка версионности. Последователь.
		/// </summary>
		public BlockAddress SuccessorID { get { return successor; } }

		/// <summary>
		/// Дата создания.
		/// </summary>
		public DateTime Created { get { return created_at; } }

		/// <summary>
		/// Конструктор
		/// </summary>
		public BlockBase(BlockAddress _b_id, BlockType _bt, BlockAddress _parent, long _order, 
			BlockAddress _fh_id, BlockAddress _predecessor, BlockAddress _successor, DateTime? _created_at = null)
		{
			b_id = _b_id;
			bt = _bt;
			parent = _parent;
			treeorder = _order;
			fh_id = _fh_id;
			predecessor = _predecessor;
			successor = _successor;
			if (_created_at == null)
				created_at = DateTime.Now;
			else
				created_at = (DateTime)_created_at;
		}
	}
}


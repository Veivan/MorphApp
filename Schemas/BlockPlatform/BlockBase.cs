using System;
using System.Collections.Generic;
using System.Linq;

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
		protected BlockAddress parent = 0;
		protected long treeorder = 0;
		protected BlockAddress fh_id = 0;
		protected BlockAddress predecessor = 0;
		protected BlockAddress successor = 0;
		protected AttrsCollection _UserAttrs;
		protected DateTime created_at;
		protected int isDeleted = 0;

		protected Blob blob;
		protected AttrsCollection _SysAttrs;

		#region Свойства
		/// <summary>
		/// Идентификатор БД.
		/// </summary>
		public BlockAddress BlockID { get { return b_id; } set { b_id = value; } }

		/// <summary>
		/// Тип блока.
		/// </summary>
		public BlockType BlockType { get { return bt; } }

		/// <summary>
		/// Адрес блока, являющегося Родителем для текущего блока.
		/// </summary>
		public BlockAddress ParentID { get { return parent; } set { parent = value; } }

		/// <summary>
		/// Порядок следования блока в дереве.
		/// </summary>
		public long Order { get { return treeorder; }  set { treeorder = value; } }

		/// <summary>
		/// Адрес фактических данных блока.
		/// </summary>
		public BlockAddress FactID { get { return fh_id; } set { fh_id = value; } }

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

		public int IsDeleted
		{
			get
			{
				return isDeleted;
			}

			set
			{
				isDeleted = value;
			}
		}

		/// <summary>
		/// Коллекция системных атрибутов.
		/// </summary>
		public AttrsCollection SysAttrs
		{
			get
			{
				return _SysAttrs;
			}
			/// TODO Сделать системный атрибут "ссылка на источник"
		}

		/// <summary>
		/// Коллекция пользовательских атрибутов.
		/// </summary>
		public AttrsCollection UserAttrs { get { return _UserAttrs; } }

		/// <summary>
		/// Двоичные данные атрибутов.
		/// </summary>
		public Blob Blob { get { return blob; } /*set { this.Blob = value; } */ }

		#endregion

		#region Constructors
		/// <summary>
		/// Конструктор
		/// </summary>
		public BlockBase(BlockAddress _b_id, BlockType _bt, BlockAddress _parent, long _order, 
			BlockAddress _fh_id, BlockAddress _predecessor, BlockAddress _successor, AttrsCollection _userAttrs, DateTime? _created_at = null)
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

			if (_userAttrs == null)
				this._UserAttrs = Session.Instance().GetUserAttrs(_bt);
			else
				this._UserAttrs = _userAttrs; 
		}

		/// <summary>
		/// Конструктор-копировщик
		/// </summary>
		public BlockBase(BlockBase srcBlock)
		{
			b_id = srcBlock.b_id;
			bt = srcBlock.bt;
			parent = srcBlock.parent;
			treeorder = srcBlock.treeorder;
			fh_id = srcBlock.fh_id;
			predecessor = srcBlock.predecessor;
			successor = srcBlock.successor;
			_UserAttrs = new AttrsCollection();
			var tplist = new List<enAttrTypes>();
			foreach (var attr in srcBlock.UserAttrs.Attrs.OrderBy(o => o.Order))
			{
				_UserAttrs.AddElement(attr);
				tplist.Add(attr.AttrType);
			}
			created_at = srcBlock.created_at;
			if (srcBlock.blob == null)
				this.blob = new Blob(tplist, null);
			else
				this.blob = new Blob(tplist, srcBlock.blob.Data);
		}

		#endregion

		/// <summary>
		/// Формирование атрибутов из блоба.
		/// </summary>
		public void PerformBlob(List<enAttrTypes> attrTypes, byte[] data)
		{
			this.blob = new Blob(attrTypes, data);
		}

		#region Methods

		public object GetValue(string attrNameKey, object defvalue = null)
		{
			var result = defvalue;
			if (_UserAttrs == null)
				return result;
			var attr = _UserAttrs.Attrs.Where(o => o.NameKey == attrNameKey).FirstOrDefault();
			if (attr != null && this.blob != null)
				return this.blob.GetAttrValue(attr.Order);
			else
				return result;
		}

		public void SetValue(string attrNameKey, object Value)
		{
			if (_UserAttrs != null)
			{
				var attr = _UserAttrs.Attrs.Where(o => o.NameKey == attrNameKey).FirstOrDefault();
				if (attr == null)
					return;
				if (this.blob == null)
				{
					var listFD = new List<AttrFactData>();
					foreach (var userAttr in _UserAttrs.Attrs.OrderBy(o => o.Order))
					{
						var factVal = userAttr.NameKey == attrNameKey ? Value : null;
						listFD.Add(new AttrFactData(userAttr.AttrType, factVal));
					}
					this.blob = new Blob(listFD);
				}
				this.blob.SetAttrValue(attr.Order, Value);
			}
		}

		public void Delete()
		{
			isDeleted = 1;
		}

		public void Recall()
		{
			isDeleted = 0;
		}

		#endregion

	}
}


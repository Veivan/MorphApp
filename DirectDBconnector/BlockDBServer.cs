using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Schemas;
using BlockAddress = System.Int64;

namespace DirectDBconnector
{
	/// <summary>
	/// Класс, реализющий интерфейс IBlockDealer - методы работы с блоками.
	/// Представляет API для работы с блоками.
	/// </summary>
	public class BlockDBServer : IBlockDealer
	{
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

		#region Функции для работы с Типами блоков
		public override BlockAddress CreateBlockType(string name)
		{
			BlockAddress result = dbConnector.dbCreateBlockType(name);
			return result;
		}

		public override BlockAddress GetBlockTypeByName(string name)
		{
			BlockAddress result = dbConnector.dbGetBlockTypeByName(name);
			return result;
		}

		public override string GetBlockTypeByAddr(BlockAddress addr)
		{
			string result = dbConnector.dbGetBlockTypeByAddr(addr);
			return result;
		}
		#endregion

		#region Функции для работы с атрибутами типов блоков
		public override BlockAddress CreateAttribute(string name, long AttrType, long BlockType, int sorder, bool mandatory = false)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Функции для работы с Блоками
		public override long CreateBlock(long BlockType, long parent, int treeorder)
		{
			BlockAddress result = dbConnector.dbCreateBlock(BlockType, parent, treeorder);
			return result;
		}

		public override Blob GetFactData(long addr)
		{
			throw new NotImplementedException();
		}

		public override int GetOrder(long addr)
		{
			int result = dbConnector.dbGetOrder(addr);
			return result;
		}

		public override long GetParent(long addr)
		{
			throw new NotImplementedException();
		}

		public override long SetFactData(long addr, Blob data, bool MakeVersion = false)
		{
			throw new NotImplementedException();
		}

		public override void SetOrder(long addr, int order)
		{
			throw new NotImplementedException();
		}

		public override void SetParent(long addr, long parent)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Функции для работы со Справочниками
		public override long CreateDictionary(string name, long BlockType)
		{
			throw new NotImplementedException();
		}

		public override long GetDictionaryByName(string name)
		{
			throw new NotImplementedException();
		}

		public override string GetDictName(long addr)
		{
			throw new NotImplementedException();
		}

		public override string GetDictType(long addr)
		{
			throw new NotImplementedException();
		}

		public override long[] GetDictContent(long addr)
		{
			throw new NotImplementedException();
		}

		public override void DictRemoveBlock(long addr, long subaddr)
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}

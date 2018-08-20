﻿using System;
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
			BlockAddress result = dbConnector.dbCreateAttribute(name, AttrType, BlockType, sorder, mandatory);
			return result;
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
			var result = dbConnector.dbGetOrder(addr);
			return result;
		}

		public override long GetParent(long addr)
		{
			throw new NotImplementedException();
		}

		public override long SetFactData(long addr, Blob blob, bool MakeVersion = false)
		{
			var result = dbConnector.dbSetFactData(addr, blob.Data, MakeVersion);
			return result;
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
		public override long CreateDictionary(string name, int BlockType)
		{
			var result = dbConnector.dbCreateDictionary(name, BlockType);
			return result;
		}

		public override long GetDictionaryByName(string name)
		{
			var result = dbConnector.dbGetDictionary(name);
			return result;
		}

		public override string GetDictName(long addr)
		{
			var result = dbConnector.dbGetDictName(addr);
			return result;
		}

		public override long GetDictType(long addr)
		{
			byte[] arbt = dbConnector.dbGetDictBlob(addr);
			var dDictBlob = new DictBlob(arbt);
			var result = dDictBlob.GetDictResolvedTypeFromBytes();
			return result;
		}

		public override long[] GetDictContent(long addr)
		{
			byte[] arbt = dbConnector.dbGetDictBlob(addr);
			var dDictBlob = new DictBlob(arbt);
			var result = dDictBlob.GetDictContentFromBytes();
			return result;
		}

		public override void DictAddBlock(long addr, long subaddr)
		{
		//	dbConnector.dbDictAddBlock(addr, subaddr);
			throw new NotImplementedException();
		}

		public override void DictRemoveBlock(long addr, long subaddr)
		{
			throw new NotImplementedException();
		}

		#endregion

	}
}

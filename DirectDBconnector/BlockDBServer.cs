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

		public override long GetBlockTypeByName(string name)
		{
			throw new NotImplementedException();
		}

		public override string GetBlockTypeByAddr(long addr)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Функции для работы с атрибутами типов блоков
		public override long CreateAttribute(string name, long AttrType, long BlockType, int sorder, bool mandatory = false)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Функции для работы с Блоками
		public override long CreateBlock(long BlockType, long parent, int treeorder)
		{
			throw new NotImplementedException();
		}

		public override Blob GetFactData(long addr)
		{
			throw new NotImplementedException();
		}

		public override int GetOrder(long addr)
		{
			throw new NotImplementedException();
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

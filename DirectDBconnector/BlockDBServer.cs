using System;
using System.Collections.Generic;
using System.Data;

using Schemas;
using Schemas.BlockPlatform;
using BlockAddress = System.Int64;

namespace DirectDBconnector
{
	/// <summary>
	/// Класс, реализющий интерфейс IBlockDealer - методы работы с блоками.
	/// Представляет API для работы с блоками напрямую в БД.
	/// </summary>
	public class BlockDBServer : IBlockDealer
	{
		SQLiteConnector dbConnector = SQLiteConnector.Instance;

		public BlockDBServer()
		{
		}

		#region Функции для работы с Типами блоков
		public override BlockType CreateBlockType(string nameKey, string nameUI)
		{
			BlockType result = dbConnector.dbCreateBlockType(nameKey, nameUI);
			return result;
		}

		public override BlockType GetBlockTypeByNameKey(string nameKey)
		{
			BlockType result = dbConnector.dbGetBlockTypeByNameKey(nameKey);
			return result;
		}

		public override BlockType GetBlockTypeByAddr(BlockAddress addr)
		{
			BlockType result = dbConnector.dbGetBlockTypeByAddr(addr);
			return result;
		}
		public override List<BlockType> GetAllBlockTypes()
		{
			return dbConnector.dbGetAllBlockTypes();
		}

		public override void BlockTypeChangeStrings(BlockType blocktype)
		{
			dbConnector.dbBlockTypeChangeStrings(blocktype);
		}


		#endregion

		#region Функции для работы с атрибутами типов блоков

		public override List<AttrType> GetAllAttrTypes()
		{
			List<AttrType> result = dbConnector.GetAllAttrTypes(); ;
			return result;
		}

		public override BlockAddress CreateAttribute(string nameKey, string nameUI, long AttrType, long BlockType, int sorder, bool mandatory = false)
		{
			BlockAddress result = dbConnector.dbCreateAttribute(nameKey, nameUI, AttrType, BlockType, sorder, mandatory);
			return result;
		}

		public override List<string> GetFildsNameKey(long BlockType)
		{
			var result = dbConnector.dbGetFildsNameKey(BlockType);
			return result;
		}

		public override AttrsCollection GetAttrsCollection(long BlockType)
		{
			return dbConnector.dbGetAttrsCollection(BlockType);
		}

		public override void AttributeUpdate(BlockAttribute attr)
		{
			dbConnector.AttributeUpdate(attr);
		}

		#endregion

		#region Функции для работы с Блоками
		public override long CreateBlock(long BlockType, long parent, int treeorder)
		{
			BlockAddress result = dbConnector.dbCreateBlock(BlockType, parent, treeorder);
			return result;
		}

		public override void SetParent(long addr, long parent)
		{
			throw new NotImplementedException();
		}

		public override BlockBase GetBlock(long addr)
		{
			BlockBase result = dbConnector.dbGetBlock(addr);
			return result;
		}

		public override long GetParent(long addr)
		{
			throw new NotImplementedException();
		}

		public override void SetOrder(long addr, int order)
		{
			throw new NotImplementedException();
		}

		public override int GetOrder(long addr)
		{
			var result = dbConnector.dbGetOrder(addr);
			return result;
		}

		public override void Delete(BlockAddress addr)
		{
			dbConnector.MarkBlock4Delete(addr, 1);
		}

		public override long SetFactData(long addr, Blob blob, bool MakeVersion = false)
		{
			if (blob == null)
				return -1;
			var result = dbConnector.dbSetFactData(addr, blob.Data, MakeVersion);
			return result;
		}

		public override Blob GetFactData(long addr)
		{
			var bytes = dbConnector.dbGetFactData(addr);
			var idtplist = dbConnector.dbGetAttrTypesList(addr);
			var tplist = new List<enAttrTypes>();
			foreach(var idtp in idtplist)
				tplist.Add((enAttrTypes)idtp);
			Blob blob = new Blob(tplist, bytes);
			return blob;
		}

		public override void AttrSetValue(long addr, int ord, object value)
		{
			byte[] bytes = dbConnector.dbGetFactData(addr);
			var idtplist = dbConnector.dbGetAttrTypesList(addr);
			var tplist = new List<enAttrTypes>();
			foreach (var idtp in idtplist)
				tplist.Add((enAttrTypes)idtp);
			Blob blob = new Blob(tplist, bytes);
			blob.SetAttrValue(ord, value);
			dbConnector.dbSetFactData(addr, blob.Data, false);
		}

		public override T AttrGetValue<T>(long addr, int ord)
		{
			byte[] bytes = dbConnector.dbGetFactData(addr);
			var idtplist = dbConnector.dbGetAttrTypesList(addr);
			var tplist = new List<enAttrTypes>();
			foreach (var idtp in idtplist)
				tplist.Add((enAttrTypes)idtp);
			Blob blob = new Blob(tplist, bytes);
			var attrval = blob.GetAttrValue(ord);
			if (attrval == null)
				return default(T);
			else
				return (T)attrval;
		}

		public override void AttrSetValue(long addr, string attrnamekey, object value)
		{
			byte[] bytes = dbConnector.dbGetFactData(addr);
			var attrs = dbConnector.dbGetAttrsCollection(addr);
			var tplist = attrs.GetAttrTypesList();
			var ord = attrs.GetOrdByNameKey(attrnamekey);
			Blob blob = new Blob(tplist, bytes);
			blob.SetAttrValue(ord, value);
			dbConnector.dbSetFactData(addr, blob.Data, false);
		}

		public override T AttrGetValue<T>(long addr, string attrnamekey)
		{
			byte[] bytes = dbConnector.dbGetFactData(addr);
			var attrs = dbConnector.dbGetAttrsCollection(addr);
			var tplist = attrs.GetAttrTypesList();
			var ord = attrs.GetOrdByNameKey(attrnamekey);
			Blob blob = new Blob(tplist, bytes);
			var attrval = blob.GetAttrValue(ord);
			if (attrval == null)
				return default(T);
			else
				return (T)attrval;
		}

		public override List<BlockBase> GetChildren(List<string> list_ids)
		{
			/// TODO нужно задавать входной параметр - тип детей. Сделать объект - фабрику и генерить детей с нужным типом.
			var result = dbConnector.dbGetChildren(list_ids);
			return result;
		}

		public override List<BlockBase> SearchBlocks(long blockType, Dictionary<string, object> args)
		{
			var result = dbConnector.dbSearchBlocks(blockType, args);
			return result;
		}
		#endregion

		#region Функции для работы со Справочниками
		public override long CreateDictionary(string name, int BlockType)
		{
			var result = dbConnector.dbCreateDictionary(name, BlockType);
			return result;
		}

		#endregion

	}
}

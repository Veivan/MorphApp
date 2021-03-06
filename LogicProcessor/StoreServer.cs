﻿using System.Collections.Generic;
using System.Linq;

using Schemas;
using Schemas.BlockPlatform;
using DirectDBconnector;
using BlockAddress = System.Int64;

namespace LogicProcessor
{
	/// <summary>
	/// Реализация интерфейса, обобщающего методы работы с хранилищем данных.
	/// Представляет API хранилища сборок, контейнеров и справочников.
	/// </summary>
	public class StoreServer : IAsmDealer
	{

		private BlockDBServer DBserver = new BlockDBServer();

		public StoreServer()
		{
			Session.Instance().Init(this, DBserver);
		}

		#region API для работы со сборками

		public override AssemblyBase CreateAssembly(BlockType type, BlockAddress ParentContID = -1)
		{
			var id = DBserver.CreateBlock(type.BlockTypeID, ParentContID, 0);
			var asm = new AssemblyBase(id, type);
			return asm;
		} 

		/// <summary>
		/// Создание сборки по образцу / шаблону.
		/// При создании новой сборки копируется структура (дочерние сборки) из образца / шаблона.
		/// </summary>
		/// <param name="templAsm">сборка - образец или шаблон</param>
		/// <param name="mode">признак, определяющий, сохранять ссылку на шаблон\образец или нет</param>
		/// <returns>сборка</returns>
		public AssemblyBase CreateAssembly(AssemblyBase templAsm, FOLLOWMODE mode = FOLLOWMODE.Forget)
		{
			var id = DBserver.CreateBlock(templAsm.BlockType.BlockTypeID, -1, 0);
			var asm = new AssemblyBase(templAsm, id, mode);
			CreateChildrenRequrs(asm.Children);
			return asm;
		}

		public override AssemblyBase GetAssembly(long Addr, bool FillChildren = true)
		{
			var block = DBserver.GetBlock(Addr);
			var asm = new AssemblyBase(block);
			if (FillChildren)
				this.AsmFillChildren(asm);
			return asm;
		}

		/// <summary>
		/// Заполнение сборки данными о детях.
		/// </summary>
		/// <param name="parentAsm">сборка</param>
		/// <returns></returns>
		public void AsmFillChildren(AssemblyBase parentAsm)
		{
			var list_ids = new List<string> { parentAsm.BlockID.ToString() };
			var listCh = this.GetChildren(list_ids);
			foreach (var child in listCh)
				parentAsm.AddChild(child);
		}

		/// <summary>
		/// Рекурсивное сохранение дочерних сборок в БД.
		/// </summary>
		/// <param name="src_children">перечень дочерних сборок</param>
		private void CreateChildrenRequrs(List<AssemblyBase> src_children)
		{
			foreach (var child in src_children)
			{
				var chid = DBserver.CreateBlock(child.BlockType.BlockTypeID, child.ParentID, (int)child.Order);
				//child.RootBlock_id = chid;
				CreateChildrenRequrs(child.Children);
			}
		}

		/// <summary>
		/// Получение дочерних сборок множества родительских сборок.
		/// Выбираются только прямые наследники (без ссылок)
		/// </summary>
		/// <param name="list_ids">список ID родительских сборок</param>
		/// <returns>DataTable</returns>
		public List<AssemblyBase> GetChildren(List<string> list_ids)
		{
			var listBlocks = DBserver.GetChildren(list_ids);
			var listAsm = new List<AssemblyBase>();
			foreach (var block in listBlocks)
			{
				var asm = new AssemblyBase(block);
				listAsm.Add(asm);
			}
			return listAsm;
		}

		public override void Add2SaveSet(AssemblyBase asm)
		{
			var saveSet = SetKeeper.Instance();
			saveSet.Add(asm);
		}

		public override void Save()
		{
			var saveSet = SetKeeper.Instance();
			if(saveSet.IsDirty)
			{
				var list = saveSet.GetSet();
				saveSet.Clear();
				//TODO В DBServer надо сделать функции с групповым входным параметром, а внутри их - транзакции
				foreach (var asm in list)
				{
					if (asm.IsVirtual)
					{
						var id = DBserver.CreateBlock(asm.BlockType.BlockTypeID, asm.ParentID, (int)asm.Order);
						asm.BlockID = id;
					}
					else
					{
						DBserver.SetParent(asm.BlockID, asm.ParentID);
						DBserver.SetOrder(asm.BlockID, (int)asm.Order);
					}
					var fh_id = DBserver.SetFactData(asm.BlockID, asm.Blob);
					asm.FactID = fh_id;
				}
			}

/*			if (asm.IsDeleted == 1)
				DBserver.Delete(asm.BlockID);
			DBserver.SetFactData(asm.BlockID, asm.Blob);*/
		}

		public override List<AssemblyBase> SearchAsms(long blockType, Dictionary<string, object> args)
		{
			var blocklist = DBserver.SearchBlocks(blockType, args);
			var result = new List<AssemblyBase>();
			foreach (var block in blocklist)
			{
				var asm = new AssemblyBase(block);
				result.Add(asm);
			}
			return result;
		}

		public override AssemblyBase GetLexema(long grenPart, string lemma, bool CreateIfNotExists = false)
		{
			var lexType = Session.Instance().GetBlockTypeByNameKey(Session.lexTypeName);
			var args = new Dictionary<string, object>();
			args.Add("GrenPart", grenPart);
			args.Add("Lemma", lemma.ToLower());

			var asm = this.SearchAsms(lexType.BlockTypeID, args).FirstOrDefault();
			if (asm == null && CreateIfNotExists)
			{
				asm = new AssemblyBase(lexType);
				asm.ParentID = Session.DictLemmsID;
			}
			if (asm != null)
			{
				asm.SetValue("Name", lemma);
				asm.SetValue("GrenPart", grenPart);
				asm.SetValue("Lemma", lemma);
				asm.Save();
			}
			return asm;
		}

		#endregion

		#region Методы работы с GREN
		public override List<SentenceMap> MorphMakeSyntan(string text)
		{
			//throw new NotImplementedException();
			return null;
		}

		public override List<string> MorphGetSeparatedSentsList(string text)
		{
			//throw new NotImplementedException();
			return null;
		}

		#endregion

	}
}

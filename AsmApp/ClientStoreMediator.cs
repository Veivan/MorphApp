using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

using Schemas; // Needs only for Session
using Schemas.BlockPlatform;
using LogicProcessor;
using AsmApp.Types;

using BlockAddress = System.Int64;

namespace AsmApp
{
	/// <summary>
	/// Класс-синглтон. 
	/// Класс поддерживает взаимодействие клиента с хранилищем данных SAGA (с LogicProcessor).
	/// </summary>
	public sealed class ClientStoreMediator
	{
		static object _sunc = new object();
		static ClientStoreMediator _session;

		// Список контейнеров для клиента
		public List<AssemblyBase> containers = new List<AssemblyBase>();

		Courier courier = new Courier();
		StoreServer store = new StoreServer();

		/// <summary>
		/// Конструктор
		/// </summary>
		private ClientStoreMediator()
		{
		}

		public static ClientStoreMediator Instance()
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
						_session = new ClientStoreMediator();
					}
				}
			}

			return _session;
		}

		#region Методы работы с хранилищем данных
		/// <summary>
		/// Заполнение внутреннего хранилища.
		/// </summary>
		public void Refresh()
		{
			containers.Clear();
			var MainStore = new AssemblyBase(Session.MainStoreID, Session.Instance().GetBlockTypeByNameKey(Session.containerTypeName));
			MainStore.IsMainDataContainer = true;
			store.AsmFillChildren(MainStore);
			containers.Add(MainStore);
		}

		public void RefreshAsm(AsmNode aNode)
		{
			//TODO обновить здесь и все остальные поля

			if (aNode.NodeType == clNodeType.clnDocument)
			{
				var nd = new DocumentAsm(aNode.Assembly);
				aNode.Assembly = nd;
			}
			store.AsmFillChildren(aNode.Assembly);
		}

		public AssemblyBase CreateContainer(string name, BlockAddress ParentID)
		{
			var containerType = Session.Instance().GetBlockTypeByNameKey(Session.containerTypeName);
			var asm = new AssemblyBase(containerType);
			asm.ParentAssemblyID = ParentID;
			asm.SetValue("Name", name);
			asm.Save();
			return asm;
		}

		public AssemblyBase CreateDocument(string name, BlockAddress ParentID)
		{
			var documentType = Session.Instance().GetBlockTypeByNameKey(Session.documentTypeName);
			var asm = new AssemblyBase(documentType);
			asm.ParentAssemblyID = ParentID;
			asm.SetValue("Name", name);
			asm.Save();
			return asm;
		}

		public AssemblyBase CreateDictionary(string name, BlockAddress ParentID)
		{
			var dictType = Session.Instance().GetBlockTypeByNameKey(Session.dictTypeName);
			var asm = new AssemblyBase(dictType);
			asm.ParentAssemblyID = ParentID;
			asm.SetValue("Name", name);
			asm.Save();
			return asm;
		}

		public AssemblyBase CreateLexema(long grenPart, string lemma)
		{
			var asm = store.GetLexema(grenPart, lemma, true);
			return asm;
		}

		public AssemblyBase CreateParagraph(BlockAddress ParentID)
		{
			var paragraphType = Session.Instance().GetBlockTypeByNameKey(Session.paragraphTypeName);
			var asm = new AssemblyBase(paragraphType);
			asm.ParentAssemblyID = ParentID;
			//asm.SetValue("Name", name);
			asm.Save();
			return asm;
		}

		/// <summary>
		/// Формирование содержимого внутреннего объекта ParagraphAsm.
		/// </summary>
		/// <param name="pAsm">объект ParagraphAsm</param>
		/// <param name="input">текстовое содержание абзаца</param>
		/// <returns></returns>
		public void UpdateParagraph(ParagraphAsm pAsm, string input, bool IsHeader)
		{
			var ihash = input.GetHashCode();
			int currenthash = pAsm.GetHashCode(IsHeader);
			var range = IsHeader ? SentTypes.enstNotActualHead : SentTypes.enstNotActualBody;
			var sentlist = pAsm.GetParagraphSents(range);
			if (ihash == currenthash && sentlist.Count == 0)
				return;

			var sents = this.MorphGetSeparatedSentsList(input);
			pAsm.RefreshParagraph(new ArrayList(sents), IsHeader);

			sentlist = pAsm.GetParagraphSents(range);
			if (sentlist.Count == 0)
				return;
			// Выполнение синтана для неактуальных предложений.
			foreach (var sent in sentlist)
			{
				var sentlistRep = this.MorphMakeSyntan(sent.sentence);
				if (sentlistRep.Count > 0)
					pAsm.UpdateSentStruct(sent.Order, Map2Asm.Convert(sentlistRep[0]) );
			} 
		}
		#endregion

		#region Методы работы с GREN

		/// <summary>
		/// Выполнение синтана текста.
		/// </summary>
		public List<SentenceMap> MorphMakeSyntan(string text)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Synt;
			courier.SendText(text);
			var sentlistRep = courier.GetSentenceStructList();
			return sentlistRep;
		}

		/// <summary>
		/// Получение списка восстановленных текстов предложений от сервиса.
		/// </summary>
		public List<string> MorphGetReparedSentsList(List<SentenceMap> sentlist)
		{
			var outlist = new List<string>();
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Repar;
			foreach (var sent in sentlist)
			{
				courier.SendStruct(sent);
				var sents = courier.GetSeparatedSentsList();
				outlist.AddRange(sents);
			}
			return outlist;
		}

		/// <summary>
		/// Разделение текста на предложения с помощью сервиса.
		/// </summary>
		public List<string> MorphGetSeparatedSentsList(string text)
		{
			courier.servType = TMorph.Schema.ServType.svMorph;
			courier.command = TMorph.Schema.ComType.Separ;
			courier.SendText(text);
			var outlist = courier.GetSeparatedSentsList();
			return outlist;
		}
		#endregion
		
	}
}

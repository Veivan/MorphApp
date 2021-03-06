﻿using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using Schemas;
using Schemas.BlockPlatform;

namespace AsmApp.Types
{

	/// <summary>
	/// Класс представляет Параграф, полученный из сборки SAGA.
	/// </summary>
	public class ParagraphAsm : AssemblyBase
	{
		#region Privates				
		private AssemblyBase srcAsm; // Сборка, из которой был сформирован Параграф
		/// <summary>
		/// Список предназначен для хранения предложений абзаца.
		/// </summary>
		private List<SentenceAsm> innerPara = new List<SentenceAsm>();
		#endregion

		#region Properties		
		#endregion

		#region Constructors
		public ParagraphAsm() : base(Session.Instance().GetBlockTypeByNameKey(Session.paragraphTypeName))
		{
			IsDirty = true;
		}

		public ParagraphAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			IsDirty = false;
			this.srcAsm = srcAsm;
			var store = Session.Instance().Store;
			var sentIDs = (List<long>)srcAsm.GetValue("Phrases");
			if (sentIDs != null)
				foreach (var sentID in sentIDs) 
				{
					var asm = store.GetAssembly(sentID, false);
					var sent = new SentenceAsm(asm);
					innerPara.Add(sent);
				}
		}

		#endregion

		#region Public Methods
		/// <summary>
		/// Возвращает предложение абзаца по порядку Position.
		/// Предложения заголовка имеют Position < 0;
		/// </summary>
		public string GetSentenseByOrder(int Position)
		{
			SentenceAsm sent = innerPara.Where(x => x.Order == Order).FirstOrDefault();
			return sent == null ? "" : sent.Text;
		}

		/// <summary>
		/// Добавление предложений из списка в хранилище абзаца.
		/// При добавлении вычисляется хэш предложения и по жэшу происходит поиск существующего предложения в хранилище.
		/// Если предложение есть, то структура переносится в новый список,
		/// если нет, то добавляется новая структура с признаком "Неактуальная".
		/// Для "Неактуальных" надо делать синтаксический анализ.
		/// По окончании новый список заменяет предыдущее содержание хранилища.
		/// Используется в UpdateParagraph при сохранении абзаца после редактирования в FormParaEdit.
		/// </summary>
		public void RefreshParagraph(ArrayList input, bool IsHeader)
		{
			List<SentenceAsm> versionPara = new List<SentenceAsm>();
			int i = 0;
			if (IsHeader) i = -1 * input.Count;
			foreach (var sent in input)
			{
				SentenceAsm newsprops;
				var ihash = sent.GetHashCode();
				var sentprops = innerPara.Where(x => x.hash.Equals(ihash) && (Belongs2Header(x) == IsHeader)).ToList();
				if (sentprops.Count == 0)
				{
					newsprops = new SentenceAsm();
					newsprops.Text = sent as string;
					newsprops.hash = ihash;
					newsprops.IsActual = false;
					newsprops.IsDirty = true;
					this.IsDirty = true;
				}
				else
				{
					newsprops = sentprops[0];
					newsprops.IsActual = true;
				}
				newsprops.Order = i;
				versionPara.Add(newsprops);
				i++;
			}

			if (! IsListsEqual(versionPara, IsHeader))
			{
				IsDirty = true;
				if (IsHeader)
					innerPara.RemoveAll(Belongs2Header);
				else
					innerPara.RemoveAll(Belongs2Body);
				innerPara.AddRange(versionPara);
			}

		}

		/// <summary>
		/// Запиcь в хранилище предложения новой структуры синтана этого предложения.
		/// Используется в UpdateParagraph при сохранении абзаца после редактирования в FormParaEdit.
		/// </summary>
		public void UpdateSentStruct(long Order, SentenceAsm sentstruct)
		{
			var sent = innerPara.Where(x => x.Order == Order).FirstOrDefault();
			if (sent != null)
				innerPara.Remove(sent);
			sentstruct.IsActual = true;
			sentstruct.Order = Order;
			innerPara.Add(sentstruct);
		}

		/// <summary>
		/// Получение списка структур предложений абзаца.
		/// </summary>
		/// <param name="sttype">диапазон выбираемых предложений</param>
		/// <returns>List of SentenceAsm</returns>
		public List<SentenceAsm> GetParagraphSents(SentTypes sttype = SentTypes.enstAll)
		{
			List<SentenceAsm> versionPara = new List<SentenceAsm>();
			switch (sttype)
			{
				case SentTypes.enstAll:
					versionPara.AddRange(innerPara);
					break;
				case SentTypes.enstNotActualHead:
					versionPara.AddRange(innerPara.Where(x => x.Order < 0 && !x.IsActual)
						.OrderBy(x => x.Order)
						.ToList());
					break;
				case SentTypes.enstNotActualBody:
					versionPara.AddRange(innerPara.Where(x => x.Order > -1 && !x.IsActual)
						.OrderBy(x => x.Order)
						.ToList());
					break;
				case SentTypes.enstHeader:
					versionPara.AddRange(innerPara.Where(x => x.Order < 0)
						.OrderBy(x => x.Order)
						.ToList());
					break;
				case SentTypes.enstBody:
					versionPara.AddRange(innerPara.Where(x => x.Order > -1)
						.OrderBy(x => x.Order)
						.ToList());
					break;
			}
			return versionPara;
		}

		/*// <summary>
		/// Обновление элемента хранения предложения.
		/// </summary>
		public void RefreshSentProp(string sentence, SentenceAsm sentstruct, bool IsActual)
		{
			var Position = sentstruct.Order;
			var sprop = innerPara.Where(x => x.Order == Position).FirstOrDefault();
			if (sprop == null)
			{
				innerPara.Add(sprop);
			}
			sprop.hash = sentence.GetHashCode();
			sprop.IsActual = true;
		} */

		public string GetHeader()
		{
			return String.Join(" ", this.GetParagraphSents(SentTypes.enstHeader)
								.Select(x => x.Text).ToList());
		}

		public string GetBody()
		{
			return String.Join(" ", this.GetParagraphSents(SentTypes.enstBody)
								.Select(x => x.Text).ToList());
		}

		public int GetHashCode(bool IsHeader)
		{
			string str;
			if (IsHeader)
				str = GetHeader();
			else
				str = GetBody();
			return str.GetHashCode();
		}

		public override void Save()
		{
			//Сохранение абзаца для получения ID
			if (this.IsVirtual)
				base.Save();
			var sentlist = new List<long>();
			foreach (var sent in innerPara) // where !sent.IsActual
			{
				sent.ParentID = this.RootBlock_id;
				sent.Save();
				sentlist.Add(sent.BlockID);
			}
			this.SetValue("Phrases", sentlist);
			base.Save();
		}

		#endregion

		#region Private Methods
		private static bool Belongs2Header(SentenceAsm p)
		{
			return p.Order < 0;
		}

		private static bool Belongs2Body(SentenceAsm p)
		{
			return p.Order > -1;
		}

		/// <summary>
		/// Сравнение двух списков на одинаковость.
		/// Сравнение идёт по количеству элементов и по одинаковости ID входящих предложений,
		/// упорядоченных по Order.
		/// Проверка выполняется 
		/// </summary>
		/// <param name="list4check">список предложений (новая версия абзаца)</param>
		/// <param name="IsHeader">признак, определяющий отношение предложений - к заголовку или содержимиму абзаца</param>
		/// <returns>bool</returns>
		private bool IsListsEqual(List<SentenceAsm> list4check, bool IsHeader)
		{
			bool result = true;
			var sentprops = innerPara.Where(x => (Belongs2Header(x) == IsHeader)).ToList();
			if (sentprops.Count != list4check.Count)
				return false;

			foreach (var sentN in list4check)
			{
				var sentP = sentprops.Where(x => x.Order == sentN.Order).FirstOrDefault();
				if (sentP == null || sentN.BlockID != sentP.BlockID)
					return false;
			}
			return result;
		}

		#endregion
	}
}

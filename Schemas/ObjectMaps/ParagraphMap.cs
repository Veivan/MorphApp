using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Schemas
{
	public class SentProps
	{
		public int order;
		public string sentence;
		public int hash;
		public bool IsActual;
		public SentenceMap sentstruct;
	}

	/// <summary>
	/// Класс представляет хранилище предложений одного абзаца.
	/// </summary>
	public class ParagraphMap
	{
		private long _pgID = -1;
		/// <summary>
		/// Идентификатор абзаца в БД.
		/// </summary>
		public long ParagraphID { get { return _pgID; } set { _pgID = value; } }

		/// <summary>
		/// Список предназначен для хранения предложений абзаца.
		/// </summary>
		private List<SentProps> innerPara = new List<SentProps>();

		/// <summary>
		/// Добавление предложений абзаца в хранилище.
		/// При добавлении вычисляется хэш предложения и по жэшу происходит поиск существующего предложения в хранилище.
		/// Если предложение есть, то структура переносится в новый список,
		/// если нет, то добавляется новая структура с признаком "Неактуальная".
		/// Для "Неактуальных" надо делать синтаксический анализ.
		/// По окончании новый список заменяет предыдущее содержание хранилища.
		/// </summary>
		public void RefreshParagraph(ArrayList input)
		{
			List<SentProps> versionPara = new List<SentProps>();

			int i = 0;
			foreach (var sent in input)
			{
				SentProps newsprops;
				var ihash = sent.GetHashCode();
				var sentex = innerPara.Where(x => x.hash.Equals(ihash)).ToList();
				if (sentex.Count == 0)
				{
					newsprops = new SentProps();
					newsprops.sentence = sent as string;
					newsprops.hash = ihash;
					newsprops.IsActual = false;
				}
				else
				{
					newsprops = sentex[0];
					newsprops.IsActual = true;
				}
				newsprops.order = i;
				versionPara.Add(newsprops);
			}

			innerPara.Clear();
			innerPara.AddRange(versionPara);
		}

		/// <summary>
		/// Получение копии списка предложений абзаца.
		/// </summary>
		public List<SentProps> GetParagraph(SentTypes sttype = SentTypes.enstAll)
		{
			List<SentProps> versionPara = new List<SentProps>();
			if (sttype == SentTypes.enstAll)
				versionPara.AddRange(innerPara);
			else
				versionPara = innerPara.Where(x => x.IsActual == false).ToList();
			return versionPara;
		}

		/// <summary>
		/// Запиcь в хранилище предложения новой структуры синтана этого предложения.
		/// </summary>
		public void UpdateSentStruct(int order, SentenceMap sentstruct)
		{
			var cnt = innerPara.Where(x => x.order == order).Count();
			if (cnt > 0)
			{
				var sent = innerPara[order];
				if (sent.sentstruct == null)
					sent.sentstruct = new SentenceMap();
				//sent.sentstruct = new SentenceMap(sentstruct);
				sent.IsActual = true;
			}
			/*
			var sent = innerPara.Where(x => x.order == order).FirstOrDefault();
			if (!String.IsNullOrEmpty(sent.sentence))
			{
				sent.sentstruct = sentstruct;
				sent.IsActual = true;
			}*/
		}

		/// <summary>
		/// Запиcь в хранилище предложения новой структуры синтана этого предложения.
		/// </summary>
		public void AddSentStruct(int order, SentenceMap sentstruct)
		{
			var sprop = new SentProps();
			sprop.order = order;
			sprop.sentstruct = new SentenceMap(sentstruct);
			innerPara.Add(sprop);
		}

	}
}

﻿using System;
using System.Collections.Generic;

using Schemas.BlockPlatform;
using BlockAddress = System.Int64;

namespace Schemas
{
	/// <summary>
	/// Класс, обобщающий методы работы с хранилищем данных.
	/// Представляет API хранилища сборок, контейнеров и справочников.
	/// Этот интерфейс нужен для возможности обращения к хранилищу из объекта Session.
	/// </summary>

	#region API для работы со сборками
	public abstract class IAsmDealer
	{

		/// <summary>
		/// Создание пустой сборки заданного типа.
		/// </summary>
		/// <param name="type">тип блока</param>
		/// <returns>сборка</returns>
		public abstract AssemblyBase CreateAssembly(BlockType type, BlockAddress ParentContID = -1);

		/// <summary>
		/// Сохранение сборки в БД.
		/// </summary>
		/// <param name="BlockType">адрес типа блока (объект справочника "Типы блоков")</param>
		/// <param name="parent">Поддержка дерева.Родитель.Это ссылка на блок, являющийся Родителем для блока.</param>
		/// <param name="treeorder">Поддержка дерева. Последовательность блока в списке детей Родителя (порядок блока в дереве).
		///		Можно задать явно.Можно задать = 0, тогда функция должна определить последний максимальный номер атрибута и
		///		присвоить новому атрибуту последний максимальный номер +1</param>
		/// <returns>адрес добавленного объекта</returns>
		public abstract void Save(AssemblyBase asm);
		#endregion

		#region Методы работы с GREN

		/// <summary>
		/// Выполнение синтана текста.
		/// </summary>
		public abstract List<SentenceMap> MorphMakeSyntan(string text);

		/// <summary>
		/// Получение списка восстановленных текстов предложений от сервиса.
		/// </summary>
//		public abstract List<string> MorphGetReparedSentsList(List<SentenceMap> sentlist);

		/// <summary>
		/// Разделение текста на предложения с помощью сервиса.
		/// </summary>
		public abstract List<string> MorphGetSeparatedSentsList(string text);

		#endregion

	}
}

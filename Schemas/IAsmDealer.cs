using System;
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
		/// Добаление объекта к набору для Сохранение в БД.
		/// </summary>
		/// <param name="asm">сборка</param>
		/// <returns></returns>
		public abstract void Add2SaveSet(AssemblyBase asm);

		/// <summary>
		/// Выполнение операции Сохранение в БД.
		/// </summary>
		/// <returns></returns>
		public abstract void Save();

		/// <summary>
		/// Поиск сборки по типу блока (если задан) и значениям атрибутов.
		/// Все значения атрибутов должны совпадать.
		/// </summary>
		/// <param name="blockType">тип блока</param>
		/// <param name="args">справочник аргументов поиска</param>
		/// <returns>список сборок</returns>
		public abstract List<AssemblyBase> SearchAsms(long blockType, Dictionary<string, object> args);

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

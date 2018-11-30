using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	public class ContainerBase : AssemblyBase
	{
		private string name;
		private ContainerBlob blob;
		private List<AssemblyBase> Content = new List<AssemblyBase>();

		/// <summary>
		/// Наименование контейнера
		/// </summary>
		public string Name { get { return name; } set { name = value; } }

		#region Конструкторы

		///TODO надо где-то иметь в кэше объект BlockType с типом Контейнер

		/// <summary>
		/// Конструктор. Создание пустого контейнера.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		public ContainerBase(string _name) :
			base(new BlockType(3, "", ""))
		{
			this.name = _name;
		}

		/// <summary>
		/// Конструктор. Создание контейнера из данных хранилища.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		/// <param name="id">адрес стартового блока сборки</param>
		public ContainerBase(BlockAddress id) :
			base(new BlockType(3, "", ""), id)
		{
			FillContent();
		}

		#endregion

		/// <summary>
		/// Метод заполняет содержимое списка Children значениями атрибута Content.
		/// </summary>
		private void FillContent()
		{
			var Content = UserAttrs["Content"];
			if (Content != null)
			{ }
		}
	}
}

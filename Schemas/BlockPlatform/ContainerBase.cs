using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	public class ContainerBase
	{
		private string name;
		private AssemblyBase assembly;
		protected List<AssemblyBase> content = new List<AssemblyBase>();

		/// <summary>
		/// Наименование контейнера
		/// </summary>
		public string Name { get { return name; } set { name = value; } }

		#region Конструкторы

		///TODO надо где-то иметь в кэше объект BlockType с типом Контейнер

		/// <summary>
		/// Конструктор. Создание пустого контейнера.
		/// </summary>
		/// <param name="_name">Наименование контейнера</param>
		public ContainerBase(string _name)
		{
			if (string.IsNullOrEmpty(_name))
				_name = Session.DefaulContainerName;
			this.name = _name;
		}

		/// <summary>
		/// Конструктор. Создание контейнера из сборки.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		public ContainerBase(AssemblyBase _assembly)
		{
			if (_assembly == null) {
				this.assembly = new AssemblyBase(new BlockType(3, "", ""));
				this.name = Session.DefaulContainerName;
			}
			else
			{
				this.assembly = _assembly;
				_assembly.
				//_assembly.UserAttrs
			}

		}

		/*// <summary>
		/// Конструктор. Создание контейнера из данных хранилища.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		/// <param name="id">адрес стартового блока сборки</param>
		public ContainerBase(BlockAddress id) :
			base(new BlockType(3, "", ""), id)
		{
		}*/

		#endregion

		/// <summary>
		/// Метод заполняет содержимое списка Children значениями атрибута Content.
		/// </summary>
		private void FillBlob()
		{
			//var Content = UserAttrs["Content"];
			if (Content != null)
			{ }
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BlockAddress = System.Int64;

namespace Schemas.BlockPlatform
{
	public class ContainerBase2 : AssemblyBase
	{
		private string name;

		#region Свойства
		/// <summary>
		/// Наименование контейнера
		/// </summary>
		public string Name { get { return name; } set { name = value; } }

		#endregion

		#region Конструкторы

		///TODO надо где-то иметь в кэше объект BlockType с типом Контейнер

		/// <summary>
		/// Конструктор. Создание пустого контейнера.
		/// </summary>
		/// <param name="_name">Наименование контейнера</param>
		public ContainerBase2(string _name = null) : base(new BlockType(3, "", ""))
		{
			if (string.IsNullOrEmpty(_name))
				_name = Session.DefaulContainerName;
			this.name = _name;
		}

		/// <summary>
		/// Конструктор. Создание контейнера из сборки.
		/// </summary>
		/// <param name="blockType">тип сборки</param>
		public ContainerBase2(AssemblyBase _assembly) : base(_assembly)
		{
			if (_assembly == null) {
				this.name = Session.DefaulContainerName;
			}
			else
			{
				this.name = (string)_assembly.GetValue("Name");
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


	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsmApp
{
	/// <summary>
	/// Тип узла в клиентском дереве отображения объектов хранилища.
	/// </summary>
	public enum clNodeType { clnContainer, clnDocument, clnParagraph };

	/// <summary>
	/// Тип операции в клиентском дереве.
	/// </summary>
	public enum treeOper { toAdd, toEdit, toDelete };
}

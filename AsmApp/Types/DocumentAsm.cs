using System.Collections.Generic;

using Schemas.BlockPlatform;
using BlockAddress = System.Int64;

namespace AsmApp.Types
{

	/// <summary>
	/// Класс представляет Документ, полученный из сборки SAGA.
	/// </summary>
	public class DocumentAsm : AssemblyBase
	{
		#region Privates				
		private AssemblyBase srcAsm; // Сборка, из которой был сформирован Документ

		/// <summary>
		/// Список предназначен для хранения ссылок на абзацы других документов.
		/// </summary>
		private List<BlockAddress> ParagraphLinks = new List<BlockAddress>();
		#endregion

		#region Конструкторы
		public DocumentAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
			var attrval = (List <BlockAddress> )srcAsm.GetValue("ParagraphLinks");
			if (attrval == null) return;
			ParagraphLinks.AddRange(attrval);
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Получение списка ссылок на абзацы других Документов, задействованных в эток Документе.
		/// </summary>
		/// <returns>List of BlockAddress</returns>
		public List<BlockAddress> GetParagraphsLinks()
		{
			var versionDoc = new List<BlockAddress>();
			versionDoc.AddRange(ParagraphLinks);
			return versionDoc;
		}
		#endregion
	}
}

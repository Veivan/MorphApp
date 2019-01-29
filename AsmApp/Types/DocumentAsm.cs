using System.Collections.Generic;

using Schemas.BlockPlatform;

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
		/// Список предназначен для хранения параграфов Документа.
		/// </summary>
		private List<ParagraphAsm> paragraphs = new List<ParagraphAsm>();
		#endregion

		#region Конструкторы
		public DocumentAsm(AssemblyBase srcAsm) : base(srcAsm)
		{
			this.srcAsm = srcAsm;
			var attrval = (List <AssemblyBase> )srcAsm.GetValue("Paragraphs");
			if (attrval == null) return;
			foreach (var item in attrval)
				paragraphs.Add(new ParagraphAsm(item));
		}
		#endregion

		#region Public Methods
		/// <summary>
		/// Получение списка абзацев Документа.
		/// </summary>
		/// <returns>List of ParagraphAsm</returns>
		public List<ParagraphAsm> GetParagraphs()
		{
			var versionDoc = new List<ParagraphAsm>();
			versionDoc.AddRange(paragraphs);
			return versionDoc;
		}
		#endregion
	}
}

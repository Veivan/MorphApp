using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using dbMQserver;
using Schemas;

namespace DirectDBA
{
	public partial class MainForm : Form
	{
		SQLiteConnector dbConnector = SQLiteConnector.Instance;
		public MainForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			#region Создание колонок для Документов
			dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "DocumentID", 
				HeaderText = "doc_id"
			}); 
			dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "ContainerID",
				HeaderText = "ct_id"
			});
			dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "Created_at",
				HeaderText = "Создан"
			});
			dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "name",
				HeaderText = "name"
			});
			#endregion
		}

		/// <summary>
		/// Пример чтения данных напрямую из БД.
		/// </summary>
		private void ReadDocsDirectExample()
		{
			DataTable dTable = dbConnector.dirCmd.GetDocumentsT();
			if (dTable.Rows.Count > 0)
			{
				dgvViewer.Columns.Clear();
				dgvViewer.Columns.Add(new DataGridViewTextBoxColumn
				{
					DataPropertyName = "sp_id",
					HeaderText = "Заголовок 1"
				});
				dgvViewer.Columns.Add(new DataGridViewTextBoxColumn
				{
					DataPropertyName = "speechpart",
					HeaderText = "Заголовок 2"
				});

				dgvViewer.Rows.Clear();
				dgvViewer.AutoGenerateColumns = true;

				for (int i = 0; i < dTable.Rows.Count; i++)
					dgvViewer.Rows.Add(dTable.Rows[i].ItemArray);
			}
			else
				MessageBox.Show("Database is empty");
		}

		/// <summary>
		/// Пример чтения данных из сформированного списка из БД.
		/// </summary>
		private void ReadDocsFromList()
		{
			var dTable = dbConnector.dirCmd.GetDocumentsL();
			var docs = new BindingList<DocumentMap>();
			foreach (var rec in dTable)
			{
				docs.Add(new DocumentMap(rec.DocumentID, rec.ContainerID, rec.Name, rec.Created_at));
			}

			bsDocuments.DataSource = docs;
			/*binding1.DataSource = docs;
			dgvViewer.DataSource = binding1;
			dgvViewer.AutoGenerateColumns = true; */
		}

		/// <summary>
		/// Пример заполнения binding из коллекции объектов.
		/// </summary>
		private void FillBindingFromCollection()
		{
			var nodes = new BindingList<mmNode>();
			for (int i = 0; i < 3; i++)
			{
				nodes.Add(new mmNode(i.ToString(), i * 2));
			}

			binding1.DataSource = nodes;
			listBox1.DataSource = binding1;
			listBox1.DisplayMember = "name";

			dgvViewer.DataSource = binding1;
			dgvViewer.AutoGenerateColumns = true;
		}

		public class mmNode
		{
			public string name { get; set; }
			public int Level { get; set; }
			public mmNode(string p, int p_2)
			{
				this.name = p;
				this.Level = p_2;
			}
		}

		private void btSelect_Click(object sender, EventArgs e)
		{
			//ReadDocsDirectExample();
			ReadDocsFromList();

			//FillBindingFromCollection();
		}


		private void btRefreshDocuments_Click(object sender, EventArgs e)
		{
			ReadDocsDirect();
			//ReadDocsFromList();
		}

		/// <summary>
		/// Чтения Документов напрямую из БД.
		/// </summary>
		private void ReadDocsDirect()
		{
			DataTable dTable = dbConnector.dirCmd.GetDocumentsT();
			bsDocuments.DataSource = dTable;
			//dgvDocuments.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

			/*if (dTable.Rows.Count > 0)
			{
				dgvDocuments.Rows.Clear();
				//dgvDocuments.AutoGenerateColumns = true;
				for (int i = 0; i < dTable.Rows.Count; i++)
					dgvDocuments.Rows.Add(dTable.Rows[i].ItemArray);
			}
			else
				MessageBox.Show("Database is empty"); */
		}

		private void navCommit_Click(object sender, EventArgs e)
		{
			//dbConnector.dirCmd.UpdateDocumentsT((DataTable)bsDocuments.DataSource);

			dbConnector.dirCmd.UpdateDocuments(bsDocuments.DataSource);
		}

	}
}

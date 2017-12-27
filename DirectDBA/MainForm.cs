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
		}

		/// <summary>
		/// Пример чтения данных напрямую из БД.
		/// </summary>
		private void ReadDocsDirect()
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
				docs.Add(new DocumentMap(rec.DocumentID, rec.ContainerID, rec.Name, rec.Created));
			}

			binding1.DataSource = docs;
			dgvViewer.DataSource = binding1;
			dgvViewer.AutoGenerateColumns = true;
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
			public string name {get;set;}
			public int Level {get;set;}
			public mmNode(string p, int p_2)
			{
				this.name = p;
				this.Level = p_2;
			}
		}

		private void btSelect_Click(object sender, EventArgs e)
		{
			//ReadDocsDirect();
			ReadDocsFromList();

			//FillBindingFromCollection();
		}


		private void btRefreshDocuments_Click(object sender, EventArgs e)
		{
			//ReadDocsDirect();
		}

             
            /*    DataTable dTable = new DataTable();
     String sqlQuery;

     if (m_dbConn.State != ConnectionState.Open)
     {
         MessageBox.Show("Open connection with database");
         return;
     }
    
     try
     {
         sqlQuery = "SELECT * FROM Catalog";
         SQLiteDataAdapter adapter = new SQLiteDataAdapter(sqlQuery, m_dbConn);
         adapter.Fill(dTable);

         if (dTable.Rows.Count > 0)
         {
             dgvViewer.Rows.Clear();

             for (int i = 0; i < dTable.Rows.Count; i++)
                 dgvViewer.Rows.Add(dTable.Rows[i].ItemArray);
         }
         else
             MessageBox.Show("Database is empty");
     }
     catch (SQLiteException ex)
     {               
         MessageBox.Show("Error: " + ex.Message);
     }           


		 }*/
	}
}

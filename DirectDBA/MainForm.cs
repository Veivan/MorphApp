using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using DirectDBconnector;
using Schemas;

namespace DirectDBA
{
	public partial class MainForm : Form
	{
		SagaDBServer dbServer = new SagaDBServer();

		SQLiteConnector dbConnector = SQLiteConnector.Instance;
		InnerStore store = new InnerStore();

		public MainForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			#region Создание колонок для Контейнеров
			dgvContainers.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "ct_id",
				HeaderText = "ct_id"
			});
			dgvContainers.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "parent_id",
				HeaderText = "parent_id"
			});
			dgvContainers.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "Created_at",
				HeaderText = "Создан"
			});
			dgvContainers.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "name",
				HeaderText = "name"
			});
			#endregion

			#region Создание колонок для Документов
			dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "doc_id",
				HeaderText = "doc_id"
			});
			dgvDocuments.Columns.Add(new DataGridViewTextBoxColumn
			{
				DataPropertyName = "ct_id",
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

		private void btRefreshContainers_Click(object sender, EventArgs e)
		{
			ReadContsDirect();
		}

		private void btRefreshDocuments_Click(object sender, EventArgs e)
		{
			ReadDocsDirect();
			//ReadDocsFromList();
		}

		/// <summary>
		/// Чтения Контейнеров напрямую из БД.
		/// </summary>
		private void ReadContsDirect()
		{
			DataTable dTable = dbConnector.dirCmd.GetDataTable(dbTables.tblContainers);
			bsContainers.DataSource = dTable;
		}

		/// <summary>
		/// Чтения Документов напрямую из БД.
		/// </summary>
		private void ReadDocsDirect()
		{
			DataTable dTable = dbConnector.dirCmd.GetDataTable(dbTables.tblDocuments);
			bsDocuments.DataSource = dTable;
		}

		private void navUpdate_Click(object sender, EventArgs e)
		{
			switch ((sender as ToolStripButton).Name)
			{
				case "btUpdDocuments":
					dbConnector.dirCmd.UpdateDataTable((DataTable)bsDocuments.DataSource, dbTables.tblDocuments);
					break;
				case "btUpdContainers":
					dbConnector.dirCmd.UpdateDataTable((DataTable)bsContainers.DataSource, dbTables.tblContainers);
					break;
			}
		}

		private void btRefreshTree_Click(object sender, EventArgs e)
		{
			var dTable = dbConnector.dirCmd.GetChildrenContainers(Session.MainStroreID);
			store.FillContainers(dTable);

			var list_ids = new List<string>();
			for (int i = 0; i < dTable.Rows.Count; i++)
			{
				var strID = dTable.Rows[i].Field<long>("ct_id");
				list_ids.Add(strID.ToString());
			}
			dTable = dbConnector.dirCmd.GetDocsInContainerList(list_ids);
			store.FillDocs(dTable);
			PopulateTreeView();
		}

		private void PopulateTreeView()
		{
			TreeNode rootNode;
			treeView1.Nodes.Clear();

			rootNode = new TreeNode("Хранилище");
			foreach (var cont in store.containers)
			{
				var aNode = new TreeNode(cont.Name, 0, 0);
				aNode.Tag = cont.ContainerID;
				PopulateTreeDocuments(cont, aNode);
				rootNode.Nodes.Add(aNode);
			}
			treeView1.Nodes.Add(rootNode);
		}

		private void PopulateTreeDocuments(ContainerNode container, TreeNode nodeToAddTo)
		{
			var docs = container.GetDocuments();
			foreach (var doc in docs)
			{
				var aNode = new TreeNode(doc.Name, 0, 0);
				aNode.Tag = doc.ContainerID;
				nodeToAddTo.Nodes.Add(aNode);
			}
		}

		private void GetDocumentsFromDB(long containerID, TreeNode nodeToAddTo)
		{
			//			 if (subSubDirs.Length != 0)
			// GetDocsInContainer
		}

		private void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			//TODO Загружать документы из БД, если не загружены
			//GetDocumentsFromDB

			//if (!HasCheckedChildNodes(e.Node)) e.Cancel = true;

			//MessageBox.Show("qq");

		}

		#region Примеры

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
		#endregion



	}
}

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
		SQLiteConnector dbConnector = SQLiteConnector.Instance;
		SagaDBServer dbServer = new SagaDBServer();
        string[] stables = { "Контейнеры", "Документы", "Абзацы", "Предложения", "Содержание фраз", "Леммы",
            "Граммемы", "Синт.связи", "Формы слов", "Термины", "Undefs",
			"Типы блоков", "Типы атрибутов", "Атрибуты", "Блоки", "Факты", "Справочники"};

        private dbTables _aspect; 
        public dbTables ActiveAspect
        {
            get { return _aspect; }
            set
            {
                _aspect = value;
                RefreshColumnSet();
                RefreshDS();
                tabControl1.TabPages[0].Text = stables[(int)_aspect];
            }
        }

        public MainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dgvCommon.DataSource = binding1;
            navigator.BindingSource = binding1;
            cbTables.Items.AddRange(stables);
            cbTables.SelectedIndex = 0;
        }

        private void cbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            ActiveAspect = (dbTables)((sender as ComboBox).SelectedIndex);
        }

        private void RefreshColumnSet()
        {
            dgvCommon.Columns.Clear();
            switch (ActiveAspect)
            {
                case dbTables.tblContainers:
                    #region Создание колонок для Контейнеров
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ct_id",
                        HeaderText = "ct_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "parent_id",
                        HeaderText = "parent_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Created_at",
                        HeaderText = "Создан"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "name",
                        HeaderText = "name"
                    });
                    #endregion
                    break;
                case dbTables.tblDocuments:
                    #region Создание колонок для Документов
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "doc_id",
                        HeaderText = "doc_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ct_id",
                        HeaderText = "ct_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Created_at",
                        HeaderText = "Создан"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "name",
                        HeaderText = "name"
                    });
                    #endregion
                    break;
                case dbTables.tblParagraphs:
                    #region Создание колонок для Абзацев
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "pg_id",
                        HeaderText = "pg_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "doc_id",
                        HeaderText = "doc_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Created_at",
                        HeaderText = "Создан"
                    });
                    #endregion
                    break;
                case dbTables.tblSents:
                    #region Создание колонок для Предложений
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ph_id",
                        HeaderText = "ph_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "pg_id",
                        HeaderText = "pg_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "sorder",
                        HeaderText = "sorder"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "Created_at",
                        HeaderText = "Создан"
                    });
                    #endregion
                    break;
                case dbTables.tblPhraseContent:
                    #region Создание колонок для Содержимого Предложений
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "с_id",
                        HeaderText = "с_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ph_id",
                        HeaderText = "ph_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "lx_id",
                        HeaderText = "lx_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "sorder",
                        HeaderText = "sorder"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "rcind",
                        HeaderText = "rcind"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "rw_id",
                        HeaderText = "rw_id"
                    });
                    #endregion
                    break;
                case dbTables.tblLemms:
                    #region Создание колонок для Лемм
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "lx_id",
                        HeaderText = "lx_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "sp_id",
                        HeaderText = "sp_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "lemma",
                        HeaderText = "lemma"
                    });
                    #endregion
                    break;
                case dbTables.tblGrammems:
                    #region Создание колонок для Граммем
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "gr_id",
                        HeaderText = "gr_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "с_id",
                        HeaderText = "с_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "sg_id",
                        HeaderText = "sg_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "intval",
                        HeaderText = "intval"
                    });
                    #endregion
                    break;
                case dbTables.tblSyntNodes:
                    #region Создание колонок для Синт.узлов
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "sn_id",
                        HeaderText = "sn_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "с_id",
                        HeaderText = "с_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "ln_id",
                        HeaderText = "ln_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "level",
                        HeaderText = "level"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "pс_id",
                        HeaderText = "pс_id"
                    });
                    #endregion
                    break;
                case dbTables.tblRealWord:
                    #region Создание колонок для Форм слов
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "rw_id",
                        HeaderText = "rw_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "wform",
                        HeaderText = "wform"
                    });
                    #endregion
                    break;
                case dbTables.tblTermContent:
                    #region Создание колонок для Терминов
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "tc_id",
                        HeaderText = "tc_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "tm_id",
                        HeaderText = "tm_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "sorder",
                        HeaderText = "sorder"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "rcind",
                        HeaderText = "rcind"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "lem_id",
                        HeaderText = "lem_id"
                    });
                    #endregion
                    break;
                case dbTables.tblUndefContent:
                    #region Создание колонок для Undefs
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "uv_id",
                        HeaderText = "uv_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "mu_id",
                        HeaderText = "mu_id"
                    });
                    dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
                    {
                        DataPropertyName = "rw_id",
                        HeaderText = "rw_id"
                    });
                    #endregion
                    break;

				case dbTables.mBlockTypes:
					#region Создание колонок для Типов блоков
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "bt_id",
						HeaderText = "bt_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "namekey",
						HeaderText = "namekey"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "nameui",
						HeaderText = "nameui"
					});
					#endregion
					break;
				case dbTables.mAttrTypes:
					#region Создание колонок для Типов атрибутов
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "mt_id",
						HeaderText = "mt_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "name",
						HeaderText = "name"
					});
					#endregion
					break;
				case dbTables.mAttributes:
					#region Создание колонок для Атрибутов
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "ma_id",
						HeaderText = "ma_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "namekey",
						HeaderText = "namekey"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "nameui",
						HeaderText = "nameui"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "mt_id",
						HeaderText = "mt_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "bt_id",					
						HeaderText = "bt_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "sorder",
						HeaderText = "sorder"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "mandatory",
						HeaderText = "mandatory"
					});
					#endregion
					break;
				case dbTables.mBlocks:
					#region Создание колонок для Блоков 
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "b_id",
						HeaderText = "b_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "bt_id",
						HeaderText = "bt_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "created_at",
						HeaderText = "created_at"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "parent",
						HeaderText = "parent"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "treeorder",
						HeaderText = "treeorder"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "fh_id",
						HeaderText = "fh_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "predecessor",
						HeaderText = "predecessor"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "successor",
						HeaderText = "successor"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "deleted",
						HeaderText = "deleted"
					});
					#endregion
					break;
				case dbTables.mFactHeap:
					#region Создание колонок для Фактических данных
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "fh_id",
						HeaderText = "fh_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "blockdata",
						HeaderText = "blockdata"
					});
					#endregion
					break;
				case dbTables.mDicts:
					#region Создание колонок для Справочников
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "md_id",
						HeaderText = "md_id"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "name",
						HeaderText = "name"
					});
					dgvCommon.Columns.Add(new DataGridViewTextBoxColumn
					{
						DataPropertyName = "b_id",
						HeaderText = "b_id"
					});
					#endregion
					break;

			}
		}

        private void btRefresh_Click(object sender, EventArgs e)
        {
            RefreshDS();
        }

        private void RefreshDS()
        {
            var retval = dbServer.ReadDataTable(ActiveAspect);
            BindingSource ds = (BindingSource)dgvCommon.DataSource;
            ds.DataSource = retval.dtable;
        }

        private void navUpdate_Click(object sender, EventArgs e)
        {
            BindingSource ds = (BindingSource)dgvCommon.DataSource;
            dbServer.UpdateDataTable((DataTable)ds.DataSource, ActiveAspect);
        }

        #region Примеры

        /// <summary>
        /// Пример чтения данных из сформированного списка из БД.
        /// </summary>
        private void ReadDocsFromList()
        {
            List<DocumentMap> dTable = null; //dbConnector.dirCmd.GetDocumentsL();
            var docs = new BindingList<DocumentMap>();
            foreach (var rec in dTable)
            {
                docs.Add(new DocumentMap(rec.DocumentID, rec.ContainerID, rec.Name, rec.Created_at));
            }

            //bsDocuments.DataSource = docs;
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

		private void AddColumn_Click(object sender, EventArgs e)
		{
			dbConnector.AddColumn();
		}

		private void DropColumn_Click(object sender, EventArgs e)
		{
			dbConnector.DropColumn();
		}

		private void RenameColumn_Click(object sender, EventArgs e)
		{
			dbConnector.RenameColumn();
		}
	}
}

namespace DirectDBA
{
	partial class MainForm
	{
		/// <summary>
		/// Требуется переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Обязательный метод для поддержки конструктора - не изменяйте
		/// содержимое данного метода при помощи редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabTree = new System.Windows.Forms.TabPage();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.panel2 = new System.Windows.Forms.Panel();
			this.btRefreshTree = new System.Windows.Forms.Button();
			this.dgvViewer = new System.Windows.Forms.DataGridView();
			this.listBox1 = new System.Windows.Forms.ListBox();
			this.tabPageContainers = new System.Windows.Forms.TabPage();
			this.dgvContainers = new System.Windows.Forms.DataGridView();
			this.bsContainers = new System.Windows.Forms.BindingSource(this.components);
			this.panel3 = new System.Windows.Forms.Panel();
			this.navContainers = new System.Windows.Forms.BindingNavigator(this.components);
			this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
			this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
			this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripTextBox1 = new System.Windows.Forms.ToolStripTextBox();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
			this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.btUpdContainers = new System.Windows.Forms.ToolStripButton();
			this.btRefreshContainers = new System.Windows.Forms.Button();
			this.tabDocuments = new System.Windows.Forms.TabPage();
			this.dgvDocuments = new System.Windows.Forms.DataGridView();
			this.bsDocuments = new System.Windows.Forms.BindingSource(this.components);
			this.panel1 = new System.Windows.Forms.Panel();
			this.btRefreshDocuments = new System.Windows.Forms.Button();
			this.navDocuments = new System.Windows.Forms.BindingNavigator(this.components);
			this.bindingNavigatorAddNewItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorCountItem = new System.Windows.Forms.ToolStripLabel();
			this.bindingNavigatorDeleteItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorMoveFirstItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorMovePreviousItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.bindingNavigatorPositionItem = new System.Windows.Forms.ToolStripTextBox();
			this.bindingNavigatorSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.bindingNavigatorMoveNextItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorMoveLastItem = new System.Windows.Forms.ToolStripButton();
			this.bindingNavigatorSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.btUpdDocuments = new System.Windows.Forms.ToolStripButton();
			this.binding1 = new System.Windows.Forms.BindingSource(this.components);
			this.tabControl1.SuspendLayout();
			this.tabTree.SuspendLayout();
			this.panel2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvViewer)).BeginInit();
			this.tabPageContainers.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvContainers)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsContainers)).BeginInit();
			this.panel3.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.navContainers)).BeginInit();
			this.navContainers.SuspendLayout();
			this.tabDocuments.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.dgvDocuments)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.bsDocuments)).BeginInit();
			this.panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.navDocuments)).BeginInit();
			this.navDocuments.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.binding1)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabTree);
			this.tabControl1.Controls.Add(this.tabPageContainers);
			this.tabControl1.Controls.Add(this.tabDocuments);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(825, 515);
			this.tabControl1.TabIndex = 0;
			// 
			// tabTree
			// 
			this.tabTree.Controls.Add(this.treeView1);
			this.tabTree.Controls.Add(this.panel2);
			this.tabTree.Controls.Add(this.dgvViewer);
			this.tabTree.Controls.Add(this.listBox1);
			this.tabTree.Location = new System.Drawing.Point(4, 22);
			this.tabTree.Name = "tabTree";
			this.tabTree.Padding = new System.Windows.Forms.Padding(3);
			this.tabTree.Size = new System.Drawing.Size(817, 489);
			this.tabTree.TabIndex = 1;
			this.tabTree.Text = "Хранилище";
			this.tabTree.UseVisualStyleBackColor = true;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
			this.treeView1.Location = new System.Drawing.Point(3, 3);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(324, 426);
			this.treeView1.TabIndex = 18;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.btRefreshTree);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel2.Location = new System.Drawing.Point(3, 429);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(811, 57);
			this.panel2.TabIndex = 17;
			// 
			// btRefreshTree
			// 
			this.btRefreshTree.Location = new System.Drawing.Point(5, 15);
			this.btRefreshTree.Name = "btRefreshTree";
			this.btRefreshTree.Size = new System.Drawing.Size(75, 23);
			this.btRefreshTree.TabIndex = 14;
			this.btRefreshTree.Text = "Refresh";
			this.btRefreshTree.UseVisualStyleBackColor = true;
			this.btRefreshTree.Click += new System.EventHandler(this.btRefreshTree_Click);
			// 
			// dgvViewer
			// 
			this.dgvViewer.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvViewer.Location = new System.Drawing.Point(333, 6);
			this.dgvViewer.Name = "dgvViewer";
			this.dgvViewer.Size = new System.Drawing.Size(476, 288);
			this.dgvViewer.TabIndex = 16;
			// 
			// listBox1
			// 
			this.listBox1.FormattingEnabled = true;
			this.listBox1.Location = new System.Drawing.Point(673, 328);
			this.listBox1.Name = "listBox1";
			this.listBox1.Size = new System.Drawing.Size(120, 95);
			this.listBox1.TabIndex = 15;
			this.listBox1.Visible = false;
			// 
			// tabPageContainers
			// 
			this.tabPageContainers.Controls.Add(this.dgvContainers);
			this.tabPageContainers.Controls.Add(this.panel3);
			this.tabPageContainers.Location = new System.Drawing.Point(4, 22);
			this.tabPageContainers.Name = "tabPageContainers";
			this.tabPageContainers.Size = new System.Drawing.Size(817, 489);
			this.tabPageContainers.TabIndex = 2;
			this.tabPageContainers.Text = "Контейнеры";
			this.tabPageContainers.UseVisualStyleBackColor = true;
			// 
			// dgvContainers
			// 
			this.dgvContainers.AutoGenerateColumns = false;
			this.dgvContainers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvContainers.DataSource = this.bsContainers;
			this.dgvContainers.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvContainers.Location = new System.Drawing.Point(0, 0);
			this.dgvContainers.Name = "dgvContainers";
			this.dgvContainers.Size = new System.Drawing.Size(817, 424);
			this.dgvContainers.TabIndex = 1;
			// 
			// panel3
			// 
			this.panel3.Controls.Add(this.navContainers);
			this.panel3.Controls.Add(this.btRefreshContainers);
			this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel3.Location = new System.Drawing.Point(0, 424);
			this.panel3.Name = "panel3";
			this.panel3.Size = new System.Drawing.Size(817, 65);
			this.panel3.TabIndex = 0;
			// 
			// navContainers
			// 
			this.navContainers.AddNewItem = this.toolStripButton1;
			this.navContainers.BindingSource = this.bsContainers;
			this.navContainers.CountItem = this.toolStripLabel1;
			this.navContainers.DeleteItem = this.toolStripButton2;
			this.navContainers.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButton4,
            this.toolStripSeparator2,
            this.toolStripTextBox1,
            this.toolStripLabel1,
            this.toolStripSeparator3,
            this.toolStripButton5,
            this.toolStripButton6,
            this.toolStripSeparator4,
            this.toolStripButton1,
            this.toolStripButton2,
            this.toolStripSeparator5,
            this.btUpdContainers});
			this.navContainers.Location = new System.Drawing.Point(0, 0);
			this.navContainers.MoveFirstItem = this.toolStripButton3;
			this.navContainers.MoveLastItem = this.toolStripButton6;
			this.navContainers.MoveNextItem = this.toolStripButton5;
			this.navContainers.MovePreviousItem = this.toolStripButton4;
			this.navContainers.Name = "navContainers";
			this.navContainers.PositionItem = this.toolStripTextBox1;
			this.navContainers.Size = new System.Drawing.Size(817, 25);
			this.navContainers.TabIndex = 1;
			this.navContainers.Text = "bindingNavigator1";
			// 
			// toolStripButton1
			// 
			this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
			this.toolStripButton1.Name = "toolStripButton1";
			this.toolStripButton1.RightToLeftAutoMirrorImage = true;
			this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton1.Text = "Добавить";
			// 
			// toolStripLabel1
			// 
			this.toolStripLabel1.Name = "toolStripLabel1";
			this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
			this.toolStripLabel1.Text = "для {0}";
			this.toolStripLabel1.ToolTipText = "Общее число элементов";
			// 
			// toolStripButton2
			// 
			this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
			this.toolStripButton2.Name = "toolStripButton2";
			this.toolStripButton2.RightToLeftAutoMirrorImage = true;
			this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton2.Text = "Удалить";
			// 
			// toolStripButton3
			// 
			this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
			this.toolStripButton3.Name = "toolStripButton3";
			this.toolStripButton3.RightToLeftAutoMirrorImage = true;
			this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton3.Text = "Переместить в начало";
			// 
			// toolStripButton4
			// 
			this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
			this.toolStripButton4.Name = "toolStripButton4";
			this.toolStripButton4.RightToLeftAutoMirrorImage = true;
			this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton4.Text = "Переместить назад";
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripTextBox1
			// 
			this.toolStripTextBox1.AccessibleName = "Положение";
			this.toolStripTextBox1.AutoSize = false;
			this.toolStripTextBox1.Name = "toolStripTextBox1";
			this.toolStripTextBox1.Size = new System.Drawing.Size(50, 23);
			this.toolStripTextBox1.Text = "0";
			this.toolStripTextBox1.ToolTipText = "Текущее положение";
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripButton5
			// 
			this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
			this.toolStripButton5.Name = "toolStripButton5";
			this.toolStripButton5.RightToLeftAutoMirrorImage = true;
			this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton5.Text = "Переместить вперед";
			// 
			// toolStripButton6
			// 
			this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
			this.toolStripButton6.Name = "toolStripButton6";
			this.toolStripButton6.RightToLeftAutoMirrorImage = true;
			this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
			this.toolStripButton6.Text = "Переместить в конец";
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
			// 
			// btUpdContainers
			// 
			this.btUpdContainers.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btUpdContainers.Image = ((System.Drawing.Image)(resources.GetObject("btUpdContainers.Image")));
			this.btUpdContainers.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btUpdContainers.Name = "btUpdContainers";
			this.btUpdContainers.Size = new System.Drawing.Size(23, 22);
			this.btUpdContainers.Click += new System.EventHandler(this.navUpdate_Click);
			// 
			// btRefreshContainers
			// 
			this.btRefreshContainers.Location = new System.Drawing.Point(3, 34);
			this.btRefreshContainers.Name = "btRefreshContainers";
			this.btRefreshContainers.Size = new System.Drawing.Size(75, 23);
			this.btRefreshContainers.TabIndex = 0;
			this.btRefreshContainers.Text = "Refresh";
			this.btRefreshContainers.UseVisualStyleBackColor = true;
			this.btRefreshContainers.Click += new System.EventHandler(this.btRefreshContainers_Click);
			// 
			// tabDocuments
			// 
			this.tabDocuments.Controls.Add(this.dgvDocuments);
			this.tabDocuments.Controls.Add(this.panel1);
			this.tabDocuments.Location = new System.Drawing.Point(4, 22);
			this.tabDocuments.Name = "tabDocuments";
			this.tabDocuments.Padding = new System.Windows.Forms.Padding(3);
			this.tabDocuments.Size = new System.Drawing.Size(817, 489);
			this.tabDocuments.TabIndex = 0;
			this.tabDocuments.Text = "Документы";
			this.tabDocuments.UseVisualStyleBackColor = true;
			// 
			// dgvDocuments
			// 
			this.dgvDocuments.AutoGenerateColumns = false;
			this.dgvDocuments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.dgvDocuments.DataSource = this.bsDocuments;
			this.dgvDocuments.Dock = System.Windows.Forms.DockStyle.Fill;
			this.dgvDocuments.Location = new System.Drawing.Point(3, 3);
			this.dgvDocuments.Name = "dgvDocuments";
			this.dgvDocuments.Size = new System.Drawing.Size(811, 421);
			this.dgvDocuments.TabIndex = 1;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.btRefreshDocuments);
			this.panel1.Controls.Add(this.navDocuments);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 424);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(811, 62);
			this.panel1.TabIndex = 0;
			// 
			// btRefreshDocuments
			// 
			this.btRefreshDocuments.Location = new System.Drawing.Point(5, 28);
			this.btRefreshDocuments.Name = "btRefreshDocuments";
			this.btRefreshDocuments.Size = new System.Drawing.Size(77, 29);
			this.btRefreshDocuments.TabIndex = 1;
			this.btRefreshDocuments.Text = "Refresh";
			this.btRefreshDocuments.UseVisualStyleBackColor = true;
			this.btRefreshDocuments.Click += new System.EventHandler(this.btRefreshDocuments_Click);
			// 
			// navDocuments
			// 
			this.navDocuments.AddNewItem = this.bindingNavigatorAddNewItem;
			this.navDocuments.BindingSource = this.bsDocuments;
			this.navDocuments.CountItem = this.bindingNavigatorCountItem;
			this.navDocuments.DeleteItem = this.bindingNavigatorDeleteItem;
			this.navDocuments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorAddNewItem,
            this.bindingNavigatorDeleteItem,
            this.toolStripSeparator1,
            this.btUpdDocuments});
			this.navDocuments.Location = new System.Drawing.Point(0, 0);
			this.navDocuments.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
			this.navDocuments.MoveLastItem = this.bindingNavigatorMoveLastItem;
			this.navDocuments.MoveNextItem = this.bindingNavigatorMoveNextItem;
			this.navDocuments.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
			this.navDocuments.Name = "navDocuments";
			this.navDocuments.PositionItem = this.bindingNavigatorPositionItem;
			this.navDocuments.Size = new System.Drawing.Size(811, 25);
			this.navDocuments.TabIndex = 0;
			this.navDocuments.Text = "bindingNavigator1";
			// 
			// bindingNavigatorAddNewItem
			// 
			this.bindingNavigatorAddNewItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorAddNewItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorAddNewItem.Image")));
			this.bindingNavigatorAddNewItem.Name = "bindingNavigatorAddNewItem";
			this.bindingNavigatorAddNewItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorAddNewItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorAddNewItem.Text = "Добавить";
			// 
			// bindingNavigatorCountItem
			// 
			this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
			this.bindingNavigatorCountItem.Size = new System.Drawing.Size(43, 22);
			this.bindingNavigatorCountItem.Text = "для {0}";
			this.bindingNavigatorCountItem.ToolTipText = "Общее число элементов";
			// 
			// bindingNavigatorDeleteItem
			// 
			this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
			this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
			this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorDeleteItem.Text = "Удалить";
			// 
			// bindingNavigatorMoveFirstItem
			// 
			this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
			this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
			this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMoveFirstItem.Text = "Переместить в начало";
			// 
			// bindingNavigatorMovePreviousItem
			// 
			this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
			this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
			this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMovePreviousItem.Text = "Переместить назад";
			// 
			// bindingNavigatorSeparator
			// 
			this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
			this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
			// 
			// bindingNavigatorPositionItem
			// 
			this.bindingNavigatorPositionItem.AccessibleName = "Положение";
			this.bindingNavigatorPositionItem.AutoSize = false;
			this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
			this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
			this.bindingNavigatorPositionItem.Text = "0";
			this.bindingNavigatorPositionItem.ToolTipText = "Текущее положение";
			// 
			// bindingNavigatorSeparator1
			// 
			this.bindingNavigatorSeparator1.Name = "bindingNavigatorSeparator1";
			this.bindingNavigatorSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// bindingNavigatorMoveNextItem
			// 
			this.bindingNavigatorMoveNextItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMoveNextItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveNextItem.Image")));
			this.bindingNavigatorMoveNextItem.Name = "bindingNavigatorMoveNextItem";
			this.bindingNavigatorMoveNextItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMoveNextItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMoveNextItem.Text = "Переместить вперед";
			// 
			// bindingNavigatorMoveLastItem
			// 
			this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
			this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
			this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
			this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
			this.bindingNavigatorMoveLastItem.Text = "Переместить в конец";
			// 
			// bindingNavigatorSeparator2
			// 
			this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
			this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
			// 
			// btUpdDocuments
			// 
			this.btUpdDocuments.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.btUpdDocuments.Image = ((System.Drawing.Image)(resources.GetObject("btUpdDocuments.Image")));
			this.btUpdDocuments.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.btUpdDocuments.Name = "btUpdDocuments";
			this.btUpdDocuments.Size = new System.Drawing.Size(23, 22);
			this.btUpdDocuments.Click += new System.EventHandler(this.navUpdate_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(825, 515);
			this.Controls.Add(this.tabControl1);
			this.Name = "MainForm";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.tabControl1.ResumeLayout(false);
			this.tabTree.ResumeLayout(false);
			this.panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvViewer)).EndInit();
			this.tabPageContainers.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvContainers)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsContainers)).EndInit();
			this.panel3.ResumeLayout(false);
			this.panel3.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.navContainers)).EndInit();
			this.navContainers.ResumeLayout(false);
			this.navContainers.PerformLayout();
			this.tabDocuments.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.dgvDocuments)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.bsDocuments)).EndInit();
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.navDocuments)).EndInit();
			this.navDocuments.ResumeLayout(false);
			this.navDocuments.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.binding1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabDocuments;
		private System.Windows.Forms.DataGridView dgvDocuments;
		private System.Windows.Forms.BindingSource bsDocuments;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btRefreshDocuments;
		private System.Windows.Forms.BindingNavigator navDocuments;
		private System.Windows.Forms.ToolStripButton bindingNavigatorAddNewItem;
		private System.Windows.Forms.ToolStripLabel bindingNavigatorCountItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorDeleteItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveFirstItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMovePreviousItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator;
		private System.Windows.Forms.ToolStripTextBox bindingNavigatorPositionItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator1;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveNextItem;
		private System.Windows.Forms.ToolStripButton bindingNavigatorMoveLastItem;
		private System.Windows.Forms.ToolStripSeparator bindingNavigatorSeparator2;
		private System.Windows.Forms.TabPage tabTree;
		private System.Windows.Forms.ListBox listBox1;
		private System.Windows.Forms.DataGridView dgvViewer;
		private System.Windows.Forms.BindingSource binding1;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button btRefreshTree;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripButton btUpdDocuments;
        private System.Windows.Forms.TabPage tabPageContainers;
        private System.Windows.Forms.DataGridView dgvContainers;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.BindingNavigator navContainers;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.BindingSource bsContainers;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripTextBox toolStripTextBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btUpdContainers;
        private System.Windows.Forms.Button btRefreshContainers;
		private System.Windows.Forms.TreeView treeView1;
	}
}


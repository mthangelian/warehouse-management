namespace CIS375_Warehouse_Management
{
    partial class formUpdateCatalogue
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(formUpdateCatalogue));
            this.radIndividual = new System.Windows.Forms.RadioButton();
            this.radFile = new System.Windows.Forms.RadioButton();
            this.grpBoxIndividual = new System.Windows.Forms.GroupBox();
            this.catalogueDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.catalogueBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.warehouseDBDataSet = new CIS375_Warehouse_Management.WarehouseDBDataSet();
            this.grpBoxFile = new System.Windows.Forms.GroupBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.FDUpdateCatalogue = new System.Windows.Forms.OpenFileDialog();
            this.catalogueTableAdapter = new CIS375_Warehouse_Management.WarehouseDBDataSetTableAdapters.CatalogueTableAdapter();
            this.tableAdapterManager = new CIS375_Warehouse_Management.WarehouseDBDataSetTableAdapters.TableAdapterManager();
            this.catalogueBindingNavigator = new System.Windows.Forms.BindingNavigator(this.components);
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
            this.catalogueBindingNavigatorSaveItem = new System.Windows.Forms.ToolStripButton();
            this.grpBoxIndividual.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.catalogueDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.catalogueBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.warehouseDBDataSet)).BeginInit();
            this.grpBoxFile.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.catalogueBindingNavigator)).BeginInit();
            this.catalogueBindingNavigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // radIndividual
            // 
            this.radIndividual.AutoSize = true;
            this.radIndividual.Checked = true;
            this.radIndividual.Location = new System.Drawing.Point(12, 48);
            this.radIndividual.Name = "radIndividual";
            this.radIndividual.Size = new System.Drawing.Size(93, 17);
            this.radIndividual.TabIndex = 0;
            this.radIndividual.TabStop = true;
            this.radIndividual.Text = "Individual Item";
            this.radIndividual.UseVisualStyleBackColor = true;
            this.radIndividual.CheckedChanged += new System.EventHandler(this.radIndividual_CheckedChanged);
            // 
            // radFile
            // 
            this.radFile.AutoSize = true;
            this.radFile.Location = new System.Drawing.Point(12, 341);
            this.radFile.Name = "radFile";
            this.radFile.Size = new System.Drawing.Size(112, 17);
            this.radFile.TabIndex = 1;
            this.radFile.Text = "Upload Whole File";
            this.radFile.UseVisualStyleBackColor = true;
            this.radFile.CheckedChanged += new System.EventHandler(this.radFile_CheckedChanged);
            // 
            // grpBoxIndividual
            // 
            this.grpBoxIndividual.Controls.Add(this.catalogueDataGridView);
            this.grpBoxIndividual.Location = new System.Drawing.Point(12, 71);
            this.grpBoxIndividual.Name = "grpBoxIndividual";
            this.grpBoxIndividual.Size = new System.Drawing.Size(565, 251);
            this.grpBoxIndividual.TabIndex = 2;
            this.grpBoxIndividual.TabStop = false;
            this.grpBoxIndividual.Text = "Individual Item";
            // 
            // catalogueDataGridView
            // 
            this.catalogueDataGridView.AutoGenerateColumns = false;
            this.catalogueDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.catalogueDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            this.catalogueDataGridView.DataSource = this.catalogueBindingSource;
            this.catalogueDataGridView.Location = new System.Drawing.Point(6, 19);
            this.catalogueDataGridView.Name = "catalogueDataGridView";
            this.catalogueDataGridView.Size = new System.Drawing.Size(544, 220);
            this.catalogueDataGridView.TabIndex = 0;
            this.catalogueDataGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dataGridView1_CellValidating);
            this.catalogueDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.catalogueDataGridView_DataError);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "ItemID";
            this.dataGridViewTextBoxColumn1.HeaderText = "ItemID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ItemName";
            this.dataGridViewTextBoxColumn2.HeaderText = "ItemName";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ItemSize";
            this.dataGridViewTextBoxColumn3.HeaderText = "ItemSize";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ItemPrice";
            this.dataGridViewTextBoxColumn4.HeaderText = "ItemPrice";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ItemDescription";
            this.dataGridViewTextBoxColumn5.HeaderText = "ItemDescription";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // catalogueBindingSource
            // 
            this.catalogueBindingSource.DataMember = "Catalogue";
            this.catalogueBindingSource.DataSource = this.warehouseDBDataSet;
            // 
            // warehouseDBDataSet
            // 
            this.warehouseDBDataSet.DataSetName = "WarehouseDBDataSet";
            this.warehouseDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // grpBoxFile
            // 
            this.grpBoxFile.Controls.Add(this.btnSelectFile);
            this.grpBoxFile.Enabled = false;
            this.grpBoxFile.Location = new System.Drawing.Point(12, 364);
            this.grpBoxFile.Name = "grpBoxFile";
            this.grpBoxFile.Size = new System.Drawing.Size(565, 51);
            this.grpBoxFile.TabIndex = 3;
            this.grpBoxFile.TabStop = false;
            this.grpBoxFile.Text = "Individual Item";
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(6, 19);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(148, 23);
            this.btnSelectFile.TabIndex = 0;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // FDUpdateCatalogue
            // 
            this.FDUpdateCatalogue.FileName = "Update Catalogue File";
            this.FDUpdateCatalogue.Filter = "txt|*.txt";
            // 
            // catalogueTableAdapter
            // 
            this.catalogueTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackOrderTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CatalogueTableAdapter = this.catalogueTableAdapter;
            this.tableAdapterManager.OrderHistoryTableAdapter = null;
            this.tableAdapterManager.SpecialOrderTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = CIS375_Warehouse_Management.WarehouseDBDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.WarehouseATableAdapter = null;
            this.tableAdapterManager.WarehouseBTableAdapter = null;
            this.tableAdapterManager.WarehouseCTableAdapter = null;
            // 
            // catalogueBindingNavigator
            // 
            this.catalogueBindingNavigator.AddNewItem = null;
            this.catalogueBindingNavigator.BindingSource = this.catalogueBindingSource;
            this.catalogueBindingNavigator.CountItem = this.bindingNavigatorCountItem;
            this.catalogueBindingNavigator.DeleteItem = this.bindingNavigatorDeleteItem;
            this.catalogueBindingNavigator.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bindingNavigatorMoveFirstItem,
            this.bindingNavigatorMovePreviousItem,
            this.bindingNavigatorSeparator,
            this.bindingNavigatorPositionItem,
            this.bindingNavigatorCountItem,
            this.bindingNavigatorSeparator1,
            this.bindingNavigatorMoveNextItem,
            this.bindingNavigatorMoveLastItem,
            this.bindingNavigatorSeparator2,
            this.bindingNavigatorDeleteItem,
            this.catalogueBindingNavigatorSaveItem});
            this.catalogueBindingNavigator.Location = new System.Drawing.Point(0, 0);
            this.catalogueBindingNavigator.MoveFirstItem = this.bindingNavigatorMoveFirstItem;
            this.catalogueBindingNavigator.MoveLastItem = this.bindingNavigatorMoveLastItem;
            this.catalogueBindingNavigator.MoveNextItem = this.bindingNavigatorMoveNextItem;
            this.catalogueBindingNavigator.MovePreviousItem = this.bindingNavigatorMovePreviousItem;
            this.catalogueBindingNavigator.Name = "catalogueBindingNavigator";
            this.catalogueBindingNavigator.PositionItem = this.bindingNavigatorPositionItem;
            this.catalogueBindingNavigator.Size = new System.Drawing.Size(590, 25);
            this.catalogueBindingNavigator.TabIndex = 4;
            this.catalogueBindingNavigator.Text = "bindingNavigator1";
            // 
            // bindingNavigatorCountItem
            // 
            this.bindingNavigatorCountItem.Name = "bindingNavigatorCountItem";
            this.bindingNavigatorCountItem.Size = new System.Drawing.Size(35, 22);
            this.bindingNavigatorCountItem.Text = "of {0}";
            this.bindingNavigatorCountItem.ToolTipText = "Total number of items";
            // 
            // bindingNavigatorDeleteItem
            // 
            this.bindingNavigatorDeleteItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorDeleteItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorDeleteItem.Image")));
            this.bindingNavigatorDeleteItem.Name = "bindingNavigatorDeleteItem";
            this.bindingNavigatorDeleteItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorDeleteItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorDeleteItem.Text = "Delete";
            // 
            // bindingNavigatorMoveFirstItem
            // 
            this.bindingNavigatorMoveFirstItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveFirstItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveFirstItem.Image")));
            this.bindingNavigatorMoveFirstItem.Name = "bindingNavigatorMoveFirstItem";
            this.bindingNavigatorMoveFirstItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveFirstItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveFirstItem.Text = "Move first";
            // 
            // bindingNavigatorMovePreviousItem
            // 
            this.bindingNavigatorMovePreviousItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMovePreviousItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMovePreviousItem.Image")));
            this.bindingNavigatorMovePreviousItem.Name = "bindingNavigatorMovePreviousItem";
            this.bindingNavigatorMovePreviousItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMovePreviousItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMovePreviousItem.Text = "Move previous";
            // 
            // bindingNavigatorSeparator
            // 
            this.bindingNavigatorSeparator.Name = "bindingNavigatorSeparator";
            this.bindingNavigatorSeparator.Size = new System.Drawing.Size(6, 25);
            // 
            // bindingNavigatorPositionItem
            // 
            this.bindingNavigatorPositionItem.AccessibleName = "Position";
            this.bindingNavigatorPositionItem.AutoSize = false;
            this.bindingNavigatorPositionItem.Name = "bindingNavigatorPositionItem";
            this.bindingNavigatorPositionItem.Size = new System.Drawing.Size(50, 23);
            this.bindingNavigatorPositionItem.Text = "0";
            this.bindingNavigatorPositionItem.ToolTipText = "Current position";
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
            this.bindingNavigatorMoveNextItem.Text = "Move next";
            // 
            // bindingNavigatorMoveLastItem
            // 
            this.bindingNavigatorMoveLastItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bindingNavigatorMoveLastItem.Image = ((System.Drawing.Image)(resources.GetObject("bindingNavigatorMoveLastItem.Image")));
            this.bindingNavigatorMoveLastItem.Name = "bindingNavigatorMoveLastItem";
            this.bindingNavigatorMoveLastItem.RightToLeftAutoMirrorImage = true;
            this.bindingNavigatorMoveLastItem.Size = new System.Drawing.Size(23, 22);
            this.bindingNavigatorMoveLastItem.Text = "Move last";
            // 
            // bindingNavigatorSeparator2
            // 
            this.bindingNavigatorSeparator2.Name = "bindingNavigatorSeparator2";
            this.bindingNavigatorSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // catalogueBindingNavigatorSaveItem
            // 
            this.catalogueBindingNavigatorSaveItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.catalogueBindingNavigatorSaveItem.Image = ((System.Drawing.Image)(resources.GetObject("catalogueBindingNavigatorSaveItem.Image")));
            this.catalogueBindingNavigatorSaveItem.Name = "catalogueBindingNavigatorSaveItem";
            this.catalogueBindingNavigatorSaveItem.Size = new System.Drawing.Size(23, 22);
            this.catalogueBindingNavigatorSaveItem.Text = "Save Data";
            this.catalogueBindingNavigatorSaveItem.Click += new System.EventHandler(this.catalogueBindingNavigatorSaveItem_Click);
            // 
            // formUpdateCatalogue
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 428);
            this.Controls.Add(this.catalogueBindingNavigator);
            this.Controls.Add(this.grpBoxFile);
            this.Controls.Add(this.grpBoxIndividual);
            this.Controls.Add(this.radFile);
            this.Controls.Add(this.radIndividual);
            this.Name = "formUpdateCatalogue";
            this.Text = "Update Catalogue";
            this.Load += new System.EventHandler(this.formUpdateCatalogue_Load);
            this.grpBoxIndividual.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.catalogueDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.catalogueBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.warehouseDBDataSet)).EndInit();
            this.grpBoxFile.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.catalogueBindingNavigator)).EndInit();
            this.catalogueBindingNavigator.ResumeLayout(false);
            this.catalogueBindingNavigator.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radIndividual;
        private System.Windows.Forms.RadioButton radFile;
        private System.Windows.Forms.GroupBox grpBoxIndividual;
        private System.Windows.Forms.GroupBox grpBoxFile;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.OpenFileDialog FDUpdateCatalogue;
        private WarehouseDBDataSet warehouseDBDataSet;
        private System.Windows.Forms.BindingSource catalogueBindingSource;
        private WarehouseDBDataSetTableAdapters.CatalogueTableAdapter catalogueTableAdapter;
        private WarehouseDBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.BindingNavigator catalogueBindingNavigator;
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
        private System.Windows.Forms.ToolStripButton catalogueBindingNavigatorSaveItem;
        private System.Windows.Forms.DataGridView catalogueDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
    }
}
namespace CIS375_Warehouse_Management
{
    partial class formAnalytics
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
            this.radAll = new System.Windows.Forms.RadioButton();
            this.radSpecifc = new System.Windows.Forms.RadioButton();
            this.grpBoxAll = new System.Windows.Forms.GroupBox();
            this.btnShowResults = new System.Windows.Forms.Button();
            this.lstPopularity = new System.Windows.Forms.ListBox();
            this.grpBoxSpecific = new System.Windows.Forms.GroupBox();
            this.lblLookUpID = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.warehouseDBDataSet = new CIS375_Warehouse_Management.WarehouseDBDataSet();
            this.orderHistoryBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.orderHistoryTableAdapter = new CIS375_Warehouse_Management.WarehouseDBDataSetTableAdapters.OrderHistoryTableAdapter();
            this.tableAdapterManager = new CIS375_Warehouse_Management.WarehouseDBDataSetTableAdapters.TableAdapterManager();
            this.orderHistoryDataGridView = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvResults = new System.Windows.Forms.DataGridView();
            this.grpBoxOrderHistory = new System.Windows.Forms.GroupBox();
            this.grpBoxResults = new System.Windows.Forms.GroupBox();
            this.lstCustomerItem = new System.Windows.Forms.ListBox();
            this.btnSubmitSingle = new System.Windows.Forms.Button();
            this.grpBoxAll.SuspendLayout();
            this.grpBoxSpecific.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warehouseDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderHistoryBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderHistoryDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).BeginInit();
            this.grpBoxOrderHistory.SuspendLayout();
            this.grpBoxResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // radAll
            // 
            this.radAll.AutoSize = true;
            this.radAll.Location = new System.Drawing.Point(12, 12);
            this.radAll.Name = "radAll";
            this.radAll.Size = new System.Drawing.Size(66, 17);
            this.radAll.TabIndex = 1;
            this.radAll.TabStop = true;
            this.radAll.Text = "Show All";
            this.radAll.UseVisualStyleBackColor = true;
            this.radAll.CheckedChanged += new System.EventHandler(this.radAll_CheckedChanged);
            // 
            // radSpecifc
            // 
            this.radSpecifc.AutoSize = true;
            this.radSpecifc.Location = new System.Drawing.Point(222, 12);
            this.radSpecifc.Name = "radSpecifc";
            this.radSpecifc.Size = new System.Drawing.Size(102, 17);
            this.radSpecifc.TabIndex = 2;
            this.radSpecifc.TabStop = true;
            this.radSpecifc.Text = "Specific Lookup";
            this.radSpecifc.UseVisualStyleBackColor = true;
            this.radSpecifc.CheckedChanged += new System.EventHandler(this.radSpecifc_CheckedChanged);
            // 
            // grpBoxAll
            // 
            this.grpBoxAll.Controls.Add(this.btnShowResults);
            this.grpBoxAll.Controls.Add(this.lstPopularity);
            this.grpBoxAll.Location = new System.Drawing.Point(12, 35);
            this.grpBoxAll.Name = "grpBoxAll";
            this.grpBoxAll.Size = new System.Drawing.Size(196, 172);
            this.grpBoxAll.TabIndex = 3;
            this.grpBoxAll.TabStop = false;
            this.grpBoxAll.Text = "Options:";
            // 
            // btnShowResults
            // 
            this.btnShowResults.Location = new System.Drawing.Point(71, 125);
            this.btnShowResults.Name = "btnShowResults";
            this.btnShowResults.Size = new System.Drawing.Size(119, 23);
            this.btnShowResults.TabIndex = 5;
            this.btnShowResults.Text = "Show Results";
            this.btnShowResults.UseVisualStyleBackColor = true;
            this.btnShowResults.Click += new System.EventHandler(this.btnShowResults_Click);
            // 
            // lstPopularity
            // 
            this.lstPopularity.FormattingEnabled = true;
            this.lstPopularity.Items.AddRange(new object[] {
            "Most loyal customer",
            "Most popular item"});
            this.lstPopularity.Location = new System.Drawing.Point(6, 19);
            this.lstPopularity.Name = "lstPopularity";
            this.lstPopularity.Size = new System.Drawing.Size(119, 82);
            this.lstPopularity.TabIndex = 0;
            // 
            // grpBoxSpecific
            // 
            this.grpBoxSpecific.Controls.Add(this.btnSubmitSingle);
            this.grpBoxSpecific.Controls.Add(this.lstCustomerItem);
            this.grpBoxSpecific.Controls.Add(this.lblLookUpID);
            this.grpBoxSpecific.Controls.Add(this.textBox1);
            this.grpBoxSpecific.Location = new System.Drawing.Point(222, 35);
            this.grpBoxSpecific.Name = "grpBoxSpecific";
            this.grpBoxSpecific.Size = new System.Drawing.Size(240, 172);
            this.grpBoxSpecific.TabIndex = 4;
            this.grpBoxSpecific.TabStop = false;
            this.grpBoxSpecific.Text = "Options:";
            // 
            // lblLookUpID
            // 
            this.lblLookUpID.AutoSize = true;
            this.lblLookUpID.Location = new System.Drawing.Point(4, 85);
            this.lblLookUpID.Name = "lblLookUpID";
            this.lblLookUpID.Size = new System.Drawing.Size(96, 13);
            this.lblLookUpID.TabIndex = 1;
            this.lblLookUpID.Text = "Customer/ Item ID:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(128, 82);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(106, 20);
            this.textBox1.TabIndex = 0;
            // 
            // warehouseDBDataSet
            // 
            this.warehouseDBDataSet.DataSetName = "WarehouseDBDataSet";
            this.warehouseDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // orderHistoryBindingSource
            // 
            this.orderHistoryBindingSource.DataMember = "OrderHistory";
            this.orderHistoryBindingSource.DataSource = this.warehouseDBDataSet;
            // 
            // orderHistoryTableAdapter
            // 
            this.orderHistoryTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackOrderTableAdapter = null;
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CatalogueTableAdapter = null;
            this.tableAdapterManager.OrderHistoryTableAdapter = this.orderHistoryTableAdapter;
            this.tableAdapterManager.SpecialOrderTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = CIS375_Warehouse_Management.WarehouseDBDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            this.tableAdapterManager.WarehouseATableAdapter = null;
            this.tableAdapterManager.WarehouseBTableAdapter = null;
            this.tableAdapterManager.WarehouseCTableAdapter = null;
            // 
            // orderHistoryDataGridView
            // 
            this.orderHistoryDataGridView.AllowUserToAddRows = false;
            this.orderHistoryDataGridView.AllowUserToDeleteRows = false;
            this.orderHistoryDataGridView.AutoGenerateColumns = false;
            this.orderHistoryDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.orderHistoryDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.orderHistoryDataGridView.DataSource = this.orderHistoryBindingSource;
            this.orderHistoryDataGridView.Location = new System.Drawing.Point(6, 19);
            this.orderHistoryDataGridView.Name = "orderHistoryDataGridView";
            this.orderHistoryDataGridView.ReadOnly = true;
            this.orderHistoryDataGridView.Size = new System.Drawing.Size(447, 220);
            this.orderHistoryDataGridView.TabIndex = 6;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "OrderID";
            this.dataGridViewTextBoxColumn1.HeaderText = "OrderID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "CustomerID";
            this.dataGridViewTextBoxColumn2.HeaderText = "CustomerID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "ItemID";
            this.dataGridViewTextBoxColumn3.HeaderText = "ItemID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "ItemQuantity";
            this.dataGridViewTextBoxColumn4.HeaderText = "ItemQuantity";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dgvResults
            // 
            this.dgvResults.AllowUserToAddRows = false;
            this.dgvResults.AllowUserToDeleteRows = false;
            this.dgvResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResults.Location = new System.Drawing.Point(6, 19);
            this.dgvResults.Name = "dgvResults";
            this.dgvResults.ReadOnly = true;
            this.dgvResults.Size = new System.Drawing.Size(290, 220);
            this.dgvResults.TabIndex = 7;
            // 
            // grpBoxOrderHistory
            // 
            this.grpBoxOrderHistory.Controls.Add(this.orderHistoryDataGridView);
            this.grpBoxOrderHistory.Location = new System.Drawing.Point(12, 213);
            this.grpBoxOrderHistory.Name = "grpBoxOrderHistory";
            this.grpBoxOrderHistory.Size = new System.Drawing.Size(464, 249);
            this.grpBoxOrderHistory.TabIndex = 8;
            this.grpBoxOrderHistory.TabStop = false;
            this.grpBoxOrderHistory.Text = "Order History:";
            // 
            // grpBoxResults
            // 
            this.grpBoxResults.Controls.Add(this.dgvResults);
            this.grpBoxResults.Location = new System.Drawing.Point(482, 213);
            this.grpBoxResults.Name = "grpBoxResults";
            this.grpBoxResults.Size = new System.Drawing.Size(302, 249);
            this.grpBoxResults.TabIndex = 9;
            this.grpBoxResults.TabStop = false;
            this.grpBoxResults.Text = "Results:";
            // 
            // lstCustomerItem
            // 
            this.lstCustomerItem.FormattingEnabled = true;
            this.lstCustomerItem.Items.AddRange(new object[] {
            "Customer",
            "Item"});
            this.lstCustomerItem.Location = new System.Drawing.Point(7, 20);
            this.lstCustomerItem.Name = "lstCustomerItem";
            this.lstCustomerItem.Size = new System.Drawing.Size(120, 30);
            this.lstCustomerItem.TabIndex = 2;
            // 
            // btnSubmitSingle
            // 
            this.btnSubmitSingle.Location = new System.Drawing.Point(128, 125);
            this.btnSubmitSingle.Name = "btnSubmitSingle";
            this.btnSubmitSingle.Size = new System.Drawing.Size(106, 23);
            this.btnSubmitSingle.TabIndex = 3;
            this.btnSubmitSingle.Text = "Show Results";
            this.btnSubmitSingle.UseVisualStyleBackColor = true;
            this.btnSubmitSingle.Click += new System.EventHandler(this.btnSubmitSingle_Click);
            // 
            // formAnalytics
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(796, 471);
            this.Controls.Add(this.grpBoxResults);
            this.Controls.Add(this.grpBoxOrderHistory);
            this.Controls.Add(this.grpBoxSpecific);
            this.Controls.Add(this.grpBoxAll);
            this.Controls.Add(this.radSpecifc);
            this.Controls.Add(this.radAll);
            this.Name = "formAnalytics";
            this.Text = "Analytics";
            this.Load += new System.EventHandler(this.formAnalytics_Load);
            this.grpBoxAll.ResumeLayout(false);
            this.grpBoxSpecific.ResumeLayout(false);
            this.grpBoxSpecific.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.warehouseDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderHistoryBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.orderHistoryDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResults)).EndInit();
            this.grpBoxOrderHistory.ResumeLayout(false);
            this.grpBoxResults.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radAll;
        private System.Windows.Forms.RadioButton radSpecifc;
        private System.Windows.Forms.GroupBox grpBoxAll;
        private System.Windows.Forms.GroupBox grpBoxSpecific;
        private System.Windows.Forms.ListBox lstPopularity;
        private System.Windows.Forms.Button btnShowResults;
        private WarehouseDBDataSet warehouseDBDataSet;
        private System.Windows.Forms.BindingSource orderHistoryBindingSource;
        private WarehouseDBDataSetTableAdapters.OrderHistoryTableAdapter orderHistoryTableAdapter;
        private WarehouseDBDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridView orderHistoryDataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridView dgvResults;
        private System.Windows.Forms.GroupBox grpBoxOrderHistory;
        private System.Windows.Forms.GroupBox grpBoxResults;
        private System.Windows.Forms.Label lblLookUpID;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button btnSubmitSingle;
        private System.Windows.Forms.ListBox lstCustomerItem;
    }
}
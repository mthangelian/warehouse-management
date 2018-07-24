using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CIS375_Warehouse_Management
{
    public partial class Orders : Form
    {
        public Orders()
        {
            InitializeComponent();
        }

        private void specialOrderBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.specialOrderBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.warehouseDBDataSet);

        }

        private void Orders_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'warehouseDBDataSet.SpecialOrder' table. You can move, or remove it, as needed.
            this.specialOrderTableAdapter.Fill(this.warehouseDBDataSet.SpecialOrder);

        }
    }
}

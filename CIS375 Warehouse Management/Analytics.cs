using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace CIS375_Warehouse_Management
{
    public partial class formAnalytics : Form
    {
        public formAnalytics()
        {
            InitializeComponent();
        }

        private void formAnalytics_Load(object sender, EventArgs e)
        {
            lstPopularity.SelectedIndex = 0;
            lstCustomerItem.SelectedIndex = 0;

            try
            {
                // TODO: This line of code loads data into the 'warehouseDBDataSet.OrderHistory' table. You can move, or remove it, as needed.
                this.orderHistoryTableAdapter.Fill(this.warehouseDBDataSet.OrderHistory);                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnShowResults_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "";
                if (lstPopularity.SelectedIndex == 0)
                {
                    query = @"SELECT CustomerID, SUM(ItemQuantity) AS ItemsPurchased
                          FROM OrderHistory
                          GROUP BY CustomerID
                          ORDER BY ItemsPurchased DESC";
                }
                else
                {
                    query = @"SELECT Catalogue.ItemName AS ItemName, OrderHistory.ItemID, SUM(ItemQuantity) AS ItemQuantity
                          FROM OrderHistory
                          LEFT JOIN Catalogue
                          ON OrderHistory.ItemID = Catalogue.ItemID
                          GROUP BY OrderHistory.ItemID, Catalogue.ItemName
                          ORDER BY ItemQuantity DESC";
                }
                SqlConnection c = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True");


                /*string query1 = @"SELECT ItemID, SUM(ItemQuantity) AS ItemQuantity
                                  FROM OrderHistory
                                  GROUP BY ItemID";*/

                SqlCommand cmd = new SqlCommand(query, c);
                //cmd.Parameters.AddWithValue("@itemID", "1234567890");
                cmd.Connection = c;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd); //c.con is the connection string

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dgvResults.ReadOnly = true;
                dgvResults.DataSource = ds.Tables[0];//investigate this line


                if (dgvResults.RowCount == 0)
                {
                    MessageBox.Show("No results were found");
                }
                else
                {
                    string line = Environment.NewLine + "Results for \"" + lstPopularity.Items[lstPopularity.SelectedIndex] + "\" on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + Environment.NewLine;

                    if (lstPopularity.SelectedIndex == 0)
                    {
                        line += "CustomerID   Sum" + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        line += "ItemName     ItemID     ItemQuantity" + Environment.NewLine + Environment.NewLine;
                    }

                    for (int i = 0; i < dgvResults.RowCount; i++)
                    {
                        string row = "";
                        for (int j = 0; j < dgvResults.ColumnCount; j++)
                        {
                            row += dgvResults.Rows[i].Cells[j].Value.ToString() + "  ";
                            //MessageBox.Show(dgvResults.Rows[i].Cells[j].Value.ToString());
                        }
                        line += row + Environment.NewLine;
                        //MessageBox.Show(row);
                    }

                    //MessageBox.Show(line);

                    //write to file here

                    string fileName = @"output\Analyrics " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    
                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.Write(line);
                        sw.Close();
                    }
                }

                

                /*
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = "";                

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {  
                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        //emptyBins.Add(Convert.ToInt32(reader["BinID"]));
                                        //need three cases for sizes for the list below
                                        //spaceAvailable.Add(250 - Convert.ToInt32(reader["CurrentQuantity"]));
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }

                */

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radAll_CheckedChanged(object sender, EventArgs e)
        {
            grpBoxAll.Enabled = true;
            grpBoxSpecific.Enabled = false;
        }

        private void radSpecifc_CheckedChanged(object sender, EventArgs e)
        {
            grpBoxSpecific.Enabled = true;
            grpBoxAll.Enabled = false;
        }

        private void btnSubmitSingle_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection c = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True");

                SqlCommand cmd;
                string query = "";

                if (lstCustomerItem.SelectedIndex == 0)
                {
                    query = @"SELECT CustomerID, SUM(ItemQuantity) AS ItemsPurchased
                          FROM OrderHistory
                          WHERE CustomerID = @custID
                          GROUP BY CustomerID";

                    cmd = new SqlCommand(query, c);
                    cmd.Parameters.AddWithValue("@custID", textBox1.Text);
                }
                else
                {
                    query = @"SELECT Catalogue.ItemName AS ItemName, OrderHistory.ItemID, SUM(ItemQuantity) AS ItemQuantity
                              FROM OrderHistory
                              LEFT JOIN Catalogue
                              ON OrderHistory.ItemID = Catalogue.ItemID
                              WHERE OrderHistory.ItemID = @itemID
                              GROUP BY OrderHistory.ItemID, Catalogue.ItemName";

                    cmd = new SqlCommand(query, c);
                    cmd.Parameters.AddWithValue("@itemID", textBox1.Text);
                }
                
                cmd.Connection = c;
                SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd); //c.con is the connection string

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                DataSet ds = new DataSet();
                dataAdapter.Fill(ds);
                dgvResults.ReadOnly = true;
                dgvResults.DataSource = ds.Tables[0];//investigate this line   



                if (dgvResults.RowCount == 0)
                {
                    MessageBox.Show("No results were found");
                }
                else//write to analytics file
                {
                    string line = Environment.NewLine + "Results for specific " + lstCustomerItem.Items[lstPopularity.SelectedIndex] + " on " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + Environment.NewLine;

                    if (lstCustomerItem.SelectedIndex == 0)
                    {
                        line += "CustomerID   Sum" + Environment.NewLine + Environment.NewLine;
                    }
                    else
                    {
                        line += "ItemName     ItemID     ItemQuantity" + Environment.NewLine + Environment.NewLine;
                    }

                    for (int i = 0; i < dgvResults.RowCount; i++)
                    {
                        string row = "";
                        for (int j = 0; j < dgvResults.ColumnCount; j++)
                        {
                            row += dgvResults.Rows[i].Cells[j].Value.ToString() + "  ";
                            //MessageBox.Show(dgvResults.Rows[i].Cells[j].Value.ToString());
                        }
                        line += row + Environment.NewLine;
                        //MessageBox.Show(row);
                    }

                    //MessageBox.Show(line);

                    //write to file here

                    string fileName = @"output\Analyrics " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";

                    using (StreamWriter sw = File.AppendText(fileName))
                    {
                        sw.Write(line);
                        sw.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

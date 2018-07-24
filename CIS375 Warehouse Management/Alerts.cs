using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.IO;
using System.Globalization;

namespace CIS375_Warehouse_Management
{
    public partial class Alerts : Form
    {
        public Alerts()
        {
            InitializeComponent();
        }

        private void Alerts_Load(object sender, EventArgs e)
        {
            int threshold = 0;
            List<string> values;
            string path = "StorageFile.txt";

            using (StreamReader sr = File.OpenText(path))//get previous sequence from the storage file
            {
                string str = "";
                str = sr.ReadLine();                

                values = str.Split(',').ToList<string>();

                threshold = Convert.ToInt32(values[5]);
                sr.Close();
            }

            lblThresholdDisplay.Text = threshold.ToString();
        }

        private void btnSetThreshold_Click(object sender, EventArgs e)
        {
            try
            {
                int threshold = Convert.ToInt32(txtNewThreshold.Text);
                if (threshold > 0 && threshold < 10000)
                {
                    List<string> values;
                    string path = "StorageFile.txt";

                    using (StreamReader sr = File.OpenText(path))//get previous sequence from the storage file
                    {
                        string str = "";
                        str = sr.ReadLine();

                        values = str.Split(',').ToList<string>();
                        sr.Close();
                    }

                    values[5] = threshold.ToString();

                    string line = string.Join(",", values.ToArray());

                    using (StreamWriter sw = File.CreateText(path))//update the date in the storage file
                    {
                        sw.WriteLine(line);
                        sw.Close();
                    }
                }
                else
                {
                    MessageBox.Show("Value must be between 0 and 10000");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}

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




namespace CIS375_Warehouse_Management
{
    public partial class formInitializeCatalogue : Form
    {
        public formInitializeCatalogue()
        {
            InitializeComponent();
        }

        /*
         * using (Form1 form = new Form1())
            {
                DialogResult dr = form.ShowDialog();
                if(dr == DialogResult.OK)
                {
                    string custName = form.CustomerName;
                    SaveToFile(custName);
                }

            } */
        
        /*private void btbOpenFile_Click(object sender, EventArgs e)
        {              
            string fileName = "";            
            int lineCount = 0;

            DialogResult dr = FDInitializeCatalogue.ShowDialog();
            if (dr == DialogResult.OK )//if a file is selected
            {
                fileName = FDInitializeCatalogue.FileName;

                lineCount = CountLines(fileName);//lineCount now contains the total number of lines - not items

                if (lineCount == 0)//check for empty file
                {
                    MessageBox.Show("File is empty!");
                    return;
                }

                if (IsHeaderOK(fileName) == true)//checks the header - missing and bad format
                {
                    MessageBox.Show("Header is acceptable");
                }
                else
                {
                    MessageBox.Show("Header either missing or has wrong format");
                    return;
                }

                if (IsTrailerOK(fileName, lineCount) == true)//checks the trailer - missing and bad format - does not check the number of items
                {
                    MessageBox.Show("Trailer is acceptable");
                }
                else
                {
                    MessageBox.Show("Trailer either missing or has wrong format");
                    return;
                }

                if (CheckNumberOfItemsFromTrailer(fileName, lineCount))//checks the number of items in the file against the number in the trailer
                {
                    MessageBox.Show("Number of items match");
                }
                else
                {
                    MessageBox.Show("Number of items does not match the number in the trailer");
                    return;
                }

                if (CheckDateCreated(fileName))//checks the date from the header against the date stored in StorageFile
                {
                    MessageBox.Show("Date from input file is fine");
                }
                else
                {
                    MessageBox.Show("Date from input file is old");
                    return;
                }

                ProcessRow(fileName, lineCount);


                ChangeToggleVariableInStorageFile();
                
                //disable button here


            }
            else//if the user does not select any file or closes the file selection window
                MessageBox.Show("Operation Cancelled");
        }

        void ChangeToggleVariableInStorageFile()
        {
            List<string> values;
            string path = "StorageFile.txt";
            string line = "";

            using (StreamReader sr = File.OpenText(path))//get toggle variable from the storage file
            {
                string str = "";
                str = sr.ReadLine();

                values = str.Split(',').ToList<string>();

                values[7] = "true";
            }
            
            line = string.Join(",", values.ToArray());

            using (StreamWriter sw = File.CreateText(path))//update the toggle variable in the storage file
            {
                sw.WriteLine(line);
            }
        }


        bool IsHeaderOK(string fileLocation)
        {
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            string pattern = @"^H\d{4}-\d{2}-\d{2}$";
            string header = SR.ReadLine();

            return (Regex.IsMatch(header, pattern));
        }

        bool IsTrailerOK(string fileLocation, int lineCount)
        {
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            string pattern = @"^T\d{1,}$";
            string trailer = "";

            for (int i = 0; i < lineCount; i++)
            {
                trailer = SR.ReadLine();
            }           

            return (Regex.IsMatch(trailer, pattern));
        }

        int CountLines(string fileLocation)
        {
            int count = 0;
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            while ((line = SR.ReadLine()) != null)
            {
                count++;
            }

            return count;
        }

        bool CheckNumberOfItemsFromTrailer(string fileLocation, int lineCount)
        {
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            for (int i = 0; i < lineCount; i++)
            {
                line = SR.ReadLine();
            }

            line = line.Substring(1, line.Length - 1);//


            if (Convert.ToInt32(line) != (lineCount - 2))//-2 for the header and trailer
            {
                //MessageBox.Show("Number of items does not match the number in the trailer");
                return false;
            }
            else
            {
                //MessageBox.Show("Number of items is accurate");
                return true;
            }            
        }
        
        bool CheckDateCreated(string fileLocation)
        {
            DateTime DTPrevious;
            DateTime DTCurrent;
            List<string> values;
            string path = "StorageFile.txt";

            using (StreamReader sr = File.OpenText(path))//get previous date from the storage file
            {
                string str = "";
                str = sr.ReadLine();
                
                //MessageBox.Show(str);

                values = str.Split(',').ToList<string>();

                MessageBox.Show(values[0] + "\n" + values[1] + "\n" + values[2] + "\n" + values[3] + "\n" + values[4] + "\n" + values[5] + "\n" + values[6] + "\n" + values[7] + "\n");

                //MessageBox.Show("value from storage file: " + values[0]);

                DTPrevious = DateTime.Parse(values[0]);           
            }

            //get the date from the file being processed
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            line = SR.ReadLine();

            line = line.Substring(1, 10);

            //MessageBox.Show("value from input file: " + line);

            DTCurrent = DateTime.Parse(line);

            if (DTCurrent >= DTPrevious)
            {
                values[0] = DTCurrent.Date.ToString("yyyy-MM-dd");

                line = string.Join(",", values.ToArray());            

                //MessageBox.Show("new line:  " + line);

                using (StreamWriter sw = File.CreateText(path))//update the date in the storage file
                {
                    sw.WriteLine(line);
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        
        void ProcessRow(string fileLocation, int lineCount)//needs fixing
        {
            string line = "";
            lineCount--;//to avoid processing the trailer

            string itemID = "";
            string itemName = "";
            string itemSize;
            string itemPrice = "";
            string itemDescription = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);
            
            do
            {
                line = SR.ReadLine();
                string headerPattern = @"^H\d{4}-\d{2}-\d{2}$";
                string pattern = @"^[\d|\w]{8}[\w|\s]{30}[SML][\d]{5}[\.][\d]{2}.{0,500}$";

                if (Regex.IsMatch(line, headerPattern) == true)//if it's the header, then skip the line
                    continue;

                if (Regex.IsMatch(line, pattern) == true)
                {
                    MessageBox.Show("inside ProcessRow:   " + line + "   " + (Regex.IsMatch(line, pattern)).ToString());
                    lineCount--;

                    itemID = line.Substring(0, 8);
                    itemName = line.Substring(8, 30);
                    itemSize = line[38].ToString();
                    itemPrice = line.Substring(39, 8);
                    itemDescription = line.Substring(47);

                    decimal itemPriceDecimal = Convert.ToDecimal(itemPrice);

                    if (itemPriceDecimal <= 0)//checks if the price is zero or negative
                    {
                        MessageBox.Show("Item price cannot be zero or negative");
                        continue;
                    }

                    MessageBox.Show("itemID: " + itemID + "\n itemName: " + itemName + "\n itemSize: " + itemSize + "\n itemPrice: " + itemPrice + "\n itemDescription: " + itemDescription);

                    this.catalogueTableAdapter.InsertNewItem(itemID, itemName, itemSize, itemPriceDecimal, itemDescription);
                }
                else
                {
                    MessageBox.Show("Row is in improper format");
                    lineCount--;
                }

            }while(lineCount != 1);
        }      
          */      



        private void catalogueBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.catalogueBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.warehouseDBDataSet);

        }

        private void formInitializeCatalogue_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'warehouseDBDataSet.Catalogue' table. You can move, or remove it, as needed.
            this.catalogueTableAdapter.Fill(this.warehouseDBDataSet.Catalogue);
        }       
    }
}



//DateTime DT = DateTime.Parse("2015-01-01");
//MessageBox.Show(DT.Date.ToString("yyyy-MM-dd"));
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
using System.IO;
using System.Globalization;
using System.Data.SqlClient;

namespace CIS375_Warehouse_Management
{
    public partial class formUpdateCatalogue : Form
    {
        public formUpdateCatalogue()
        {
            InitializeComponent();
        }

        private void catalogueBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            string outputFileName = "";
            string errorMessage = "";

            if ((catalogueDataGridView.Rows.Count - 1) <= 400)
            {

                try
                {
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Updates committed to the database" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    this.Validate();
                    //111
                    this.catalogueBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.warehouseDBDataSet);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Encountered the following error: " + ex.Message + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                }
            }
            else
            {
                MessageBox.Show("Cannot have more than 400 items");
            }
         

        }

        private void formUpdateCatalogue_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'warehouseDBDataSet.Catalogue' table. You can move, or remove it, as needed.
                this.catalogueTableAdapter.Fill(this.warehouseDBDataSet.Catalogue);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.Catalogue' table. You can move, or remove it, as needed.
                //111
                this.catalogueTableAdapter.Fill(this.warehouseDBDataSet.Catalogue);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void radIndividual_CheckedChanged(object sender, EventArgs e)
        {
            //111
            catalogueBindingNavigator.Enabled = true;
            grpBoxIndividual.Enabled = true;
            grpBoxFile.Enabled = false;
        }

        private void radFile_CheckedChanged(object sender, EventArgs e)
        {
            //111
            catalogueBindingNavigator.Enabled = false;
            grpBoxIndividual.Enabled = false;
            grpBoxFile.Enabled = true;
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string pattern = "";//pattern placeholder
            
            string headerText =
                catalogueDataGridView.Columns[e.ColumnIndex].HeaderText;

            // Abort validation if cell is not in the ItemID column.
            //if (!headerText.Equals("ItemID")) return;

            /*if (string.IsNullOrEmpty(e.FormattedValue.ToString()))// Confirm that the cell is not empty. 
            {
                catalogueDataGridView.Rows[e.RowIndex].ErrorText =
                    "Company Name must not be empty";
                e.Cancel = true;
            }*/

            if (headerText == "ItemID")
            {
                pattern = @"^[\w]{10}$";

                if (Regex.IsMatch(e.FormattedValue.ToString(), pattern) == false)//
                {
                    //catalogueDataGridView.Rows[e.RowIndex].ErrorText = "you dun goofed";
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }

            if (headerText == "ItemName")
            {
                pattern = @"^[A-z|\s]{1,30}$";

                if (Regex.IsMatch(e.FormattedValue.ToString(), pattern) == false)//
                {                    
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }

            if (headerText == "ItemSize")
            {
                pattern = @"^[SML]$";

                if (Regex.IsMatch(e.FormattedValue.ToString(), pattern) == false)//
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }           
            
            if (headerText == "ItemPrice")
            {
                pattern = @"^[\d]{1,5}[\.][\d]{1,2}$";

                if (Regex.IsMatch(e.FormattedValue.ToString(), pattern) == false)//
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }

            if (headerText == "ItemDescription")
            {
                pattern = @"^.{0,500}$";

                if (Regex.IsMatch(e.FormattedValue.ToString(), pattern) == false)//
                {
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = false;
                }
            }
        }

        private void catalogueDataGridView_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            try
            {
                if (e.Context.ToString() != "")//this whole thing is to delete the "changes committed" line when the save button was clicked
                {
                    //MessageBox.Show("aaaaaaaaaaaaaaaaaaaa");

                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";

                    //MessageBox.Show(outputFileName);
                    var lines = System.IO.File.ReadAllLines(outputFileName);
                    System.IO.File.WriteAllLines(outputFileName, lines.Take(lines.Length - 1).ToArray());
                }

                MessageBox.Show(e.Context.ToString() + " error, try again");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            //formInitializeCatalogue IC = new formInitializeCatalogue();
            //IC.Show(); 

            string fileName = "";
            int lineCount = 0;

            string outputFileName = "";
            string errorMessage = "";

            DialogResult dr = FDUpdateCatalogue.ShowDialog();
            if (dr == DialogResult.OK)//if a file is selected
            {
                fileName = FDUpdateCatalogue.FileName;

                if (CheckFileExtension(fileName))
                {
                    //MessageBox.Show("File extension is acceptable");
                }
                else
                {
                    MessageBox.Show("File extension not acceptable");
                    return;
                }

                lineCount = CountLines(fileName);//lineCount now contains the total number of lines - not items

                if (lineCount == 0)//check for empty file
                {
                    MessageBox.Show("Catalogue file is empty");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Catalogue file is empty, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    return;                    
                }

                if (IsHeaderOK(fileName) == true)//checks the header - missing and bad format
                {
                    //MessageBox.Show("Header is acceptable");
                }
                else
                {
                    MessageBox.Show("Header either missing or has wrong format");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Header either missing or has wrong format" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (CheckDateCreated(fileName))//checks the date from the header against the date stored in StorageFile
                {
                    //MessageBox.Show("Date from input file is fine");
                }
                else
                {
                    MessageBox.Show("Date from input file is invalid");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Date from input file is invalid" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (IsTrailerOK(fileName, lineCount) == true)//checks the trailer - missing and bad format - does not check the number of items
                {
                    //MessageBox.Show("Trailer is acceptable");
                }
                else
                {
                    MessageBox.Show("Trailer either missing or has wrong format");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Trailer either missing or has wrong format" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (CheckNumberOfItemsFromTrailer(fileName, lineCount))//checks the number of items in the file against the number in the trailer
                {
                    //MessageBox.Show("Number of items match");
                }
                else
                {
                    MessageBox.Show("Number of items does not match the number in the trailer or item count over 400");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Number of items does not match the number in the trailer" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }
                
                //111
                //this.catalogueTableAdapter.TruncateTable();//delete everything from the catalogue table
                TruncateCatalogueTable(fileName);

                ProcessRow(fileName, lineCount);
            }
            else
            {
                MessageBox.Show("Operation Cancelled");
            }
        }

        bool IsHeaderOK(string fileLocation)  
          
        {
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            string pattern = @"^H\d{4}-\d{2}-\d{2}$";
            string header = SR.ReadLine();
            SR.Close();

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
            SR.Close();

            return (Regex.IsMatch(trailer, pattern));
        }

        int CountLines(string fileLocation)
        {
            int count = 0;
            string line = "";
            //string pattern = @"^[\w]{10}[A-z|\s]{30}[SML][-|\d][\d]{4}[\.][\d]{2}.{0,500}$";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            while ((line = SR.ReadLine()) != null)
            {
                count++;
            }
            SR.Close();

            return count;
        }

        bool CheckNumberOfItemsFromTrailer(string fileLocation, int lineCount)
        {
            int itemCount = 0;
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            while ((line = SR.ReadLine()) != null)//this is to count the number of items - not the lines
            {
                if (line != "")
                {
                    itemCount++;
                }
            }
            SR.Close();

            SR = new System.IO.StreamReader(fileLocation);

            for (int i = 0; i < lineCount; i++)//this is just to get the trailer
            {
                line = SR.ReadLine();
                //MessageBox.Show(line);
            }
            line = line.Substring(1, line.Length - 1);

            if ((Convert.ToInt32(line) != (itemCount - 2)) || (itemCount > 400) || (itemCount <= 0))//-2 for the header and trailer
            {
                //MessageBox.Show("Number of items does not match the number in the trailer");
                SR.Close();
                return false;
            }
            else
            {
                //MessageBox.Show("Number of items is accurate");
                SR.Close();
                return true;
            }
        }

        bool CheckDateCreated(string fileLocation)
        {
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            line = SR.ReadLine();

            line = line.Substring(1, 10);

            DateTime dateValue;

            SR.Close();

            return (DateTime.TryParseExact(line, "yyyy-MM-dd", null, DateTimeStyles.None, out dateValue));

            //the below section was checking the the sequence of dates from the input files, which is redundant now
            /*DateTime DTPrevious;
            DateTime DTCurrent;
            List<string> values;
            string path = "StorageFile.txt";

            using (StreamReader sr = File.OpenText(path))//get previous date from the storage file
            {
                string str = "";
                str = sr.ReadLine();

                //MessageBox.Show(str);

                values = str.Split(',').ToList<string>();

                //MessageBox.Show(values[0] + "\n" + values[1] + "\n" + values[2] + "\n" + values[3] + "\n" + values[4] + "\n" + values[5] + "\n" + values[6] + "\n" + values[7] + "\n");

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
            }*/
        }

        void ProcessRow(string fileLocation, int lineCount)
        {
            string line = "";
            lineCount--;//to avoid processing the trailer

            string itemID = "";
            string itemName = "";
            string itemSize;
            string itemPrice = "";
            string itemDescription = "";

            string headerPattern = @"^H\d{4}-\d{2}-\d{2}$";
            string pattern = @"^[\w]{10}[A-z|\s]{30}[SML][-|\d][\d]{4}[\.][\d]{2}.{0,500}$";

            string outputFileName = "";
            string errorMessage = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            do
            {
                line = SR.ReadLine();

                if (Regex.IsMatch(line, headerPattern) == true)//if it's the header, then skip the line
                    continue;

                if (Regex.IsMatch(line, pattern) == true)
                {
                    //MessageBox.Show("inside ProcessRow:   " + line + "\n\n" + (Regex.IsMatch(line, pattern)).ToString());
                    lineCount--;

                    itemID = line.Substring(0, 10);
                    itemName = line.Substring(10, 30);
                    itemSize = line[40].ToString();
                    itemPrice = line.Substring(41, 8);
                    itemDescription = line.Substring(49);

                    decimal itemPriceDecimal;
                    decimal.TryParse(itemPrice, out itemPriceDecimal);

                    if (itemPriceDecimal <= 0)//checks if the price is zero or negative
                    {
                        //MessageBox.Show("Item price cannot be zero or negative");

                        outputFileName = @"output\Analyst Report " + GetDateFromFile(fileLocation) + ".txt";
                        errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: itemID: " + itemID + ", itemName: " + itemName + " has zero or negative price" + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);

                        continue;
                    }

                    //MessageBox.Show("itemID: " + itemID + "\n itemName: " + itemName + "\n itemSize: " + itemSize + "\n itemPrice: " + itemPrice + "\n itemDescription: " + itemDescription);

                    //try
                    //{
                    //111
                    //this.catalogueTableAdapter.InsertNewItem(itemID, itemName, itemSize, itemPriceDecimal, itemDescription);


                    string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True";
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(connectionString))
                        {
                            using (SqlCommand command = new SqlCommand())
                            {
                                command.Connection = conn;
                                command.CommandType = CommandType.Text;
                                command.CommandText = "INSERT INTO Catalogue (ItemID, ItemName, ItemSize, ItemPrice, ItemDescription) VALUES (@ItemID, @ItemName, @ItemSize, @ItemPrice, @ItemDescription)";

                                command.Parameters.AddWithValue("@ItemID", itemID);
                                command.Parameters.AddWithValue("@ItemName", itemName);
                                command.Parameters.AddWithValue("@ItemSize", itemSize);
                                command.Parameters.AddWithValue("@ItemPrice", itemPriceDecimal);
                                command.Parameters.AddWithValue("@ItemDescription", itemDescription);

                                try
                                {
                                    conn.Open();
                                    command.ExecuteNonQuery();

                                    outputFileName = @"output\Analyst Report " + GetDateFromFile(fileLocation) + ".txt";
                                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: \"" + line + "\" committed to the database " + Environment.NewLine;
                                    WriteToFile(outputFileName, errorMessage);
                                }
                                catch (Exception ex)
                                {
                                    outputFileName = @"output\Analyst Report " + GetDateFromFile(fileLocation) + ".txt";
                                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: \"" + line + "\" encountered the following issue: " + ex.Message + Environment.NewLine;
                                    WriteToFile(outputFileName, errorMessage);
                                }
                                finally
                                {
                                    conn.Close();
                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                    //}
                    /*catch (SqlException ex)//this is not the correct class of exceptions for some reason
                    {
                        MessageBox.Show(ex.Number.ToString());
                    }*/
                    /* catch (Exception ex)//this one works fine
                     {
                         //MessageBox.Show("itemID: " + itemID + "itemName: " + itemName + " - " + ex.Message);                    
                        
                         outputFileName = @"output\Analyst Report " + GetDateFromFile(fileLocation) + ".txt";
                         errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - \"" + line + "\" encountered the following issue: " + ex.Message + Environment.NewLine;
                         WriteToFile(outputFileName, errorMessage);
                     }*/
                }
                else
                {
                    //MessageBox.Show("Row is in improper format");

                    outputFileName = @"output\Analyst Report " + GetDateFromFile(fileLocation) + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Line \"" + line + "\"" + " is in improper format" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    lineCount--;
                }

            } while (lineCount != 1);

            MessageBox.Show("File processed, review the analyst report for details");//this is not for debugging
           
            SR.Close();
        }

        string GetDateFromFile(string fileLocation)
        {
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            line = SR.ReadLine();

            line = line.Substring(1, 10);

            SR.Close();

            return line;
        }

        void WriteToFile(string fileName, string message)
        {
            using (StreamWriter sw = File.AppendText(fileName))
            {
                sw.Write(message);
                sw.Close();
            }
        }

        void TruncateCatalogueTable(string fileName)
        {
            string outputFileName;
            string errorMessage;

            try
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand())
                    {
                        command.Connection = conn;
                        command.CommandType = CommandType.Text;
                        command.CommandText = "DELETE FROM Catalogue";

                        try
                        {
                            conn.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            outputFileName = @"output\Analyst Report " + GetDateFromFile(fileName) + ".txt";
                            errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - encountered the following issue: " + ex.Message + Environment.NewLine;
                            WriteToFile(outputFileName, errorMessage);
                        }
                        finally
                        {
                            conn.Close();
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        bool CheckFileExtension(string fileName)
        {
            string pattern = @"^.{1,}\.txt$";

            return (Regex.IsMatch(fileName, pattern));
        }
          
    }
}

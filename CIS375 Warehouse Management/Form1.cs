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
    public partial class formMain : Form
    {
        public formMain()
        {
            InitializeComponent();
        }

        decimal subtotal = 0;

        private void btnInitializeCatalogue_Click(object sender, EventArgs e)
        {
            //formInitializeCatalogue IC = new formInitializeCatalogue();
            //IC.Show();

            string fileName = "";
            int lineCount = 0;

            string outputFileName = "";
            string errorMessage = "";

            DialogResult dr = FDInitializeCatalogue.ShowDialog();
            if (dr == DialogResult.OK)//if a file is selected
            {
                fileName = FDInitializeCatalogue.FileName;

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

                ProcessRow(fileName, lineCount);

                ChangeToggleVariableInStorageFile();

                btnInitializeCatalogue.Enabled = false;//disable and enable buttons for the current instance of the programme
                btnUpdateCatalogue.Enabled = true;
                btnAnalytics.Enabled = true;
                btnAlertsSetup.Enabled = true;
                btnReturnOrders.Enabled = true;
                btnVendorShipment.Enabled = true;
                btnCustomerOrders.Enabled = false;
            }
            else//if the user does not select any file or closes the file selection window
                MessageBox.Show("Operation Cancelled");
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

            return(DateTime.TryParseExact(line, "yyyy-MM-dd", null, DateTimeStyles.None, out dateValue));
            
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

                        outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
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

                                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: \"" + line + "\" committed to the database " + Environment.NewLine;
                                    WriteToFile(outputFileName, errorMessage);
                                }
                                catch (Exception ex)
                                {
                                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
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

                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Catalogue File: Line \"" + line + "\"" + " is in improper format" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    lineCount--;
                }

            } while (lineCount != 1);

            MessageBox.Show("File processed, review the analyst report for details");//this is not for debugging

            SR.Close();
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
                sw.Close();
            }


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

        bool CheckFileExtension(string fileName)
        {
            string pattern = @"^.{1,}\.txt$";

            return (Regex.IsMatch(fileName, pattern));
        }
                
        private void formMain_Load(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'warehouseDBDataSet.SpecialOrder' table. You can move, or remove it, as needed.
                this.specialOrderTableAdapter.Fill(this.warehouseDBDataSet.SpecialOrder);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.SpecialOrder' table. You can move, or remove it, as needed.
                //this.specialOrderTableAdapter.Fill(this.warehouseDBDataSet.SpecialOrder);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.BackOrder' table. You can move, or remove it, as needed.
                this.backOrderTableAdapter.Fill(this.warehouseDBDataSet.BackOrder);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.WarehouseC' table. You can move, or remove it, as needed.
                this.warehouseCTableAdapter.Fill(this.warehouseDBDataSet.WarehouseC);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.WarehouseB' table. You can move, or remove it, as needed.
                this.warehouseBTableAdapter.Fill(this.warehouseDBDataSet.WarehouseB);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.WarehouseA' table. You can move, or remove it, as needed.
                this.warehouseATableAdapter.Fill(this.warehouseDBDataSet.WarehouseA);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            // TODO: This line of code loads data into the 'warehouseDBDataSet.Catalogue' table. You can move, or remove it, as needed.
            //111 this.catalogueTableAdapter.Fill(this.warehouseDBDataSet.Catalogue);
            // TODO: This line of code loads data into the 'warehouseDBDataSet.Catalogue' table. You can move, or remove it, as needed.
            //111 this.catalogueTableAdapter.Fill(this.warehouseDBDataSet.Catalogue);
            string path = "StorageFile.txt";
            List<string> values;
            string toggle;
            string VendorCustomerToggle;

            using (StreamReader sr = File.OpenText(path))//get previous date from the storage file
            {
                string str = "";
                str = sr.ReadLine();

                //MessageBox.Show(str);
                
                values = str.Split(',').ToList<string>();

                //MessageBox.Show(values[0] + "\n" + values[1] + "\n" + values[2] + "\n" + values[3] + "\n" + values[4] + "\n" + values[5] + "\n" + values[6] + "\n" + values[7] + "\n");

                //MessageBox.Show("Toggle value: " + values[7]);

                toggle = values[7];
                VendorCustomerToggle = values[9];

            }


            

            if (toggle == "true")
            {
                btnInitializeCatalogue.Enabled = false;
                btnUpdateCatalogue.Enabled = true;
                btnAnalytics.Enabled = true;
                btnAlertsSetup.Enabled = true;
                btnReturnOrders.Enabled = true;
                btnVendorShipment.Enabled = true;
                btnCustomerOrders.Enabled = true;

                if (VendorCustomerToggle == "0")
                {
                    btnVendorShipment.Enabled = true;
                    btnCustomerOrders.Enabled = false;
                }
                else
                {
                    btnVendorShipment.Enabled = false;
                    btnCustomerOrders.Enabled = true;
                }

            }
            else
            {
                btnInitializeCatalogue.Enabled = true; ;
                btnUpdateCatalogue.Enabled = false;
                btnAnalytics.Enabled = false;
                btnAlertsSetup.Enabled = false;
                btnReturnOrders.Enabled = false;
                btnVendorShipment.Enabled = false;
                btnCustomerOrders.Enabled = false;
            }
        }
     
        private void btnUpdateCatalogue_Click(object sender, EventArgs e)
        {
            formUpdateCatalogue UC = new formUpdateCatalogue();
            UC.ShowDialog();
        }

        //***************************************************************************************************************************************
        //***************************************************************************************************************************************
        //***************************************************************************************************************************************
        //***************************************************************************************************************************************
        //vendor file processing below

        private void btnVendorShipment_Click(object sender, EventArgs e)
        {
            //a bunch of variables will go here
            string fileName = "";
            int lineCount = 0;

            string outputFileName = "";
            string errorMessage = "";
            
            DialogResult dr = FDInitializeCatalogue.ShowDialog();
            if (dr == DialogResult.OK)//if a file is selected
            {
                fileName = FDInitializeCatalogue.FileName;
                lineCount = CountLines(fileName);

                if (CheckFileExtension(fileName))
                {
                    //MessageBox.Show("File extension is acceptable");
                }
                else
                {
                    MessageBox.Show("File extension not acceptable");
                    return;
                }

                if (lineCount == 0)//checks if the file is empty
                {
                    MessageBox.Show("Vendor file is empty");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Vendor file is empty, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (IsVendorFileHeaderOK(fileName))//checks if the header is in the correct format - doesn't check the sequence number and the date
                {
                    //MessageBox.Show("header is fine");
                }
                else
                {
                    MessageBox.Show("Vendor file header is either missing or has wrong format");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Vendor file header is either missing or has wrong format, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (CheckSequenceAndDate(fileName, 1))
                {
                    //MessageBox.Show("Sequence and date are not outdated");
                }
                else
                {
                    MessageBox.Show("File is either not in sequence or date is invalid");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: File is either not in sequence or date is invalid, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (IsVendorFileTrailerOK(fileName, lineCount))
                {
                    //MessageBox.Show("trailer is fine");
                }
                else
                {
                    MessageBox.Show("Vendor file trailer is either missing or has wrong format");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Vendor file trailer is either missing or has wrong format, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                if (CheckNumberOfVendorsAndItems(fileName, lineCount))
                {
                    //MessageBox.Show("number of vendors and items match");
                }
                else
                {
                    MessageBox.Show("Number of vendors or items does not match trailer");
                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Number of vendors or items does not match trailer, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }

                ProcessVendorFileLine(fileName, lineCount);

            }
            else//if the user does not select any file or closes the file selection window
                MessageBox.Show("Operation Cancelled");
        }

        bool IsVendorFileHeaderOK(string fileLocation)
        {
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            string pattern = @"^H\d{1,}-\d{4}-\d{2}-\d{2}$";
            string header = SR.ReadLine();
            SR.Close();

            return (Regex.IsMatch(header, pattern));
        }

        bool IsVendorFileTrailerOK(string fileLocation, int lineCount)
        {
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            string pattern = @"^T\d{1,}-\d{1,}$";
            string trailer = "";

            for (int i = 0; i < lineCount; i++)
            {
                trailer = SR.ReadLine();
            }
            SR.Close();

            return (Regex.IsMatch(trailer, pattern));
        }

        bool CheckNumberOfVendorsAndItems(string fileLocation, int lineCount)
        {
            string line = "";
            string vendorPattern = @"^[\w|\s]{1,50}\d{4}-\d{2}-\d{2}([\d|-]\d{1,}|(\d{1,}))$";
            //string vendorPattern = @"^[\w|\s]{1,50}\d{4}-\d{2}-\d{2}\d{1,}$";
            string itemPattern = @"^[\w]{10}-[123]-[\d]{1,}$";

            int numOfVendors = 0;
            int numOfItems = 0;

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            for (int i = 0; i < lineCount; i++)//counts the number of vendor and item lines and exits with the trailer from the fiel stored in the varaible 'line'
            {
                line = SR.ReadLine();

                if (Regex.IsMatch(line, vendorPattern))
                    numOfVendors++;

                if (Regex.IsMatch(line, itemPattern))
                    numOfItems++;
            }
            SR.Close();
            //MessageBox.Show("vendor: " + numOfVendors + "\nitems: " + numOfItems);

            line = line.Substring(1, line.Length - 1);

            string[] splitTrailer = line.Split('-');//number of vendors in now in splitTrailer[0], number of items is now in splitTrailer[1]
            
            //MessageBox.Show("line = " + line + "\n First Number = " + splitTrailer[0] + "\n Second Number = " + splitTrailer[1]);

            if (numOfVendors == Convert.ToInt32(splitTrailer[0]) && numOfItems == Convert.ToInt32(splitTrailer[1]))
                return true;
            else
                return false;
        }

        bool CheckSequenceAndDate(string fileLocation,int index)
        {
            //DateTime DTPrevious;
            //DateTime DTCurrent;
            int previousSequence = 0;
            int currentSequence = 0;
            List<string> values;
            string path = "StorageFile.txt";

            using (StreamReader sr = File.OpenText(path))//get previous sequence from the storage file
            {
                string str = "";
                str = sr.ReadLine();

                //MessageBox.Show(str);

                values = str.Split(',').ToList<string>();

                //MessageBox.Show(values[0] + "\n" + values[1] + "\n" + values[2] + "\n" + values[3] + "\n" + values[4] + "\n" + values[5] + "\n" + values[6] + "\n" + values[7] + "\n");

                //MessageBox.Show("value from storage file: " + values[2]);

                //DTPrevious = DateTime.Parse(values[2]);
                previousSequence = Convert.ToInt32(values[index]);

                sr.Close();
            }

            //get the sequence from the file being processed
            string line = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            line = SR.ReadLine();
            line = line.Substring(1, line.Length - 1);//remove the 'H'

            string[] splitHeader = line.Split('-');

            //MessageBox.Show("value from input file: " + line);

            //MessageBox.Show("splitHeader[0] = " + splitHeader[0] + "\nsplitHeader[1] = " + splitHeader[1]);

            string currentDate = splitHeader[1] + '-' + splitHeader[2] + '-' + splitHeader[3];

            currentSequence = Convert.ToInt32(splitHeader[0]);
            //DTCurrent = DateTime.Parse(splitHeader[1] + '-' + splitHeader[2] + '-' + splitHeader[3]);//this is done becasue the date in the line is also broken up becasue a hyphen was used as a splitting character

            DateTime DTtemp; //not actually used for anything - the TryParseExact method needs something for the output

            SR.Close();
            if (/*DTCurrent >= DTPrevious &&*/DateTime.TryParseExact(currentDate, "yyyy-MM-dd", null, DateTimeStyles.None, out DTtemp) && currentSequence == (previousSequence + 1))
            {
                values[index] = currentSequence.ToString();
                //values[2] = DTCurrent.Date.ToString("yyyy-MM-dd");
              
                line = string.Join(",", values.ToArray());

                //MessageBox.Show("new line:  " + line);

                using (StreamWriter sw = File.CreateText(path))//update the date in the storage file
                {
                    sw.WriteLine(line);
                    sw.Close();
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        void ProcessVendorFileLine(string fileLocation, int lineCount)
        {
            string line = "";
            //lineCount--;//to avoid processing the trailer

            //varaibles go here

            string headerPattern = @"^H\d{1,}-\d{4}-\d{2}-\d{2}$";
            string vendorPattern = @"^[\w|\s]{1,50}\d{4}-\d{2}-\d{2}([\d|-]\d{1,}|(\d{1,}))$";
            string itemPattern = @"^[\w]{10}-[123]-[\d]{1,}$";
            string trailerPatter = @"^T\d{1,}-\d{1,}$";

            string outputFileName = "";
            string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt");
            
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            line = SR.ReadLine();//to skip the header

            //bool insideTheBlock;
            int numOfLines = 5;

            List<string> fileInformation = new List<string>();

            while (true)
            {
                line = SR.ReadLine();

                if (Regex.IsMatch(line, trailerPatter))//the exit condition
                {
                    //MessageBox.Show("exiting");
                    break;
                }

                if (line == "")//if line is empty, ignore it
                {                    
                    continue;
                }
                else
                {                   
                    fileInformation.Add(line);
                }
            }
            SR.Close();

            CleanUpBeforeFirstVendor(ref fileInformation);

            //****below is for debugging
            string temp = "";
            for (int i = 0; i < fileInformation.Count; i++)
            {
                temp = temp + "\n" + fileInformation[i];
            }
            //MessageBox.Show(temp);
            //*****

            List<string> items = new List<string>();

            while (fileInformation.Count != 0)
            {
                string vendor = fileInformation[0];
                //MessageBox.Show("outside: " + vendor);
                fileInformation.RemoveAt(0);

                //CHECK THE LINE BELOW
                numOfLines = Convert.ToInt32(vendor.Substring(60, vendor.Length - 60));//check if the declaration for this is in the correct place - also, check wtf is going on with this line becasue it shouldn't work but it's working
                int num = 0;//redundant at this point

                items.Clear();
                
                while (fileInformation.Count != 0 && !Regex.IsMatch(fileInformation[0], vendorPattern))//the order of the two conditions is important - do not change this. The count must be checked before
                {
                    num++; //not used for anything 

                    //MessageBox.Show("inside: " + fileInformation[0]);      
                    items.Add(fileInformation[0]);//add the item to another list
                    fileInformation.RemoveAt(0);//remove from the original list
                    //MessageBox.Show("zrhzrh");
                }

                CleanUpTheItemList(ref items);

                DateTime DTtemp;


                if (/*num != numOfLines*/ items.Count != numOfLines || numOfLines >= 9 || numOfLines <= 0)
                {
                    //MessageBox.Show("numbers doooooon't match");//skip the vendor and its items

                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Vendor " + vendor + " has provided an incorrect number of items, vendor ignored" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                }
                else
                    if (!DateTime.TryParseExact(vendor.Substring(50, 10), "yyyy-MM-dd", null, DateTimeStyles.None, out DTtemp))
                    {                        
                        outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Vendor " + vendor + " has provided a shipment on an invalid date, vendor ignored" + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);
                    }
                    else//this is where the items are processed
                    {
                        //MessageBox.Show("process");

                        string str = "";
                        for (int i = 0; i < items.Count; i++)
                        {
                            str = str + "\n" + items[i];
                        }
                        //MessageBox.Show(str);

                        for (int i = 0; i < items.Count; i++)
                        {
                            //if (Regex.IsMatch(items[i], itemPattern))//I don't why i'm doing the check here - kinda not doing anything sisnce it's already been filtered 
                            //{
                            List<string> values;
                            values = items[i].Split('-').ToList<string>();

                            //MessageBox.Show("ItemID: " + values[0] + "\nWarehouse: " + values[1] + "\nQuantity: " + values[2]);

                            if (DoesItemExistInDatabase(values[0]))
                            {
                                //check if there is space and insert
                                IsThereEnoughSpace(values[0], Convert.ToChar(values[1]), Convert.ToInt32(values[2]));
                            }
                            else
                            {
                                //item does not exist in catalogue
                                //MessageBox.Show("Item does not exist in catalogue so go away");

                                outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                                errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Item " + values[0] + " does not exist in catalogue, item ignored" + Environment.NewLine;
                                WriteToFile(outputFileName, errorMessage);
                            }


                            /*}
                              else//write a message to the analyst report
                              {
                                  MessageBox.Show("item is corrupt");
                              }*/
                        }
                    }
            }

            MessageBox.Show("Processing file complete, check analyst report for further detail");

            btnVendorShipment.Enabled = false;
            btnCustomerOrders.Enabled = true;

            ChangeVendorCustomerToggle(1);

            ProcessBackOrder(fileLocation);
        }

        //private void button1_Click(object sender, EventArgs e)//for debugging
        //{
        //    /*DateTime dateValue;
        //    MessageBox.Show(DateTime.TryParseExact("2014-05-05", "yyyy-MM-dd", null, DateTimeStyles.None, out dateValue).ToString());

        //    MessageBox.Show(dateValue.Date.ToString());*/

        //    //ProcessBackOrder();

        //    //MessageBox.Show(Convert.ToInt32("12").ToString());
        //}

        private void CleanUpBeforeFirstVendor(ref List<string> myList)//removes any bad lines before the first vendor is encountered
        {
            string vendorPattern = @"^[\w|\s]{1,50}\d{4}-\d{2}-\d{2}([\d|-]\d{1,}|(\d{1,}))$";

            for (;;)
            {
                if (Regex.IsMatch(myList[0], vendorPattern))
                {
                    break;
                }
                else
                {
                    //MessageBox.Show(myList[0] + "\n" + (Regex.IsMatch(myList[0], vendorPattern)).ToString());
                    myList.RemoveAt(0);
                }
            }
        }

        private void CleanUpTheItemList(ref List<string> myList)
        {
            string itemPattern = @"^[\w]{10}-[123]-[\d]{1,}$";
            for (int i = myList.Count - 1; i >= 0; i--)
            {
                //MessageBox.Show("i = " + i);
                if (!Regex.IsMatch(myList[i], itemPattern))
                {
                    myList.RemoveAt(i);
                }
                else
                {
                    ;//do nothing
                }
            }
        }
        
        //the below event handler is for debugging only
        //private void button2_Click(object sender, EventArgs e)
        //{
        //    SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\v11.0;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True");
                                                    
        //    //Data Source=""
        //    try
        //    {               
        //        SqlCommand cmd = new SqlCommand();
        //        Object returnValue;

        //        cmd.CommandText = "SELECT COUNT(*) FROM Catalogue WHERE ItemID = @varItemID";
        //        cmd.Parameters.AddWithValue("@varItemID", "1234567890");
        //        cmd.CommandType = CommandType.Text;
        //        cmd.Connection = con;

        //        con.Open();
        //        returnValue = cmd.ExecuteScalar();//returns something of type 'object'
        //        MessageBox.Show(returnValue.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        bool DoesItemExistInDatabase(string itemID)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True");

            try
            {
                SqlCommand cmd = new SqlCommand();
                Object returnValue;

                cmd.CommandText = "SELECT COUNT(*) FROM Catalogue WHERE ItemID = @varItemID";
                cmd.Parameters.AddWithValue("@varItemID", itemID);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                con.Open();
                returnValue = cmd.ExecuteScalar();//returns something of type 'object'

                if ((int)returnValue != 0)//if items exists, return true
                {
                    //MessageBox.Show(returnValue.ToString());
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        void IsThereEnoughSpace(string itemID, char warehouse, int quantity)
        {
            List<int> halfFilledBins = new List<int>();
            List<int> spaceAvailable = new List<int>();
            List<int> emptyBins = new List<int>();

            char itemSize = GetItemSize(itemID);
            int binCapacity;

            if (itemSize == 'S')
            {
                binCapacity = 250;
            }
            else
                if (itemSize == 'M')
                {
                    binCapacity = 100;
                }
                else//'L'
                {
                    binCapacity = 10;
                }
            
            //get partially filled bins
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = "";
                    if (warehouse == '1')
                    {
                        query = @"SELECT * 
                              FROM WarehouseA
                              WHERE BinSize = @binSize AND ItemID = @itemID AND CurrentQuantity < @capacity";
                    }
                    else
                        if (warehouse == '2')
                        {
                            query = @"SELECT * 
                                  FROM WarehouseB
                                  WHERE BinSize = @binSize AND ItemID = @itemID AND CurrentQuantity < @capacity";
                        }
                        else
                        {
                            query = @"SELECT * 
                                  FROM WarehouseC
                                  WHERE BinSize = @binSize AND ItemID = @itemID AND CurrentQuantity < @capacity";
                        }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@binSize", itemSize);
                        cmd.Parameters.AddWithValue("@itemID", itemID);
                        cmd.Parameters.AddWithValue("@capacity", binCapacity);

                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        halfFilledBins.Add(Convert.ToInt32(reader["BinID"]));
                                        //need three cases for sizes for the list below
                                        spaceAvailable.Add(binCapacity - Convert.ToInt32(reader["CurrentQuantity"]));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            //get all empty bins
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = "";
                    if (warehouse == '1')
                    {
                        query = @"SELECT *
                              FROM WarehouseA
                              WHERE BinSize = @binSize AND ItemID IS NULL AND CurrentQuantity IS NULL";
                    }
                    else
                        if (warehouse == '2')
                        {
                            query = @"SELECT *
                                  FROM WarehouseB
                                  WHERE BinSize = @binSize AND ItemID IS NULL AND CurrentQuantity IS NULL";
                        }
                        else
                        {
                            query = @"SELECT * 
                                  FROM WarehouseC
                                  WHERE BinSize = @binSize AND ItemID IS NULL AND CurrentQuantity IS NULL";
                        }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@binSize", itemSize);
                        //cmd.Parameters.AddWithValue("@itemID", "NULL");
                        //cmd.Parameters.AddWithValue("@capacity", "NULL");

                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        emptyBins.Add(Convert.ToInt32(reader["BinID"]));
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            /*string eee = "";
            string aaa = "";
            for (int i = 0; i < emptyBins.Count; i++)
            {
                eee = eee + "\nbin = " + emptyBins[i];
            }
            MessageBox.Show(eee);

            for (int i = 0; i < spaceAvailable.Count; i++)
            {
                aaa = aaa + "\nsapce available = " + spaceAvailable[i]; 
            }
            MessageBox.Show(aaa);*/


            int space = 0;
            for (int i = 0; i < spaceAvailable.Count; i++)
            {
                space += spaceAvailable[i];
            }
            

            int allSpace = space;
            for (int i = 0; i < emptyBins.Count; i++)
            {
                if (itemSize == 'S')
                {
                    allSpace += 250;
                }
                else
                    if (itemSize == 'M')
                    {
                        allSpace += 100;
                    }
                    else
                    {
                        allSpace += 10;
                    }
            }

            //MessageBox.Show("space abailable = " + space + "\nall space available = " + allSpace);

            if (space >= quantity)//insert the incoming quantity into the partially filled bins
            {
                for (int i = 0; i < spaceAvailable.Count; i++)
                {
                    /*if (spaceAvailable[i] > quantity)
                    {
                        MessageBox.Show("first bin is enough");

                        //insert into the first bin you encounter
                        InsertIntoBin(halfFilledBins[i], (quantity + (binCapacity - spaceAvailable[i])), warehouse);
                        break;
                    }
                    else
                    {*/
                        //fill however many spots are empty from the first and suntract the same number from quantity

                    if (quantity > spaceAvailable[i])
                    {
                        quantity = quantity - spaceAvailable[i];
                        //MessageBox.Show("quantty in if = " + quantity);
                        InsertIntoBin(halfFilledBins[i], binCapacity, warehouse, itemID);//possibly change this
                    }
                    else
                    {
                        //MessageBox.Show("quantty in else = " + quantity);
                        InsertIntoBin(halfFilledBins[i], (quantity + (binCapacity - spaceAvailable[i])), warehouse, itemID);//possibly change the formula in this
                        break;
                    }
                    //}
                }
            }
            else
                if (allSpace >= quantity)//check if there are any empty bins that can accomodate the rest
                {
                    for (int i = 0; i < spaceAvailable.Count; i++)//insert whatever you can into the partially 
                    {
                        if (quantity > spaceAvailable[i])
                        {
                            quantity = quantity - spaceAvailable[i];
                            InsertIntoBin(halfFilledBins[i], binCapacity, warehouse, itemID);//possibly change this
                        }
                    }

                    int j = 0;
                    while (quantity != 0)//insert whatever is left into empty bins
                    {
                        if (quantity >= binCapacity)
                        {
                            quantity = quantity - binCapacity;
                            InsertNewIntoBin(emptyBins[j], binCapacity, warehouse, itemID); 
                        }
                        else
                        {
                            InsertNewIntoBin(emptyBins[j], quantity, warehouse, itemID);
                            quantity = 0;//at this point evenything should ahve been placed somewhere
                        }
                        j++;
                    }
                }
                else
                    if((quantity - allSpace) != quantity)//take whatever we can take from the order and throw awaya the rest
                    {
                        int temp = quantity - allSpace;
                        int quantityToProcess = allSpace;

                        for (int i = 0; i < spaceAvailable.Count; i++)//insert whatever you can into the partially 
                        {
                            if (allSpace > spaceAvailable[i])
                            {
                                allSpace = allSpace - spaceAvailable[i];
                                InsertIntoBin(halfFilledBins[i], binCapacity, warehouse, itemID);//possibly change this
                            }
                        }

                        int j = 0;
                        while (allSpace != 0)//insert whatever is left into empty bins
                        {
                            if (allSpace >= binCapacity)
                            {
                                allSpace = allSpace - binCapacity;
                                InsertNewIntoBin(emptyBins[j], binCapacity, warehouse, itemID);
                            }
                            else
                            {
                                InsertNewIntoBin(emptyBins[j], allSpace, warehouse, itemID);
                                allSpace = 0;//at this point evenything should ahve been placed somewhere
                            }
                            j++;
                        }

                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Not enough space to accomodate " + quantity + " of item " + itemID + " - only " + quantityToProcess + " item(s) were processed " + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);
                    }
                    else
                        {
                            //MessageBox.Show("There isn't enough space for this quantity");
                            string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                            string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Not enough space to accomodate " + quantity + " of item " + itemID + " - No bins are available with the appropriate size" + Environment.NewLine;
                            WriteToFile(outputFileName, errorMessage);
                        }     

        }

        void InsertIntoBin(int binID, int quantity, char warehouse, string itemID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    string query = "";
                    if (warehouse == '1')
                    {
                        query = @"UPDATE WarehouseA 
                              SET CurrentQuantity = @CQ
                              WHERE BinID = @binID";
                    }
                    else
                        if (warehouse == '2')
                        {
                            query = @"UPDATE WarehouseB
                                  SET CurrentQuantity = @CQ
                                  WHERE BinID = @binID";
                        }
                        else
                        {
                            query = @"UPDATE WarehouseC
                                  SET CurrentQuantity = @CQ
                                  WHERE BinID = @binID";
                        }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@CQ", quantity);
                        cmd.Parameters.AddWithValue("@binID", binID);

                        cmd.ExecuteNonQuery();

                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Inserted " + quantity + " of item " + itemID + " into bin " + binID + " at warehouse " + warehouse + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void InsertNewIntoBin(int binID, int quantity, char warehouse, string itemID)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    string query = "";
                    if (warehouse == '1')
                    {
                        query = @"UPDATE WarehouseA 
                              SET CurrentQuantity = @CQ,
                                  ItemID = @itemID
                              WHERE BinID = @binID";
                    }
                    else
                        if (warehouse == '2')
                        {
                            query = @"UPDATE WarehouseB
                                  SET CurrentQuantity = @CQ,
                                      ItemID = @itemID
                                  WHERE BinID = @binID";
                        }
                        else
                        {
                            query = @"UPDATE WarehouseC
                                  SET CurrentQuantity = @CQ,
                                      ItemID = @itemID
                                  WHERE BinID = @binID";
                        }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@CQ", quantity);
                        cmd.Parameters.AddWithValue("@binID", binID);
                        cmd.Parameters.AddWithValue("@itemID", itemID);

                        cmd.ExecuteNonQuery();

                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Vendor File: Inserted " + quantity + " of item " + itemID + " into bin " + binID + " at warehouse " + warehouse + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        char GetItemSize(string itemID)
        {
            char size = 'X';
            //needs a try catch block
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = "SELECT * FROM Catalogue WHERE ItemID = @itemID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@itemID", itemID);

                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        size = Convert.ToChar(reader["ItemSize"]);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return size;
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: This line of code loads data into the 'warehouseDBDataSet.WarehouseC' table. You can move, or remove it, as needed.
                this.warehouseCTableAdapter.Fill(this.warehouseDBDataSet.WarehouseC);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.WarehouseB' table. You can move, or remove it, as needed.
                this.warehouseBTableAdapter.Fill(this.warehouseDBDataSet.WarehouseB);
                // TODO: This line of code loads data into the 'warehouseDBDataSet.WarehouseA' table. You can move, or remove it, as needed.
                this.warehouseATableAdapter.Fill(this.warehouseDBDataSet.WarehouseA);

                this.backOrderTableAdapter.Fill(this.warehouseDBDataSet.BackOrder);

                this.specialOrderTableAdapter.Fill(this.warehouseDBDataSet.SpecialOrder);

                //this.specialOrderTableAdapter.Fill(this.warehouseDBDataSet.SpecialOrder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAnalytics_Click(object sender, EventArgs e)
        {
            formAnalytics fA = new formAnalytics();
            fA.ShowDialog();
            
        }

        /*private void btnCustomerOrders_Click(object sender, EventArgs e)
        {
            //*******************Cutoer file processing starts here
        }*/

        //*******************************************************************************************************
        //*******************************************************************************************************
        //*******************************************************************************************************
        //*******************************************************************************************************
        //Process the customer file 

        private void btnCustomerOrders_Click(object sender, EventArgs e)
        {
            string fileName = "";
            int lineCount = 0;

            DialogResult dr = FDInitializeCatalogue.ShowDialog();
            if (dr == DialogResult.OK)//if a file is selected
            {
                fileName = FDInitializeCatalogue.FileName;
                lineCount = CountLines(fileName);

                if (CheckFileExtension(fileName))
                {
                    //MessageBox.Show("File extension is acceptable");
                }
                else
                {
                    MessageBox.Show("File extension not acceptable");
                    return;
                }

                if (lineCount == 0)//checks if the file is empty
                {
                    MessageBox.Show("file is empty");
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Customer file is empty, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    return;
                }

                //Same format for Customer and Vendor Headers
                if (IsVendorFileHeaderOK(fileName))//checks if the header is in the correct format - doesn't check the sequence number and the date
                {
                    //MessageBox.Show("header is fine");
                }
                else
                {
                    MessageBox.Show("header format is incorret");
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Header either missing or has wrong format, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    return;
                }

                if (CheckSequenceAndDate(fileName, 3))
                {
                    //MessageBox.Show("Sequence and date are not outdated");
                }
                else
                {
                    MessageBox.Show("Sequence and date are old");
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: File is either not in sequence or date is invalid, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    return;
                }

                if (IsVendorFileTrailerOK(fileName, lineCount))
                {
                    //MessageBox.Show("trailer is fine");
                }
                else
                {
                    MessageBox.Show("trailer formar is incorrect");
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: File trailer is either missing or has wrong format, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    return;
                }

                if (CheckNumberOfCustomersAndItems(fileName, lineCount))
                {
                    //MessageBox.Show("number of customers and items match");
                }
                else
                {
                    MessageBox.Show("Number of customers or items does not match trailer");
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Number of customers or items does not match trailer, operation aborted" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    return;
                }

                processCustomerItems(fileName);
                

            }
            else//if the user does not select any file or closes the file selection window
                MessageBox.Show("Operation Cancelled");
        }


        bool CheckNumberOfCustomersAndItems(string fileLocation, int lineCount)
        {
            string line = "";
                                      //        name or entiry         address
            string customerNamePattern = @"^((P[A-Za-z][A-Za-z|\s]{29}[A-Za-z][A-Z-a-z|\s]{29})|(B[A-Za-z][A-Za-z|\s]{59}))[A-Za-z0-9][\w|\s]{29},[A-Za-z][A-Za-z|\s]{19}[A-Za-z][A-Za-z|\s]{19}\d[\d|\s]{9}[A-Za-z][A-Za-z|\s]{39}\d{4}-\d{2}-\d{2}\d$";
            string customerIDPattern = @"^\d{10}\d{10}[A-Z-a-z][A-Za-z|\s]{9}\d{1,2}$";
            string itemPattern = @"^[\w]{10}-[123]-[\d]{1,}$";

            int numOfCustomerNames = 0;
            int numOfCustomerID = 0;
            int numOfItems = 0;

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileLocation);

            for (int i = 0; i < lineCount; i++)//counts the number of vendor and item lines and exits with the trailer from the fiel stored in the varaible 'line'
            {
                line = SR.ReadLine();

                if (Regex.IsMatch(line, customerNamePattern))
                    numOfCustomerNames++;

                if (Regex.IsMatch(line, customerIDPattern))
                    numOfCustomerID++;

                if (Regex.IsMatch(line, itemPattern))
                    numOfItems++;
            }
            SR.Close();
            //MessageBox.Show("Customer Names: " + numOfCustomerNames + "\nnumOfCustomerID: " + numOfCustomerID + "\nItems: " + numOfItems);

            line = line.Substring(1, line.Length - 1);

            string[] splitTrailer = line.Split('-');//number of customer in now in splitTrailer[0], number of items is now in splitTrailer[1]

            //MessageBox.Show("line = " + line + "\n First Number = " + splitTrailer[0] + "\n Second Number = " + splitTrailer[1]);

           if (numOfCustomerNames == numOfCustomerID && numOfCustomerID == Convert.ToInt32(splitTrailer[0]) && numOfItems == Convert.ToInt32(splitTrailer[1]))
                return true;
            else
                return false;
        }
        
        public void processCustomerItems(string fileName)
        {
            string line = "";
            //lineCount--;//to avoid processing the trailer

            //varaibles go here

            //string headerPattern = @"^H\d{1,}-\d{4}-\d{2}-\d{2}$";
            string customerPattern = @"^((P[A-Za-z][A-Za-z|\s]{29}[A-Za-z][A-Z-a-z|\s]{29})|(B[A-Za-z][A-Za-z|\s]{59}))[A-Za-z0-9][\w|\s]{29},[A-Za-z][A-Za-z|\s]{19}[A-Za-z][A-Za-z|\s]{19}\d[\d|\s]{9}[A-Za-z][A-Za-z|\s]{39}\d{4}-\d{2}-\d{2}\d$";
            string idPattern = @"^\d{10}\d{10}[A-Z-a-z][A-Za-z|\s]{9}\d{1,2}$";
            //string itemPattern = @"^\w{10}[A|B|C]\d{1,}$";
            string trailerPatter = @"^T\d{1,}-\d{1,}$";

            string outputFileName = "";
            string errorMessage = "";

            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileName);

            line = SR.ReadLine();//to skip the header

            //bool insideTheBlock;
            int numOfLines = 5;

            List<string> fileInformation = new List<string>();

            while (true)
            {
                line = SR.ReadLine();

                if (Regex.IsMatch(line, trailerPatter))//the exit condition
                {
                    //MessageBox.Show("exiting");
                    break;
                }

                if (line == "")//if line is empty, ignore it
                {
                    continue;
                }
                else
                {
                    fileInformation.Add(line);
                }
            }

            CleanUpBeforeFirstCustomer(ref fileInformation);

            //****below is for debugging
            string temp = "";
            for (int i = 0; i < fileInformation.Count; i++)
            {
                temp = temp + "\n" + fileInformation[i];
            }
            //MessageBox.Show(temp);
            //*****

            List<string> itemList = new List<string>();

            while (fileInformation.Count != 0)
            {
                string customerInfo = fileInformation[0];
                //string idInfo = fileInformation[1];

                //fileInformation.RemoveAt(0);
                //fileInformation.RemoveAt(0);

                string idInfo = "";

                fileInformation.RemoveAt(0);//for the customer name   

                if (CleanUpBetweenNumberAndID(ref fileInformation) == false)//this means we've reached the next customer name line without finding the customer id line for the previous one
                {
                    //MessageBox.Show("fileinformation[0] = " + fileInformation[0]);
                    continue;
                }
                else
                {
                    idInfo = fileInformation[0];
                    fileInformation.RemoveAt(0);//for the customer id
                    //MessageBox.Show("idinfo = " + idInfo);
                }

                

                //MessageBox.Show("customer name: " + customerInfo + "\ncustomer id: " + idInfo + "\nfileInformation size = " + fileInformation.Count.ToString());

                //CHECK THE LINE BELOW
                numOfLines = Convert.ToInt32(customerInfo.Substring(192, customerInfo.Length - 192));//check if the declaration for this is in the correct place - also, check wtf is going on with this line becasue it shouldn't work but it's working
                int num = 0;//redundant at this point

                itemList.Clear();

                while (fileInformation.Count != 0 && (!Regex.IsMatch(fileInformation[0], customerPattern) /*|| !Regex.IsMatch(fileInformation[0], idPattern)*/))//the order of the two conditions is important - do not change this. The count must be checked before
                {
                    num++; //not used for anything 

                    //MessageBox.Show("inside: " + fileInformation[0]);      
                    itemList.Add(fileInformation[0]);//add the item to another list
                    fileInformation.RemoveAt(0);//remove from the original list
                    //MessageBox.Show("zrhzrh");
                }

                CleanUpTheItemList(ref itemList);

                //for debugging
                string sss = "";
                for (int i = 0; i < itemList.Count; i++)
                {
                    sss += itemList[i] + "\n";
                }
                //MessageBox.Show("item list: " + sss);

                DateTime DTtemp;

                int discount = Convert.ToInt32(idInfo.Substring(30, (idInfo.Length - 30) ));

                if (/*num != numOfLines*/ itemList.Count != numOfLines || numOfLines >= 7 || numOfLines <= 0)
                {
                    //MessageBox.Show("numbers doooooon't match");//skip the vendor and its items

                    outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Customer " + idInfo.Substring(0, 10) + " has provided an incorrect number of items, customer ignored" + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);
                    
                }
                else
                    if (!DateTime.TryParseExact(customerInfo.Substring(182, 10), "yyyy-MM-dd", null, DateTimeStyles.None, out DTtemp))
                    {                        
                        outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Customer " + customerInfo.Substring(1, 60) + " has made an order on an invalid date, customer ignored" + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);
                    }
                    else
                        if (!(discount >= 0 && discount <= 50))
                        {
                            outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                            errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Customer " + customerInfo.Substring(1, 60) + " has invalid discount value, customer ignored" + Environment.NewLine;
                            WriteToFile(outputFileName, errorMessage);
                        }
                        else//this is where the items are processed
                        {
                            subtotal = 0;
                            bool listToggle = false;

                            bool warehouseARequest = false;
                            bool warehouseBRequest = false;
                            bool warehouseCRequest = false;

                            //MessageBox.Show("process");                            

                            for (int i = 0; i < itemList.Count; i++)
                            {                                
                                List<string> values;
                                values = itemList[i].Split('-').ToList<string>();

                                //MessageBox.Show("ItemID: " + values[0] + "\nWarehouse: " + values[1] + "\nQuantity: " + values[2]);
                                char warehouse = Convert.ToChar(values[1]);
                                int count = Convert.ToInt32(values[2]);

                                if (DoesItemExistInDatabase(values[0]))//check if items exists in catalogue table
                                {
                                    //MessageBox.Show("Item exists in catalogue");

                                    if (DoesItemExistInWarehouseDatabase(values[0], warehouse, count))//check if item exists in warehouses
                                    {
                                        listToggle = true;
                                        //MessageBox.Show("Item " + values[0] + " was found in the warehouse");

                                        WriteCustomerInfoToInvoice(customerInfo, idInfo, fileName);//write the custoemr info to the invoice file

                                        if (warehouse == '1' && warehouseARequest == false)
                                        {
                                            warehouseARequest = true;
                                            WriteCustomerInfoToPackingFile(customerInfo, idInfo, fileName, warehouse);
                                        }
                                        else
                                            if (warehouse == '2' && warehouseBRequest == false)
                                            {
                                                warehouseBRequest = true;
                                                WriteCustomerInfoToPackingFile(customerInfo, idInfo, fileName, warehouse);
                                            }
                                            else
                                                if (warehouse == '3' && warehouseCRequest == false)
                                                {
                                                    warehouseCRequest = true;
                                                    WriteCustomerInfoToPackingFile(customerInfo, idInfo, fileName, warehouse);
                                                } 

                                        takeItemsFromWarehouseTable(values[0], Convert.ToChar(values[1]), Convert.ToInt32(values[2]), customerInfo, idInfo, fileName);
                                    }
                                    else//the entire things goes on backorder
                                    {
                                        outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                                        errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: No inventory was found for item " + values[0] + " in warehouse " + values[1] + Environment.NewLine;
                                        WriteToFile(outputFileName, errorMessage);

                                        CreateNewBackOrder(customerInfo, idInfo, values[0], count, warehouse);
                                    }
                                }
                                else//special order
                                {
                                    //MessageBox.Show("Item does not exist in catalogue - special order");
                                    CreateSpecialOrder(customerInfo, idInfo, values[0], count);//message will be written to analyst report in CreateSpecialOrder
                                }                               
                            }
                            if (listToggle == true)
                            {                                
                                WriteTotalsToInvoice(fileName, idInfo);
                            }
                        }
            }

            MessageBox.Show("Processing file complete, check analyst report for further detail");

            btnVendorShipment.Enabled = true;
            btnCustomerOrders.Enabled = false;

            ChangeVendorCustomerToggle(0);


            CheckForLowInventory();//the alerts

            //MessageBox.Show("Reached the end!!!");
            //SR.Close();
        }

        private void ChangeVendorCustomerToggle(int toggle)
        {
            List<string> values;
            string path = "StorageFile.txt";
            string line = "";

            using (StreamReader sr = File.OpenText(path))//get toggle variable from the storage file
            {
                string str = "";
                str = sr.ReadLine();

                values = str.Split(',').ToList<string>();

                values[9] = toggle.ToString();
            }

            line = string.Join(",", values.ToArray());

            using (StreamWriter sw = File.CreateText(path))//update the toggle variable in the storage file
            {
                sw.WriteLine(line);
                sw.Close();
            }
        }

        private bool CleanUpBetweenNumberAndID(ref List<string> myList)
        {
            string customerNamePattern = @"^((P[A-Za-z][A-Za-z|\s]{29}[A-Za-z][A-Z-a-z|\s]{29})|(B[A-Za-z][A-Za-z|\s]{59}))[A-Za-z0-9][\w|\s]{29},[A-Za-z][A-Za-z|\s]{19}[A-Za-z][A-Za-z|\s]{19}\d[\d|\s]{9}[A-Za-z][A-Za-z|\s]{39}\d{4}-\d{2}-\d{2}\d$";
            string customerIDPattern = @"^\d{10}\d{10}[A-Z-a-z][A-Za-z|\s]{9}\d{1,2}$";

            for (; ; )
            {
                if (Regex.IsMatch(myList[0], customerIDPattern))
                {
                    return true;
                }
                if (Regex.IsMatch(myList[0], customerNamePattern))
                {
                    return false;
                }
                else
                    if (myList.Count != 0)
                    {
                        myList.RemoveAt(0);
                    }
                    else
                        return false;

            }
        }

        private void CleanUpBeforeFirstCustomer(ref List<string> myList)//removes any bad lines before the first vendor is encountered
        {
            string customerPattern = @"^((P[A-Za-z][A-Za-z|\s]{29}[A-Za-z][A-Z-a-z|\s]{29})|(B[A-Za-z][A-Za-z|\s]{59}))[A-Za-z0-9][\w|\s]{29},[A-Za-z][A-Za-z|\s]{19}[A-Za-z][A-Za-z|\s]{19}\d[\d|\s]{9}[A-Za-z][A-Za-z|\s]{39}\d{4}-\d{2}-\d{2}\d$";

            //string customerPattern = @"^[B|P][a-zA-Z]{60}\w{1,30},[a-zA-Z]{1,20}[a-zA-Z]{1,20}\d{1,10}[a-zA-Z]{1,40}\d{4}-\d{2}-\d{2}\d{1}$";
            //string idPattern = @"^\w{10}\w{10}[a-zA-Z]{1,10}\d{1}$";

            for (; ; )
            {
                if (Regex.IsMatch(myList[0], customerPattern)/* || Regex.IsMatch(myList[0], idPattern)*/)
                {
                    break;
                }
                else
                {
                    //MessageBox.Show(myList[0] + "\n" + (Regex.IsMatch(myList[0], vendorPattern)).ToString());
                    myList.RemoveAt(0);
                }
            }
        }
          
        public bool DoesItemExistInWarehouseDatabase(string itemID, char warehouse, int count)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True");

            try
            {
                SqlCommand cmd = new SqlCommand();
                Object returnValue;
                bool located = false;

                switch (warehouse)
                {
                    case '1':
                        cmd.CommandText = "SELECT COUNT(*) FROM WarehouseA WHERE ItemID = @varItemID";
                        break;
                    case '2':
                        cmd.CommandText = "SELECT COUNT(*) FROM WarehouseB WHERE ItemID = @varItemID";
                         break;
                    case '3':
                        cmd.CommandText = "SELECT COUNT(*) FROM WarehouseA WHERE ItemID = @varItemID";//AND CurrentQuantity >= @itemQty
                        break;
                    default:
                        located = false;
                        break;
                }

                cmd.Parameters.AddWithValue("@varItemID", itemID);
                //cmd.Parameters.AddWithValue("@itemQty", count);
                cmd.CommandType = CommandType.Text;
                cmd.Connection = con;

                con.Open();
                returnValue = cmd.ExecuteScalar();//returns something of type 'object'

                if ((int)returnValue != 0)//if items exists, return true
                {
                    //MessageBox.Show(returnValue.ToString());
                    located = true;
                }

                return located;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public void LoadItemsToOrderHistoryTable(string itemID, int itemQuantity, string idInfo)
        {
            SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True");

            string customerID = idInfo.Substring(0, 10);
            string orderID = idInfo.Substring(10, 10);

            SqlCommand insertInfo = new SqlCommand();

            insertInfo.CommandType = CommandType.Text;
            insertInfo.Connection = con;

            con.Open();

            insertInfo.CommandText = "INSERT INTO OrderHistory VALUES (@orderID, @customerID, @itemID, @itemQty)";
            insertInfo.Parameters.AddWithValue("@orderID", orderID);
            insertInfo.Parameters.AddWithValue("@customerID", customerID);
            insertInfo.Parameters.AddWithValue("@itemID", itemID);
            insertInfo.Parameters.AddWithValue("@itemQty", itemQuantity);

            insertInfo.ExecuteNonQuery();

        }

        public void takeItemsFromWarehouseTable(string itemID, char warehouse, int count, string customerInfo, string idInfo, string fileName)
        {
            int originalCount = count;

            List<int> itemBins = new List<int>();
            List<int> itemQuantity = new List<int>();
            int inventoryAvailable = 0;

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    string query = "";
                    if (warehouse == '1')
                    {
                        query = @"SELECT * 
                              FROM WarehouseA
                              WHERE ItemID = @itemID";
                    }
                    else
                        if (warehouse == '2')
                        {
                            query = @"SELECT * 
                                  FROM WarehouseB
                                  WHERE ItemID = @itemID";
                        }
                        else
                        {
                            query = @"SELECT * 
                                  FROM WarehouseC
                                  WHERE ItemID = @itemID";
                        }

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@itemID", itemID);

                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        itemBins.Add(Convert.ToInt32(reader["BinID"]));
                                        itemQuantity.Add(Convert.ToInt32(reader["CurrentQuantity"]));

                                        inventoryAvailable += Convert.ToInt32(reader["CurrentQuantity"]);

                                        //MessageBox.Show(inventoryAvailable.ToString());
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //222
            }
            
            while(itemBins.Count != 0)
            {
                if (count == 0)
                    break;

                if (itemQuantity[0] >= count)
                {
                    //update itemQuantity[i] - set it to itemQuantity[i] - count
                    UpdateItemInWarehouse(itemBins[0], warehouse, (itemQuantity[0] - count), itemID, itemQuantity[0]);

                    itemBins.RemoveAt(0);
                    itemQuantity.RemoveAt(0);

                    count = 0;
                }
                else
                    if (count > itemQuantity[0])
                    {
                        count -= itemQuantity[0];

                        //update itemQuantity[i] - set it to null
                        UpdateItemInWarehouse(itemBins[0], warehouse, 0, itemID, itemQuantity[0]);

                        itemBins.RemoveAt(0);
                        itemQuantity.RemoveAt(0);                        
                    }
            }

            if (count > 0)
            {
                //MessageBox.Show("Customer requested more items than we have in the inventory - backorder   " + count);

                CreateNewBackOrder(customerInfo, idInfo, itemID, count, warehouse);

                LoadItemsToOrderHistoryTable(itemID, (originalCount - count), idInfo);//345

                //InsertNewBackOrder(
            }
            else
            {
                LoadItemsToOrderHistoryTable(itemID, originalCount, idInfo);//345
            }

            WriteItemInfoToInvoice(itemID, (originalCount - count), count, fileName);
        }
        
        private void UpdateItemInWarehouse(int binID, char warehouse, int newQuantity, string itemID, int previousQuantity)
        {
            //MessageBox.Show("binID = " + binID + "\nwarehouse = " + warehouse + "\nnewquantity = " + newQuantity + "\nitemId = " + itemID + "\npreviousQuantity = " + previousQuantity);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    string query = "";
                    if (warehouse == '1' && newQuantity > 0)
                    {
                        query = @"UPDATE WarehouseA 
                                  SET CurrentQuantity = @CQ
                                  WHERE BinID = @binID";

                        WriteItemInfoToPackingFile(itemID, binID, (previousQuantity - newQuantity), warehouse);
                    }
                    else
                        if (warehouse == '2' && newQuantity > 0)
                        {
                            query = @"UPDATE WarehouseB
                                      SET CurrentQuantity = @CQ
                                      WHERE BinID = @binID";

                            WriteItemInfoToPackingFile(itemID, binID, (previousQuantity - newQuantity), warehouse);
                        }
                        else
                            if (warehouse == '3' && newQuantity > 0)
                            {
                                query = @"UPDATE WarehouseC
                                          SET CurrentQuantity = @CQ
                                          WHERE BinID = @binID";

                                WriteItemInfoToPackingFile(itemID, binID, (previousQuantity - newQuantity), warehouse);
                            }
                            else
                                if (warehouse == '1' && newQuantity == 0)
                                {
                                    query = @"UPDATE WarehouseA 
                                              SET CurrentQuantity = NULL,
                                              ItemId = NULL
                                              WHERE BinID = @binID";

                                    WriteItemInfoToPackingFile(itemID, binID, previousQuantity, warehouse);
                                }
                                else
                                    if (warehouse == '2' && newQuantity == 0)
                                    {
                                        query = @"UPDATE WarehouseB
                                                  SET CurrentQuantity = NULL,
                                                  ItemId = NULL
                                                  WHERE BinID = @binID";

                                        WriteItemInfoToPackingFile(itemID, binID, previousQuantity, warehouse);
                                    }
                                    else
                                    {
                                        query = @"UPDATE WarehouseC
                                                  SET CurrentQuantity = NULL,
                                                  ItemId = NULL
                                                  WHERE BinID = @binID";

                                        WriteItemInfoToPackingFile(itemID, binID, previousQuantity, warehouse);
                                    }
                    //MessageBox.Show(query);                 

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        if (newQuantity != 0)
                        {
                            cmd.Parameters.AddWithValue("@CQ", newQuantity);
                        }
                        cmd.Parameters.AddWithValue("@binID", binID);

                        cmd.ExecuteNonQuery();

                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Customer File: Sold " + (previousQuantity - newQuantity) + " of item " + itemID + " from bin " + binID + " at warehouse " + warehouse + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CreateNewBackOrder(string customerInfo, string idInfo, string itemID, int count, char warehouse)
        {
            Guid g = Guid.NewGuid();
            string backOrderID = g.ToString();
            backOrderID = backOrderID.Substring(0, 10);
            string customerAddress = customerInfo.Substring(61, 121); 
            string customerID = idInfo.Substring(0, 10);           

            DateTime date;
            DateTime.TryParseExact(customerInfo.Substring(182, 10), "yyyy-MM-dd", null, DateTimeStyles.None, out date);

            //MessageBox.Show(date.ToString());
            //MessageBox.Show("date = " + customerInfo.Substring(182, 10).ToString());

            //MessageBox.Show("back order id = " + backOrderID);
            //MessageBox.Show("address = " + customerAddress);
            //MessageBox.Show("customer id = " + customerID);
            //MessageBox.Show("item id = " + itemID);
            //MessageBox.Show("item quantity " + count);
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    string query = @"INSERT INTO BackOrder (BackOrderID,CustomerID,CustomerAddress,ItemID,ItemQuantity,OrderDate,Warehouse)
                                     VALUES(@backOrderID,@customerID,@customerAdress,@itemID,@itemQuantity,@date,@warehouse)";


                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {

                        cmd.Parameters.AddWithValue("@backOrderID", backOrderID);
                        cmd.Parameters.AddWithValue("@customerID", customerID);
                        cmd.Parameters.AddWithValue("@customerAdress", customerAddress);
                        cmd.Parameters.AddWithValue("@itemID", itemID);
                        cmd.Parameters.AddWithValue("@itemQuantity", count);
                        cmd.Parameters.AddWithValue("@date", date);
                        cmd.Parameters.AddWithValue("@warehouse", warehouse);

                        cmd.ExecuteNonQuery();

                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Back Order: Put " + count + " of item " + itemID + " on back order for customer " + customerID + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);

                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }






        }

        private void CreateSpecialOrder(string customerInfo, string idInfo, string itemID, int count)
        {
            int specialOrderID = 0;
            List<string> values;
            string path = "StorageFile.txt";

            using (StreamReader sr = File.OpenText(path))//get previous sequence from the storage file
            {
                string str = "";
                str = sr.ReadLine();

                values = str.Split(',').ToList<string>();

                specialOrderID = Convert.ToInt32(values[9]);
                sr.Close();
            }

            string customerAddress = customerInfo.Substring(61, 121); 
            string customerID = idInfo.Substring(0, 10);
            string itemName = "SO" + specialOrderID.ToString();

            DateTime date;
            DateTime.TryParseExact(customerInfo.Substring(182, 10), "yyyy-MM-dd", null, DateTimeStyles.None, out date);
            
            //MessageBox.Show(date.ToString());
            //MessageBox.Show("date = " + customerInfo.Substring(182, 10).ToString());

            //MessageBox.Show("back order id = " + specialOrderID);
            //MessageBox.Show("address = " + customerAddress);
            //MessageBox.Show("customer id = " + customerID);
            //MessageBox.Show("item id = " + itemID);
            //MessageBox.Show("item quantity " + count);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    string query = @"INSERT INTO SpecialOrder (SpecialOrderID,CustomerID,CustomerAddress,ItemID,ItemName,ItemQuantity,OrderDate)
                                     VALUES(@specialOrderID,@customerID,@customerAdress,@itemID,@itemName,@itemQuantity,@date)";
                    
                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@specialOrderID", (specialOrderID + 1));
                        cmd.Parameters.AddWithValue("@customerID", customerID);
                        cmd.Parameters.AddWithValue("@customerAdress", customerAddress);
                        cmd.Parameters.AddWithValue("@itemID", itemID);
                        cmd.Parameters.AddWithValue("@itemName", itemName);
                        cmd.Parameters.AddWithValue("@itemQuantity", count);
                        cmd.Parameters.AddWithValue("@date", date);                        

                        cmd.ExecuteNonQuery();

                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Special Order: Put " + count + " of item " + itemID + " on special order for customer " + customerID + Environment.NewLine;
                        WriteToFile(outputFileName, errorMessage);

                        con.Close();
                    }
                }                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            specialOrderID++;
            values[9] = specialOrderID.ToString();
            //values[2] = DTCurrent.Date.ToString("yyyy-MM-dd");

            string line = string.Join(",", values.ToArray());

            //MessageBox.Show("new line:  " + line);            

            using (StreamWriter sw = File.CreateText(path))//update the date in the storage file
            {
                sw.WriteLine(line);
                sw.Close();
            }                        
        }


        //****
        private void WriteCustomerInfoToInvoice(string customerInfo, string idInfo, string fileName)
        {
            string line = "";

            line += Environment.NewLine + Environment.NewLine;
            line += "Customer Name: " + customerInfo.Substring(1, 60) + Environment.NewLine;
            line += "Address: " + customerInfo.Substring(61, 30) + Environment.NewLine;
            line += customerInfo.Substring(92, 90) + Environment.NewLine + Environment.NewLine;
            line += "Customer ID: " + idInfo.Substring(0, 10) + Environment.NewLine;
            line += "Order ID: " + idInfo.Substring(10, 10) + Environment.NewLine;
            line += "Order Date: " + customerInfo.Substring(182, 10) + Environment.NewLine;
            line += "Shipping Date: " + GetDateFromCustomerFile(fileName) + Environment.NewLine;
            line += "Payment Type: " + idInfo.Substring(20, 10) + Environment.NewLine + Environment.NewLine;   
            line += "Item ID     Item Name  Quantity   Price   Item total" + Environment.NewLine;

            string outputFileName = @"output\Invoice " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
            WriteToFile(outputFileName, line);
        }

        private string GetDateFromCustomerFile(string fileName)
        {
            string line = "";

            using (StreamReader SR = new StreamReader(fileName))
            {
                line = SR.ReadLine();               
               
                SR.Close();
            }

            string[] splitHeader = line.Split('-');
           
            string currentDate = splitHeader[1] + '-' + splitHeader[2] + '-' + splitHeader[3];

            //MessageBox.Show("date from file = " + currentDate);

            return (currentDate);
        }

        private void WriteItemInfoToInvoice(string itemID, int quantity, int backorder, string fileName)
        {
            string line = Environment.NewLine;

            decimal total = quantity * GetItemPrice(itemID);

            line += itemID + "   " + GetItemName(itemID) + "          " + quantity + "   " + GetItemPrice(itemID) + "   " + total;

            if (backorder > 0)
            {
                line += " --- " + backorder + " items were placed on backorder"; 
            }

            subtotal += total;

            string outputFileName = @"output\Invoice " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
            WriteToFile(outputFileName, line);
        }

        private string GetItemName(string itemID)
        {
            string name = "";
            //needs a try catch block
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = "SELECT * FROM Catalogue WHERE ItemID = @itemID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@itemID", itemID);

                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        name = reader["ItemName"].ToString();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return name;
        }
        
        private decimal GetItemPrice(string itemID)
        {
            decimal price = 0;
            //needs a try catch block
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = "SELECT * FROM Catalogue WHERE ItemID = @itemID";

                    using (SqlCommand cmd = new SqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@itemID", itemID);

                        try
                        {
                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                if (reader != null)
                                {
                                    while (reader.Read())
                                    {
                                        price = Convert.ToDecimal(reader["ItemPrice"]);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return price;
        }

        private void WriteTotalsToInvoice(string fileName, string idInfo)
        {
            string line = Environment.NewLine + Environment.NewLine;

            decimal discount = (Convert.ToDecimal(idInfo.Substring(30, (idInfo.Length - 30))))/100;

            //MessageBox.Show(discount.ToString());

            decimal tax = 0.06M;

            discount = subtotal * discount;
            decimal orderTotal = subtotal - discount;
            decimal taxValue = tax * orderTotal;
            
            line += "Subtotal: " + subtotal + Environment.NewLine;
            line += "Discount: " + discount + Environment.NewLine;
            line += "Order Total: " + orderTotal + Environment.NewLine;
            line += "Tax 6%: " + taxValue + Environment.NewLine;
            line += "Amount due in " + idInfo.Substring(20, 10) + ": " + (orderTotal + taxValue) + Environment.NewLine;
            
            string outputFileName = @"output\Invoice " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
            WriteToFile(outputFileName, line);
        }

        private void WriteCustomerInfoToPackingFile(string customerInfo, string idInfo, string fileName, char warehouse)
        {
            string line = "";

            line += Environment.NewLine + Environment.NewLine;
            line += "Customer ID: " + idInfo.Substring(0, 10) + Environment.NewLine;
            line += "Customer Name: " + customerInfo.Substring(1, 60) + Environment.NewLine;
            line += "Address: " + customerInfo.Substring(61, 30) + Environment.NewLine;
            line += customerInfo.Substring(92, 90) + Environment.NewLine + Environment.NewLine;
            line += "Shipping Date: " + GetDateFromCustomerFile(fileName) + Environment.NewLine;
            line += "Order ID: " + idInfo.Substring(10, 10) + Environment.NewLine;
            line += "Order Date: " + customerInfo.Substring(182, 10) + Environment.NewLine + Environment.NewLine;
                        
            line += "Item ID     Item Name  Location    Quantity" + Environment.NewLine;

            string outputFileName = @"output\Warehouse " + warehouse + " Packing Slip " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
            WriteToFile(outputFileName, line);

        }

        private void WriteItemInfoToPackingFile(string itemID, int warehouseBin, int quantity, char warehouse)
        {
            string line = Environment.NewLine;

            //decimal total = quantity * GetItemPrice(itemID);

            line += itemID + "   " + GetItemName(itemID) + "    " + warehouseBin + "          " + quantity;

            string outputFileName = @"output\Warehouse " + warehouse + " Packing Slip " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
            WriteToFile(outputFileName, line);

        }

        
        //customer file methods stop here
        //*****************************************************************************************************
        //*****************************************************************************************************

        private void CheckForLowInventory()
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

            //MessageBox.Show("threshold = " + threshold);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();                    

                    List<string> queries = new List<string>();

                    queries.Add(@"SELECT * 
                                   FROM WarehouseA
                                   WHERE CurrentQuantity < @threshold");

                    queries.Add(@"SELECT * 
                                   FROM WarehouseB
                                   WHERE CurrentQuantity < @threshold");

                    queries.Add(@"SELECT * 
                                   FROM WarehouseC
                                   WHERE CurrentQuantity < @threshold");

                    List<char> warehouse = new List<char>();

                    warehouse.Add('1');
                    warehouse.Add('2');
                    warehouse.Add('3');

                    for (int i = 0; i < 3; i++)
                    {
                        //MessageBox.Show("inside the for loop");
                        using (SqlCommand cmd = new SqlCommand(queries[i], con))
                        {
                            cmd.Parameters.AddWithValue("@threshold", threshold);

                            try
                            {
                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader != null)
                                    {
                                        while (reader.Read())
                                        {
                                            //MessageBox.Show("inside reader");

                                            string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                                            string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Alert: Item " + reader["ItemID"] + " in bin " + reader["BinID"] + " at warehouse " + warehouse[i] + " is below the threshold of " + threshold.ToString() + Environment.NewLine;
                                            WriteToFile(outputFileName, errorMessage);
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }                        
        }

        //****************************************
        //private void btnShowOrders_Click(object sender, EventArgs e)
        //{
        //    Orders o = new Orders();
        //    o.ShowDialog();
        //}

        private void btnAlertsSetup_Click(object sender, EventArgs e)
        {
            Alerts a = new Alerts();
            a.ShowDialog();
        }

        //*****************************************************************************************************
        //*****************************************************************************************************


        private void ProcessBackOrder(string fileName)
        {
            List<string> fullfilledOrders = new List<string>();
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                {
                    con.Open();

                    //need three cases for warehouses for the query below
                    string query = @"SELECT * FROM BackOrder
                                     ORDER BY OrderDate";

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
                                        //build customerInfo and idInfo here and pass them to the ProcessBackOrder method
                                        //need to delete the tuple we just read
                                        
                                        DateTime dt = Convert.ToDateTime(reader[5]);

                                        string customerInfo = "PNA                                                          " + reader["CustomerAddress"].ToString() + dt.ToString("yyyy-MM-dd") + "1";
                                        string idInfo = reader["CustomerID"].ToString() + reader["BackOrderID"].ToString() + "NA        " + "0";
                                        string itemID = reader["ItemID"].ToString();
                                        int count = Convert.ToInt32(reader["ItemQuantity"]);
                                        char warehouse = Convert.ToChar(reader["Warehouse"]);                                        

                                        if (ProcessBackOrderItem(customerInfo, idInfo, itemID, count, warehouse, fileName) == true)
                                        {
                                            fullfilledOrders.Add(reader["BackOrderID"].ToString());
                                        }
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            string queryDelete = "";
            for (int i = 0; i < fullfilledOrders.Count; i++)//loop through all the backOrderID's in fullfilledOrders and delete them from database
            {
                queryDelete = @"DELETE FROM BackOrder
                                WHERE BackOrderID = @backOrderID";
                try
                {
                    using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                    {
                        con.Open();
                        using (SqlCommand c = new SqlCommand(queryDelete, con))
                        {
                            c.Parameters.AddWithValue("@backOrderID", fullfilledOrders[i]);
                            c.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

                //MessageBox.Show(fullfilledOrders[i]);
            }

        }
        
        private bool ProcessBackOrderItem(string customerInfo, string idInfo, string itemID, int count, char warehouse, string fileName)
        {
            //MessageBox.Show("process back order item");    

            if (DoesItemExistInDatabase(itemID))//check if items exists in catalogue table
            {
                //MessageBox.Show("Item exists in catalogue");

                if (DoesItemExistInWarehouseDatabase(itemID, warehouse, count))//check if item exists in warehouses
                {
                    //listToggle = true;
                    //MessageBox.Show("Item " + values[0] + " was found in the warehouse");

                    WriteCustomerInfoToInvoice(customerInfo, idInfo, fileName);//write the custoemr info to the invoice file
                                      
                    WriteCustomerInfoToPackingFile(customerInfo, idInfo, fileName, warehouse);
                 
                    takeItemsFromWarehouseTable(itemID, warehouse, count, customerInfo, idInfo, fileName);

                    return true;
                }
                else//the entire things goes on backorder
                {
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " - Back order: No inventory was found for item " + itemID + " in warehouse " + warehouse + Environment.NewLine;
                    WriteToFile(outputFileName, errorMessage);

                    CreateNewBackOrder(customerInfo, idInfo, itemID, count, warehouse);

                    return true;
                }
            }
            else//special order
            {
                //MessageBox.Show("Item does not exist in catalogue - special order");
                //message will be written to analyst report in CreateSpecialOrder
                CreateSpecialOrder(customerInfo, idInfo, itemID, count);

                return false;
            }   
        }
        //**********************************************************************************************
        //**********************************************************************************************
        //**********************************************************************************************
        //**********************************************************************************************
        //return orders below

        private void btnReturnOrders_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.

            if (result == DialogResult.OK) // Test result.
            {
                string fileName = openFileDialog1.FileName;

                bool correctHeaderContent = checkHeader(fileName);
                int numOfLines = 0;
                int lineCount = CountLines(fileName);
                lineCount = lineCount - 2;
                bool correctTailContent = checkTailFormat(fileName, lineCount);
                bool correctSequence = CheckSequenceAndDate(fileName, 8);

                if (!correctHeaderContent)
                {
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " Header is not in correct format.";
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }
                if (!correctTailContent)
                {
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " Trailer is not in correct format.";
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }
                if (!correctSequence)
                {
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " Sequence number is off.";
                    WriteToFile(outputFileName, errorMessage);

                    return;
                }
                try
                {
                    StreamReader reader = new StreamReader(fileName);

                    string line = reader.ReadLine();

                    while ((line = reader.ReadLine()) != null) //loops until end of file or tail is reached
                    {
                        if (line.Length != 0)
                        {
                            if (line[0] == 'T')
                            {
                                break;
                            }

                            bool validLine = checkLine(line); //Validates that line format is valid

                            if (validLine)
                            {
                                using (SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename = |DataDirectory|WarehouseDB.mdf;Integrated Security=True"))
                                {
                                    con.Open();

                                    //need three cases for warehouses for the query below
                                    string query = "SELECT COUNT(*) FROM OrderHistory WHERE OrderID = @orderID AND "
                                                + "CustomerID = @customerID AND ItemID = @itemID AND ItemQuantity = @itemQty";

                                    //Checks file OrderID to OrderIDs listed in database
                                    string orderID = line.Substring(0, 10);
                                    string customerID = line.Substring(10, 10);
                                    string itemID = line.Substring(20, 10);

                                    string itemQtyStr = line.Substring(30, (line.Length - 30));
                                    int itemQty = Convert.ToInt32(itemQtyStr);

                                    int rowExists = 0;

                                    //Use SQL to check for matching items
                                    SqlCommand checkItemInfo = new SqlCommand(query, con);

                                    checkItemInfo.Parameters.AddWithValue("@orderID", orderID);
                                    checkItemInfo.Parameters.AddWithValue("@customerID", customerID);
                                    checkItemInfo.Parameters.AddWithValue("@itemID", itemID);
                                    checkItemInfo.Parameters.AddWithValue("@itemQty", itemQty);

                                    //Checks the number of rows that exist with restrictions in place
                                    rowExists = Convert.ToInt32(checkItemInfo.ExecuteScalar());

                                    //MessageBox.Show(rowExists.ToString());

                                    if (rowExists > 0)
                                    {
                                        query = "DELETE FROM OrderHistory WHERE OrderID = @orderID AND "
                                            + "CustomerID = @customerID AND ItemID = @itemID AND ItemQuantity = @itemQty";

                                        SqlCommand deleteItem = new SqlCommand(query, con);

                                        deleteItem.Parameters.AddWithValue("@orderID", orderID);
                                        deleteItem.Parameters.AddWithValue("@customerID", customerID);
                                        deleteItem.Parameters.AddWithValue("@itemID", itemID);
                                        deleteItem.Parameters.AddWithValue("@itemQty", itemQty);

                                        deleteItem.ExecuteNonQuery();

                                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " " + customerID + " " + orderID + " " + itemQtyStr +
                                                                " " + itemID + " was successfully returned." + Environment.NewLine;

                                        WriteToFile(outputFileName, errorMessage);

                                        MessageBox.Show("Return file done processing. Check the report for more details.");
                                    }
                                    else
                                    {
                                        string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                                        string errorMessage = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss tt") + " " + customerID + " " + orderID + " " + itemID + " " + itemQtyStr +
                                                                Environment.NewLine + itemQty + " " + itemID + " does not exist as an order.";

                                        WriteToFile(outputFileName, errorMessage);

                                        MessageBox.Show("Return file done processing. Check the report for more details.");
                                    }
                                }
                            }
                        }
                    }
                    reader.Close();
                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        public bool checkLine(string line)
        {
            string middle = @"^\w{31,}$";
            if (Regex.IsMatch(line, middle))
            {
                return true;
            }
            else
            {
                string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                string errorMessage = "Error " + line + " is not in the correct format.";
                WriteToFile(outputFileName, errorMessage);

                MessageBox.Show("Return file done processing. Check the report for more details.");

                return false;
            }
        }

        public bool checkHeader(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(fileName);
                string line = reader.ReadLine();

                string header = @"^H\d{1,}-\d{4}-\d{2}-\d{2}$";

                if (!Regex.IsMatch(line, header))
                {
                    string outputFileName = @"output\Analyst Report " + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
                    string errorMessage = "Error " + line + " is not in the correct format.";
                    WriteToFile(outputFileName, errorMessage);

                    MessageBox.Show("Return file done processing. Check the report for more details.");

                    return false;
                }
                else
                {
                    int dateStartPos = line.Length - 10;
                    line = line.Substring(dateStartPos, 10);
                    DateTime dateValue;
                    return (DateTime.TryParseExact(line, "yyyy-MM-dd", null, DateTimeStyles.None, out dateValue));
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error. Could not read file.");
                return false;
            }
            return true;
        }

        public bool checkTailFormat(string fileName, int lineCount)
        {
            System.IO.StreamReader SR;
            SR = new System.IO.StreamReader(fileName);

            string pattern = @"^T\d{1,}$";
            string trailer = File.ReadLines(fileName).Last();

            if (Regex.IsMatch(trailer, pattern))
            {
                int numOfLines = Convert.ToInt32(trailer.Substring(1, trailer.Length - 1));

                if (lineCount == numOfLines)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}


     


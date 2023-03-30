using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WikiApplication
{
    public partial class WikiApplication : Form
    {   //Ather: Jun Sumida
        //ID: 30057205
        //Date: 01/03/2023
        //AT 1 "Wiki Application"

        public WikiApplication()
        {
            InitializeComponent();
            InitializeArray();
        }
        //9.1 Create a global 2D string array, use static variable for the dimensions
        static int rows = 12;
        static int columns = 4;
        int ptr = 0;
        string[,] DataTable = new string[rows, columns]; // declaration 2D

        private void Form1_Load(object sender, EventArgs e)
        {
            //enable unecessary buttons when Form1 open
            DeleteButton.Enabled = false;
            SearchButton.Enabled = false;
            SortButton.Enabled = false;
            SaveButton.Enabled = false;
            EditButton.Enabled = false;
            AddButton.Enabled = false;
        }
        #region DisplayTable & Initialize Array  
        private void DisplayTable()
        {
            ListViewDisplay.Items.Clear();
            SortTable();
            for (int x = 0; x < rows; x++)
            {
                ListViewItem lvi = new ListViewItem(DataTable[x, 0]);
                lvi.SubItems.Add(DataTable[x, 1]);
                lvi.SubItems.Add(DataTable[x, 2]);
                lvi.SubItems.Add(DataTable[x, 3]);
                ListViewDisplay.Items.Add(lvi);
            }
            if (ListViewDisplay.Items.Count == 0)
            {
                AddButton.Enabled = false;
                EditButton.Enabled = false;
                DeleteButton.Enabled = false;
                SearchButton.Enabled = false;
                SortButton.Enabled = false;
                SaveButton.Enabled = false;
            }
            else
            {
                if (ListViewDisplay.Items.Count == 12)
                {
                    AddButton.Enabled = false;
                }

                AddButton.Enabled = true;
                EditButton.Enabled = true;
                DeleteButton.Enabled = true;
                SearchButton.Enabled = true;
                SaveButton.Enabled = true;
            }
        }
        private void InitializeArray()
        {
            for (int x = 0; x < rows; x++)
            {
                DataTable[x, 0] = ""; //name
                DataTable[x, 1] = ""; //category
                DataTable[x, 2] = ""; //structure
                DataTable[x, 3] = ""; //definition
            }
            DisplayTable();
        }
        #endregion 
        #region ADD Button
        //9.2 ADD button to store the information from the 4 textboxes into the 2D array.
        private void AddButton_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < rows; x++)
            {
                if (DataTable[x, 0] == "~")
                {
                    // Add data to the DataTable
                    DataTable[x, 0] = TextBoxName.Text;
                    DataTable[x, 1] = TextBoxCategory.Text;
                    DataTable[x, 2] = TextBoxStructure.Text;
                    DataTable[x, 3] = TextBoxDefinition.Text;

                    var result = MessageBox.Show("Proceed with new Record?", "Add new Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (result == DialogResult.Yes)
                        break;
                    else
                    {
                        DataTable[x, 0] = TextBoxName.Text;
                        DataTable[x, 1] = TextBoxCategory.Text;
                        DataTable[x, 2] = TextBoxStructure.Text;
                        DataTable[x, 3] = TextBoxDefinition.Text;
                        break;
                    }
                }
                if (x == rows - 1)
                {
                    toolStripStatusLabel.Text = "Data Table is Full";
                }
            }
            DisplayTable();

            if (!string.IsNullOrEmpty(TextBoxName.Text) &&
                !string.IsNullOrEmpty(TextBoxCategory.Text) &&
                !string.IsNullOrEmpty(TextBoxStructure.Text) &&
                !string.IsNullOrEmpty(TextBoxDefinition.Text) && (ptr < rows))
            {
                //Add data to the ListView
                ListViewItem lvi;
                lvi = ListViewDisplay.Items.Add(TextBoxName.Text);
                lvi.SubItems.Add(TextBoxCategory.Text);
                lvi.SubItems.Add(TextBoxStructure.Text);
                lvi.SubItems.Add(TextBoxDefinition.Text);
            }
            else
            {
                toolStripStatusLabel.Text = "Plese fill in the details";
            }
            ClearTextBox();
            TextBoxName.Focus();
        }
        #endregion 
        #region EDIT Button
        //9.3 EDIT button to modify any information from the 4 textbox into the 2D array.
        private void EditButton_Click(object sender, EventArgs e)
        {
            //check if an item is selected in the ListViewBox
            if (ListViewDisplay.SelectedItems.Count == 0)
            {
                toolStripStatusLabel.Text = "There is no item in the TextBox";
                return;
            }
            //check if item selected from item's index
            int currentItem = ListViewDisplay.SelectedIndices[0];
            if (string.IsNullOrEmpty(TextBoxName.Text) &&
               string.IsNullOrEmpty(TextBoxCategory.Text) &&
               string.IsNullOrEmpty(TextBoxStructure.Text) &&
               string.IsNullOrEmpty(TextBoxDefinition.Text))
            {
                toolStripStatusLabel.Text = "Please select item from the ListViewBox";
                return;
            }
            //if item is in the textbox update in the DataTable 
            if (currentItem > -1)
            {
                DataTable[currentItem, 0] = TextBoxName.Text;
                DataTable[currentItem, 1] = TextBoxName.Text;
                DataTable[currentItem, 2] = TextBoxName.Text;
                DataTable[currentItem, 3] = TextBoxName.Text;

                //update the ListViewItems
                ListViewDisplay.Items[currentItem].SubItems[0].Text = TextBoxName.Text;
                ListViewDisplay.Items[currentItem].SubItems[1].Text = TextBoxCategory.Text;
                ListViewDisplay.Items[currentItem].SubItems[2].Text = TextBoxStructure.Text;
                ListViewDisplay.Items[currentItem].SubItems[3].Text = TextBoxDefinition.Text;

                toolStripStatusLabel.Text = "Items updated successfully";

                //highlight or change the colour of the text                 
            }
            else
            {
                toolStripStatusLabel.Text = "Unable to Edit";
            }
            ClearTextBox();
        }
        #endregion 
        #region DELETE Button
        //9.4 DELETE button that removes all the information from a single entry of array.
        // the user must be prompted before the final deletion occurs.
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (ListViewDisplay.SelectedItems.Count == 0)
            {
                toolStripStatusLabel.Text = "Please select an item from the ListView!";
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete all the information ?", "Delete Record",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    if (ListViewDisplay.Items.Count > 0)
                    //remove all the information from the array                
                    {
                        int currentItems = ListViewDisplay.SelectedIndices[0];
                        DataTable[currentItems, 0] = "~";  //name
                        DataTable[currentItems, 1] = "~";  //category
                        DataTable[currentItems, 2] = "~";  //structure
                        DataTable[currentItems, 3] = "~";  //definition   

                        ClearTextBox();

                        //update the ListViewItems with "~"
                        ListViewDisplay.Items[currentItems].SubItems[0].Text = "~";
                        ListViewDisplay.Items[currentItems].SubItems[1].Text = "~";
                        ListViewDisplay.Items[currentItems].SubItems[2].Text = "~";
                        ListViewDisplay.Items[currentItems].SubItems[3].Text = "~";
                    }
                    else
                    {
                        MessageBox.Show("Could not Delete the items",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        #endregion 
        #region Clear Method
        //9.5 Clear method to clear the four textboxes
        private void ClearTextBox()
        {
            TextBoxName.Clear();
            TextBoxCategory.Clear();
            TextBoxStructure.Clear();
            TextBoxDefinition.Clear();

            TextBoxName.Focus();
        }
        #endregion
        #region Bubble Sort & Swap Method
        //9.6 Bubble Sort to sort the 2D array by Name ascending,
        //     use a separate swap method that passes the array element to be swapped.
        private void SortButton_Click(object sender, EventArgs e)
        {
            SortTable();
        }
        private void SortTable()
        {
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < rows - 1; j++)
                {
                    if (string.Compare(DataTable[j, 0], DataTable[j + 1, 0]) > 0) //CompareOrdinal 
                    {
                        Swap(j, j + 1);
                    }
                }
            }
            //DisplayTable();
        }

        private void Swap(int index1, int index2)
        {
            string temp;
            for (int z = 0; z < columns; z++)
            {
                temp = DataTable[index1, z];
                DataTable[index1, z] = DataTable[index2, z];
                DataTable[index2, z] = temp;
            }
        }
        #endregion
        #region Binary Search
        //9.7 Binary Search for the Name in the 2D array and display the information in the other textboxes
        //    when found, add suitable feedback if the search in not successful and clear the search textbox.
        private void SearchButton_Click(object sender, EventArgs e)
        {
            SortTable();

            if (!string.IsNullOrEmpty(TextBoxSearch.Text))
            {
                bool found = false;
                int min = 0;
                int max = rows - 1;
                string findThis = TextBoxSearch.Text;

                while (min <= max)
                {
                    int mid = ((min + max) / 2);
                    if (findThis.CompareTo(DataTable[mid, 0]) == 0)
                    {
                        found = true;
                        //Display the item in Texboxs
                        TextBoxName.Text = DataTable[mid, 0];
                        TextBoxCategory.Text = DataTable[mid, 1];
                        TextBoxStructure.Text = DataTable[mid, 2];
                        TextBoxDefinition.Text = DataTable[mid, 3];
                        break; //this will break while loop
                    }
                    else if (findThis.CompareTo(DataTable[mid, 0]) < 0)
                    {
                        max = mid - 1;
                    }
                    else
                    {
                        min = mid + 1;
                    }
                }
                if (!found)
                {
                    toolStripStatusLabel.Text = "Item is Not found";
                }
                else
                {
                    toolStripStatusLabel.Text = "Item was found";
                }
            }
            else
            {
                toolStripStatusLabel.Text = "Please enter the item into search textbox";
            }
            TextBoxSearch.Clear();
            TextBoxSearch.Focus();
        }
        #endregion
        #region Display method
        //9.8 Display method that will show Name and Category information in a ListView
        private void DisplayNameCategory()
        {
            for (int x = 0; x < rows; x++)
            {
                for (int y = 0; y < columns; y++)
                {
                    ListViewItem lvi = new ListViewItem(DataTable[x, 0]);
                    lvi.SubItems.Add(DataTable[x, 1]);
                    ListViewDisplay.Items.Add(lvi);
                }
            }
        }
        #endregion
        #region ListViewItems Mouse Click
        //9.9 Create a method so the user can select a definition (name) from the ListView and 
        //    all the information is displayed in the appropriate Textboxes.
        private void ListViewItems_MouseClick(object sender, MouseEventArgs e)
        {
            if (ListViewDisplay.SelectedIndices.Count > 0)
            {
                ListViewItem selectedItem = ListViewDisplay.SelectedItems[0];
                //put the selected listViewItem into textbox and display in the appropriate textboxes.           
                TextBoxName.Text = selectedItem.SubItems[0].Text;
                TextBoxCategory.Text = selectedItem.SubItems[1].Text;
                TextBoxStructure.Text = selectedItem.SubItems[2].Text;
                TextBoxDefinition.Text = selectedItem.SubItems[3].Text;
            }
        }
        #endregion
        #region SAVE Button and Save Record
        //9.10 SAVE button so the information from the 2D array can be written into a binary file.=> "definitions.dot"
        //     which is sorted by Name, ensure the user has the option to select an alternative file.
        //     *Use a file stream and "BinaryWriter" to create the file.
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Application.StartupPath;
            saveFileDialog.Filter = "Binary files (*.bin, *.dat)|*.bin;*.dat";
            saveFileDialog.Title = "Save A DAT file";
            saveFileDialog.DefaultExt = ".dat";
            saveFileDialog.FileName = "defintions.dat";
            //string fileName = saveFileDialog.FileName;

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                SaveRecord(saveFileDialog.FileName);
            }
        }
        private void SaveRecord(string saveFileName)
        {
            //create file stream and binary writer to write file
            BinaryWriter writer;
            try
            {
                using (Stream stream = File.Open(saveFileName, FileMode.Create))
                {
                    using (writer = new BinaryWriter(stream, Encoding.UTF8, false))
                    {
                        //write the dimensions of the array to the file
                        writer.Write(DataTable.GetLength(0));
                        writer.Write(DataTable.GetLength(1));

                        //write elements of DataTable to file
                        for (int x = 0; x < DataTable.GetLength(0); x++)
                        {
                            for (int y = 0; y < DataTable.GetLength(1); y++)
                            {
                                writer.Write(DataTable[x, y]);
                            }
                        }
                    }
                }
                toolStripStatusLabel.Text = "Succesfully saved";
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        #endregion
        #region LOAD Button & Open Record
        //9.11 LOAD button that will read the information from a binary file => "definitions.dot"
        //     into the 2D array, ensure has the option to select an alternative file.
        //     *Use a file stream and "BinaryReader"
        private void LoadButton_Click(object sender, EventArgs e)
        {
            ListViewDisplay.Items.Clear();
            //OpenBianryFile();            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Filter = "Binary files (*.bin, *.dat)|*.bin;*.dat";
            openFileDialog.Title = "Open a DAT file ";
            openFileDialog.DefaultExt = "*.dat";
            openFileDialog.FileName = $"definitions.dat";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                OpenRecord(openFileDialog.FileName);
            }
        }

        private void OpenRecord(string openFileName)
        {
            try
            {
                using (Stream stream = File.Open(openFileName, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        //read the dimensions of the array from the file
                        int rows = reader.ReadInt32();
                        int columns = reader.ReadInt32();

                        //Create a new array with the same dimension as the original array
                        string[,] data = new string[rows, columns];

                        //Read the elements of the array from the file
                        for (int x = 0; x < rows; x++)
                        {
                            for (int y = 0; y < columns; y++)
                            {
                                data[x, y] = reader.ReadString();
                            }
                        }
                        //Assign the data to the original array
                        DataTable = data;
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.ToString());
            }
            DisplayTable();

        }
        #endregion  
    }
}

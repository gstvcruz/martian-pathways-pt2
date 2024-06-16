// 23513 - Diogo Lourenço
// 23521 - Gustavo Cruz

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace apMartianPathways
{
    public partial class FrmPaths : Form
    {
        IHashTable<City> table;
        string fileName = null;
        public FrmPaths()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            if (dlgOpen.ShowDialog() == DialogResult.Cancel)
                return;

            if (rbBucketHashing.Checked)
                table = new BucketHashing<City>();
            else
                if (rbLinearProbing.Checked)
                    table = new LinearProbing<City>();
                else
                    if (rbQuadraticProbing.Checked)
                        table = new QuadraticProbing<City>();
                    else
                        if (rbDoubleHashing.Checked)
                            table = new DoubleHashing<City>();

            fileName = dlgOpen.FileName;
            var file = new StreamReader(fileName);
            while (!file.EndOfStream)
            {
                City city = new City();
                city.ReadRegistry(file);
                table.Insert(city);
            }
            ShowCities();
            file.Close();
        }

        private void FrmPaths_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (fileName == null)
                return;

            var file = new StreamWriter(fileName);
            List<City> cities = table.Content();
            foreach (City c in cities)
                c.WriteData(file);
            file.Close();
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string cap = "Add city";
            try
            {
                if (txtCity.Text.Trim() == string.Empty)
                    throw new ArgumentException();
                City city = new City(txtCity.Text, (double)udX.Value, (double)udY.Value);
                if (table.Insert(city))
                {
                    string msg = "City added.";
                    MessageBoxButtons btn = MessageBoxButtons.OK;
                    MessageBoxIcon ico = MessageBoxIcon.Information;
                    MessageBox.Show(msg, cap, btn, ico);
                }
                else
                {
                    string msg = "City already added.";
                    MessageBoxButtons btn = MessageBoxButtons.OK;
                    MessageBoxIcon ico = MessageBoxIcon.Exclamation;
                    MessageBox.Show(msg, cap, btn, ico);
                }
            }
            catch (NullReferenceException)
            {
                string msg = "No opened file to add a city. Open a file first.";
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon ico = MessageBoxIcon.Error;
                MessageBox.Show(msg, cap, btn, ico);
            }
            catch (ArgumentOutOfRangeException f)
            {
                string msg = f.Message;
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon ico = MessageBoxIcon.Exclamation;
                MessageBox.Show(msg, cap, btn, ico);
            }
            catch (ArgumentException)
            {
                string msg = "City name can't be empty.";
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon ico = MessageBoxIcon.Exclamation;
                MessageBox.Show(msg, cap, btn, ico);
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string cap = "Remove city";
            try
            {
                City city = new City(txtCity.Text, (double)udX.Value, (double)udY.Value);
                if (table.Remove(city))
                {
                    string msg = "City removed.";
                    MessageBoxButtons btn = MessageBoxButtons.OK;
                    MessageBoxIcon ico = MessageBoxIcon.Information;
                    MessageBox.Show(msg, cap, btn, ico);
                }
                else
                {
                    string msg = "City not found.";
                    MessageBoxButtons btn = MessageBoxButtons.OK;
                    MessageBoxIcon ico = MessageBoxIcon.Exclamation;
                    MessageBox.Show(msg, cap, btn, ico);
                }
            } 
            catch (NullReferenceException)
            {
                string msg = "No opened file to remove a city. Open a file first.";
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon ico = MessageBoxIcon.Error;
                MessageBox.Show(msg, cap, btn, ico);
            }
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string cap = "Search city";
            try
            {
                City city = new City(txtCity.Text, (double)udX.Value, (double)udY.Value);
                if (table.Exists(city, out _))
                {
                    string msg = "City found.";
                    MessageBoxButtons btn = MessageBoxButtons.OK;
                    MessageBoxIcon ico = MessageBoxIcon.Information;
                    MessageBox.Show(msg, cap, btn, ico);
                }
                else
                {
                    string msg = "City not found.";
                    MessageBoxButtons btn = MessageBoxButtons.OK;
                    MessageBoxIcon ico = MessageBoxIcon.Exclamation;
                    MessageBox.Show(msg, cap, btn, ico);
                }
            }
            catch (NullReferenceException)
            {
                string msg = "No opened file to search for a city. Open a file first.";
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon ico = MessageBoxIcon.Error;
                MessageBox.Show(msg, cap, btn, ico);
            }
        }

        private void btnList_Click(object sender, EventArgs e)
        {
            string cap = "List cities";
            try
            {
                ShowCities();

            }
            catch (NullReferenceException)
            {
                string msg = "No opened file to list cities. Open a file first.";
                MessageBoxButtons btn = MessageBoxButtons.OK;
                MessageBoxIcon ico = MessageBoxIcon.Error;
                MessageBox.Show(msg, cap, btn, ico);
            }
        }

        private void ShowCities()
        {
            lsbCities.Items.Clear();
            var cities = table.Content();
            foreach (City c in cities)
                lsbCities.Items.Add(c);
        }
    }
}

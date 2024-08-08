using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hardware_Store_Inventory_Management_System
{
    public partial class ItemsForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public ItemsForm()
        {
            InitializeComponent();
            LoadItems();
        }

        public void LoadItems()
        {
            int i = 0;
            dvgItems.Rows.Clear();

            // SQL query to retrieve items based on search criteria
            cm = new SqlCommand("SELECT * FROM tbItems WHERE CONCAT(iname, iprice, idescription, icategory) LIKE '%" + txtSearch.Text + "%'", con);
            con.Open();
            dr = cm.ExecuteReader();

            while (dr.Read())
            {
                i++;
                dvgItems.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString());
            }

            dr.Close();
            con.Close();
        }

        private void dvgItems_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dvgItems.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                // Open the ItemsModuleForm to edit the selected item
                ItemsModuleForm ItemsModule = new ItemsModuleForm();
                ItemsModule.LableId.Text = dvgItems.Rows[e.RowIndex].Cells[1].Value.ToString();
                ItemsModule.txtIName.Text = dvgItems.Rows[e.RowIndex].Cells[2].Value.ToString();
                ItemsModule.txtIQuntity.Text = dvgItems.Rows[e.RowIndex].Cells[3].Value.ToString();
                ItemsModule.txtIPrice.Text = dvgItems.Rows[e.RowIndex].Cells[4].Value.ToString();
                ItemsModule.txtIDes.Text = dvgItems.Rows[e.RowIndex].Cells[5].Value.ToString();
                ItemsModule.comboICat.Text = dvgItems.Rows[e.RowIndex].Cells[6].Value.ToString();

                ItemsModule.btnAdd.Enabled = false;
                ItemsModule.btnUpdate.Enabled = true;
                ItemsModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Delete the selected item record from the database
                    con.Open();
                    cm = new SqlCommand("DELETE FROM tbItems WHERE iid LIKE '" + dvgItems.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record has been successfully deleted!");
                }
            }

            LoadItems();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        private void customButtons1_Click(object sender, EventArgs e)
        {
            // Open the ItemsModuleForm to add a new item
            ItemsModuleForm formModule = new ItemsModuleForm();
            formModule.btnAdd.Enabled = true;
            formModule.btnUpdate.Enabled = false;
            formModule.ShowDialog();

            LoadItems();
        }
    }
}
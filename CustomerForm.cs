using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient; // Impport Sql client

namespace Hardware_Store_Inventory_Management_System
{
    public partial class CustomerForm : Form
    {
        // Database connection and command objects
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public CustomerForm()
        {
            InitializeComponent();
            LoadCustomer();
        }

        // Load customer data from the database
        public void LoadCustomer()
        {
            int i = 0;
            dvgCustomer.Rows.Clear();

            cm = new SqlCommand("SELECT * FROM tbCustomer", con);
            con.Open();
            dr = cm.ExecuteReader();

            // Iterate over the data reader 
            while (dr.Read())
            {
                i++;
                dvgCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString());
            }

            dr.Close();
            con.Close();
        }

        private void dvgCustomer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dvgCustomer.Columns[e.ColumnIndex].Name;

            // Handle the Edit button click
            if (colName == "Edit")
            {
                CustomerModuleForm customerModule = new CustomerModuleForm();
                customerModule.CId.Text = dvgCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
                customerModule.txtCName.Text = dvgCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
                customerModule.txtCPhone.Text = dvgCustomer.Rows[e.RowIndex].Cells[3].Value.ToString();

                customerModule.btnAdd.Enabled = false;
                customerModule.btnUpdate.Enabled = true;
                customerModule.ShowDialog();
            }
            // Handle the Delete button click
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("Delete From tbCustomer Where cid Like '" + dvgCustomer.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Customer has been successfully deleted!");
                }
            }
            

            LoadCustomer();
        }

        private void customButtons1_Click(object sender, EventArgs e)
        {
            // Open the CustomerModuleForm to add a new customer
            CustomerModuleForm moduleForm = new CustomerModuleForm();
            moduleForm.btnAdd.Enabled = true;
            moduleForm.btnUpdate.Enabled = false;
            moduleForm.ShowDialog();
        }
    }
}
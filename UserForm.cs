using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting.Contexts;


namespace Hardware_Store_Inventory_Management_System
{
    public partial class UserForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        public UserForm()
        {
            InitializeComponent();
            LoadUser();
        }

        public void LoadUser()
        {
            int i = 0;
            dvgUser.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbUser", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dvgUser.Rows.Add(i, dr[0].ToString(), dr[1].ToString(), dr[2].ToString(), dr[3].ToString());
            }
            dr.Close();
            con.Close();
        }

        private void dvgUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dvgUser.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                UserModuleForm userModule = new UserModuleForm();
                userModule.txtName.Text = dvgUser.Rows[e.RowIndex].Cells[1].Value.ToString();
                userModule.txtPassword.Text = dvgUser.Rows[e.RowIndex].Cells[2].Value.ToString();
                userModule.txtPhone.Text = dvgUser.Rows[e.RowIndex].Cells[3].Value.ToString();
                userModule.txtId.Text = dvgUser.Rows[e.RowIndex].Cells[4].Value.ToString();

                userModule.btnAdd.Enabled = false;
                userModule.btnUpdate.Enabled = true;
                userModule.txtName.Enabled = false;
                userModule.ShowDialog();

               }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("Delete From tbUser Where username Like '" + dvgUser.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been successfully deleted!");

                }
            }
            LoadUser();
        }
        private void customButtons1_Click(object sender, EventArgs e)
        {
            UserModuleForm userModule = new UserModuleForm();
            userModule.btnAdd.Enabled = true;
            userModule.btnUpdate.Enabled = false;
            userModule.ShowDialog();
            LoadUser();
        }
    }
}

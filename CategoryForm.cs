using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hardware_Store_Inventory_Management_System
{
    public partial class CategoryForm : Form
    {
        private readonly SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        private SqlCommand cm;
        private SqlDataReader dr;

        public CategoryForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        private void LoadCategory()
        {
            int i = 0;
            dvgCategory.Rows.Clear();
            // Fetch all categories from the tbCategory table
            string query = "SELECT * FROM tbCategory";
            using (cm = new SqlCommand(query, con))
            {
                con.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    i++;
                    dvgCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
                }
                dr.Close();
                con.Close();
            }
        }

        private void dvgCategory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dvgCategory.Columns[e.ColumnIndex].Name;
            if (colName == "Edit")
            {
                CategoryModuleForm formModule = new CategoryModuleForm();
                // Pass the category ID and name to the CategoryModuleForm for editing
                formModule.categoryID.Text = dvgCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                formModule.txtCatName.Text = dvgCategory.Rows[e.RowIndex].Cells[2].Value.ToString();

                formModule.btnAdd.Enabled = false;
                formModule.btnUpdate.Enabled = true;
                formModule.ShowDialog();
            }
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Delete the selected category from the tbCategory table
                    string categoryId = dvgCategory.Rows[e.RowIndex].Cells[1].Value.ToString();
                    string deleteQuery = "DELETE FROM tbCategory WHERE catid = @catid";
                    using (cm = new SqlCommand(deleteQuery, con))
                    {
                        cm.Parameters.AddWithValue("@catid", categoryId);
                        con.Open();
                        cm.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Category has been successfully deleted!");
                }
            }
            LoadCategory();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadCategory();
        }

        private void customButtons1_Click(object sender, EventArgs e)
        {
            CategoryModuleForm formModule = new CategoryModuleForm();
            formModule.btnAdd.Enabled = true;
            formModule.btnUpdate.Enabled = false;
            formModule.ShowDialog();
            LoadCategory();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtSearch.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                // Fetch categories that match the search term from the tbCategory table
                string query = "SELECT * FROM tbCategory WHERE catname LIKE @searchTerm";
                using (cm = new SqlCommand(query, con))
                {
                    cm.Parameters.AddWithValue("@searchTerm", "%" + searchTerm + "%");
                    con.Open();
                    dr = cm.ExecuteReader();
                    int i = 0;
                    dvgCategory.Rows.Clear();
                    while (dr.Read())
                    {
                        i++;
                        dvgCategory.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
                    }
                    dr.Close();
                    con.Close();
                }
            }
            else
            {
                // If the search term is empty, load all categories
                LoadCategory();
            }
        }
    
    }
}
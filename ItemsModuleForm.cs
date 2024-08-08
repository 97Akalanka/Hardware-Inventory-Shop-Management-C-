using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Hardware_Store_Inventory_Management_System
{
    public partial class ItemsModuleForm : Form
    {
        private readonly SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        private SqlCommand cm;
        private SqlDataReader dr;

        public ItemsModuleForm()
        {
            InitializeComponent();
            LoadCategory();
        }

        private void LoadCategory()
        {
            comboICat.Items.Clear();
            // Fetch category names from the tbCategory table
            string query = "SELECT catname from tbCategory";
            using (cm = new SqlCommand(query, con))
            {
                con.Open();
                dr = cm.ExecuteReader();
                while (dr.Read())
                {
                    comboICat.Items.Add(dr[0].ToString());
                }
                dr.Close();
                con.Close();
            }
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Close(); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("Are you sure you want to save this item?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Insert the item record into the tbItems table
                    string query = "INSERT INTO tbItems(iname,iqty,iprice,idescription,icategory) VALUES(@iname, @iqty, @iprice, @idescription, @icategory)";
                    using (cm = new SqlCommand(query, con))
                    {
                        cm.Parameters.AddWithValue("@iname", txtIName.Text);
                        cm.Parameters.AddWithValue("@iqty", Convert.ToInt16(txtIQuntity.Text));
                        cm.Parameters.AddWithValue("@iprice", Convert.ToInt16(txtIPrice.Text));
                        cm.Parameters.AddWithValue("@idescription", txtIDes.Text);
                        cm.Parameters.AddWithValue("@icategory", comboICat.Text);
                        con.Open();
                        cm.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Item has been saved");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Clear()
        {
            // Clear the input fields
            txtIName.Clear();
            txtIQuntity.Clear();
            txtIPrice.Clear();
            comboICat.SelectedIndex = -1;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            btnAdd.Enabled = true;
            btnUpdate.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // Confirm the user's action
                if (MessageBox.Show("Are you sure you want to update this Item?", "Update Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // Update the item record in the tbItems table based on the selected item ID
                    string query = "UPDATE tbItems SET iname=@iname, iqty=@iqty, iprice=@iprice, idescription=@idescription, icategory=@icategory WHERE iid = @itemid";
                    using (cm = new SqlCommand(query, con))
                    {
                        cm.Parameters.AddWithValue("@iname", txtIName.Text);
                        cm.Parameters.AddWithValue("@iqty", Convert.ToInt16(txtIQuntity.Text));
                        cm.Parameters.AddWithValue("@iprice", Convert.ToInt16(txtIPrice.Text));
                        cm.Parameters.AddWithValue("@idescription", txtIDes.Text);
                        cm.Parameters.AddWithValue("@icategory", comboICat.Text);
                        cm.Parameters.AddWithValue("@itemid", LableId.Text);
                        con.Open();
                        cm.ExecuteNonQuery();
                        con.Close();
                    }
                    MessageBox.Show("Item has been updated!");
                    this.Close(); // Close the form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
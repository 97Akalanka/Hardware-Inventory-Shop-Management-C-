using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Hardware_Store_Inventory_Management_System
{
    public partial class OderModuleForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;
        int qty = 0;
        public OderModuleForm()
        {
            InitializeComponent();
            LoadCustomer();
            LoadItems();
        }

        private void pictureBoxClose_Click(object sender, EventArgs e)
        {
            this.Dispose();
            
        }

        public void LoadCustomer()
        {
            int i = 0;
            dvgCustomer.Rows.Clear();
            cm = new SqlCommand("SELECT cid, cname FROM tbCustomer WHERE CONCAT(cid,cname)Like '%"+txtSearchCust.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dvgCustomer.Rows.Add(i, dr[0].ToString(), dr[1].ToString());
            }
            dr.Close();
            con.Close();
        }

        public void LoadItems()
        {
            int i = 0;
            dvgItems.Rows.Clear();
            cm = new SqlCommand("SELECT * FROM tbItems WHERE CONCAT(iid, iname, iprice, idescription, icategory) LIKE '%" + txtSearchItems.Text + "%'", con);
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

        private void txtSearchCust_TextChanged(object sender, EventArgs e)
        {
            LoadCustomer();
        }

        private void txtSearchItems_TextChanged(object sender, EventArgs e)
        {
            LoadItems();
        }

        

        private void numericQty_ValueChanged(object sender, EventArgs e)
        {
            GetQty();
            if (Convert.ToInt16(numericQty.Value) > qty)
            {
                MessageBox.Show("Instock quantity is not enough!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                numericQty.Value = numericQty.Value - 1;
                return;
            }
            if (Convert.ToInt16(numericQty.Value) > 0)
            {
                int total = Convert.ToInt16(txtPrice.Text) * Convert.ToInt16(numericQty.Value);
                txtTotal.Text = total.ToString();
            }
        }

        private void dvgCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCId.Text = dvgCustomer.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtCName.Text = dvgCustomer.Rows[e.RowIndex].Cells[2].Value.ToString();
        }

        private void dvgItems_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtIId.Text = dvgItems.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtIName.Text = dvgItems.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtPrice.Text = dvgItems.Rows[e.RowIndex].Cells[4].Value.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCId.Text == "")
                {
                    MessageBox.Show("Please select customer!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (txtIId.Text == "")
                {
                    MessageBox.Show("Please select item!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Are you sure you want to save this order?", "Saving Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    cm = new SqlCommand("INSERT INTO tbOrder(odate, iid, cid, qty, price, total)VALUES(@odate, @iid, @cid, @qty, @price, @total)", con);
                    cm.Parameters.AddWithValue("@odate", dtOrder.Value);
                    cm.Parameters.AddWithValue("@iid", Convert.ToInt16(txtIId.Text));
                    cm.Parameters.AddWithValue("@cid", Convert.ToInt16(txtCId.Text));
                    cm.Parameters.AddWithValue("@qty", Convert.ToInt16(numericQty.Value));
                    cm.Parameters.AddWithValue("@price", Convert.ToInt16(txtPrice.Text));
                    cm.Parameters.AddWithValue("@total", Convert.ToInt16(txtTotal.Text));
                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Order has been saved.");

                    cm = new SqlCommand("UPDATE tbItems SET iqty = (iqty-@iqty) WHERE iid Like '" + txtIId.Text + "' ", con);
                    cm.Parameters.AddWithValue("@iqty", Convert.ToInt16(numericQty.Text));

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();
                    Clear();
                    LoadCustomer();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void Clear()
        {
            txtCId.Clear();
            txtCName.Clear();
            txtIId.Clear();
            txtIName.Clear();
            txtPrice.Clear();
            numericQty.Value = 0;
            txtTotal.Clear();
            dtOrder.Value =DateTime.Now;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void GetQty()
        {
            cm = new SqlCommand("SELECT iqty FROM tbItems WHERE iid = '" + txtIId.Text + "'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                qty = Convert.ToInt32(dr[0].ToString());
            }
            dr.Close();
            con.Close();
        }
    }
}

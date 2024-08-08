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
    public partial class OderForm : Form
    {
        SqlConnection con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\user\Documents\dbIMS.mdf;Integrated Security=True;Connect Timeout=30");
        SqlCommand cm = new SqlCommand();
        SqlDataReader dr;

        public OderForm()
        {
            InitializeComponent();
            LoadOrder();
        }

        public void LoadOrder()
        {
            double total = 0;
            int i = 0;
            dvgOrder.Rows.Clear();
            cm = new SqlCommand("SELECT oid, odate, O.iid, I.iname, O.cid, C.cname, qty, price, total FROM tbOrder AS O JOIN tbCustomer AS C ON O.cid=C.cid JOIN tbItems AS I ON O.iid = I.iid WHERE CONCAT (oid, odate, O.iid, I.iname, O.cid, C.cname, qty, price) Like '%"+txtSearch.Text+"%'", con);
            con.Open();
            dr = cm.ExecuteReader();
            while (dr.Read())
            {
                i++;
                dvgOrder.Rows.Add(i, dr[0].ToString(),Convert.ToDateTime(dr[1].ToString()).ToString("dd/MM/yyyy"), dr[2].ToString(), dr[3].ToString(), dr[4].ToString(), dr[5].ToString(), dr[6].ToString(), dr[7].ToString(), dr[8].ToString());
                total += Convert.ToInt32(dr[8].ToString());
            }
            dr.Close();
            con.Close();

            lblQty.Text = i.ToString();
            lblTotal.Text = total.ToString();
        }

        private void dvgUser_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string colName = dvgOrder.Columns[e.ColumnIndex].Name;
           
            if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete?", "Delete Record", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    con.Open();
                    cm = new SqlCommand("Delete From tbOrder Where oid Like '" + dvgOrder.Rows[e.RowIndex].Cells[1].Value.ToString() + "'", con);
                    cm.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("User has been successfully deleted!");

                    cm = new SqlCommand("UPDATE tbItems SET iqty = (iqty+@iqty) WHERE iid Like '" + dvgOrder.Rows[e.RowIndex].Cells[3].Value.ToString() + "'", con);
                    cm.Parameters.AddWithValue("@iqty", Convert.ToInt16(dvgOrder.Rows[e.RowIndex].Cells[5].Value.ToString()));

                    con.Open();
                    cm.ExecuteNonQuery();
                    con.Close();

                }

            }
            LoadOrder();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrder();
        }

        private void customButtons1_Click(object sender, EventArgs e)
        {
            OderModuleForm moduleForm = new OderModuleForm();
            moduleForm.ShowDialog();
            LoadOrder();
        }
    }
}

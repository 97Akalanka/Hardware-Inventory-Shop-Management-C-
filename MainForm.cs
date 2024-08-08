using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Hardware_Store_Inventory_Management_System
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private Form activeForm = null;

        // Function to open a child form inside the Main
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)
                activeForm.Close();

            activeForm = childForm;

            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;

            // Add the child form to the Main
            panelMain.Controls.Add(childForm);
            panelMain.Tag = childForm;

            // Bring the child form to the front and show it
            childForm.BringToFront();
            childForm.Show();
        }

        private void cbCustomer_Click(object sender, EventArgs e)
        {
            // Open the CustomerForm as a child form
            openChildForm(new CustomerForm());
        }

        private void cbItems_Click(object sender, EventArgs e)
        {
            // Open the ItemsForm as a child form
            openChildForm(new ItemsForm());
        }

        private void cbCategories_Click(object sender, EventArgs e)
        {
            // Open the CategoryForm as a child form
            openChildForm(new CategoryForm());
        }

        private void cbUsers_Click(object sender, EventArgs e)
        {
            // Open the UserForm as a child form
            openChildForm(new UserForm());
        }

        private void cbOrders_Click(object sender, EventArgs e)
        {
            // Open the OderForm as a child form
            openChildForm(new OderForm());
        }
    }
}

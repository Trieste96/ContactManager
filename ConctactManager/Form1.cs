using ConctactManager.Models;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ConctactManager
{
    public partial class fContactManager : Form
    {
        private ContactManagerDBEntities _entities = new ContactManagerDBEntities();
        private void initControls()
        {
            gridContacts.ReadOnly = true;
            gridContacts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }
        private void indexView()
        {
            gridContacts.DataSource = _entities.Contacts.ToList();
        }
        private void editView()
        {
            
        }
        public fContactManager()
        {
            InitializeComponent();
            initControls();
            indexView();
        }

        private void Edit_view(Contact contactToEdit)
        {
            tabPage1.Text = "Edit";
            txtFname.Text = contactToEdit.FirstName;
            txtLname.Text = contactToEdit.LastName;
            txtPhone.Text = contactToEdit.Phone;
            txtEmail.Text = contactToEdit.Email;
            btnSave.Tag = contactToEdit.Id;
        }
        private void Edit_click()
        {
            int id = (int)gridContacts.CurrentRow.Cells[0].Value;
            var contactToEdit = ( 
                from c in _entities.Contacts
                where c.Id == id
                select c
                ).FirstOrDefault();
            Edit_view(contactToEdit);
        }

        private void gridContacts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Edit", null, new EventHandler(Edit_click));
                menu.Items.Add("Delete", null);

                DataGridView grid = sender as DataGridView;
                grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Point pt = grid.PointToClient(Control.MousePosition);
                //System.Console.WriteLine("Mouse's location: " + Control.MousePosition.X + ", " + Control.MousePosition.Y);
                //System.Console.WriteLine("Point's grid location: " + pt.X + ", " + pt.Y);
                menu.Show(grid, pt);
            }
        }
    }
}

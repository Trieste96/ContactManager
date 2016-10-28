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
            tabControl1.SelectedTab = tabView;
        }
        public fContactManager()
        {
            InitializeComponent();
            initControls();
            indexView();
        }
        private void Add_view()
        {
            tabPage1.Text = "Add";
            btnSave.Tag = null;
            txtFname.Clear();
            txtLname.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
        }
        private void Edit_view(Contact contactToEdit)
        {
            tabPage1.Text = "Edit";
            txtFname.Text = contactToEdit.FirstName;
            txtLname.Text = contactToEdit.LastName;
            txtPhone.Text = contactToEdit.Phone;
            txtEmail.Text = contactToEdit.Email;
            btnSave.Tag = contactToEdit.Id;
            tabControl1.SelectedTab = tabPage1;
        }
        private void Edit_click(object sender, EventArgs e)
        {
            int id = (int)gridContacts.CurrentRow.Cells[0].Value;
            var contactToEdit = ( 
                from c in _entities.Contacts
                where c.Id == id
                select c
                ).FirstOrDefault();
            Edit_view(contactToEdit);
        }
        private void delete_click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xoá học sinh này khỏi CSDL?", "Xoá", MessageBoxButtons.OKCancel);
            if(result == DialogResult.OK)
            {
                Contact contactToDelete = new Contact() {
                    Id = (int)gridContacts.SelectedRows[0].Cells[0].Value,
                    FirstName = gridContacts.SelectedRows[0].Cells[1].Value.ToString(),
                    LastName = gridContacts.SelectedRows[0].Cells[2].Value.ToString(),
                    Phone = gridContacts.SelectedRows[0].Cells[3].Value.ToString(),
                    Email = gridContacts.SelectedRows[0].Cells[4].Value.ToString(),
                };
                _entities.Entry(contactToDelete).State = System.Data.Entity.EntityState.Added;
                _entities.Contacts.Remove(contactToDelete);

                _entities.SaveChanges();
                indexView();
            }
        }
        private void gridContacts_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Edit", null, new EventHandler(Edit_click));
                menu.Items.Add("Delete", null, new EventHandler(delete_click));

                DataGridView grid = sender as DataGridView;
                grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Point pt = grid.PointToClient(Control.MousePosition);
                //System.Console.WriteLine("Mouse's location: " + Control.MousePosition.X + ", " + Control.MousePosition.Y);
                //System.Console.WriteLine("Point's grid location: " + pt.X + ", " + pt.Y);
                menu.Show(grid, pt);
            }
        }
        private Contact getContactToCreate()
        {
            return new Contact(){
                Id = btnSave.Tag == null ? 0 : (int)(btnSave.Tag),
                FirstName = txtFname.Text.Trim(),
                LastName = txtLname.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Email = txtEmail.Text.Trim()
            };
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            Contact contactToEdit = getContactToCreate();
            Console.WriteLine(contactToEdit.Id);
            if(contactToEdit.Id > 0)
            {
                var originalContact = (
                    from c in _entities.Contacts
                    where c.Id == contactToEdit.Id
                    select c
                    ).FirstOrDefault();
                _entities.Database.BeginTransaction();
                _entities.Entry(originalContact).CurrentValues.SetValues(contactToEdit);
                _entities.SaveChanges();
                MessageBox.Show("Edit successfully!");
                indexView();
                Add_view();
            }
        }
    }
}

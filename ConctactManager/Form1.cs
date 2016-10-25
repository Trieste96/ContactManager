using ConctactManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public fContactManager()
        {
            InitializeComponent();
            initControls();
            indexView();
        }

        private void Edit_view()
        {

        }
        private void gridContacts_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                ContextMenuStrip menu = new ContextMenuStrip();
                menu.Items.Add("Edit", null);
                menu.Items.Add("Delete", null);

                DataGridView grid = sender as DataGridView;
                grid.CurrentCell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                Point pt = grid.PointToClient(Control.MousePosition);
                System.Console.WriteLine("Mouse's location: "+Control.MousePosition.X + ", " + Control.MousePosition.Y);
                System.Console.WriteLine("Point's grid location: "+pt.X + ", " + pt.Y);
                menu.Show(grid, pt);
            }
            catch (ArgumentOutOfRangeException)
            {   }
        }
    }
}

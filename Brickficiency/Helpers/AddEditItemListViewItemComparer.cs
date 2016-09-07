using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Brickficiency.Helpers
{
    public class AddEditItemListViewItemComparer : IComparer
    {
        private readonly int _columnIndex;

        public AddEditItemListViewItemComparer()
        {
            _columnIndex = 0;
        }

        public AddEditItemListViewItemComparer(int column)
        {
            _columnIndex = column;
        }

        public int Compare(object x, object y)
        {
            return string.Compare(((ListViewItem)x).SubItems[_columnIndex].Text, ((ListViewItem)y).SubItems[_columnIndex].Text);
        }
    }
}

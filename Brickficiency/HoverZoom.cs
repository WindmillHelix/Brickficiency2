using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Brickficiency
{
    public partial class HoverZoom : Form
    {
        public HoverZoom()
        {
            InitializeComponent();
        }

        public void HideLabel()
        {
            loadingLabel.Visible = false;
            notFoundLabel.Visible = false;
        }
        public void ShowLabel()
        {
            loadingLabel.Visible = true;
            notFoundLabel.Visible = false;
        }
        public void NotFound()
        {
            loadingLabel.Visible = false;
            notFoundLabel.Visible = true;
        }
    }
}

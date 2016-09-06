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

        public void ShowImage(Image image)
        {
            this.InvokeAction(
                () =>
                    {
                        loadingLabel.Visible = false;
                        notFoundLabel.Visible = false;

                        Width = image.Width;
                        Height = image.Height;
                        BackgroundImage = image;
                    });
        }

        public void ShowLoading()
        {
            this.InvokeAction(
                () =>
                    {
                        BackgroundImage = null;
                        Width = 100;
                        Height = 50;
                        loadingLabel.Visible = true;
                        notFoundLabel.Visible = false;
                    });
        }

        public void ShowNotFound()
        {
            this.InvokeAction(
                () =>
                {
                    BackgroundImage = null;
                    Width = 100;
                    Height = 50;
                    loadingLabel.Visible = false;
                    notFoundLabel.Visible = true;
                });
        }

        [Obsolete]
        public void HideLabel()
        {
            loadingLabel.Visible = false;
            notFoundLabel.Visible = false;
        }

        [Obsolete]
        public void ShowLabel()
        {
            loadingLabel.Visible = true;
            notFoundLabel.Visible = false;
        }

        [Obsolete]
        public void NotFound()
        {
            loadingLabel.Visible = false;
            notFoundLabel.Visible = true;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindmillHelix.Brickficiency2.Common.Domain;
using WindmillHelix.Brickficiency2.Services;

namespace Brickficiency
{
    public partial class GetPassword : Form
    {
        private readonly ICredentialService _credentialService;

        public GetPassword(ICredentialService credentialService)
        {
            _credentialService = credentialService;
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            var credential = _credentialService.GetCredential(ExternalSystem.Bricklink);
            UsernameTextBox.Text = credential?.UserName;

            base.OnShown(e);

            if (!string.IsNullOrWhiteSpace(UsernameTextBox.Text))
            {
                PasswordTextBox.Focus();
            }
            else
            {
                UsernameTextBox.Focus();
            }
        }

        public string UserName { get; private set; }

        public string Password { get; private set; }

        public bool ShouldRememberPassword { get; private set; }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UsernameTextBox.Text) || string.IsNullOrWhiteSpace(PasswordTextBox.Text))
            {
                MessageBox.Show("UserName and Password are required.");
                return;
            }

            var credential = new NetworkCredential(UsernameTextBox.Text, PasswordTextBox.Text);
            bool areCredentialsValid = _credentialService.ValidateCredential(
                ExternalSystem.Bricklink, 
                credential);

            if(!areCredentialsValid)
            {
                MessageBox.Show("Invalid credentials.");
                return;
            }

            if(RememberPasswordCheckBox.Checked)
            {
                _credentialService.SetCredential(ExternalSystem.Bricklink, credential);
            }
            else
            {
                _credentialService.SetCredential(
                    ExternalSystem.Bricklink, 
                    new NetworkCredential(UsernameTextBox.Text, string.Empty));
            }

            UserName = UsernameTextBox.Text;
            Password = PasswordTextBox.Text;
            ShouldRememberPassword = RememberPasswordCheckBox.Checked;

            PasswordTextBox.Text = string.Empty;
            DialogResult = DialogResult.OK;
        }

        private void pwBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                DialogResult = DialogResult.Cancel;
            }

            if (e.KeyChar == (char)Keys.Return)
            {
                okButton_Click(new object(), new EventArgs());
            }
        }

        private void GetPassword_Shown(object sender, EventArgs e)
        {
            PasswordTextBox.Select();
        }
    }
}

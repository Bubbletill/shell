namespace BT_SHELL
{
    using System.Security.Cryptography;
    using System.Text;

    public partial class Shell : Form
    {

        private string passwordHash = "86F35B58DD51D308154332C77E11BC960B2FEEA12D18AB34A79824DE3017C5946160A6722F859010EBE6EED29530A17F5EABC953440BB4F2B1D0757FD8B29428";
        private Boolean canClose = false;

        public Shell()
        {
            InitializeComponent();
        }

        private void Shell_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canClose)
            {
                e.Cancel = true;
            }
        }

        private void Shell_KeyDown(object sender, KeyEventArgs e)
        {
            // Master login to escape the shell and launch explorer
            if ((e.Control && e.Shift && e.Alt) && e.KeyCode == Keys.X)
            {
                string inputPass = ShowDialog("Enter master password", "Authorization required");

                const string passSalt = "ilovemen123";
                const int passKeySize = 64;
                const int passIterations = 350000;

                HashAlgorithmName hashAlgorithm = HashAlgorithmName.SHA512;

                byte[] hashedInput = Rfc2898DeriveBytes.Pbkdf2(
                    Encoding.UTF8.GetBytes(inputPass),
                    Encoding.ASCII.GetBytes(passSalt),
                    passIterations,
                    hashAlgorithm,
                    passKeySize
                );

                string hashedInputStr = Convert.ToHexString(hashedInput);

                bool passwordOK = passwordHash == hashedInputStr;

                if (passwordOK == true)

                {
                    canClose = true;
                    Application.Exit();
                    System.Diagnostics.Process.Start("C:\\Windows\\explorer.exe");
                }
            }

            // Restart the POS application
            if ((e.Control && e.Shift && e.Alt) && e.KeyCode == Keys.R)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to restart the application?", "Confirm restart", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    //do something
                }
            }
        }

        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Width = 400, Text = text };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400, UseSystemPasswordChar = true };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}
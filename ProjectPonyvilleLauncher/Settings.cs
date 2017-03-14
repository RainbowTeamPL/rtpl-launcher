using Microsoft.Win32;
using System;
using System.IO;
using System.Windows.Forms;

namespace ProjectPonyvilleLauncher
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void DeleteAllDataBtn_Click(object sender, EventArgs e)
        {
            DialogResult confirm = MessageBox.Show("Do you really want to delete ALL data?\n You will need to re-download everything!", "Confirm Action", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (confirm == DialogResult.Yes)
            {
                DeleteAll();
                MessageBox.Show("Launcher needs to be re-opened", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form1._restart = true;
                Application.Exit();
            }
            else
            { }
        }

        private void DeleteAll()
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + Form1.currGame.ToString(), "Version", "0");
            //Registry.CurrentUser.DeleteSubKey("Software\\RainbowTeamPL\\ProjectPonyville\\installDir", false);

            try
            {
                Directory.Delete(Application.StartupPath + "/Temp", true);
            }
            catch
            {
            }

            try
            {
                Directory.Delete(Application.StartupPath + "/Tools", true);
            }
            catch
            {
            }

            if (Directory.Exists(Form1.installDir + @"\" + Form1.currGame.ToString()))
            {
                try
                {
                    Directory.Delete(Form1.installDir + @"\" + Form1.currGame.ToString(), true);
                }
                catch
                {
                }
            }
            File.Delete(Application.StartupPath + "/version.v");
            File.Delete(Application.StartupPath + "/changelog.tmp");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + Form1.currGame.ToString(), "Server", comboBox1.SelectedItem);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedItem = Convert.ToString(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + Form1.currGame.ToString(), "Server", "rtpl.dynu.com"));
            checkBox1.Checked = Convert.ToBoolean(Registry.GetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + Form1.currGame.ToString(), "force32bitBuild", false));
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Registry.SetValue("HKEY_CURRENT_USER\\Software\\RainbowTeamPL\\" + Form1.currGame.ToString(), "force32bitBuild", checkBox1.Checked);
        }

        private void BuildIndexBtn_Click(object sender, EventArgs e)
        {
            Form1.rebuildIndex = true;
            Close();
        }

        private void SetupAutoLoginBtn_Click(object sender, EventArgs e)
        {
        }
    }
}
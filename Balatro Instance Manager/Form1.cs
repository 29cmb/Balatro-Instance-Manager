using System.IO;
using Newtonsoft.Json;
using Balatro_Instance_Manager.Properties;

namespace Balatro_Instance_Manager
{
    public partial class Form1 : Form
    {
        string balatroPathStr;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            balatroPathStr = Properties.Settings.Default.BalatroPath;
            if (string.IsNullOrEmpty(balatroPathStr))
            {
                balatroPathStr = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Balatro");
            }

            balatroPath.Text = balatroPathStr;
            loadProfiles();
        }

        private void balatroPath_Unfocused(object sender, EventArgs e)
        {
            if (balatroPath.Text == "")
            {
                balatroPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Balatro");
            }

            if (!IsDirectoryAccessible(balatroPath.Text))
            {
                MessageBox.Show("Balatro path is not accessible. Please check the path and try again.", "Information", MessageBoxButtons.OK);
                return;
            }

            balatroPathStr = balatroPath.Text;

            Properties.Settings.Default.BalatroPath = balatroPathStr;
            Properties.Settings.Default.Save();

            loadProfiles();
        }

        private void balatroPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                this.ActiveControl = null;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void loadProfiles()
        {
            string modProfilesPath = Path.Combine(balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath))
            {
                MessageBox.Show("Balatro path is not accessible, make sure to check the path before launching.", "Information", MessageBoxButtons.OK);
                return;
            }

            Directory.CreateDirectory(modProfilesPath);
            var subdirectories = Directory.GetDirectories(modProfilesPath);

            instances.Controls.Clear();

            foreach (var subdirectory in subdirectories)
            {
                string profilePath = Path.Combine(subdirectory, "profile.json");
                File.WriteAllText(profilePath, "{\"Enabled\": false}");

                try
                {
                    string jsonContent = File.ReadAllText(profilePath);
                    var profile = JsonConvert.DeserializeObject<Profile>(jsonContent);

                    CheckBox checkBox = new CheckBox();
                    checkBox.Text = Path.GetFileName(subdirectory);
                    checkBox.Checked = profile.Enabled;

                    instances.Controls.Add(checkBox);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON in {subdirectory}: {ex.Message}");
                }
            }
        }
        private bool IsDirectoryAccessible(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}

public class Profile
{
    public string Name { get; set; }
    public bool Enabled { get; set; }
}
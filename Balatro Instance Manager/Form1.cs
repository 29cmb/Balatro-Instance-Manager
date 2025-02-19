using System.Diagnostics;
using Newtonsoft.Json;


namespace Balatro_Instance_Manager
{
    public partial class Form1 : Form
    {
        string balatroPathStr;
        string balatroSteamDirStr;
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

            balatroSteamDirStr = Properties.Settings.Default.BalatroSteamPath;
            if (string.IsNullOrEmpty(balatroPathStr))
            {
                balatroSteamDirStr = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Balatro";
            }

            balatroPath.Text = balatroPathStr;
            balatroSteamPath.Text = balatroSteamDirStr;
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
                MessageBox.Show("Balatro data path is not accessible. Please check the path and try again.", "Information", MessageBoxButtons.OK);
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
                MessageBox.Show("Balatro data path is not accessible, make sure to check the path before launching.", "Information", MessageBoxButtons.OK);
                return;
            }

            Directory.CreateDirectory(modProfilesPath);
            var subdirectories = Directory.GetDirectories(modProfilesPath);

            instances.Controls.Clear();

            foreach (var subdirectory in subdirectories)
            {
                string profilePath = Path.Combine(subdirectory, "profile.json");
                if (!File.Exists(profilePath))
                    File.WriteAllText(profilePath, "{\"Enabled\": false}");

                try
                {
                    string jsonContent = File.ReadAllText(profilePath);
                    var profile = JsonConvert.DeserializeObject<Profile>(jsonContent);

                    CheckBox checkBox = new CheckBox();
                    checkBox.Text = Path.GetFileName(subdirectory);
                    checkBox.Checked = profile.Enabled;
                    checkBox.CheckedChanged += onCheckClicked;

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

        private void onCheckClicked(object? sender, EventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                string profilePath = Path.Combine(balatroPathStr, "ModProfiles", checkBox.Text, "profile.json");
                try
                {
                    string jsonContent = File.ReadAllText(profilePath);
                    var profile = JsonConvert.DeserializeObject<Profile>(jsonContent);
                    if (profile != null)
                    {
                        profile.Enabled = checkBox.Checked;
                        File.WriteAllText(profilePath, JsonConvert.SerializeObject(profile));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON in {profilePath}: {ex.Message}");
                }
            }
        }

        private void balatroSteamPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Enter)
            {
                this.ActiveControl = null;
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void balatroSteamPath_Unfocused(object sender, EventArgs e)
        {
            if (balatroSteamPath.Text == "")
            {
                balatroSteamPath.Text = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Balatro";
            }

            if (!IsDirectoryAccessible(balatroSteamPath.Text))
            {
                MessageBox.Show("Balatro steam path is not accessible. Please check the path and try again.", "Information", MessageBoxButtons.OK);
                return;
            }

            balatroSteamDirStr = balatroSteamPath.Text;

            Properties.Settings.Default.BalatroSteamPath = balatroSteamDirStr;
            Properties.Settings.Default.Save();

            loadProfiles();
        }

        private void launch_Click(object sender, EventArgs e)
        {
            if (!IsDirectoryAccessible(balatroSteamDirStr))
            {
                MessageBox.Show("Balatro steam path is not accessible. Please check the path and try again.", "Information", MessageBoxButtons.OK);
                return;
            }

            string modProfilesPath = Path.Combine(balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath))
            {
                MessageBox.Show("Balatro data path is not accessible, make sure to check the path before launching.", "Information", MessageBoxButtons.OK);
                return;
            }

            List<string> mods = GetProfileMods();
            if (mods == null) return;

            HashSet<string> loadedModIds = new HashSet<string>();
            List<string> validMods = new List<string>();

            foreach (var mod in mods)
            {
                var jsonFiles = Directory.GetFiles(mod, "*.json");

                bool isValidMod = true;

                foreach (var jsonFile in jsonFiles)
                {
                    try
                    {
                        string jsonContent = File.ReadAllText(jsonFile);
                        var modData = JsonConvert.DeserializeObject<ModData>(jsonContent);

                        if (modData != null)
                        {
                            if (!string.IsNullOrEmpty(modData.id))
                            {
                                if (loadedModIds.Contains(modData.id))
                                {
                                    isValidMod = false;
                                    break;
                                }
                            }
                            else if (!string.IsNullOrEmpty(modData.name))
                            {
                                if (loadedModIds.Contains(modData.name))
                                {
                                    isValidMod = false;
                                    break;
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading JSON in {jsonFile}: {ex.Message}");
                        isValidMod = false;
                        break;
                    }
                }

                if (isValidMod)
                {
                    validMods.Add(mod);
                    foreach (var jsonFile in jsonFiles)
                    {
                        try
                        {
                            string jsonContent = File.ReadAllText(jsonFile);
                            var modData = JsonConvert.DeserializeObject<ModData>(jsonContent);

                            if (modData != null)
                            {
                                if (!string.IsNullOrEmpty(modData.id))
                                {
                                    loadedModIds.Add(modData.id);
                                }
                                else if (!string.IsNullOrEmpty(modData.name))
                                {
                                    loadedModIds.Add(modData.name);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error reading JSON in {jsonFile}: {ex.Message}");
                        }
                    }
                }
            }


            string modsDirectory = Path.Combine(balatroPathStr, "Mods");
            if (!Directory.Exists(modsDirectory))
            {
                MessageBox.Show("Mods directory does not exist. Make sure you have lovely installed and have opened the game at least once since then and try again.", "Error", MessageBoxButtons.OK);
                return;
            }

            if(deselected.Checked) ClearProfileMods();

            foreach (var mod in validMods)
            {
                string output = Path.Combine(modsDirectory, Path.GetFileName(mod));
                CopyDirectory(mod, output);
                File.WriteAllText(Path.Combine(output, ".bim_profile"), "");
                if (lovelyIgnore.Checked)
                {
                    string lovelyIgnorePath = Path.Combine(output, ".lovelyignore");
                    if (File.Exists(lovelyIgnorePath))
                    {
                        try
                        {
                            File.Delete(lovelyIgnorePath);
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                }
            }


            string exePath = Path.Combine(balatroSteamDirStr, "Balatro.exe");
            if (File.Exists(exePath))
            {
                Process.Start(exePath);
            }
            else
            {
                MessageBox.Show("Executable not found. Please check the path and try again.", "Error", MessageBoxButtons.OK);
            }
        }
        private List<string> GetProfileMods()
        {
            string modProfilesPath = Path.Combine(balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath)) return null;

            Directory.CreateDirectory(modProfilesPath);

            var subdirectories = Directory.GetDirectories(modProfilesPath);

            List<string> mods = new List<string>();

            foreach (var subdirectory in subdirectories)
            {
                string profilePath = Path.Combine(subdirectory, "profile.json");
                if (!File.Exists(profilePath))
                    File.WriteAllText(profilePath, "{\"Enabled\": false}");

                try
                {
                    string jsonContent = File.ReadAllText(profilePath);
                    var profile = JsonConvert.DeserializeObject<Profile>(jsonContent);

                    if (!profile.Enabled) continue;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading JSON in {subdirectory}: {ex.Message}");
                    continue;
                }

                var modsInProfile = Directory.GetDirectories(subdirectory);
                foreach (var mod in modsInProfile)
                {
                    mods.Add(mod);
                }
            }

            return mods;
        }

        private void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var subdir in Directory.GetDirectories(sourceDir))
            {
                string destSubdir = Path.Combine(destDir, Path.GetFileName(subdir));
                CopyDirectory(subdir, destSubdir);
            }
        }

        private void ClearProfileMods()
        {
            string modsDirectory = Path.Combine(balatroPathStr, "Mods");
            if (!Directory.Exists(modsDirectory)) return;

            var modFolders = Directory.GetDirectories(modsDirectory);
            foreach (var modFolder in modFolders)
            {
                string bimProfilePath = Path.Combine(modFolder, ".bim_profile");
                if (File.Exists(bimProfilePath))
                {
                    try
                    {
                        Directory.Delete(modFolder, true);
                    }
                    catch (Exception)
                    {
                        continue;
                    }
                }
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            string modProfilesPath = Path.Combine(balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath))
            {
                MessageBox.Show("Balatro data path is not accessible, make sure to check the path before launching.", "Information", MessageBoxButtons.OK);
                return;
            }

            if (!IsDirectoryAccessible(balatroSteamDirStr))
            {
                MessageBox.Show("Balatro steam path is not accessible. Please check the path and try again.", "Information", MessageBoxButtons.OK);
                return;
            }

            string modsDirectory = Path.Combine(balatroPathStr, "Mods");
            if (!Directory.Exists(modsDirectory))
            {
                MessageBox.Show("Mods directory does not exist. Make sure you have lovely installed and have opened the game at least once since then and try again.", "Error", MessageBoxButtons.OK);
                return;
            }

            ClearProfileMods();
            MessageBox.Show("Mods cleared successfully.", "Information", MessageBoxButtons.OK);
        }
    }

}

public class Profile
{
    public bool Enabled { get; set; }
}

public class ModData
{
    public string id { get; set; }
    public string name { get; set; }
}
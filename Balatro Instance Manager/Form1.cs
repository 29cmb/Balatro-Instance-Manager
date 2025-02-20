using System.Diagnostics;
using Newtonsoft.Json;
using Balatro_Instance_Manager.Helpers;

namespace Balatro_Instance_Manager
{
    public partial class Form1 : Form
    {
        private string? _balatroPathStr;
        private string? _balatroSteamDirStr;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            LocalizationManager.LoadLocalizations();
            
            _balatroPathStr = Properties.Settings.Default.BalatroPath;
            if (string.IsNullOrEmpty(_balatroPathStr))
            {
                _balatroPathStr = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Balatro");
            }

            _balatroSteamDirStr = Properties.Settings.Default.BalatroSteamPath;
            if (string.IsNullOrEmpty(_balatroPathStr))
            {
                _balatroSteamDirStr = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Balatro";
            }

            balatroPath.Text = _balatroPathStr;
            balatroSteamPath.Text = _balatroSteamDirStr;
            LoadProfiles();
        }

        private void balatroPath_Unfocused(object sender, EventArgs e)
        {
            if (balatroPath.Text == "")
            {
                balatroPath.Text = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Balatro");
            }

            if (!IsDirectoryAccessible(balatroPath.Text))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"], LocalizationManager.LocalizedStrings["InfoHeader"], MessageBoxButtons.OK);
                return;
            }

            _balatroPathStr = balatroPath.Text;

            Properties.Settings.Default.BalatroPath = _balatroPathStr;
            Properties.Settings.Default.Save();

            LoadProfiles();
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

        private void LoadProfiles()
        {
            if (_balatroPathStr == null)
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            var modProfilesPath = Path.Combine(_balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Directory.CreateDirectory(modProfilesPath);
            var subdirectories = Directory.GetDirectories(modProfilesPath);

            instances.Controls.Clear();

            foreach (var subdirectory in subdirectories)
            {
                var profilePath = Path.Combine(subdirectory, "profile.json");
                if (!File.Exists(profilePath))
                    File.WriteAllText(profilePath, "{\"Enabled\": false}");
                
                var jsonContent = File.ReadAllText(profilePath);
                var profile = JsonConvert.DeserializeObject<Types.Profile>(jsonContent);
                if (profile == null) return;
                
                var checkBox = new CheckBox();
                checkBox.Text = Path.GetFileName(subdirectory);
                checkBox.Checked = profile.Enabled;
                checkBox.CheckedChanged += OnCheckClicked;

                instances.Controls.Add(checkBox);
            }
        }
        private static bool IsDirectoryAccessible(string path)
        {
            try
            {
                if (Directory.Exists(path))
                {
                    Directory.GetFiles(path);
                    return true;
                }

                return false;
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

        private void OnCheckClicked(object? sender, EventArgs e)
        {
            if (sender is not CheckBox checkBox || _balatroPathStr == null) return;
            var profilePath = Path.Combine(_balatroPathStr, "ModProfiles", checkBox.Text, "profile.json");
            if (!File.Exists(profilePath)) return;
            
            var jsonContent = File.ReadAllText(profilePath);
            var profile = JsonConvert.DeserializeObject<Types.Profile>(jsonContent);
            if (profile == null) return;
            
            profile.Enabled = checkBox.Checked;
            File.WriteAllText(profilePath, JsonConvert.SerializeObject(profile));
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
                MessageBox.Show(LocalizationManager.LocalizedStrings["SteamInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _balatroSteamDirStr = balatroSteamPath.Text;

            Properties.Settings.Default.BalatroSteamPath = _balatroSteamDirStr;
            Properties.Settings.Default.Save();

            LoadProfiles();
        }

        private void launch_Click(object sender, EventArgs e)
        {
            if (_balatroSteamDirStr == null || !IsDirectoryAccessible(_balatroSteamDirStr))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["SteamInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (_balatroPathStr == null)
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var modProfilesPath = Path.Combine(_balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"],
                    LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var mods = GetProfileMods();
            if (mods == null || mods.Count == 0) return;

            var loadedModIds = new HashSet<string>();
            var validMods = new List<string>();

            foreach (var mod in mods)
            {
                var jsonFiles = Directory.GetFiles(mod, "*.json");
                var isValidMod = true;

                foreach (var jsonFile in jsonFiles)
                {
                    var jsonContent = File.ReadAllText(jsonFile);
                    var modData = JsonConvert.DeserializeObject<Types.ModData>(jsonContent);

                    if (modData != null)
                    {
                        if (!string.IsNullOrEmpty(modData.Id))
                        {
                            if (loadedModIds.Contains(modData.Id))
                            {
                                isValidMod = false;
                                break;
                            }
                        }
                        else if (!string.IsNullOrEmpty(modData.Name))
                        {
                            if (loadedModIds.Contains(modData.Name))
                            {
                                isValidMod = false;
                                break;
                            }
                        }
                    }
                }

                if (isValidMod)
                {
                    validMods.Add(mod);
                    foreach (var jsonFile in jsonFiles)
                    {
                        string jsonContent = File.ReadAllText(jsonFile);
                        var modData = JsonConvert.DeserializeObject<Types.ModData>(jsonContent);

                        if (modData != null)
                        {
                            if (!string.IsNullOrEmpty(modData.Id))
                            {
                                loadedModIds.Add(modData.Id);
                            }
                            else if (!string.IsNullOrEmpty(modData.Name))
                            {
                                loadedModIds.Add(modData.Name);
                            }
                        }
                    }
                }
            }

            var modsDirectory = Path.Combine(_balatroPathStr, "Mods");
            if (!Directory.Exists(modsDirectory))
            {
                MessageBox.Show(
                    LocalizationManager.LocalizedStrings["NoModsDirectory"],
                    LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (deselected.Checked) ClearProfileMods();

            foreach (var mod in validMods)
            {
                var output = Path.Combine(modsDirectory, Path.GetFileName(mod));
                CopyDirectory(mod, output);
                File.WriteAllText(Path.Combine(output, ".bim_profile"), "");
                if (lovelyIgnore.Checked)
                {
                    var lovelyIgnorePath = Path.Combine(output, ".lovelyignore");
                    if (File.Exists(lovelyIgnorePath))
                    {
                        File.Delete(lovelyIgnorePath);
                    }
                }
            }


            var exePath = Path.Combine(_balatroSteamDirStr, "Balatro.exe");
            if (File.Exists(exePath))
            {
                var processName = Path.GetFileNameWithoutExtension(exePath);
                if (Process.GetProcessesByName(processName).Length > 0 && !multiInstance.Checked)
                {
                    MessageBox.Show(LocalizationManager.LocalizedStrings["AlreadyRunning"], LocalizationManager.LocalizedStrings["ErrorHeader"],
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    Process.Start(exePath);
                }
            }
            else
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["BalatroNotFound"], LocalizationManager.LocalizedStrings["ErrorHeader"],
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<string>? GetProfileMods()
        {
            if (_balatroPathStr == null) return null;
            var modProfilesPath = Path.Combine(_balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath)) return null;

            Directory.CreateDirectory(modProfilesPath);

            var subdirectories = Directory.GetDirectories(modProfilesPath);

            var mods = new List<string>();

            foreach (var subdirectory in subdirectories)
            {
                var profilePath = Path.Combine(subdirectory, "profile.json");
                if (!File.Exists(profilePath))
                    File.WriteAllText(profilePath, "{\"Enabled\": false}");
                
                var jsonContent = File.ReadAllText(profilePath);
                var profile = JsonConvert.DeserializeObject<Types.Profile>(jsonContent);
                if (profile is not {Enabled: true}) continue;

                var modsInProfile = Directory.GetDirectories(subdirectory);
                mods.AddRange(modsInProfile);
            }

            return mods;
        }

        private static void CopyDirectory(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);

            foreach (var file in Directory.GetFiles(sourceDir))
            {
                var destFile = Path.Combine(destDir, Path.GetFileName(file));
                File.Copy(file, destFile, true);
            }

            foreach (var subdir in Directory.GetDirectories(sourceDir))
            {
                var destSubdir = Path.Combine(destDir, Path.GetFileName(subdir));
                CopyDirectory(subdir, destSubdir);
            }
        }

        private void ClearProfileMods()
        {
            if (_balatroPathStr == null) return;
            var modsDirectory = Path.Combine(_balatroPathStr, "Mods");
            if (!Directory.Exists(modsDirectory)) return;

            var modFolders = Directory.GetDirectories(modsDirectory);
            foreach (var modFolder in modFolders)
            {
                var bimProfilePath = Path.Combine(modFolder, ".bim_profile");
                if (File.Exists(bimProfilePath))
                {
                    Directory.Delete(modFolder, true);
                }
            }
        }

        private void clear_Click(object sender, EventArgs e)
        {
            if (_balatroPathStr == null)
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"],
                    LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (_balatroSteamDirStr == null)
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["SteamInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var modProfilesPath = Path.Combine(_balatroPathStr, "ModProfiles");
            if (!IsDirectoryAccessible(modProfilesPath))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["DataInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!IsDirectoryAccessible(_balatroSteamDirStr))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["SteamInaccessible"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var modsDirectory = Path.Combine(_balatroPathStr, "Mods");
            if (!Directory.Exists(modsDirectory))
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["NoModsDirectory"], LocalizationManager.LocalizedStrings["ErrorHeader"], MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ClearProfileMods();
            MessageBox.Show(LocalizationManager.LocalizedStrings["ModsCleared"], LocalizationManager.LocalizedStrings["InfoHeader"], MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void multiInstance_CheckedChanged(object sender, EventArgs e)
        {
            if (multiInstance.Checked)
            {
                MessageBox.Show(LocalizationManager.LocalizedStrings["MultiInstanceUnstable"], LocalizationManager.LocalizedStrings["WarningHeader"], MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }

}
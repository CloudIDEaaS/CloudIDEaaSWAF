using System.Windows.Forms.Design;

public class LanguageListBox : System.Windows.Forms.ListBox
{
    // The editor service displaying us.
    private IWindowsFormsEditorService m_EditorService;

    public LanguageListBox(IWindowsFormsEditorService editor_service, string Value)
    {
        m_EditorService = editor_service;
        LoadLanguages(Value);
    }

    private void LoadLanguages(string Value = "")
    {
        this.Items.Clear();
        // Get all languages in list
        Microsoft.Win32.RegistryKey regKey;
        regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\NHunspellTextBoxExtender\\Languages");
        string[] languages = regKey.GetValue("LanguageList") as string[];
        foreach (string language in languages)
        {
            this.Items.Add(language);
        }
        regKey.Dispose();
        this.Items.Add("<Add a new language>");
        for (int i = 0; i <= this.Items.Count - 1; i++)
        {
            if (this.Items[i] == Value)
            {
                this.SelectedIndex = i;
                break;
            }
        }
    }

    private void LanguageListBox_Click(object sender, System.EventArgs e)
    {
        if (this.SelectedItem == "<Add a new language>")
        {
            AddLanguage newAddLang = new AddLanguage();
            newAddLang.ShowDialog();
            if (newAddLang.Result == DialogResult.Cancel)
            {
                return;
            }
            // Add the Item to the registry
            Microsoft.Win32.RegistryKey regKey;
            regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\NHunspellTextBoxExtender\\Languages", true);
            string[] languages = regKey.GetValue("LanguageList") as string[];
            bool boolFound = false;
            foreach (string language in languages)
            {
                if (language == newAddLang.txtName.Text)
                {
                    boolFound = true;
                    break;
                }
            }
            if (!boolFound)
            {
                Array.Resize(ref languages, languages.Length + 1);
                languages[languages.Length - 1] = newAddLang.txtName.Text;
                regKey.SetValue("LanguageList", languages, Microsoft.Win32.RegistryValueKind.MultiString);
            }
            string[] paths = new string[2];
            paths[0] = newAddLang.txtAff.Text;
            paths[1] = newAddLang.txtDic.Text;
            regKey.SetValue(newAddLang.txtName.Text, paths, Microsoft.Win32.RegistryValueKind.MultiString);
            regKey.Close();
            regKey.Dispose();
            int selection = this.Items.Add(newAddLang.txtName.Text);
            this.SelectedIndex = selection;
        }
        else
        {
            // Check if the paths are still valid
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\NHunspellTextBoxExtender\\Languages", true);
            string[] paths = regKey.GetValue((string) this.SelectedItem) as string[];
            if (paths == null)
            {
                System.Windows.Forms.MessageBox.Show("Aff and Dic files are missing");
                string[] languages = regKey.GetValue("LanguageList") as string[];
                string[] newLanguageList = new string[languages.Length - 1];
                int count = 0;
                foreach (string language in languages)
                {
                    if (language != this.SelectedItem)
                    {
                        newLanguageList[count] = language;
                        count += 1;
                    }
                }
                regKey.SetValue("LanguageList", newLanguageList, Microsoft.Win32.RegistryValueKind.MultiString);
                regKey.DeleteValue((string) this.SelectedItem);
                LoadLanguages();
                regKey.Close();
                regKey.Dispose();
                return;
            }
            else
            {
                foreach (string path in paths)
                {
                    if (!System.IO.File.Exists(path))
                    {
                        System.Windows.Forms.MessageBox.Show("Aff and Dic files are missing");
                        string[] languages = regKey.GetValue("LanguageList") as string[];
                        string[] newLanguageList = new string[languages.Length - 1];
                        int count = 0;
                        foreach (string language in languages)
                        {
                            if (language != this.SelectedItem)
                            {
                                newLanguageList[count] = language;
                                count += 1;
                            }
                        }
                        regKey.SetValue("LanguageList", newLanguageList, Microsoft.Win32.RegistryValueKind.MultiString);
                        regKey.DeleteValue((string) this.SelectedItem);
                        LoadLanguages();
                        regKey.Close();
                        regKey.Dispose();
                        return;
                    }
                }
            }
        }
        if (m_EditorService != null)
        {
            m_EditorService.CloseDropDown();
        }
    }
}



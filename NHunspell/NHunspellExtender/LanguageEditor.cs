using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using Microsoft.Win32;

public class LanguageEditor : UITypeEditor
{
    public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
    {
        return UITypeEditorEditStyle.DropDown;
    }

    public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
    {
        // Get an IWindowsFormsEditorService.
        IWindowsFormsEditorService editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
        // If we failed to get the editor service, return the value.
        if (editorService == null) return value;
        string strValue = value as string;
        if (strValue == null) return value;
        LanguageListBox newListBox = new LanguageListBox(editorService, strValue);
        editorService.DropDownControl(newListBox);
        // Add the Item to the registry
        RegistryKey regKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\NHunspellTextBoxExtender\\Languages", true);
        regKey.SetValue("Default", newListBox.SelectedItem);
        regKey.Close();
        regKey.Dispose();
        return newListBox.SelectedItem;
    }
}



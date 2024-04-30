using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Utils
{
    /// <summary>
    /// This is a UITypeEditor base class usefull for simple editing of control properties 
    /// in a DropDown or a ModalDialogForm window at design mode (in VisualStudio.Net IDE). 
    /// To use this, inherits a class from it and add this attribute to your control property(ies): 
    /// &lt;Editor(GetType(MyPropertyEditor), GetType(System.Drawing.Design.UITypeEditor))&gt;  
    /// </summary>
    public abstract class PropertyEditorBase : UITypeEditor
    {
        protected IWindowsFormsEditorService editorService;
        private bool escapePressed;
        private Control editControl;

        public virtual Control EditControl
        {
            get
            {
                return editControl;
            }

            set
            {
                PreviewKeyDownEventHandler previewKeyDownEventHandler = new PreviewKeyDownEventHandler(this.editControl_PreviewKeyDown);
                Control editControl = this.editControl;

                if (editControl != null)
                {
                    editControl.PreviewKeyDown -= previewKeyDownEventHandler;
                }

                this.editControl = value;

                editControl = this.editControl;

                if (editControl != null)
                {
                    editControl.PreviewKeyDown += previewKeyDownEventHandler;
                }
            }
        }

        protected PropertyEditorBase()
        {
        }

        /// <summary>Close DropDown window to finish editing</summary>
        public void CloseDropDownWindow()
        {
            if (this.editorService != null)
            {
                this.editorService.CloseDropDown();
            }
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            object editValue;

            try
            {
                if ((context == null ? false : provider != null))
                {
                    this.editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));

                    if (this.editorService != null)
                    {
                        var propertyName = context.PropertyDescriptor.Name;

                        this.editControl = this.GetEditControl(propertyName, RuntimeHelpers.GetObjectValue(value));

                        if (this.editControl != null)
                        {
                            this.escapePressed = false;

                            if (!(this.editControl is Form))
                            {
                                this.editorService.DropDownControl(this.editControl);
                            }
                            else
                            {
                                this.editorService.ShowDialog((Form)this.editControl);
                            }

                            if (!this.escapePressed)
                            {
                                editValue = this.GetEditedValue(this.editControl, propertyName, RuntimeHelpers.GetObjectValue(value));
                                return editValue;
                            }
                            else
                            {
                                editValue = value;
                                return editValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }

            editValue = base.EditValue(context, provider, RuntimeHelpers.GetObjectValue(value));

            return editValue;
        }

        /// <summary>
        /// The driven class should provide its edit Control to be shown in the 
        /// DropDown or DialogForm window by means of this function. 
        /// If specified control be a Form, it is shown in a Modal Form, otherwise, it is shown as in a DropDown window. 
        /// This edit control should return its final value at GetEditedValue() method. 
        /// </summary>
        protected abstract Control GetEditControl(string propertyName, object currentValue);

        /// <summary>The driven class should return the New Value for edited property at this function.</summary>
        /// <param name="EditControl">
        /// The control shown in DropDown window and used for editing. 
        /// This is the control you pass in GetEditControl() function.
        /// </param>
        /// <param name="OldValue">The original value of the property before editing through the DropDown window.</param>
        protected abstract object GetEditedValue(Control editControl, string propertyName, object oldValue);

        /// <summary>
        /// Sets the edit style mode based on the type of EditControl: DropDown or Modal(Dialog). 
        /// Note that the driven class can also override this function and explicitly specify the EditStyle value.
        /// </summary>
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
        {
            UITypeEditorEditStyle style;

            style = UITypeEditorEditStyle.DropDown;

            if (context == null)
            {
                return style;
            }

            try
            {
                if (this.GetEditControl(context.PropertyDescriptor.Name, RuntimeHelpers.GetObjectValue(context.PropertyDescriptor.GetValue(RuntimeHelpers.GetObjectValue(context.Instance)))) is Form)
                {
                    style = UITypeEditorEditStyle.Modal;
                    return style;
                }
            }
            catch (Exception ex)
            {
                DebugUtils.Break();
            }

            style = UITypeEditorEditStyle.DropDown;
            
            return style;
        }

        /// <summary>
        /// Provides the interface for this UITypeEditor to display Windows Forms or to 
        /// display a control in a DropDown area from the property grid control in design mode.
        /// </summary>
        public IWindowsFormsEditorService GetIWindowsFormsEditorService()
        {
            return this.editorService;
        }

        private void editControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.escapePressed = true;
            }
        }
    }
}

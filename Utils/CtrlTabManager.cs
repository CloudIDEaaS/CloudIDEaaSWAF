using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils
{
    public class CtrlTabManager : IMessageFilter
    {
        public List<Control> ControlTabInclusions { get; private set; }
        private Control lastCtrlTabFrom;
        public event EventHandler<CtrlTabEventArgs> OnControlTab;
        public event EventHandler PostControlTab;
        private bool keydown;

        public CtrlTabManager(Form form)
        {
            ControlTabInclusions = new List<Control>();
            Application.AddMessageFilter(this);

            foreach (var tabControl in form.GetAllControls().OfType<TabControl>())
            {
                tabControl.PreviewKeyDown += new PreviewKeyDownEventHandler(tabControl_PreviewKeyDown);
            }
        }

        private void tabControl_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab && e.Modifiers.HasFlag(Keys.Control))
            {
                e.IsInputKey = true;
            }
        }

        public bool PreFilterMessage(ref Message m)
        {
            var message = (Utils.ControlExtensions.WindowsMessage)m.Msg;

            if (message == ControlExtensions.WindowsMessage.KEYDOWN)
            {
                switch ((Keys)m.WParam)
                {
                    case Keys.Tab:

                        if (Keys.ControlKey.IsPressed())
                        {
                            var selectedForm = Form.ActiveForm;
                            var selectedControl = selectedForm.GetFocus();

                            keydown = true;

                            if (OnControlTab != null)
                            {
                                CtrlTabEventArgs args;

                                if (selectedControl == null)
                                {
                                    args = new CtrlTabEventArgs(ControlExtensions.GetFocus());
                                }
                                else
                                {
                                    args = new CtrlTabEventArgs(selectedControl);
                                }

                                OnControlTab(this, args);

                                if (args.CancelOperation)
                                {
                                    return true;
                                }
                                else if (args.SelectedControl != selectedControl)
                                {
                                    args.SelectedControl.ActivateOrSetFocus();
                                    return true;
                                }
                            }

                            if (lastCtrlTabFrom == null || lastCtrlTabFrom == selectedControl)
                            {
                                lastCtrlTabFrom = selectedControl;

                                DoControlTab(Keys.ShiftKey.IsPressed());
                            }
                            else if (lastCtrlTabFrom != null)
                            {
                                lastCtrlTabFrom.ActivateOrSetFocus();
                                lastCtrlTabFrom = selectedControl;
                            }

                            return true;
                        }

                        break;
                }
            }
            else if (message == ControlExtensions.WindowsMessage.KEYUP)
            {
                if (!Keys.ControlKey.IsPressed())
                {
                    if (keydown)
                    {
                        keydown = false;

                        if (PostControlTab != null)
                        {
                            PostControlTab(this, EventArgs.Empty);
                        }
                    }
                }
            }

            return false;
        }

        private Control DoControlTab(bool reverse = false)
        {
            var formsAndControls = Application.OpenForms.Cast<Control>().ToList();
            var parentFormRemove = new List<Control>();
            var index = 0;
            var form = Form.ActiveForm;
            var currentlySelectedControl = (Control)form;
            Control selectControl;

            // remove forms if its child controls are in the list

            foreach (var childControl in ControlTabInclusions)
            {
                var parentForm = childControl.GetParentForm();
                var indexOfParentForm = formsAndControls.IndexOf(parentForm);

                if (!parentFormRemove.Contains(parentForm))
                {
                    parentFormRemove.Add(parentForm);
                }
            }

            // for each form that has child controls in the list, insert the child controls, then remove the forms

            foreach (var openForm in formsAndControls.ToList().Cast<Form>())
            {
                var childControlInserts = ControlTabInclusions.Where(c => c.GetParentForm() == openForm);
                var childControlCount = childControlInserts.Count();

                if (childControlCount > 0)
                {
                    if (openForm == form)
                    {
                        var focusControl = form.GetFocus();

                        if (childControlInserts.Any(c => c == focusControl))
                        {
                            currentlySelectedControl = childControlInserts.Single(c => c == focusControl);
                        }
                    }

                    formsAndControls.InsertRange(index, childControlInserts);
                    index += childControlInserts.Count() + 1;
                }
                else
                {
                    index++;
                }
            }

            foreach (var formRemove in parentFormRemove)
            {
                formsAndControls.Remove(formRemove);
            }

            // get next index of current selected

            index = formsAndControls.IndexOf(currentlySelectedControl);

            if (reverse)
            {
                index = index - 1 > 1 ? formsAndControls.Count() - 1 : index - 1;
            }
            else
            {
                index = index + 1 >= formsAndControls.Count() ? 0 : index + 1;
            }

            selectControl = formsAndControls.ElementAt(index);

            // select (focus or activate) the next in list

            selectControl.ActivateOrSetFocus();

            return selectControl;
        }
    }
}

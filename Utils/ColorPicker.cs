using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Collections.Specialized;
using System.Collections;
using System.Linq;

namespace Utils
{
    public partial class ColorPicker : ComboBox
    {
        public Dictionary<string, Color?> Colors { get; private set; }

        public ColorPicker()
        {
            var colorType = typeof(System.Drawing.Color);
            var properties = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);

            this.Colors = new Dictionary<string, Color?>();

            this.Colors.Add(string.Empty, null);

            foreach (PropertyInfo property in properties.Where(p => p.Name.IsOneOf("Red", "Orange", "Green", "Blue", "Purple")).DistinctByUtils(p => p.Name))
            {
                this.Colors.Add(property.Name, Color.FromName(property.Name));
            }

            foreach (PropertyInfo property in properties.Where(p => !p.Name.IsOneOf("Red", "Orange", "Green", "Blue", "Purple")).OrderBy(p => p.Name).DistinctByUtils(p => p.Name))
            {
                this.Colors.Add(property.Name, Color.FromName(property.Name));
            }

            this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DrawItem += new DrawItemEventHandler(ColorPicker_DrawItem);
            this.Sorted = false;
        }

        public Color? SelectedColor
        {
            get
            {
                var colorName = (string)this.SelectedItem;

                if (!colorName.IsNullOrEmpty())
                {
                    return this.Colors[colorName];
                }
                else
                {
                    return null;
                }
            }
        }

        private void ColorPicker_DrawItem(object sender, DrawItemEventArgs e)
        {
            var graphics = e.Graphics;
            var rect = e.Bounds;

            if (e.Index >= 0 && e.Index < this.Items.Count)
            {
                var colorText = this.Items[e.Index].ToString();
                var color = (Color?) this.Colors[colorText];

                if (color.HasValue)
                {
                    var brush = new SolidBrush(color.Value);

                    graphics.FillRectangle(new SolidBrush(Color.White), rect.X, rect.Y, rect.Width, rect.Height);
                    graphics.DrawString(colorText, this.Font, Brushes.Black, rect.X, rect.Top);
                    graphics.FillRectangle(brush, rect.X + 110, rect.Y + 2, rect.Width - 4, rect.Height - 4);
                }
                else
                {
                    graphics.FillRectangle(new SolidBrush(Color.White), rect.X, rect.Y, rect.Width, rect.Height);
                }
            }
        }

        public void AddCustomColor(string name, Color color, bool select = false)
        {
            var index = 0;

            if (this.Colors.ContainsKey(name))
            {
                index = this.Items.Cast<string>().ToList().IndexOf(name);

                this.Colors[name] = color;
            }
            else
            {
                this.Colors.Add(name, color);
                this.Items.Insert(index, name);
            }

            if (select)
            {
                this.SelectedIndex = index;
            }

            this.Refresh();
        }

        protected override void OnCreateControl()
        {
            this.Items.Clear();

            foreach (var pair in this.Colors)
            {
                this.Items.Add(pair.Key);
            }

            base.OnCreateControl();
        }

        public void ResetColors()
        {
            var colorType = typeof(System.Drawing.Color);
            var properties = colorType.GetProperties(BindingFlags.Static | BindingFlags.DeclaredOnly | BindingFlags.Public);

            this.Colors = new Dictionary<string, Color?>();

            this.Colors.Add(string.Empty, null);

            foreach (PropertyInfo property in properties.Where(p => p.Name.IsOneOf("Red", "Orange", "Green", "Blue", "Purple")).DistinctByUtils(p => p.Name))
            {
                this.Colors.Add(property.Name, Color.FromName(property.Name));
            }

            foreach (PropertyInfo property in properties.Where(p => !p.Name.IsOneOf("Red", "Orange", "Green", "Blue", "Purple")).OrderBy(p => p.Name).DistinctByUtils(p => p.Name))
            {
                this.Colors.Add(property.Name, Color.FromName(property.Name));
            }
        }
    }
}

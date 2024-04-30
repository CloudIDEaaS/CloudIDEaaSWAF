using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Utils.GlyphDrawing
{
    public class GlyphDictionary : BaseDictionary<Control, List<ControlGlyph>>
    {
        private Dictionary<Control, List<ControlGlyph>> internalDictionary;

        public GlyphDictionary()
        {
            internalDictionary = new Dictionary<Control, List<ControlGlyph>>();
        }

        public override int Count
        {
            get 
            {
                return internalDictionary.Count;
            }
        }

        public override void Clear()
        {
            using (var notifier = this.Notify("this[]", "Count"))
            {
                internalDictionary.Clear();
            }
        }

        public override void Add(Control key, List<ControlGlyph> value)
        {
            using (var notifier = this.Notify("this[]", "Count"))
            {
                internalDictionary.Add(key, value);
            }
        }

        public override bool ContainsKey(Control key)
        {
            return internalDictionary.ContainsKey(key);
        }

        public override bool Remove(Control key)
        {
            using (var notifier = this.Notify("this[]", "Count"))
            {
                return internalDictionary.Remove(key);
            }
        }

        public override bool TryGetValue(Control key, out List<ControlGlyph> value)
        {
            return internalDictionary.TryGetValue(key, out value);
        }

        public override IEnumerator<KeyValuePair<Control, List<ControlGlyph>>> GetEnumerator()
        {
            return internalDictionary.GetEnumerator();
        }

        protected override void SetValue(Control key, List<ControlGlyph> value)
        {
            using (var notifier = this.Notify("this[]", "Count"))
            {
                internalDictionary[key] = value;
            }
        }
    }
}

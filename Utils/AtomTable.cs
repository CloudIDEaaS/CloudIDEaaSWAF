using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class AtomTable : BaseDictionary<ushort, string>
    {
        public override int Count => throw new NotImplementedException();

        static AtomTable()
        {
            StringExtensions.InitAtomTable((uint)(1024 * NumberExtensions.KB));
        }

        public uint Add(string value)
        {
            return StringExtensions.AddAtom(value);
        }

        public override void Add(ushort key, string value)
        {
            throw new NotImplementedException();
        }

        public override void Clear()
        {
            throw new NotImplementedException();
        }

        public override bool ContainsKey(ushort key)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator<KeyValuePair<ushort, string>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public override bool Remove(ushort key)
        {
            throw new NotImplementedException();
        }

        public override bool TryGetValue(ushort key, out string value)
        {
            var builder = new StringBuilder(255);
            string name;

            if (StringExtensions.GetAtomName(key, builder, 255) > 0)
            {
                name = builder.ToString().RemoveTrailingNulls();

                value = name;

                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }

        protected override void SetValue(ushort key, string value)
        {
            throw new NotImplementedException();
        }
    }
}

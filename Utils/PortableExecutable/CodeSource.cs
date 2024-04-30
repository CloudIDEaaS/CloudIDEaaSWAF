using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils;

namespace Utils.PortableExecutable
{
    public class CodeSource
    {
        public CodeTypeFlags CodeType { get; private set; }
        public string Name { get; private set; }
        public string FullName { get; private set; }
        public string SyntaxLanguageKey { get; private set; }
        public byte[] CodeBytes { get; private set; }
        public IPEImage CurrentPEImage { get; set; }
        public IImageReader ImageReader { get; set; }
        public ulong Offset { get; private set; }

        public CodeSource(string name, string fullName, CodeTypeFlags codeType, string syntaxLanguageKey, byte[] codeBytes, ulong offset)
        {
            this.Name = name;
            this.FullName = fullName;
            this.CodeType = codeType;
            this.SyntaxLanguageKey = syntaxLanguageKey;
            this.Offset = offset;
            this.CodeBytes = codeBytes;
        }

        public bool IsDisposed
        {
            get 
            {
                return false;
            }
        }

        public bool QueryInterface(Guid iid, out IUnknown interfaceObject)
        {
            throw new NotImplementedException();
        }

        public bool QueryInterface<T>(out T interfaceObject)
        {
            var type = this.GetType();

            if (type.Implements(typeof(T)))
            {
                interfaceObject = (T)(object) this;
                return true;
            }
            else
            {
                interfaceObject = default(T);
                return false;
            }
        }
    }
}

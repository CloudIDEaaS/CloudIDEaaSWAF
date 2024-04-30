using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.XCopy
{
    public sealed class FolderXCopyResult
    {
        public int CopiedFileCount
        {
            get;
            set;
        }

        public int CopiedFolderCount
        {
            get;
            set;
        }

        public int SkippedFileCount
        {
            get;
            set;
        }

        public int SkippedFolderCount
        {
            get;
            set;
        }

        public FolderXCopyResult()
        {
        }
    }
}
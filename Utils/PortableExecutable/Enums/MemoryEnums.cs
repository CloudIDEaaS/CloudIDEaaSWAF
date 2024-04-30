using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.PortableExecutable.Enums
{
    [Flags()]
    public enum HeapCreateOptions : uint
    {
        HEAP_CREATE_ENABLE_EXECUTE = 0x00040000,
        HEAP_GENERATE_EXCEPTIONS = 0x00000004,
        HEAP_NO_SERIALIZE = 0x00000001,
    }

    [Flags()]
    public enum HeapAllocFlags : uint
    {
        HEAP_GENERATE_EXCEPTIONS = 0x00000004,
        HEAP_NO_SERIALIZE = 0x00000001,
        HEAP_ZERO_MEMORY = 0x00000008
    }

    [Flags()]
    public enum HeapReallocFlags : uint
    {
        HEAP_GENERATE_EXCEPTIONS = 0x00000004,
        HEAP_NO_SERIALIZE = 0x00000001,
        HEAP_REALLOC_IN_PLACE_ONLY = 0x00000010,
        HEAP_ZERO_MEMORY = 0x00000008
    }

    [Flags()]
    public enum AllocationType : uint
    {
        COMMIT = 0x1000,
        RESERVE = 0x2000,
        RESET = 0x80000,
        LARGE_PAGES = 0x20000000,
        PHYSICAL = 0x400000,
        TOP_DOWN = 0x100000,
        WRITE_WATCH = 0x200000
    }

    [Flags()]
    public enum MemoryProtection : uint
    {
        EXECUTE = 0x10,
        EXECUTE_READ = 0x20,
        EXECUTE_READWRITE = 0x40,
        EXECUTE_WRITECOPY = 0x80,
        NOACCESS = 0x01,
        READONLY = 0x02,
        READWRITE = 0x04,
        WRITECOPY = 0x08,
        GUARD_Modifierflag = 0x100,
        NOCACHE_Modifierflag = 0x200,
        WRITECOMBINE_Modifierflag = 0x400
    }

    [Flags()]
    public enum VirtualFreeType : uint
    {
        MEM_DECOMMIT = 0x4000,
        MEM_RELEASE = 0x8000
    }
}

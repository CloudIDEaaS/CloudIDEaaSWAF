using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Utils
{
    public abstract class TestsBase
    {
        public IDisposable AnnounceTest(string name)
        {
            var disposable = name.CreateDisposable(() =>
            {
                Console.WriteLine("\r\n***** {0} passed!", name);

            });

            Console.WriteLine("\r\n***** {0} {1}\r\n", name, "*".Repeat(100));

            return disposable;
        }

        public IDisposable AnnounceSetup()
        {
            var name = "Setup";

            var disposable = name.CreateDisposable(() =>
            {
                Console.WriteLine("\r\n***** {0} complete", name);

            });

            Console.WriteLine("\r\n***** {0} started {1}\r\n", name, "*".Repeat(100));

            return disposable;
        }

        public ConsoleKeyInfo WriteLineAndReadKey(string format, params object[] args)
        {
            Console.WriteLine(format, args);
            return Console.ReadKey();
        }

        public IDisposable AnnounceTeardown()
        {
            var name = "Teardown";

            var disposable = name.CreateDisposable(() =>
            {
                Console.WriteLine("\r\n***** {0} complete", name);

            });

            Console.WriteLine("\r\n***** {0} started {1}\r\n", name, "*".Repeat(100));

            return disposable;
        }
    }
}

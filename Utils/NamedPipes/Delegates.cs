using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.NamedPipes
{
    public delegate void PipeExceptionEventHandler(object sender, Exception exception);
    public delegate void PipeMessageEventHandler(object sender, CommandPacket commandPacket);

}

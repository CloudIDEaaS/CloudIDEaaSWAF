using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class EmbeddedEmoji
    {
        public EmbeddedEmoji(REOBJECT reObject, Emoji emoji)
        {
            ReObject = reObject;
            Emoji = emoji;
        }

        public REOBJECT ReObject { get; }
        public Emoji Emoji { get; }
    }
}

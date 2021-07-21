using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace serverClient.XMessages
{
    public enum XMessageProtocols : byte
    {
        HEADER = 254,
        CHAT_EVENT = 1,
        INFO_EVENT = 2,
    }
}

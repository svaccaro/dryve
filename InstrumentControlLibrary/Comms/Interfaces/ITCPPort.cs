using System;
using System.Collections.Generic;
using System.Text;

namespace InstrumentControlLibrary.Comm
{
    interface ITcpPort
    {
        void SendMessage(byte[] msg);
        byte[] ReceiveMessage(int length);
    }
}

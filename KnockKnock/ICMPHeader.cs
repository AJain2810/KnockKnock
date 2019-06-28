using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;

namespace KnockKnock
{
    public class ICMPHeader
    {
        //ICMP Header Fields

        private int type;                //Type code
        private int code;                //ICMP code

        private byte[] byUDPData = new byte[4096];  //Data carried by the packet (For further use)

        public ICMPHeader(byte[] byBuffer, int nReceived)
        {
            MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
            BinaryReader binaryReader = new BinaryReader(memoryStream);

            //The first eight bits contain the type
            type = IPAddress.NetworkToHostOrder(binaryReader.ReadByte());

            //The next eight bits contain the code
            code = IPAddress.NetworkToHostOrder(binaryReader.ReadByte());

            //The next sixteen bits contain the length of the UDP packet

            //Copy the data carried by the UDP packet into the data buffer
            Array.Copy(byBuffer,
                       8,               //The UDP header is of 8 bytes so we start copying after it
                       byUDPData,
                       0,
                       nReceived - 8);
        }

        public int Type
        {
            get
            {
                return type;
            }
        }

        public int Code
        {
            get
            {
                return code;
            }
        }


        public byte[] Data
        {
            get
            {
                return byUDPData;
            }
        }


    }
}

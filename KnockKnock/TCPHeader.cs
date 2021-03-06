﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Net;
using System.Text;
using System;
using System.IO;
using System.Windows.Forms;

namespace KnockKnock
{
    public class TCPFlags
    {
        bool FINFlag;
        bool SYNFlag;
        bool RSTFlag;
        bool ACKFlag;
        bool URGFlag;
        bool PSHFlag;
        public bool PSH
        {
            get
            {
                return this.PSHFlag;
            }
            set
            {
                this.PSHFlag = value;
            }
        }
        public bool SYN
        {
            get
            {
                return this.SYNFlag;
            }
            set
            {
                this.SYNFlag = value;
            }
        }
        public bool RST
        {
            get
            {
                return this.RSTFlag;
            }
            set
            {
                this.RSTFlag = value;
            }
        }
        public bool ACK
        {
            get
            {
                return this.ACKFlag;
            }
            set
            {
                this.ACKFlag = value;
            }
        }
        public bool URG
        {
            get
            {
                return this.URGFlag;
            }
            set
            {
                this.URGFlag = value;
            }
        }
        public bool FIN
        {
            get
            {
                return this.FINFlag;
            }
            set
            {
                this.FINFlag = value;
            }
        }
    }
    public class TCPHeader
    {
        //TCP header fields
        private ushort usSourcePort;              //Sixteen bits for the source port number
        private ushort usDestinationPort;         //Sixteen bits for the destination port number
        private uint uiSequenceNumber = 555;          //Thirty two bits for the sequence number
        private uint uiAcknowledgementNumber = 555;   //Thirty two bits for the acknowledgement number
        private ushort usDataOffsetAndFlags = 555;      //Sixteen bits for flags and data offset
        private ushort usWindow = 555;                  //Sixteen bits for the window size
        private short sChecksum = 555;                 //Sixteen bits for the checksum
                                                       //(checksum can be negative so taken as short)
        private ushort usUrgentPointer;           //Sixteen bits for the urgent pointer
        //End TCP header fields

        private byte byHeaderLength;            //Header length
        private ushort usMessageLength;           //Length of the data being carried
        private byte[] byTCPData = new byte[4096];//Data carried by the TCP packet

        public TCPHeader(byte[] byBuffer, int nReceived)
        {
            try
            {
                MemoryStream memoryStream = new MemoryStream(byBuffer, 0, nReceived);
                BinaryReader binaryReader = new BinaryReader(memoryStream);

                //The first sixteen bits contain the source port
                usSourcePort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //The next sixteen contain the destiination port
                usDestinationPort = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //Next thirty two have the sequence number
                uiSequenceNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

                //Next thirty two have the acknowledgement number
                uiAcknowledgementNumber = (uint)IPAddress.NetworkToHostOrder(binaryReader.ReadInt32());

                //The next sixteen bits hold the flags and the data offset
                usDataOffsetAndFlags = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //The next sixteen contain the window size
                usWindow = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //In the next sixteen we have the checksum
                sChecksum = (short)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //The following sixteen contain the urgent pointer
                usUrgentPointer = (ushort)IPAddress.NetworkToHostOrder(binaryReader.ReadInt16());

                //The data offset indicates where the data begins, so using it we
                //calculate the header length
                byHeaderLength = (byte)(usDataOffsetAndFlags >> 12);
                byHeaderLength *= 4;

                //Message length = Total length of the TCP packet - Header length
                usMessageLength = (ushort)(nReceived - byHeaderLength);

                //Copy the TCP data into the data buffer
                Array.Copy(byBuffer, byHeaderLength, byTCPData, 0, nReceived - byHeaderLength);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MJsniff TCP" + (nReceived), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string SourcePort
        {
            get
            {
                return usSourcePort.ToString();
            }
        }

        public string DestinationPort
        {
            get
            {
                return usDestinationPort.ToString();
            }
        }

        public string SequenceNumber
        {
            get
            {
                return uiSequenceNumber.ToString();
            }
        }

        public string AcknowledgementNumber
        {
            get
            {
                //If the ACK flag is set then only we have a valid value in
                //the acknowlegement field, so check for it beore returning 
                //anything
                if ((usDataOffsetAndFlags & 0x10) != 0)
                {
                    return uiAcknowledgementNumber.ToString();
                }
                else
                    return "";
            }
        }

        public string HeaderLength
        {
            get
            {
                return byHeaderLength.ToString();
            }
        }

        public string WindowSize
        {
            get
            {
                return usWindow.ToString();
            }
        }

        public string UrgentPointer
        {
            get
            {
                //If the URG flag is set then only we have a valid value in
                //the urgent pointer field, so check for it beore returning 
                //anything
                if ((usDataOffsetAndFlags & 0x20) != 0)
                {
                    return usUrgentPointer.ToString();
                }
                else
                    return "";
            }
        }

        public TCPFlags Flags
        {
            get
            {
                TCPFlags tcp = new TCPFlags();
                
                int nFlags = usDataOffsetAndFlags & 0x3F;

                //string strFlags = string.Format("0x{0:x2} (", nFlags);

                //Now we start looking whether individual bits are set or not
                if ((nFlags & 0x01) != 0)
                {
                    tcp.FIN = true;
                }
                if ((nFlags & 0x02) != 0)
                {
                    tcp.SYN = true;
                }
                if ((nFlags & 0x04) != 0)
                {
                    tcp.RST = true;
                }
                if ((nFlags & 0x08) != 0)
                {
                    tcp.PSH = true;
                }
                if ((nFlags & 0x10) != 0)
                {
                    tcp.ACK = true;
                }
                if ((nFlags & 0x20) != 0)
                {
                    tcp.URG = true;
                }

                return tcp;
            }
        }

        public string Checksum
        {
            get
            {
                //Return the checksum in hexadecimal format
                return string.Format("0x{0:x2}", sChecksum);
            }
        }

        public byte[] Data
        {
            get
            {
                return byTCPData;
            }
        }

        public ushort MessageLength
        {
            get
            {
                return usMessageLength;
            }
        }
    }
}

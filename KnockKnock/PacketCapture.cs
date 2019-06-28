using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;

namespace KnockKnock
{
    class PacketCapture
    {
        private Socket mainSocket;                          //The socket which captures all incoming packets
        private byte[] byteData = new byte[4096];
        public void DriverCode()
        {
            var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork).AsEnumerable();
            Console.WriteLine("Protocol\tSourceIP:Port\t===>\tDestinationIP:Port");
            
            foreach (IPAddress ip in IPv4Addresses)
                Sniff(ip);
            
        }

        public PacketCapture()
        {
        }

        private void Sniff(IPAddress ip)
        {
            mainSocket = new Socket(AddressFamily.InterNetwork,
                        SocketType.Raw, ProtocolType.IP);

            //Bind the socket to the selected IP address
            mainSocket.Bind(new IPEndPoint(ip, 0));

            //Set the socket  options
            mainSocket.SetSocketOption(SocketOptionLevel.IP,            //Applies only to IP packets
                                       SocketOptionName.HeaderIncluded, //Set the include the header
                                       true);                           //option to true

            byte[] byTrue = new byte[4] { 1, 0, 0, 0 };
            byte[] byOut = new byte[4] { 1, 0, 0, 0 }; //Capture outgoing packets

            //Socket.IOControl is analogous to the WSAIoctl method of Winsock 2
            mainSocket.IOControl(IOControlCode.ReceiveAll,              //Equivalent to SIO_RCVALL constant
                                                                        //of Winsock 2
                                 byTrue,
                                 byOut);

            //Start receiving the packets asynchronously
            mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                new AsyncCallback(OnReceive), null);

        }
        private void OnReceive(IAsyncResult ar)
        {
            try
            {
                int nReceived = mainSocket.EndReceive(ar);

                //Analyze the bytes received...

                ParseData(byteData, nReceived);

                byteData = new byte[4096];

                //Another call to BeginReceive so that we continue to receive the incoming
                //packets
                mainSocket.BeginReceive(byteData, 0, byteData.Length, SocketFlags.None,
                   new AsyncCallback(OnReceive), null);

            }
            catch (ObjectDisposedException)
            {
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Exception");
            }
        }

        private void ParseData(byte[] byteData, int nReceived)
        {
            //Since all protocol packets are encapsulated in the IP datagram
            //so we start by parsing the IP header and see what protocol data
            //is being carried by it
            //Console.WriteLine("Packet Captured");
            IPHeader ipHeader = new IPHeader(byteData, nReceived);


            //Now according to the protocol being carried by the IP datagram we parse 
            //the data field of the datagram
            Console.WriteLine(ipHeader.ProtocolType.ToString());
            switch (ipHeader.ProtocolType)
            {
                case Protocol.TCP:

                    TCPHeader tcpHeader = new TCPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                    //carried by the IP datagram
                                                        ipHeader.MessageLength);//Length of the data field                    
                                                                                
                    //Console.WriteLine("TCP");
                   DBConn.InsertTCPRecord(ipHeader, tcpHeader);


                    break;

                case Protocol.UDP:

                    UDPHeader udpHeader = new UDPHeader(ipHeader.Data,              //IPHeader.Data stores the data being 
                                                                                    //carried by the IP datagram
                                                       (int)ipHeader.MessageLength);//Length of the data field                    
                                                                                    //MakeUDPTreeNode(udpHeader);
                    //Console.WriteLine("UDP Successful");
                    DBConn.InsertUDPRecord(ipHeader, udpHeader);
                    break;

                case Protocol.ICMP:
                    ICMPHeader icmpHeader = new ICMPHeader(ipHeader.Data, (int)ipHeader.MessageLength);
                    // Get the IPHeader packet data segregated
                    //MessageBox.Show("Someone is pinging on this PC");
                    //Insert into Database
                    //Console.WriteLine("ICMP retrieval successful");
                    DBConn.InsertICMPRecord(ipHeader, icmpHeader);

                    break;

            }


            //Thread safe adding of the nodes
        }
    }
}

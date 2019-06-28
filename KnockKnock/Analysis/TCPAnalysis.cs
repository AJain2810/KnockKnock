using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace KnockKnock.Analysis
{
    class TCP_Packet_Info
    {
        public string Time_Stamp;
        public string Source_IP;
        public string Source_Port;
        public string Dest_IP;
        public string Dest_Port;
        public string Time_To_Live;
        public int SYN_Flag;
        public int ACK_Flag;
        public int FIN_Flag;
    }
    class TCPAnalysis
    {
        public static void startAnalysis()
        {
            TCP_Packet_Info obj = new TCP_Packet_Info();
            
            while (1 > 0)
            {
                MySqlDataReader tcp_packets = DBConn.getTCPPackets();
                Console.WriteLine("Packets retrieved");
                if (tcp_packets == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                //Console.WriteLine(tcp_packets.HasRows);

                if (!tcp_packets.HasRows)
                {
                    //1000 second timeout
                    tcp_packets.Close();
                    Thread.Sleep(1000);
                    Console.WriteLine("Sleep mode");

                }

                else
                {
                    while (tcp_packets.Read())
                    {
                        obj.Time_Stamp = tcp_packets[0]+"";
                        obj.Source_IP = tcp_packets[1] + "";
                        obj.Source_Port = tcp_packets[2] + "";
                        obj.Dest_IP = tcp_packets[3] + "";
                        obj.Dest_Port = tcp_packets[4]+"";
                        obj.SYN_Flag = Int32.Parse(tcp_packets[6] + "");
                        obj.ACK_Flag = Int32.Parse(tcp_packets[7] + "");
                        obj.FIN_Flag = Int32.Parse(tcp_packets[8] + "");

                        bool reverseTCPstatus = scanReverseTCP(obj);
                        Console.WriteLine(reverseTCPstatus);
                        if (reverseTCPstatus==true)
                        {
                            LogRecord lg = new LogRecord();
                            MessageBox.Show("Possible Reverse TCP being established");
                            lg.Attack_Type = "Reverse_TCP";
                            lg.Source_IP = obj.Source_IP;
                            lg.TimeStamp = obj.Time_Stamp;
                            Analysis.obj.addNode(lg);
                            //Analysis.obj.addNode(lg);
                            //DBConn.insertLog(lg);

                        }
                       // MessageBox.Show("Someone is pinging");
                        //Console.WriteLine("Possible ping request");
                        //RowStyle rs = new RowStyle();
                        //DataRow workRow = Form1.dataSet1.NewRow();

                        //Form1.tableLayoutPanel1.RowStyles.Add(new RowStyle("possible ping occuring"));
                    }
                    tcp_packets.Close();
                    DBConn.clearICMPRecords();
                }
            }
        }

        public static bool scanReverseTCP(TCP_Packet_Info packet)
        {
            //Console.WriteLine("Sccanning for Reverse TCP");
            string source_ip = packet.Source_IP;
            var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork).AsEnumerable();
            foreach (IPAddress ip in IPv4Addresses)
            {
                if (ip.ToString().Equals(source_ip))
                    source_ip = "0";
            }
            Console.WriteLine(packet.ACK_Flag + packet.Dest_IP + packet.SYN_Flag);
            if (source_ip.Equals("0") && packet.ACK_Flag == 0 && packet.SYN_Flag == 1)
                return true;
            else
                return false;
        }
    }
}

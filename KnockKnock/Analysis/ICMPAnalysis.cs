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
    class ICMPAnalysis
    {
        // get info from the database
        // search for the entire packet for the same
        // update the table and the database

        public static void startAnalysis()
        {
            while (1 > 0)
            {
                MySqlDataReader icmp_packets = DBConn.getICMPPackets();
                Console.WriteLine("Packets retrieved");
                if (icmp_packets == null)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                Console.WriteLine(icmp_packets.HasRows);
                if (!icmp_packets.HasRows)
                {
                    //1000 second timeout
                    icmp_packets.Close();
                    Thread.Sleep(1000);
                    Console.WriteLine("Sleep mode");

                }

                else
                {
                    while(icmp_packets.Read())
                    {
                        LogRecord obj = new LogRecord();
                        obj.TimeStamp = icmp_packets[0]+"";
                        obj.Source_IP = icmp_packets[1]+"";
                        obj.Attack_Type = "Ping warning";
                        DBConn.insertLog(obj);
                        MessageBox.Show("Someone is pinging");
                        Console.WriteLine("Possible ping request");
                        //RowStyle rs = new RowStyle();
                        //DataRow workRow = Form1.dataSet1.NewRow();

                        //Form1.tableLayoutPanel1.RowStyles.Add(new RowStyle("possible ping occuring"));
                    }
                    icmp_packets.Close();
                    DBConn.clearICMPRecords();
                }
            }
        }


    }
}

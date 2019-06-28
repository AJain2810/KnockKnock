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


namespace KnockKnock
{
    class DBConn
    {
        private static MySqlConnection connection;
        private static string server;
        private static string database;
        private static string uid;
        private static string password;
        public static int i;
        public static bool conn_status = false;
        //Constructor
        public DBConn()
        {
            Initialize();
        }

        //Initialize values
        public static void Initialize()
        {
            server = "localhost";
            database = "Minor_Project";
            uid = "root";
            password = "abcd1234";
            string connectionString;
            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

            connection = new MySqlConnection(connectionString);
            if (connection.State != null)
                connection.Close();
            try
            {
                connection.Open();
            }
            catch(Exception e)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception ex)

                {
                    connection.Close();
                    connection.Open();
                }
            }
        }
        //string connstring = string.Format("Server=localhost; database={0}; UID=UserName; password=your password", databaseName);
        //open connection to database

        //Close connection
        public static bool CloseConnection()
        {
            
            try
            {
                connection.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }



        //Insert statement
        public static void InsertICMPRecord(IPHeader ipHeader, ICMPHeader icmpHeader)
        {
            String table_name = "ICMP_Packet";

            string timestamp = DateTime.Now.ToString();
            string source_ip = ipHeader.SourceAddress.ToString();
            string dest_ip = ipHeader.DestinationAddress.ToString();
            
            // IPAddress 0 for host machine, 1 for external I.P.

            int type = icmpHeader.Type;
            int code = icmpHeader.Code;
            string time_to_live = ipHeader.TTL;

            string query = string.Format("Insert into {0} values( '{1}','{2}','{3}',{4},{5}, {[6});", table_name, timestamp, source_ip, dest_ip, type, code, time_to_live);
            Console.WriteLine(query);

            //open connection
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Cancel();
            //Execute command
            cmd.ExecuteNonQueryAsync();

        }


        public static void InsertTCPRecord(IPHeader ipHeader, TCPHeader tCPHeader)
        {
            String table_name = "TCP_Packet";

            string timestamp = DateTime.Now.ToString();
            string source_ip = ipHeader.SourceAddress.ToString();
            string dest_ip = ipHeader.DestinationAddress.ToString();
            
            
            string ttl = ipHeader.TTL;
            string header_length = tCPHeader.HeaderLength;
            TCPFlags obj = tCPHeader.Flags;
            int FIN = Convert.ToInt32(obj.FIN);
            int SYN = Convert.ToInt32(obj.SYN);
            int ACK = Convert.ToInt32(obj.ACK);

            /*
            var IPv4Addresses = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(al => al.AddressFamily == AddressFamily.InterNetwork).AsEnumerable();
            foreach (IPAddress ip in IPv4Addresses)
            {
                if (ip.ToString().Equals(source_ip_in_address_format))
                    source_ip = "0";
                else if (ip.ToString().Equals(dest_ip_in_address_format))
                    dest_ip = "0";

            }

            */
            // IPAddress 0 for host machine, 1 for external I.P.

            string source_port = tCPHeader.SourcePort;
            string dest_port = tCPHeader.DestinationPort;
            string time_to_live = ipHeader.TTL;
            
            //string connstring = string.Format("Server=localhost; database={0}; UID=UserName; password=your password", databaseName);
            string query = string.Format("Insert into {0} values( '{1}','{2}',{3},'{4}',{5},{6}, {7}, {8}, {9});", table_name, timestamp, source_ip, source_port, dest_ip, dest_port, time_to_live, SYN, ACK, FIN);
            //string query = string.Format("Insert into {0} values( '{1}','{2}',{3},'{4}',{5},{6}, 1 , 0 , 0 ,{7}, {8} , {9} , {10},,'',,);", table_name, timestamp, source_ip, source_port, dest_ip, dest_port, time_to_live, header_length, SYN, ACK, FIN);
            //String query = "Insert into " + table_name + " values('" + timestamp + "','" + source_ip + "'," + source_port + ",'" + dest_ip + "'," + dest_port + ");";

            Console.WriteLine(query);
            //open connection
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            cmd.ExecuteNonQueryAsync();
            //cmd.ExecuteNonQuery();


        }
        public static void InsertUDPRecord(IPHeader ipHeader, UDPHeader uDPHeader)
        {
            String table_name = "UDP_Packet";

            string timestamp = DateTime.Now.ToString();
            string source_ip = ipHeader.SourceAddress.ToString();
            string dest_ip = ipHeader.DestinationAddress.ToString();
            
            string header_length = uDPHeader.Length;
            string checksum = uDPHeader.Checksum;
            string ttl = ipHeader.TTL;
            // IPAddress 0 for host machine, 1 for external I.P.

            int source_port = uDPHeader.SourcePort;
            int dest_port = uDPHeader.DestinationPort;
            string time_to_live = ipHeader.TTL;
            //String query = "Insert into " + table_name + " values('" + timestamp + "','" + source_ip + "'," + source_port + ",'" + dest_ip + "'," + dest_port + ");";
            string query = string.Format("Insert into {0} values( '{1}','{2}',{3},'{4}',{5},{6}, {7});", table_name, timestamp, source_ip, source_port, dest_ip, dest_port, time_to_live, header_length);
            //string query = string.Format("Insert into {0} values( '{1}','{2}',{3},'{4}',{5}, {6}, 0 , 1 , 0 ,,,,, {7},'{8}',,);", table_name, timestamp, source_ip, source_port, dest_ip, dest_port, time_to_live, header_length, checksum);
            Console.WriteLine(query);

            //open connection
            //Console.WriteLine(query);
            MySqlCommand cmd = new MySqlCommand(query, connection);

            //Execute command
            cmd.ExecuteNonQueryAsync();

        }

    }
}

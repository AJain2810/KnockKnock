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

namespace KnockKnock.Analysis
{
    public class LogRecord
    {
        public string Source_IP;
        public string Attack_Type;
        public string TimeStamp;
    }
    class DBConn
    {
        private static MySqlConnection connection;
        private static MySqlConnection connection2;
        private static MySqlConnection connection3;
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
            connection2 = new MySqlConnection(connectionString);
            connection3 = new MySqlConnection(connectionString);
            //connection.Open();
            try
            {
                connection.Open();
                connection2.Open();
                connection3.Open();
            }
            catch (Exception e)
            {
                connection.Close();
                connection2.Close();
                connection3.Close();
                connection.Open();
                connection2.Open();
                connection3.Open();
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
                connection2.Close();
                connection3.Close();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
        }

        public static void insertLog(LogRecord lg)
        {
            
            //Console.WriteLine("Fuck");
            string table_name = "Log_Record";
            string Source_IP = lg.Source_IP;
            string time_stamp = lg.TimeStamp;
            string attack_type = lg.Attack_Type;
            string query = string.Format("Insert into {0} values( '{1}','{2}','{3}'});", table_name, time_stamp, Source_IP, attack_type);
            Console.WriteLine(query);

            //open connection
            MySqlCommand cmd = new MySqlCommand(query, connection3);
        }
        public static MySqlDataReader getICMPPackets()
        {
            String query = "SELECT * from  ICMP_Packet group by Source_IP";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Cancel();
            try
            {

                MySqlDataReader read = cmd.ExecuteReader();
                return read;
            }
            catch(Exception ex)
            {
                return null;
            }


            
            
            //return read;
        }

        public static MySqlDataReader getTCPPackets()
        {
            String query = "SELECT * from  TCP_Packet";
            MySqlCommand cmd = new MySqlCommand(query, connection2);
            cmd.Cancel();
            try
            {
                MySqlDataReader read = cmd.ExecuteReader();
                return read;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public static MySqlDataReader getUDPPackets()
        {
            String query = "SELECT * from  UDP_Packet";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Cancel();
            MySqlDataReader read = cmd.ExecuteReader();
            return read;
        }

        public static MySqlDataReader getGroupedPackets()
        {
            String query = "SELECT TCP_Header.Source_IP, Source_Port, Time_To_Live, Count(SYN_Flag) as SYN, Count(ACK_Flag) as ACK, Count(FIN_Flag) as FIN, ICMP_Type, ICMP_Code from  TCP_Packet, ICMP_Packet where TCP_Packet.Source_IP = ICMP_Packet.Source_IP group by TCP_Packet.Source_IP";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.Cancel();
            MySqlDataReader read = cmd.ExecuteReader();
            return read;
        }

        public static void clearICMPRecords()
        {
            string timestamp = DateTime.Now.AddMilliseconds(-1100).ToString();
            String query = String.Format("Delete from ICMP_Packet where Time_Stamp < {0}",timestamp);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQueryAsync();
        }

        public static void clearTCPRecords()
        {
            string timestamp = DateTime.Now.AddMilliseconds(-1100).ToString();
            String query = String.Format("Delete from TCP_Packet where Time_Stamp < {0}", timestamp);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQueryAsync();
        }

        public static void clearUDPRecords()
        {
            string timestamp = DateTime.Now.AddMilliseconds(-1100).ToString();
            String query = String.Format("Delete from UDP_Packet where Time_Stamp < {0}", timestamp);
            MySqlCommand cmd = new MySqlCommand(query, connection);
            cmd.ExecuteNonQueryAsync();
        }

        // Methods to extract records from the corresponding tables
        // return datareader stream
        //they'll parse it themselves
    }
}

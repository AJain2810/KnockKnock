using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
namespace KnockKnock.Analysis
{
    class Analysis
    {
        public static Res obj = new Res();
        //create threads to analyze the three table records
        public static void run()
        {
            DBConn.Initialize();
            DBConn.clearICMPRecords();
            DBConn.clearTCPRecords();
            DBConn.clearUDPRecords();
            Console.WriteLine("DB initialized");
            /*
            ThreadStart childref = new ThreadStart(ICMPAnalysis.startAnalysis);
            
            Thread childThread = new Thread(childref);
            childThread.Start();
            */
            //Res obj = new Res();
            obj.Show();
            //ThreadStart childref2 = new ThreadStart(TCPAnalysis.startAnalysis);
            TCPAnalysis.startAnalysis();
            //Thread childThread2 = new Thread(childref2);
            //childThread2.Start();
        }
    }
}

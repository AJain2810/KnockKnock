using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace KnockKnock
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ThreadStart childref = new ThreadStart(new PacketCapture().DriverCode);
            new PacketCapture().DriverCode();
            //Console.WriteLine("In Main: Creating the Child thread");

            //Thread childThread = new Thread(childref);
            //childThread.Start();
            DBConn.Initialize();
            //Analysis.DBConn.Initialize();
            Analysis.Analysis.run();
            //DBConn.OpenConnection();

            
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            DBConn.CloseConnection();
            Analysis.DBConn.CloseConnection();
            Application.Exit();
            //this.tableLayoutPanel1
        }
    }
}

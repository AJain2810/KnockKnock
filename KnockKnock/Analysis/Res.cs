using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
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
    
    public partial class Res : Form
    {
        
        public Res()
        {
            InitializeComponent();
        }
        
        
        
        public void addNode(LogRecord lg)
        {
            Console.WriteLine("Adding nodes");
            treeView1.BeginUpdate();
            TreeNode ipNode = new TreeNode();

            //TreeNode rootNode = new TreeNode();
            ipNode.Text = lg.Attack_Type;
            ipNode.Nodes.Add("Timestamp" + lg.TimeStamp);
            ipNode.Nodes.Add("IP Address:" + lg.Source_IP);
            treeView1.Nodes.Add(ipNode);
            treeView1.EndUpdate();
        }
        void Res_Load(object sender, EventArgs e)
        {
            
            treeView1.BeginUpdate();

            TreeNode ipNode = new TreeNode();
            
            //TreeNode rootNode = new TreeNode();
            ipNode.Text = "Reverse_TCP";
            ipNode.Nodes.Add("Timestamp"+ "5 / 2 / 2019 13:22:54");
            ipNode.Nodes.Add("IP Address:" + "192.168.43.189");
            treeView1.Nodes.Add(ipNode);
            treeView1.EndUpdate();
            //AddTreeNode addTreeNode = new AddTreeNode(OnAddTreeNode)
        }

        private void TreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }
    }
}

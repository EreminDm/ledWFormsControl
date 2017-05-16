using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ledman;
using System.Runtime.InteropServices;

namespace ledWFormsControl
{
    public partial class Form1 : Form
    {
        [DllImport(@"C:\Users\BBV\Documents\visual studio 2017\Projects\ledWFormsControl\ledWFormsControl\bin\x86\Debug\LED_Setup_Interface.dll")]
        public static extern int SetForegroundWindow(IntPtr point);
        public Form1()
        {
            InitializeComponent();
            
            
                
           
            

            //Initialization senders
            //if (NativeMethods.ReConnectSender())
            //{
            //    Console.WriteLine("Is connected");
            //    Console.WriteLine("NativeMethods found senders " + NativeMethods.GetConnectSenderCount());
            //    Console.WriteLine("Try to connect to receivers!");

            //}
            //else
            //{
            //    Console.WriteLine("Reconnect is false;");
            //}

        }
        private void Form1_Load(object sender, EventArgs e)
        {

            //System.Threading.Thread.Sleep(3000);
            InitSenders();
        }




        public void InitSenders()
        {
            // Initialization senders
            if (NativeMethods.ReConnectSender())
            {
                if (NativeMethods.ReConnectSender())
                {
                    ushort sendersCount = NativeMethods.GetConnectSenderCount(); // Senders Count (ushort)
                    label2.Text = sendersCount.ToString();
                }
                else
                {
                    MessageBox.Show("Reconnect is false;");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
       {
 
       }

        private void button2_Click(object sender, EventArgs e)
        {
            NativeMethods.ReSearchReceivers();
            if (NativeMethods.IsSearchingReceiver())
            {
                MessageBox.Show("research receivers is started!");
                Console.WriteLine("wait until searching is end");
                //while (NativeMethods.IsSearchingReceiver())
                //{
                //}
                
            }
            else
            {
                MessageBox.Show("Brocken");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NativeMethods.StopSearchReceiver();
            MessageBox.Show("Search method is stoped");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var FoundReceivers = NativeMethods.GetFoundReceiverCount();
            MessageBox.Show("NativeMethods found receivers, count " + FoundReceivers);
        }

        
    }
}

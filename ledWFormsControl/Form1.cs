using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Ledman;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ledWFormsControl
{
    public partial class Form1 : Form
    {
        [DllImport(@"C:\Users\BBV\Documents\visual studio 2017\Projects\ledWFormsControl\ledWFormsControl\bin\x86\Debug\LED_Setup_Interface.dll")]
        public static extern int SetForegroundWindow(IntPtr point);
        public Form1()
        {
            InitializeComponent();

            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += BW_DoWork;
            BW.RunWorkerAsync();
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(500);
            label3.Invoke((MethodInvoker)delegate
            {
                label3.Text = "Senders initialization...";
                label3.ForeColor = Color.Blue;
            });
            Thread.Sleep(300);
            button1.Invoke((MethodInvoker)delegate
            {
                button1.PerformClick();
            });
            Thread.Sleep(300);
            label6.Invoke((MethodInvoker)delegate
            {
                label6.Text = "Searching receivers...";
                label6.ForeColor = Color.Blue;
            });
            button2.Invoke((MethodInvoker)delegate
            {
                button2.PerformClick();
            });
            Thread.Sleep(30000);
            button3.Invoke((MethodInvoker)delegate
            {
                button3.PerformClick();
            });
            Thread.Sleep(300);
            button4.Invoke((MethodInvoker)delegate
            {
                button4.PerformClick();
            });
        }        

        public void InitSenders()
        {            
            // Initialization senders

            if (NativeMethods.ReConnectSender())
            {
                ushort sendersCount = NativeMethods.GetConnectSenderCount(); // Senders Count (ushort)
                string countOfSenders = sendersCount.ToString();
                label2.Invoke((MethodInvoker)delegate
                {
                    label2.Text = countOfSenders;
                        
                });
                label3.Invoke((MethodInvoker)delegate
                {
                    label3.Text = "Initialization complete";
                    label3.ForeColor = Color.Green;
                });

            }
            else
            {
                label3.Invoke((MethodInvoker)delegate
                {
                    label3.Text = "Initialization failed";
                    label3.ForeColor = Color.Red;
                });
            }
                
            
        }

        private void SearchReceivers()
        {
            NativeMethods.ReSearchReceivers();
            if (NativeMethods.IsSearchingReceiver())
            {
                //MessageBox.Show("research receivers is started!");
                //Console.WriteLine("wait until searching is end");
            }
            else
            {
                //MessageBox.Show("Brocken");
                label6.Invoke((MethodInvoker)delegate
                {
                    label6.Text = "Receivers search canceled";
                    label6.ForeColor = Color.Red;
                });
            }         
        }

        private void button1_Click(object sender, EventArgs e)
        {
            InitSenders();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SearchReceivers();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            NativeMethods.StopSearchReceiver();
            //MessageBox.Show("Search method is stoped");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var FoundReceivers = NativeMethods.GetFoundReceiverCount();
            if (FoundReceivers == 0)
            {
                    label6.Text = "Receivers not found";
                    label6.ForeColor = Color.Orange;
            }
            else
            {
                    label5.Text = FoundReceivers.ToString();

                    label6.Text = "Receivers found";
                    label6.ForeColor = Color.Green;

            }
        }

        
    }
}

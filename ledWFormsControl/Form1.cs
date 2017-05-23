﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace ledWFormsControl
{
    public partial class Form1 : Form
    {
        ushort sendersCount;
       
        int[] senderIDs = new int[2];

        // Params wich send to post method
        ushort senderID1;
        ushort senderID2;
        int FoundReceivers;
        bool DVIinput;


        struct tagApiBigWord
        {
            byte hi;
            byte lo;
            public ushort GetWord()
            {
                return (ushort)(this.hi << 8 + this.lo);
            }
        };
        public class TEST {
            public string moduleInfo { get; set; }
        }

        public Form1()
        {
            InitializeComponent();

            BackgroundWorker BW = new BackgroundWorker();
            BW.DoWork += BW_DoWork;
            BW.RunWorkerAsync();
        }

        private void BW_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string> InfoList = new List<string>();

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
            Thread.Sleep(300);
            button5.Invoke((MethodInvoker)delegate
            {
                button5.PerformClick();
            });
            Thread.Sleep(300);
            label7.Invoke((MethodInvoker)delegate
            {
                label7.Text = "Searching module info...";
                label7.ForeColor = Color.Blue;
            });
            Thread.Sleep(300);
            button6.Invoke((MethodInvoker)delegate
            {
                button6.PerformClick();
            });

        }        

        public void InitSenders()
        {            
            // Initialization senders

            if (NativeMethods.ReConnectSender())
            {
                sendersCount = NativeMethods.GetConnectSenderCount(); // Senders Count (ushort)
                for (int step = 0; step < sendersCount; step++) {
                    senderIDs[step] = NativeMethods.GetSenderId((byte)step);
                }
                senderID1 = (ushort)senderIDs[0];
                senderID2 = (ushort)senderIDs[1];
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
            FoundReceivers = NativeMethods.GetFoundReceiverCount();
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

        private void button5_Click(object sender, EventArgs e)
        {
            // START DVI IPUT STATUS
            for (ushort step1 = 0; step1 < sendersCount; step1++)
            {
                bool bResult = NativeMethods.IsSenderDviInputOk((byte)step1);
                if (bResult)
                {
                    DVIinput = true;
                    label9.Text = "DVI signal OK";
                    label9.ForeColor = Color.Green;
                }
                else
                {
                    DVIinput = false;
                    label9.Text = "No DVI signal";
                    label9.ForeColor = Color.Red;
                }
            }
            //END DVI INPUT
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //START GEt module voltage and temperature Information
            NativeMethods.tagReceiverModuleDetailInfo stModuleDetailInfo;
            int x, y;
            double voltage;
            int j;
            
            for (int step1 = 0; step1 < FoundReceivers; step1++)
            {
                string[][] iReceivers = new string[FoundReceivers][];
                
                if (NativeMethods.QueryReceiverModuleDetailInfo(out stModuleDetailInfo, (ushort)step1))
                    {
                    string[] jReceivers = new string[stModuleDetailInfo.nRealModuleAmount];

                    for (j = 0; j < stModuleDetailInfo.nRealModuleAmount; j++)
                        {
                            x = stModuleDetailInfo.stReceiverModlInfo[j].byModuleVoltage;
                            voltage = x * 32 / 1000.0;
                            y = stModuleDetailInfo.stReceiverModlInfo[j].sbyModuleTemperature;
                        string a = "ReceiverCountValue " + step1.ToString() + " Module number " + j.ToString() + " voltage: " + voltage.ToString() + " temperature: " + y.ToString();
                        iReceivers[step1][j] = a;
                       

                       // MessageBox.Show("ReceiverCount "+ step1 + " Module "+stModuleDetailInfo.stReceiverModlInfo[j].byModuleAddr + " voltage: " + voltage + " temperature: " + y);
                        }
                    label7.Text = "Module information found";
                    label7.ForeColor = Color.Green;
                    
                    
                    }
                    else
                {
                    label7.Text = "Module information not found";
                    label7.ForeColor = Color.Red;
                }
                

            }
            // END
        }
        public class PostData
        {
            public int SendersCount { get; set; }
            public ushort SenderID2 { get; set; }
            public ushort SenderID1 { get; set; }
            public int ReceiversCount { get; set; }
           // public string Modules 
        }
    }               
    

   

  

    /////start get Bad panels
    ////int reseivers = (int)NativeMethods.GetFoundReceiverCount();
    //if (FoundReceivers == 0)
    //{
    //    label7.Text = "Couldn't search bad panels";
    //    label7.ForeColor = Color.Orange;
    //}
    //else
    //{
    //    NativeMethods.tagReceiverBadPanels badPanels;

    //    for (ushort step1 = 0; step1 < FoundReceivers; step1++)
    //    {
    //        bool status;
    //        for (int step = 0; step < sendersCount; step++)
    //        {

    //            //NativeMethods.tagReceiverMonitorData monitor;
    //            //status = GetReceiverMonitorData(out monitor, (ushort)step1);
    //            //MessageBox.Show(monitor.bwPowerVolt.hi.ToString() + " "+monitor.bwPowerVolt.lo.ToString());
    //             status = NativeMethods.GetReceiverBadPanels(step1, out badPanels);;
    //            //status = NativeMethods.GetReceiverBadPanels_ById((ushort)senderIDs[step], (byte)step, step1, out badPanels);


    //            if (status == false)
    //            {
    //                MessageBox.Show("read dataFail");
    //                return;
    //                //MessageBox.Show("col " + badPanels.wReceiverCol.ToString() + " row " + badPanels.wReceiverRow.ToString() + " " + badPanels.byBadPanelCol[step1].ToString());
    //            }
    //            if (badPanels.byBadPanelCount < 1)
    //            {
    //                //all panels ok
    //            }
    //            else
    //            {
    //                MessageBox.Show("Receiver:row " + badPanels.wReceiverRow + " col " + badPanels.wReceiverCol);
    //                for (int i = 0; i < badPanels.byBadPanelCount; i++)
    //                {
    //                    // string message bad panel row and col badPanels.byBadPanelRow[i], badPanels.byBadPanelCol[i]
    //                    MessageBox.Show("Receiver:row " + badPanels.byBadPanelRow[i] + " col " + badPanels.byBadPanelCol[i]);

    //                }
    //            }

    //            MessageBox.Show("col " + badPanels.wReceiverCol.ToString() + " row " + badPanels.wReceiverRow.ToString() + " " + badPanels.byBadPanelCol[step1].ToString());

    //        }
    //    }

    //    //int reseivers = (int)NativeMethods.GetFoundReceiverCount();
    //    ////NativeMethods.tagReceiverModuleDetailInfo ModuleDetails;
    //    //NativeMethods.tagReceiverBadPanels BadPanels;
    //    //for (int step = 0; step < sendersCount; step++)
    //    //{
    //    //    ushort senderID = NativeMethods.GetSenderId((byte)step);



    //    //    for (int step1 = 0; step1 < reseivers; step1++)
    //    //    {
    //    //        // NativeMethods.GetReceiverBadPanels((ushort)step1, out BadPanels);

    //    //        NativeMethods.GetReceiverBadPanels_ById(senderID, (byte)step, (ushort)step1, out BadPanels);
    //    //    }
    //    //}
    //}
    ////end get Bad Panels
}

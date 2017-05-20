using System;
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
        int FoundReceivers;
        int[] senderIDs = new int[2];
        ushort senderID1;
        ushort senderID2;

        struct tagApiBigWord
        {
            byte hi;
            byte lo;
            public ushort GetWord()
            {
                return (ushort)(this.hi << 8 + this.lo);
            }
        };

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
            Thread.Sleep(300);
            label7.Invoke((MethodInvoker)delegate
            {
                label7.Text = "Searching bad panels...";
                label7.ForeColor = Color.Blue;
            });
            button5.Invoke((MethodInvoker)delegate
            {
                button5.PerformClick();
            });

            //int reseivers = NativeMethods.GetFoundReceiverCount();
            //NativeMethods.tagReceiverModuleDetailInfo ModuleDetails;
           

          //  for (int step = 0; step < sendersCount; step++)
           // {
                //  ushort senderID = NativeMethods.GetSenderId((byte)step);



                //for (ushort step1 = 1; step1 <= reseivers; step1++)
                //{
                //    NativeMethods.tagReceiverBadPanels badPanels;
                //    badPanels.byBadPanelCol = new byte[16];
                //    badPanels.byBadPanelCount = 0;
                //    badPanels.byBadPanelRow = new byte[16];
                //    GetReceiverBadPanels(step1, out badPanels);
                //}
            //}
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
            //            // bool status = GetReceiverBadPanels(step1, out badPanels);;
            //            status = GetReceiverBadPanels_ById((ushort)senderIDs[step], (byte)step, step1, out badPanels);


            //            if (status == false)
            //            {
            //                MessageBox.Show("read dataFail");
            //                return;
            //                //MessageBox.Show("col " + badPanels.wReceiverCol.ToString() + " row " + badPanels.wReceiverRow.ToString() + " " + badPanels.byBadPanelCol[step1].ToString());
            //            }
            //            if(badPanels.byBadPanelCount < 1)
            //            {
            //                //all panels ok
            //            }
            //            else
            //            {
            //                MessageBox.Show("Receiver:row " + badPanels.wReceiverRow + " col " + badPanels.wReceiverCol);
            //                for (int i=0; i <badPanels.byBadPanelCount; i++)
            //                {
            //                    // string message bad panel row and col badPanels.byBadPanelRow[i], badPanels.byBadPanelCol[i]
            //                    MessageBox.Show("Receiver:row " + badPanels.byBadPanelRow[i] + " col " + badPanels.byBadPanelCol[i] );

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
    }
    //START GEt module voltage and temperature Information
    //NativeMethods.tagReceiverModuleDetailInfo stModuleDetailInfo;
    //     for (int step1 = 0; step1 < FoundReceivers; step1++) { 
    //         if ( NativeMethods.QueryReceiverModuleDetailInfo(out stModuleDetailInfo, 0)) {
    //         int x, y;
    //         double voltage;
    //         int j;
    //         for( j=0; j < stModuleDetailInfo.nRealModuleAmount; j++)
    //         {
    //             x = stModuleDetailInfo.stReceiverModlInfo[j].byModuleVoltage;
    //             voltage = x * 32 / 1000.0;
    //             y = stModuleDetailInfo.stReceiverModlInfo[j].sbyModuleTemperature;
    //             MessageBox.Show(stModuleDetailInfo.stReceiverModlInfo[j].byModuleAddr +" "+voltage + " " + y);
    //         }
    //     }
    // } 
    // END

    //// START DVI IPUT STATUS
    //for (ushort step1 = 0; step1 < sendersCount; step1++)
    //{
    //    bool bResult = NativeMethods.IsSenderDviInputOk((byte)step1);
    //    if (bResult)
    //    {
    //        ///dvi input ok

    //    }
    //    else
    //    {
    //        // no dvi input
    //    }

    //}
    ////END DVI INPUT
}

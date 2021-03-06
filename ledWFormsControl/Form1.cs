﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using System.IO;

namespace ledWFormsControl
{
    public partial class Form1 : Form
    {
        Thread thr;
        
        ushort sendersCount;
       
        int[] senderIDs = new int[2];

        DateTime cdt;
        

        // Params wich send to post method
        ushort senderID1;
        ushort senderID2;
        int FoundReceivers;
        bool ReceiverSearchingStatus = true;
        string ServerIp;
        string ServerPort;
        bool boolSendToServer;
       
        public class PostData
        {
            public int SendersCount;
            public ushort SenderID2;
            public ushort SenderID1;
            public int ReceiversCount;
            public string Modules;
            public bool DVIinput;
            public string Time;
        }
        PostData PD = new PostData();

        //public Form1(string p, string p2)
        public Form1()
        {
            InitializeComponent();

            fillIP();

            fillLstShedule();
            
        }

        //start button
        private void button8_Click_1(object sender, EventArgs e)
        {
            button8.Enabled = false;
            button9.Enabled = true;

            ServerIp = textBox1.Text;
            ServerPort = textBox2.Text;
            if(checkBox3.Checked)
            {
                boolSendToServer = true;
            }
            else
            {
                boolSendToServer = false;
            }

            writeIPToFile();

            if (FoundReceivers < 1)
            {
                Connect();
            }

            thr = new Thread(CheckDisplay);
            thr.IsBackground = true;
            thr.Start();
   
        }

        //stop program
        private void button9_Click(object sender, EventArgs e)
        {
            button8.Enabled = true;
            button9.Enabled = false;
            thr.Abort();
        }

        private void fillLstShedule()
        {
            using (StreamReader r = new StreamReader("time.json"))
            {
                string json = r.ReadToEnd();

                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    ListViewItem itm = new ListViewItem((String)item.Name);

                    itm.SubItems.Add((String)item.Value );

                    lstShedule.Items.Add(itm);
                }
            }
        }

        private void fillIP()
        {
            using (StreamReader r = new StreamReader("send.json"))
            {
                string json = r.ReadToEnd();

                dynamic array = JsonConvert.DeserializeObject(json);
                foreach (var item in array)
                {
                    if(item.Name == "send")
                    {
                        if(item.Value == "True")
                        {
                            checkBox3.Checked = true;
                            boolSendToServer = true;
                        }

                        if (item.Value == "False")
                        {
                            checkBox3.Checked = false;
                            boolSendToServer = false;
                        }
                    }
                    else
                    {
                        ServerIp = item.Name;
                        textBox1.Text = item.Name;

                        ServerPort = item.Value;
                        textBox2.Text = item.Value;
                    }
                }
            }
        }

        

        private void Connect()
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

        }

        private void Connect2()
        {
            


            Thread.Sleep(5000);
            label6.Invoke((MethodInvoker)delegate
            {
                label6.Text = "Searching receivers...";
                label6.ForeColor = Color.Blue;
            });

            label5.Invoke((MethodInvoker)delegate
            {
                label5.Text = "0";
                label5.ForeColor = Color.Blue;
            });

            button2.Invoke((MethodInvoker)delegate
            {
                button2.PerformClick();
            });

            cdt = DateTime.Now; 


            while (ReceiverSearchingStatus == true)
            {
                
                Thread.Sleep(10000);
                button3.Invoke((MethodInvoker)delegate
                {
                    button3.PerformClick();
                });

                if ((DateTime.Now - cdt).Seconds > 90)
                {
                    ReceiverSearchingStatus = false;
                    NativeMethods.StopSearchReceiver();
                }
            }
            Thread.Sleep(5000);
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

        }



        private void CheckDisplay()
        {
            while (true)
            {
                FoundReceivers = NativeMethods.GetFoundReceiverCount();
                
                Connect2();

                Thread.Sleep(300);
                if (FoundReceivers > 0)
                {
                    setBrightness();
                }
                 
                Thread.Sleep(300);
                label7.Invoke((MethodInvoker)delegate
                {
                    label7.Text = "Searching module info...";
                    label7.ForeColor = Color.Blue;
                });
                Thread.Sleep(500);
                button6.Invoke((MethodInvoker)delegate
                {
                    button6.PerformClick();
                });
            }
        }

        private void setBrightness()
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;

                DateTime localDate = DateTime.Now;
                TimeSpan ct = localDate.TimeOfDay;
                var brightnessValue = 0;


                using (StreamReader r = new StreamReader("time.json"))
                {
                    string json = r.ReadToEnd();

                    dynamic array = JsonConvert.DeserializeObject(json);

                    foreach (var item in array)
                    {
                        TimeSpan t = TimeSpan.Parse(item.Name);
                        if (ct > t)
                        {
                            brightnessValue = item.Value;

                        }
                        else
                        {
                            break;
                        }
                    }
                    try
                    {
                        NativeMethods.SetDisplayBrightness((byte)brightnessValue);
                    }
                    catch (Exception e)
                    {

                    }
                    
                }
            }
        }

        

        public void InitSenders()
        {
            // Initialization senders

            if (NativeMethods.ReConnectSender())
            {
                sendersCount = NativeMethods.GetConnectSenderCount();
                if (sendersCount<1)
                {
                    label3.Invoke((MethodInvoker)delegate
                    {
                        label3.Text = "Initialization failed";
                        label3.ForeColor = Color.Red;
                    });

                    
                }
                else
                {
                    PD.SendersCount = sendersCount;
                    // Senders Count (ushort)
                    for (int step = 0; step < sendersCount; step++) {
                        senderIDs[step] = NativeMethods.GetSenderId((byte)step);
                    }
                    senderID1 = (ushort)senderIDs[0];
                    senderID2 = (ushort)senderIDs[1];
                    PD.SenderID1 = senderID1;
                    PD.SenderID2 = senderID2;
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
            
        }

        private void  isReceiversearching()
        {
            
            if (NativeMethods.IsSearchingReceiver() == true)
            {
                
                button3.Invoke((MethodInvoker)delegate
                {
                    button3.PerformClick();
                });
            }
            else
            {
               
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
            bool a = NativeMethods.IsSearchingReceiver();
          
                if (a == true)
                {
                     ReceiverSearchingStatus = true;

                }
                else
                {
                    ReceiverSearchingStatus = false;

                }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FoundReceivers = NativeMethods.GetFoundReceiverCount();
            PD.ReceiversCount = FoundReceivers;
            if (FoundReceivers < 1)
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
                    PD.DVIinput = true;
                    label9.Text = "DVI signal OK";
                    label9.ForeColor = Color.Green;
                }
                else
                {


                    PD.DVIinput = false;
                    label9.Text = "No DVI signal";
                    label9.ForeColor = Color.Red;
                }
            }
            //END DVI INPUT
        }

        private void button6_Click(object sender, EventArgs e)
        {
            string[] iReceivers = new string[0];

            if (FoundReceivers < 1)
            {
                label7.Text = "Module information not found";
                label7.ForeColor = Color.Red;

                
            }
            else
            {
                iReceivers = new string[FoundReceivers];
                //START GEt module voltage and temperature Information
                NativeMethods.tagReceiverModuleDetailInfo stModuleDetailInfo;
                int x, y;
                double voltage;
                int j;

                for (int step1 = 0; step1 < FoundReceivers; step1++)
                {

                    if (NativeMethods.QueryReceiverModuleDetailInfo(out stModuleDetailInfo, (ushort)step1))
                    {
                        ushort ModuleAmount = stModuleDetailInfo.nRealModuleAmount;
                        string[] jReceivers = new string[ModuleAmount];
                        if (ModuleAmount < 1) {
                            label7.Text = "Module amount information not found";
                            label7.ForeColor = Color.Red;
                        }
                        else {
                            for (j = 0; j < ModuleAmount; j++)
                            {
                                x = stModuleDetailInfo.stReceiverModlInfo[j].byModuleVoltage;
                                voltage = x * 32 / 1000.0;
                                y = stModuleDetailInfo.stReceiverModlInfo[j].sbyModuleTemperature;
                                string a = "ReceiverCountValue: " + step1.ToString() + " Module number: " + j.ToString() + " voltage: " + voltage.ToString() + " temperature: " + y.ToString();
                                jReceivers[j] = a;


                                // MessageBox.Show("ReceiverCount "+ step1 + " Module "+stModuleDetailInfo.stReceiverModlInfo[j].byModuleAddr + " voltage: " + voltage + " temperature: " + y);
                            }
                            label7.Text = "Module information found";
                            label7.ForeColor = Color.Green;
                            string json = JsonConvert.SerializeObject(jReceivers);
                            iReceivers[step1] = json;
                        }
                    }
                    else
                    {
                        label7.Text = "Module information not found";
                        label7.ForeColor = Color.Red;
                    }

                }
            }
           PD.Modules = JsonConvert.SerializeObject(iReceivers);
           PD.Time = DateTime.Now.ToString("HH:mm:ss");

            Server server = new Server();
           server.SendRequest(PD, boolSendToServer, ServerIp, ServerPort);


        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Manual brightness set
            var brightnessValue = Int32.Parse(maskedTextBox13.Text);
            if (brightnessValue>100)
            {
                brightnessValue = 100;
            }
            NativeMethods.SetDisplayBrightness((byte)brightnessValue);            
           
        }
        private void button8_Click(object sender, EventArgs e)
        {
            // auto brightness set
        }



        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                checkBox1.Checked = false;
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                checkBox2.Checked = false;
            }
        }


        private void btnEditShedule_Click(object sender, EventArgs e)
        {
            ListView.SelectedListViewItemCollection lv = lstShedule.SelectedItems;
            if (lv.Count > 0){ 
                txtTime.Text = lv[0].Text;
                txtBrightness.Text = lv[0].SubItems[1].Text;
                lstShedule.Items.Remove(lv[0]);
                lstShedule.Refresh();
                writeToFile();
            }
        }

        private void writeToFile()
        {
            Dictionary<String, int> d = new Dictionary<String, int>();
            foreach (ListViewItem item in lstShedule.Items)
            {
                d[item.Text] = Int32.Parse(item.SubItems[1].Text);
            }
            String s = JsonConvert.SerializeObject(d);

            using (StreamWriter w = new StreamWriter("time.json"))
            {
                w.Write(s);
            }

        }

        private void writeIPToFile()
        {
            Dictionary<String, String> d = new Dictionary<String, String>();
            
            d[textBox1.Text] = textBox2.Text;
            d["send"] = checkBox3.Checked.ToString();

            String s = JsonConvert.SerializeObject(d);

            using (StreamWriter w = new StreamWriter("send.json"))
            {
                w.Write(s);
            }

            

        }


        private void btnApplyEditShedule_Click(object sender, EventArgs e)
        {
            if (!txtTime.Text.Equals("") & !txtBrightness.Text.Equals("")) { 
                ListViewItem itm = new ListViewItem((String)txtTime.Text);

                itm.SubItems.Add((String)txtBrightness.Text);

                lstShedule.Items.Add(itm);
                writeToFile();

                txtTime.Text = "";
                txtBrightness.Text = "";
            }
        }

        private void btnSetDefaultBrightness_Click(object sender, EventArgs e)
        {
            Connect();
            Connect2();

            NativeMethods.SetSenderBrightness((byte)0, (byte)Byte.Parse(maskedTextBox13.Text));
            NativeMethods.SaveSetsToSender((byte)0);
            NativeMethods.SetSenderBrightness((byte)1, (byte)Byte.Parse(maskedTextBox13.Text));
            NativeMethods.SaveSetsToSender((byte)1);
        }


        // END get module info
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

/*
    private void Connect()
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


        Thread.Sleep(5000);
        label6.Invoke((MethodInvoker)delegate
        {
            label6.Text = "Searching receivers...";
            label6.ForeColor = Color.Blue;
        });
        button2.Invoke((MethodInvoker)delegate
        {
            button2.PerformClick();
        });

        while (ReceiverSearchingStatus == true)
        {
            Thread.Sleep(10000);
            button3.Invoke((MethodInvoker)delegate
            {
                button3.PerformClick();
            });
        }
        Thread.Sleep(5000);
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
            Thread.Sleep(5000);
            label6.Invoke((MethodInvoker)delegate
            {
                label6.Text = "Searching receivers...";
                label6.ForeColor = Color.Blue;
            });
            button2.Invoke((MethodInvoker)delegate
            {
                button2.PerformClick();
            });

            while (ReceiverSearchingStatus == true)
            {
                
                Thread.Sleep(10000);
                button3.Invoke((MethodInvoker)delegate
                {
                    button3.PerformClick();
                });
            }
            Thread.Sleep(5000);
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
            Thread.Sleep(500);
            button6.Invoke((MethodInvoker)delegate
            {
                button6.PerformClick();
            });

        }        

    */

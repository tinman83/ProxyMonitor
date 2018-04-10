using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Threading;

namespace ProxyMonitor
{
    public partial class Form1 : Form
    {
        const int PORT_NO = 5000;
        const string SERVER_IP = "127.0.0.1";
        private Boolean process= false;
        public Form1()
        {
            InitializeComponent();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtIPAddress.Text = "127.0.0.1";
            txtPortNumber.Text = "5000";
        }

        private void btnStart_Click(object sender, EventArgs e)
        {

            process = true;
            btnStop.Enabled = true;
            btnStart.Enabled = false;

            Thread oThread = new Thread(new ThreadStart(portReaderHandler));
            oThread.Start();

        }

        private void btnStop_Click(object sender, EventArgs e)
        {

            process=false;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
        }

        public void portReaderHandler()
        {

            while (true)
            {
                if (process==false) { break; }
                try
                {
                    string server_ip = txtIPAddress.Text;
                    int portNo = int.Parse(txtPortNumber.Text);
                    //---listen at the specified IP and port no.---
                    IPAddress localAdd = IPAddress.Parse(server_ip);
                    TcpListener listener = new TcpListener(localAdd, portNo);
                    //Console.WriteLine("Listening...");
                    listener.Start();

                    //---incoming client connected---
                    TcpClient client = listener.AcceptTcpClient();

                    //---get the incoming data through a network stream---
                    NetworkStream nwStream = client.GetStream();
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    //---read incoming stream---
                    int bytesRead = nwStream.Read(buffer, 0, client.ReceiveBufferSize);

                    //---convert the data received into a string---
                    string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine(dataReceived);

                    this.Invoke((Action)delegate () { txtData.Text = txtData.Text + System.Environment.NewLine + dataReceived; });

                    client.Close();
                    listener.Stop();

                }
                catch (SocketException se)
                {

                }
                finally
                {

                }
            }
        }


    }
}

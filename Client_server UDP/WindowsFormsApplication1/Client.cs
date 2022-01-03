using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Client : Form
    {
        // Define the sockets and ip 

        Socket clientsocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);     
        static IPAddress SAdress = IPAddress.Parse("127.0.0.1");
        static IPAddress clientip = IPAddress.Parse("127.0.0.2");
        IPEndPoint endpoint1 = new IPEndPoint(clientip, 55);
        EndPoint endpoint = new IPEndPoint(SAdress, 6767);
        Thread thread;


        public Client()
        {
            InitializeComponent();
            clientsocket.Bind(endpoint1); 
            thread = new Thread(new ThreadStart(listening));
            thread.Start();

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {

                MessageBox.Show("Please Enter your request");

            }
            else
            {
                byte[] buffer = Encoding.ASCII.GetBytes(textBox1.Text.Trim());
                clientsocket.SendTo(buffer, endpoint);
                textBox1.Text = "";
            }
      }
        void listening()
        {
            while (true)
            {
                byte[] buffer = new byte[4096];
                int bytescount = clientsocket.ReceiveFrom(buffer, ref endpoint);
                string msg = Encoding.ASCII.GetString(buffer, 0, bytescount);
                textBox2.Invoke(new Action(
                    delegate
                    {
                        textBox2.AppendText( "Server : " + msg + "\r\n"
                           );
                     }
                    )
                   );


            }
        }
    }
}

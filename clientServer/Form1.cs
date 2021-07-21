using AsenkronClient.XSockets;
using AsenkronServer.AsenkronSockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clientServer
{
    public partial class Form1 : Form
    {

        private TextBox textBox = new TextBox();
        private Button button = new Button();
        private OurSockets os = new OurSockets(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 1453));
        public Form1()
        {
           

            InitializeComponent();
            this.Text = "CLIENT";
            
            button.DialogResult = DialogResult.OK;

            button.Text = "SEND";
            button.Location = new Point(1000, 50);
            button.Size = new System.Drawing.Size(50, 30);


            textBox.Text = "give me command";
            textBox.Location = new Point(50, 50);
            textBox.Size = new System.Drawing.Size(900, 200);


           

            os.StartConnect();
           

            button.Click += new EventHandler(button_Click);

            



            Controls.Add(textBox);
            Controls.Add(button);
        }
        protected void button_Click(object sender, EventArgs e)
        {
             string payload = this.textBox.Text.ToString();
             this.os.SendChat(payload);
        }

    }
}

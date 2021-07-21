using AsenkronServer.AsenkronSockets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace serverClient
{
    public partial class Form1 : Form
    {
        TextBox textBox = new TextBox();
        private Listener listener = new Listener(1453);
        public Form1()
        {
            InitializeComponent();
           
            listener.StartListen();
             Console.ReadLine();

            this.Text = "SERVER";
            Button button = new Button();
            button.DialogResult = DialogResult.OK;

            button.Text = "SEND";
            button.Location = new Point(1000, 50);
            button.Size = new  System.Drawing.Size(50, 30);


           
            textBox.Text = "give me command";
            textBox.Location = new Point(50, 50);
            textBox.Size = new System.Drawing.Size(900, 200);
          
           
             textBox.TextChanged += new EventHandler(textChanged);




           
            Controls.Add(textBox);
            Controls.Add(button);
           
        }

        protected void textChanged(object sender, EventArgs e)
        {
            // this.listener.StartListen()
          
          
            this.textBox.Text = this.listener.getMessage();
            this.textBox.Visible = true;

        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace ComunicazioneSeriale
{ 
    public partial class Form1 : Form
    {

        string[] ports;

        string port_IN;
        int port_IN_index = -1;

        string port_OUT;
        int port_OUT_index = -1;

        bool aperta_comunicazione = false;

        Comunicazione comunicazione = new Comunicazione();

        string str = "";
        string now = "";

        //StreamWriter sw;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            DateTime thisDay = DateTime.Today;


             now = DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss") + ".txt";

            //MessageBox.Show(now);

            string[] lines = { "First line", "Second line", "Third line" };

            // Set a variable to the Documents path.
            string docPath =
              Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //outputFile = new StreamWriter(Path.Combine(docPath, now));
            //sw = File.AppendText(Path.Combine(docPath, now));

            // Write the string array to a new file named "WriteLines.txt".
            //using ( outputFile = new StreamWriter(Path.Combine(docPath, now)))
            //{
            //    foreach (string line in lines)
            //        outputFile.WriteLine(line);
            //}




            ports = SerialPort.GetPortNames();


            foreach (string port in ports)
            {
               
                listBox1.Items.Add(port);
            }



            foreach (string port in ports)
            {
              
                listBox2.Items.Add(port);
            }

            

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            port_IN_index = listBox1.SelectedIndex ;

            button_communication(port_IN_index, port_OUT_index);

            if ((port_OUT_index == port_IN_index))
            {
                MessageBox.Show("Questa porta l'hai gia scelta per l'uscita");
                listBox1.SetSelected(port_IN_index, false);
                return;
            }
            else
            {
                if (port_IN_index >= 0)
                {
                    port_IN = ports[port_IN_index];
                }
                    
            }
            
        }

  

        private void listBox2_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            port_OUT_index = listBox2.SelectedIndex ;

            button_communication(port_IN_index, port_OUT_index);

            if ((port_OUT_index == port_IN_index))
            {
                MessageBox.Show("Questa porta l'hai gia scelta per l'ingresso");
                listBox2.SetSelected(port_OUT_index, false);
                return;
            }
            else
            {
                if(port_OUT_index>=0)
                {
                    port_OUT = ports[port_OUT_index];
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (aperta_comunicazione)
            {
                //chiude la comunicazione
                aperta_comunicazione = false;
                serialPort1.Close();
                serialPort2.Close();
                button1.Text = "APRI COMUNICAZIONE";
                textBox1.BackColor = Color.Yellow;

            }
            else
            {
                aperta_comunicazione = true;
                //apre la comunicazione
                serialPort1.PortName = port_IN;
                serialPort2.PortName = port_OUT;
                //SerialPort serialPort1 = new SerialPort(port_IN);
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(MyDataReceivedHandler);
                serialPort1.Open();
                serialPort2.Open();
                button1.Text = "CHIUDI COMUNICAZIONE";
                textBox1.BackColor = Color.GreenYellow;
            }
        }

        void MyDataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            int count = serialPort1.BytesToRead;
            char[] ByteArray = new char[count];
            serialPort1.Read(ByteArray, 0, count);

            foreach (char ch in ByteArray)
            {
                str += ch;
            }

            if (str.Contains("\r\n"))
            {
                using (StreamWriter sw = File.AppendText(now))
                {
            
                    sw.WriteLine(DateTime.Now.ToString() + " → " + str);
                }
                AddListBoxItem(str);
                serialPort2.Write(str);

                serialPort1.Write("6\r\n");

                str = "";
              
            }
            
        }


        private delegate void AddListBoxItemDelegate(object item);

        private void AddListBoxItem(object item)
        {
            if (this.listBox3.InvokeRequired)
            {
                // This is a worker thread so delegate the task.
                this.listBox3.Invoke(new AddListBoxItemDelegate(this.AddListBoxItem), item);
            }
            else
            {
                // This is the UI thread so perform the task.
                //this.listBox3.Items.Add(item);
                this.listBox3.Items.Insert(0, item);
                //outputFile.WriteLine("fenguk");

            }
        }


        private void button_communication(int port_1, int port_2)
        {
            button1.Enabled =  comunicazione.is_selected_port(port_1, port_2);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox3.Items.Clear();
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

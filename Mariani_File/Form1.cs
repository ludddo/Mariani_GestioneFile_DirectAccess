using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Mariani_File
{
    public partial class Form1 : Form
    {
        public struct Prodotto
        {
            public string nome;
            public float prezzo;
            //public int qnt;
        }
        public string fileName = @"testo.csv";
        public Prodotto prodotto;
        public int dim;
        public int recordLenght = 24;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public static void scritturaFileAppend(string content)
        {
            var oStream = new FileStream(@"testo.csv", FileMode.Append, FileAccess.Write, FileShare.Read);
            StreamWriter sw = new StreamWriter(oStream);
            sw.WriteLine(content);
            sw.Close();
        }

        public static string ToString(Prodotto prodotto, string sep = ";")
        {

            return (prodotto.nome + sep + prodotto.prezzo + sep).PadRight(20) + "##";

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool asd = String.IsNullOrEmpty(textBox1.Text);
            bool asd1 = String.IsNullOrEmpty(textBox2.Text);
            if (asd != true && asd1 != true)
            {
                prodotto.nome = textBox1.Text;
                prodotto.prezzo = float.Parse(textBox2.Text);
                scritturaFileAppend(ToString(prodotto));
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            AperturaFile();
            listView1.Show();
        }

        private void AperturaFile()
        {
            String line;
            byte[] br;
            var f = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(f);
            f.Seek(0, SeekOrigin.Begin);
            while (f.Position < f.Length)
            {
                br = reader.ReadBytes(recordLenght);
                //converte in stringa
                line = Encoding.ASCII.GetString(br, 0, br.Length);
                listView1.Items.Add(line);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView1.Clear();

            using (StreamWriter writer = new StreamWriter(fileName, append: false))
            {
                writer.Close();
            }
        }

        private void Cancellazione(string oggetto)
        {
            string s = "";
            using (StreamReader reader = File.OpenText(fileName))
            {
                using (StreamWriter writer = new StreamWriter(@"appoggio.csv", append: true))
                {
                    while ((s = reader.ReadLine()) != null)
                    {
                        string[] splittaggio1 = s.Split(';');

                        if (splittaggio1[0] == oggetto)
                        {
                        splittaggio1[2] = "true";
                        }
                    
                        if (splittaggio1[2] == "false")
                        {
                            writer.WriteLine(s);
                        }
                        else
                        {
                            writer.WriteLine(splittaggio1[0] + ";" + splittaggio1[1] + ";" + splittaggio1[2]);
                        }
                        
                    }
                }
                reader.Close();
            }
            File.Delete(@"testo.csv");
            File.Move(@"appoggio.csv", @"testo.csv");
            listView1.Clear();
            AperturaFile();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string oggetto = textBox1.Text;
            Cancellazione(oggetto);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string parola = textBox4.Text;
            string modificato = textBox5.Text;

            Modifica(parola.ToString(), modificato.ToString());
        }

        private void Modifica(string parola, string oggetto)
        {
            string s = "";
            using (StreamReader reader = File.OpenText(fileName))
            {
                using (StreamWriter writer = new StreamWriter(@"appoggio.csv", append: true))
                {
                    while ((s = reader.ReadLine()) != null)
                    {
                        string[] splittaggio1 = s.Split(';');
                        string[] splittaggio2 = splittaggio1[1].Split(';');
                    
                        if (parola == splittaggio1[0])
                        {
                            writer.WriteLine(oggetto + ";" + splittaggio1[1] + ";" + splittaggio1[2]);
                        }
                        else
                        {
                            writer.WriteLine(s);
                        }
                        
                    }
                }
                reader.Close();
            }
            File.Delete(@"testo.csv");
            File.Move(@"appoggio.csv", @"testo.csv");
            listView1.Clear();
            AperturaFile();
            groupBox1.Hide();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox1.Show();
        }
    }
}

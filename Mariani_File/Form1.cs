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

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void ScritturaFile(Prodotto prodotto)
        {
            using (StreamWriter writer = new StreamWriter(fileName, append: true))
            {
                writer.WriteLine(prodotto.nome + ";" + prodotto.prezzo + ";false") ;
                writer.Close();
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool asd = String.IsNullOrEmpty(textBox1.Text);
            bool asd1 = String.IsNullOrEmpty(textBox2.Text);
            if (asd != true && asd1 != true)
            {
                prodotto.nome = textBox1.Text;
                prodotto.prezzo = float.Parse(textBox2.Text);
                ScritturaFile(prodotto);
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            AperturaFile();
        }

        private void AperturaFile()
        {
            using (StreamReader sr = File.OpenText(fileName))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    string[] splittaggio = s.Split(';');
                    if (splittaggio[2] == "false")
                    {
                        listView1.Items.Add("Nome: " + splittaggio[0] + " Prezzo: " + splittaggio[1]);
                    }
                }
                sr.Close();
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

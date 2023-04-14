using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
        public int recordLenght = 60;
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

        private void button1_Click(object sender, EventArgs e)
        {
            bool buonFine = true;
            bool asd = String.IsNullOrEmpty(textBox1.Text);
            bool asd1 = String.IsNullOrEmpty(textBox2.Text);
            if (asd != true && asd1 != true)
            {
                if (textBox1.Text.Length < 20)
                {
                    prodotto.nome = textBox1.Text;
                }
                else
                {
                    MessageBox.Show("Il nome del prodotto é troppo lungo");
                }
                if (textBox2.Text.Length < 20)
                {

                    buonFine = float.TryParse(textBox2.Text, out float prezzo);
                    prodotto.prezzo = prezzo;
                }
                else
                {
                    MessageBox.Show("Il nome del prodotto é troppo lungo");
                }
                
                if (buonFine != true)
                {
                    MessageBox.Show("Formato non Valido!!!");
                }
                
                scritturaFileAppend(ToString(prodotto));
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
            else
            {
                MessageBox.Show("Inserisci qualcosa nei campi Nome/Prezzo");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listView1.Clear();
            AperturaFile();
        }
        public static string ToString(Prodotto prodotto, string sep = ";")
        {
            return (prodotto.nome + sep + prodotto.prezzo + sep + "false").PadRight(58);

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
                //listView1.Items.Add(line);
                //controllo logico
                string[] split = line.Split(';');
                string[] split2 = split[2].Split(' ');
                if (split2[0] == "false")
                {
                    listView1.Items.Add(line);
                }
            }
            
            reader.Close();
            f.Close();
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
            byte[] br;
            String line;
            
            int numLinea = Ricerca(oggetto);
            var f = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(f);
            if (numLinea != -1)
            {
                
                f.Seek(0, SeekOrigin.Begin);

                f.Seek((recordLenght * numLinea), SeekOrigin.Current);
                br = reader.ReadBytes(recordLenght);
                line = Encoding.ASCII.GetString(br, 0, br.Length);
                listView1.Items.Add(br.Length.ToString());
                listView1.Items.Add((line));
                string[] split = line.Split(';');
                string[] split1 = split[1].Split(' ');
                f.Seek(-recordLenght, SeekOrigin.Current);
                line = (split[0] + ";" + split[1] + ";" + "true").PadRight(58);
                br = Encoding.ASCII.GetBytes(line);
                reader.BaseStream.Write(br, 0, br.Length);
                reader.Close();
                f.Close();
            }
            else
            {
                reader.Close();
                f.Close();
                MessageBox.Show("il prodotto che si vuole rimuovere non esiste");
            }
            
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
            if (parola.Length < 20 && modificato.Length < 20)
            {
                Modifica(parola.ToString(), modificato.ToString());
            }
            else
            {
                MessageBox.Show("Il nome del prodotto é troppo lungo");
            }       
        }

        private void Modifica(string parola, string oggetto)
        {
            byte[] br;
            String line;
            int numLinea = Ricerca(parola);
            var f = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
            BinaryReader reader = new BinaryReader(f);

            if (numLinea != -1)
            {
                
                f.Seek(0, SeekOrigin.Begin);

                f.Seek((recordLenght * numLinea), SeekOrigin.Current);
                br = reader.ReadBytes(recordLenght);
                line = Encoding.ASCII.GetString(br, 0, br.Length);
                listView1.Items.Add(br.Length.ToString());
                listView1.Items.Add((line));
                string[] split = line.Split(';');
                string[] split1 = split[2].Split(' ');
                split[0] = oggetto;
                f.Seek(-recordLenght, SeekOrigin.Current);
                line = (oggetto + ";" + split[1] + ";" + split1[0]).PadRight(58);
                br = Encoding.ASCII.GetBytes(line);
                reader.BaseStream.Write(br, 0, br.Length);
                reader.Close();
                f.Close();
                listView1.Clear();
                AperturaFile();
            }
            else
            {
                reader.Close();
                f.Close();
                MessageBox.Show("il prodotto che si vuole modificare non esiste");
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {
            groupBox1.Show();
        }

        private int RicercaEliminato(string parola)
        {
            string s = "";
            int i = 0;

            using (StreamReader reader = File.OpenText(fileName))
            {
                while ((s = reader.ReadLine()) != null)
                {
                    
                    string[] split = s.Split(';');
                    string[] split1 = split[2].Split(' ');
                    if (split1[0] == "true")
                    {
                        reader.Close();
                        return i;
                    }
                    if (parola == split[0])
                    {
                        reader.Close();
                        return i;
                    }
                    i++;
                }
                reader.Close();
                return -1;
            }
        }

        private int Ricerca(string parola)
        {
            string s = "";
            int i = 0;

            using (StreamReader reader = File.OpenText(fileName))
            {
                while ((s = reader.ReadLine()) != null)
                {

                    string[] split = s.Split(';');
                    
                    if (parola == split[0])
                    {
                        reader.Close();
                        return i;
                    }
                    i++;
                }
                reader.Close();
                return -1;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Recupero();
        }

        private void Recupero()
        {
            byte[] br;
            String line;
            int numLinea = RicercaEliminato("true");
            if (numLinea != -1)
            {
                listView1.Items.Add(numLinea.ToString());
                var f = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite);
                BinaryReader reader = new BinaryReader(f);
                f.Seek(0, SeekOrigin.Begin);

                f.Seek((recordLenght * numLinea), SeekOrigin.Current);
                br = reader.ReadBytes(recordLenght);
                line = Encoding.ASCII.GetString(br, 0, br.Length);
                listView1.Items.Add(br.Length.ToString());
                listView1.Items.Add((line));
                string[] split = line.Split(';');
                string[] split1 = split[1].Split(' ');
                f.Seek(-recordLenght, SeekOrigin.Current);
                line = (split[0] + ";" + split[1] + ";" + "false").PadRight(58);
                br = Encoding.ASCII.GetBytes(line);
                reader.BaseStream.Write(br, 0, br.Length);
                reader.Close();
                f.Close();
                listView1.Clear();
                AperturaFile();
            }
            else
            {
                MessageBox.Show("Non c'é nulla da Recuperare");
            }
            
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Ricompatta();
        }

        private void Ricompatta()
        {
            string s = "";

            using (StreamReader reader = File.OpenText(fileName))
            {
                using (StreamWriter writer = new StreamWriter(@"appoggio.csv", append: true))
                {
                    while ((s = reader.ReadLine()) != null)
                    {
                        string[] split = s.Split(';');
                        string[] split1 = split[2].Split(' ');

                        if (split1[0] != "true")
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
    }
}
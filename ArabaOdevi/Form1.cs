using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace ArabaOdevi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            ShowList();
        }
        List<Araba> arabalar = new List<Araba>()
        {
        new Araba()
        {
            Plaka = "14AB25",
            Marka = "fiat",
            Model = "egea",
            Yakıt = "1.5",
            Renk = "beyaz",
            Vites = "6 vites" ,
            KasaTipi = "beyaz kasa",
            Açıklama = "sahibinden az kullanılmış",
            Date = new DateTime(21)
},
        new Araba()
        {
            Plaka = "24AC28",
            Marka = "mercedes",
            Model = "a80",
            Yakıt = "1.8",
            Renk = "siyah",
            Vites = "7 vites" ,
            KasaTipi = "siyah kasa",
            Açıklama = "sahibinden az kullanılmış",
            Date = new DateTime(24)
                }
    };

        public void ShowList()
        {
            listView1.Items.Clear();
            foreach (Araba araba in arabalar)
            {
                AddList(araba);
            }
        }

        public void AddList(Araba araba)
        {
            ListViewItem item = new ListViewItem(new string[]
                {
                araba.Plaka,
                araba.Marka,
                araba.Model,
                araba.Yakıt,
                araba.Renk,
                araba.Vites,
                araba.KasaTipi,
                araba.Açıklama,
                araba.Date.ToShortDateString(),

                });
            item.Tag = araba;
            listView1.Items.Add(item);

        }

        void EditAraba(ListViewItem pItem, Araba araba)
        {
            pItem.SubItems[0].Text = araba.Plaka;
            pItem.SubItems[1].Text = araba.Marka;
            pItem.SubItems[2].Text = araba.Model;
            pItem.SubItems[3].Text = araba.Yakıt;
            pItem.SubItems[4].Text = araba.Renk;
            pItem.SubItems[5].Text = araba.Vites;
            pItem.SubItems[6].Text = araba.KasaTipi;
            pItem.SubItems[7].Text = araba.Açıklama;
            pItem.SubItems[8].Text = araba.Date.ToShortDateString();

            pItem.Tag = araba;
        }
        private void AddComand(object sender, EventArgs e)
        {
            FmAraba frm = new FmAraba()
            {
                Text = "kişi ekle",
                StartPosition = FormStartPosition.CenterParent,
                araba = new Araba()
            };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                arabalar.Add(frm.araba);
                AddList(frm.araba);
            }


        }

        void EditCommand(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
                return;
            ListViewItem pItem = listView1.SelectedItems[0];
            Araba secili = pItem.Tag as Araba;


            FmAraba frm = new FmAraba()
            {

                Text = "kişi düzenle",
                StartPosition = FormStartPosition.CenterParent,
                araba = Clone(secili),

            };

            if (frm.ShowDialog() == DialogResult.OK)
            {
                secili = frm.araba;
                EditAraba(pItem, secili);

            }
        }
        Araba Clone(Araba araba)

        {
            return new Araba()
            {
                id = araba.ID,
                Plaka = araba.Plaka,
                Marka = araba.Marka,
                Model = araba.Model,
                Yakıt = araba.Yakıt,
                Renk = araba.Renk,
                Vites = araba.Vites,
                KasaTipi = araba.KasaTipi,
                Açıklama = araba.Açıklama,
                Date = araba.Date,


            };


        }
        [Serializable]
        public class Araba
        {
            
            public string id;
            [Browsable(false)]
            public string ID
            {
                get
                {
                    if (id == null)
                        id = Guid.NewGuid().ToString();
                    return id;
                }
                set { id = value; }
            }
            [Category("Araba bilgileri"),DisplayName("Adı")]
            public string Plaka { get; set; }
            [Category("Araba bilgileri"), DisplayName("plaka")]
            public string Marka { get; set; }
            [Category("Araba bilgileri"), DisplayName("marka")]
            public string Model { get; set; }
            [Category("Araba bilgileri"), DisplayName("model")]
            public string Yakıt { get; set; }
            [Category("Araba bilgileri"), DisplayName("yakıt")]
            public string Renk { get; set; }
            [Category("Araba görünüş"), DisplayName("renk")]
            public string Vites { get; set; }
            [Category("Araba bilgileri"), DisplayName("vites")]
            public string KasaTipi { get; set; }
            [Category("Araba görünüş"), DisplayName("kasa tipi")]
            public string Açıklama { get; set; }
            [Category("Araba bilgileri"), DisplayName("açıklama")]

            public DateTime Date { get; set; }

        }

        private void DeleteCommand(object sender, EventArgs e)
        {
            {
                if (listView1.SelectedItems.Count == 0)
                    return;
                ListViewItem pItem = listView1.SelectedItems[0];
                Araba secili = pItem.Tag as Araba;
                var sonuc = MessageBox.Show($"Seçili Araba Silinsin mi?\n\n{secili.Plaka} {secili.Marka}", "Silmeyi onayla", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    arabalar.Remove(secili);
                    listView1.Items.Remove(pItem);
                }

            }

        }

        private void SaveCommand(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog()
            {
                Filter = "Json Formatı|*.json|Xml Formatı|*.xml",
            };
            if (sf.ShowDialog() == DialogResult.OK)
            {
                if (sf.FileName.EndsWith("json"))
                {
                    string data = JsonSerializer.Serialize(arabalar);
                    File.WriteAllText(sf.FileName, data);

                }
                else if (sf.FileName.EndsWith("xml"))
                {
                    StreamWriter sw = new StreamWriter(sf.FileName);
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Araba>));
                    serializer.Serialize(sw, arabalar);
                    sw.Close();
                }
            }

        }

        private void LoadCommand(object sender, EventArgs e)
        {
            OpenFileDialog of = new OpenFileDialog()
            {
                Filter = "Json, Xml Formatları|*.json;*.xml",
            };
            if (of.ShowDialog() == DialogResult.OK)
            {
                if (of.ShowDialog() == DialogResult.OK)
                {
                    if (of.FileName.ToLower().EndsWith("json"))
                    {
                        string jsondata = File.ReadAllText(of.FileName);
                        arabalar = JsonSerializer.Deserialize<List<Araba>>(jsondata);
                    }
                    else if (of.FileName.ToLower().EndsWith("xml"))
                    {
                        StreamReader sr = new StreamReader(of.FileName);
                        XmlSerializer serializer = new XmlSerializer(typeof(List<Araba>));
                        arabalar = (List<Araba>)serializer.Deserialize(sr);
                        sr.Close();
                    }
                    ShowList();

                }
            }
        }
        string temp = Path.Combine(Application.CommonAppDataPath, "data");

        protected override void OnClosing(CancelEventArgs e)
        {
            string data = JsonSerializer.Serialize(arabalar);
            File.WriteAllText(temp, data); ;

            base.OnClosing(e);
        }

        private void hakkındaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }
    }

    }


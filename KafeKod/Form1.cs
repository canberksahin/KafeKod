using KafeKod.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KafeKod
{
    public partial class Form1 : Form
    {
        KafeVeri db;
        int masaAdet = 21;


        public Form1()
        {
            db = new KafeVeri();
            OrnekVerileriYukle();
            InitializeComponent();
            MasalariOlustur();
        }

        private void OrnekVerileriYukle()
        {
            db.Urunler = new List<Urun> { new Urun { UrunAd = "Kola", BirimFiyat = 6.99m }, new Urun { UrunAd = "Çay", BirimFiyat = 3.99m } };

        }

        private void MasalariOlustur()
        {
            #region Listview imagelerinin Hazırlanmasi
            ImageList il = new ImageList();
            il.Images.Add("bos", Properties.Resources.masabos);
            il.Images.Add("dolu", Properties.Resources.masadolu);
            il.ImageSize = new Size(64, 64);
            lvwMasalar.LargeImageList = il;
            #endregion

            ListViewItem lvi;
            for (int i = 1; i <= masaAdet; i++)
            {
                lvi = new ListViewItem("Masa " + i);
                lvi.Tag = i;
                
                lvi.ImageKey = "bos";
                lvwMasalar.Items.Add(lvi);
            }
        }

        private void lvwMasalar_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var lvi = lvwMasalar.SelectedItems[0];
            if (e.Button == MouseButtons.Left)
            {
                lvi.ImageKey = "dolu";
                Siparis sip ;

                if (!(lvi.Tag is Siparis))
                {
                    sip = new Siparis();
                    sip.MasaNo = (int)lvi.Tag;
                    sip.AcilisZamani = DateTime.Now;
                    lvi.Tag = sip;
                    db.AktifSiparisler.Add(sip);
                }
                else
                {
                    sip = (Siparis)lvi.Tag;
                }

                SiparisForm frmsiparis = new SiparisForm(db,sip);
                frmsiparis.ShowDialog();
                if (sip.Durum != SiparisDurum.Aktif)
                {
                    lvi.Tag = sip.MasaNo;
                    lvi.ImageKey = "bos";
                    db.AktifSiparisler.Remove(sip);
                    db.GecmisSiparisler.Add(sip);
                }

            }
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            var frm =new GecmisSiparislerForm(db);
            frm.ShowDialog();
        }
    }
}

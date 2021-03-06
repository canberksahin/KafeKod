﻿using KafeKod.Data;
using Newtonsoft.Json;
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

namespace KafeKod
{
    public partial class Form1 : Form
    {
        KafeVeri db;


        public Form1()
        {
            VerileriOku();
            InitializeComponent();
            MasalariOlustur();
        }

        private void VerileriOku()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                db = JsonConvert.DeserializeObject<KafeVeri>(json);
            }
            catch (Exception)
            {
                db = new KafeVeri();
            }
        }

        private void OrnekVerileriYukle()
        {
            db.Urunler = new List<Urun> { new Urun { UrunAd = "Kola", BirimFiyat = 6.99m }, new Urun { UrunAd = "Çay", BirimFiyat = 3.99m } };
            db.Urunler.Sort();
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
            for (int i = 1; i <= db.MasaAdet; i++)
            {
                lvi = new ListViewItem("Masa " + i);
                #region Masada önceden sipariş varsa getir yoksa boş masa aç
                Siparis sip = db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == i);
                if (sip == null)
                {
                    lvi.Tag = i;
                    lvi.ImageKey = "bos";

                }
                else
                {
                    lvi.Tag = sip;
                    lvi.ImageKey = "dolu";
                } 
                #endregion
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
                frmsiparis.MasaTasiniyor += Frmsiparis_MasaTasindi;
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

        private void Frmsiparis_MasaTasindi(object sender, MasaTasimaEventArgs e)
        {
            ListViewItem lviEskiMasa = MasaBul(e.EskiMasaNo);
            lviEskiMasa.Tag = e.EskiMasaNo;
            lviEskiMasa.ImageKey = "bos";

            ListViewItem livYeniMasa = MasaBul(e.YeniMasaNo);
            livYeniMasa.Tag = e.TasinanSiparis;
            livYeniMasa.ImageKey = "dolu";
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            var frm =new GecmisSiparislerForm(db);
            frm.ShowDialog();
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            var frm = new UrunlerForm(db);
            frm.ShowDialog();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            string json = JsonConvert.SerializeObject(db);
            File.WriteAllText("veri.json", json);
        }

        private ListViewItem MasaBul(int masaNo)
        {
            foreach (ListViewItem item in lvwMasalar.Items)
            {
                if (item.Tag is int &&(int)item.Tag == masaNo)
                {
                    return  item;
                }
               else if (item.Tag is Siparis && ((Siparis)item.Tag).MasaNo == masaNo)
                {
                    return item;
                }

            }
            return null;
        }
    }
}

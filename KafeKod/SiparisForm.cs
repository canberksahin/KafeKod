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
    public partial class SiparisForm : Form
    {
        KafeVeri db;
        Siparis siparis;
        BindingList<SiparisDetay> blSiparisDetaylar;


        public SiparisForm(KafeVeri kafeVeri,Siparis siparis)
        {
            InitializeComponent();
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            db = kafeVeri;
            cboUrun.DataSource=(db.Urunler);
            MasaNoGüncelle();
            TutarGuncelle();
            for (int i = 1; i < 22; i++)
            {
                cboMasaNo.Items.Add(i);
            }
            
            dgvSiparisDetaylari.DataSource = blSiparisDetaylar;

        }

        private void TutarGuncelle()
        {
           lblTutar.Text = siparis.ToplamtutarTL;
        }

        private void MasaNoGüncelle()
        {
            Text = "Masa " + siparis.MasaNo;
            lblMasaNo.Text = siparis.MasaNo.ToString("00");
            cboMasaNo.SelectedItem = siparis.MasaNo;
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            if (cboUrun.SelectedItem==null)
            {
                MessageBox.Show("Lütfen bir ürün seçiniz.");
                return;
            }

            Urun seciliUrun = (Urun)cboUrun.SelectedItem;
            var sd = new SiparisDetay
            {
                UrunAd = seciliUrun.UrunAd,
                BirimFiyat = seciliUrun.BirimFiyat,
                Adet = (int)nudAdet.Value
            };
            blSiparisDetaylar.Add(sd);
            TutarGuncelle();
            nudAdet.Value = 1;
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Sipariş iptal edilecektir onaylıyor musunuz?","Sipariş iptal " +
                "onayı",MessageBoxButtons.YesNo,MessageBoxIcon.Warning,MessageBoxDefaultButton.Button2);
            if (dr== DialogResult.Yes)
            {
                siparis.Durum = SiparisDurum.Iptal;
                siparis.KapanisZamani = DateTime.Now;
                Close();

            }
            
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            var dr = MessageBox.Show("Hesap kapatılacaktır.", "Ödeme onayı " 
               , MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
            {
                siparis.Durum = SiparisDurum.Odendi;
                siparis.KapanisZamani = DateTime.Now;
                siparis.OdenenTutar = siparis.ToplamTutar();
                Close();

            }
            siparis.Durum = SiparisDurum.Iptal;

            DialogResult = DialogResult.Cancel;
        }
    }
}

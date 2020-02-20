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
        public event EventHandler<MasaTasimaEventArgs> MasaTasiniyor;


        KafeVeri db;
        Siparis siparis;
        BindingList<SiparisDetay> blSiparisDetaylar;


        public SiparisForm(KafeVeri kafeVeri, Siparis siparis)
        {
            InitializeComponent();
            this.siparis = siparis;
            blSiparisDetaylar = new BindingList<SiparisDetay>(siparis.SiparisDetaylar);
            db = kafeVeri;
            db.Urunler.Sort();
            cboUrun.DataSource = db.Urunler.OrderBy(x=>x.UrunAd).ToList();
            MasaNoGüncelle();
            TutarGuncelle();
            MasaNolariYükle();


            dgvSiparisDetaylari.DataSource = blSiparisDetaylar;

        }

        private void MasaNolariYükle()
        {
            for (int i = 1; i <= db.MasaAdet; i++)
            {

                if (!(db.AktifSiparisler.Any(x=>x.MasaNo == i)))
                {
                cboMasaNo.Items.Add(i);

                }
            }
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
            if (cboUrun.SelectedItem == null)
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
            var dr = MessageBox.Show("Sipariş iptal edilecektir onaylıyor musunuz?", "Sipariş iptal " +
                "onayı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.Yes)
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

            DialogResult = DialogResult.Cancel;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.dgvSiparisDetaylari.SelectedRows.Count > 0)
            {
                dgvSiparisDetaylari.Rows.RemoveAt(this.dgvSiparisDetaylari.SelectedRows[0].Index);
            }
            TutarGuncelle();
        }

        private void button1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void dgvSiparisDetaylari_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int rowIndex = dgvSiparisDetaylari.HitTest(e.X, e.Y).RowIndex;
                if (rowIndex > -1)
                {
                    dgvSiparisDetaylari.ClearSelection();
                    dgvSiparisDetaylari.Rows[rowIndex].Selected = true;
                    cmsSiparisDetay.Show(MousePosition);
                }

            }
        }

        private void tsmiSiparisDetaySil_Click(object sender, EventArgs e)
        {
            if (this.dgvSiparisDetaylari.SelectedRows.Count > 0)
            {
                var seciliSatir = dgvSiparisDetaylari.SelectedRows[0];
                var sipDetay = (SiparisDetay)seciliSatir.DataBoundItem;
                blSiparisDetaylar.Remove(sipDetay);
            }
            TutarGuncelle();
        }

        private void btnMasaTasi_Click(object sender, EventArgs e)
        {
            if (cboMasaNo.SelectedItem == null)
            {
                MessageBox.Show("Lütfen bir masa no seçiniz.");
                return;
            }

            int eskiMasaNo = siparis.MasaNo;

            int hedafMasaNo = (int)cboMasaNo.SelectedItem;
            if (MasaTasiniyor != null)
            {
                var args = new MasaTasimaEventArgs
                {
                    TasinanSiparis = siparis,
                    EskiMasaNo = eskiMasaNo,
                    YeniMasaNo = hedafMasaNo

                };
                MasaTasiniyor(this, args);
            }
            siparis.MasaNo = hedafMasaNo;
            MasaNoGüncelle();
            MasaNolariYükle();

        }
    }

    public class MasaTasimaEventArgs : EventArgs
    {
        public Siparis TasinanSiparis { get; set; }

        public int EskiMasaNo { get; set; }

        public int YeniMasaNo { get; set; }


    }
}

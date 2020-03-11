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
    public partial class UrunlerForm : Form
    {
        KafeContext db;
        BindingList<Urun> blUrunler;
        public UrunlerForm( KafeContext kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            blUrunler = new BindingList<Urun>(db.Urunler);
            dgvUrunler.DataSource = blUrunler;
        }

        public void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtUrunAd.Text == "")
            {
                MessageBox.Show("Bir ürün adı girmediniz.");
            }
            else
            {
            Urun yeniUrun = new Urun {UrunAd = txtUrunAd.Text.Trim(), BirimFiyat = nudBirimFiyat.Value };
            blUrunler.Add(yeniUrun);
            txtUrunAd.Clear();
                db.Urunler.Sort();
            }
        }

        private void dgvUrunler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Geçerli bir değer giriniz. ");

        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex ==0)
            {
                if (e.FormattedValue.ToString().Trim() == "")
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "Ürün adı boş geçilemez";
                    e.Cancel = true;
                }
                else
                {
                    dgvUrunler.Rows[e.RowIndex].ErrorText = "";
                }
            }
        }
    }
}

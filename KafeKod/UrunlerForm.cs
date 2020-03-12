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
        public UrunlerForm(KafeContext kafeVeri)
        {
            db = kafeVeri;
            InitializeComponent();
            dgvUrunler.AutoGenerateColumns = false;
            dgvUrunler.DataSource = new BindingSource(db.Urunler.OrderBy(x => x.UrunAd).ToList(), null);
        }

        public void btnEkle_Click(object sender, EventArgs e)
        {
            if (txtUrunAd.Text == "")
            {
                MessageBox.Show("Bir ürün adı girmediniz.");
            }
            else
            {
                Urun yeniUrun = new Urun { UrunAd = txtUrunAd.Text.Trim(), BirimFiyat = nudBirimFiyat.Value };
                db.Urunler.Add(yeniUrun);
                db.SaveChanges();
                txtUrunAd.Clear();

                dgvUrunler.DataSource = new BindingSource(db.Urunler.OrderBy(x => x.UrunAd).ToList(), null);
            }
        }

        private void dgvUrunler_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            MessageBox.Show("Geçerli bir değer giriniz. ");

        }

        private void dgvUrunler_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
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
            db.SaveChanges();
            }
        }

        private void dgvUrunler_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Urun urun = (Urun)e.Row.DataBoundItem;
            try
            {
                db.Urunler.Remove(urun);
                db.SaveChanges();
            }
            catch (Exception)
            {
                MessageBox.Show("Bu ürün geçmiş siparişlerle ilişkili oldugu için silinemez");
                e.Cancel = true;
            }

        }

        private void UrunlerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            txtUrunAd.Focus();
        }
    }
}

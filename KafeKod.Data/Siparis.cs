﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KafeKod.Data
{
    public enum SiparisDurum {Aktif,Odendi,Iptal }
    public class Siparis
    {
        public Siparis()
        {
            SiparisDetaylar = new List<SiparisDetay>();
        }

        public int MasaNo { get; set; }

        public DateTime? AcilisZamani { get; set; }

        public DateTime? KapanisZamani { get; set; }

        public SiparisDurum Durum { get; set; }

        public List<SiparisDetay> SiparisDetaylar { get; set; }

        public string ToplamtutarTL => string.Format("{0:0.00 ₺}", ToplamTutar());

        public decimal ToplamTutar() => SiparisDetaylar.Sum(x => x.Tutar());

        public decimal OdenenTutar { get; set; }

    }
}

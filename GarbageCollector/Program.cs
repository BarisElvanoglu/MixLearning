using System;
using System.Collections.Generic;
using System.Runtime.InteropServices; // Unmanaged (Vahşi Batı) için gerekli

namespace MemoryMasterDemo
{
    // -----------------------------------------------------------------------
    // 1. SINIF TANIMI (LOADER HEAP - MİMARLIK OFİSİ)
    // -----------------------------------------------------------------------
    // Bu sınıfın tanımı (MethodTable) ve Static değişkenleri
    // LOADER HEAP'te tutulur. Uygulama kapanana kadar silinmez.
    class Program
    {
        // Static bir liste: Kökü Loader Heap'tedir. 
        // İçine eklenen veriler Gen 2'ye taşınır ve ölümsüzleşir (Dikkat!).
        static List<string> _staticLoglar = new List<string>();

        static void Main(string[] args)
        {
            Console.WriteLine("=== .NET HAFIZA HARİTASI TURU BAŞLIYOR ===\n");

            // -------------------------------------------------------------------
            // 2. STACK (ÇALIŞMA MASASI)
            // -------------------------------------------------------------------
            // Bu değişkenler sadece Main metodu bitene kadar yaşar.
            int turSayisi = 1;
            bool islemTamam = false;
            Console.WriteLine($"[Stack] Yerel değişkenler oluşturuldu. (Tur: {turSayisi})");

            // -------------------------------------------------------------------
            // 3. STRING INTERN POOL (ÖLÜMSÜZLER KULÜBÜ)
            // -------------------------------------------------------------------
            // Elle yazılan string -> Intern Pool (Kalıcı)
            string s1 = "Merhaba";
            // Hesaplanan string -> Gen 0 (Geçici)
            string s2 = new DateTime(2023, 1, 1).ToString();

            Console.WriteLine($"[Intern Pool] '{s1}' havuza atıldı. '{s2}' ise Gen 0'a gitti.");

            // -------------------------------------------------------------------
            // 4. LOH (LARGE OBJECT HEAP - KIRMIZI KUTU)
            // -------------------------------------------------------------------
            // 85.000 byte sınırını geçtiğimiz için direkt LOH'a gider.
            // Gen 0'ı meşgul etmez.
            byte[] buyukTampon = new byte[90000];
            Console.WriteLine($"[LOH] 90.000 byte'lık dizi LOH'ta oluşturuldu.");

            // -------------------------------------------------------------------
            // 5. POH (PINNED OBJECT HEAP - BETON ZEMİN)
            // -------------------------------------------------------------------
            // .NET 5+ özelliği. GC bunu temizler ama yerinden oynatmaz.
            // Genelde Network/Socket işlemleri için kullanılır.
            int[] sabitDizi = GC.AllocateArray<int>(100, pinned: true);
            Console.WriteLine($"[POH] Sabitlenmiş (Pinned) dizi oluşturuldu.");

            Console.WriteLine("\n--- UNMANAGED DÜNYAYA GİRİŞ (GEN 0 -> UNMANAGED) ---\n");

            // -------------------------------------------------------------------
            // 6. UNMANAGED MEMORY & WRAPPER (GEN 0 + VAHŞİ BATI)
            // -------------------------------------------------------------------
            // 'using' bloğu: Doğru kaynak yönetimi (Best Practice).
            // 'dosyaYonetici' (Wrapper) -> Gen 0'da yaşar.
            // İçindeki 1024 byte -> Unmanaged Memory'de yaşar.
            using (var dosyaYonetici = new NativeKaynakYonetici(1024))
            {
                dosyaYonetici.VeriYaz("Kritik Veri Paketi");
                // Buradayken:
                // Stack -> dosyaYonetici -> Unmanaged Pointer -> RAM
                // Bağlantı tam ve canlı.
            }
            // <--- BURADA: Dispose() otomatik çalışır.
            // 1. Unmanaged RAM iade edilir.
            // 2. Gen 0'daki 'dosyaYonetici' sahipsiz kalır (Çöp olur).

            Console.WriteLine("\n--- TUR BİTTİ ---");
            Console.WriteLine("Enter'a basınca uygulama (ve Loader Heap) kapanacak.");
            Console.ReadLine();
        }
    }

    // -----------------------------------------------------------------------
    // UNMANAGED WRAPPER (KÖPRÜ SINIF)
    // -----------------------------------------------------------------------
    // Bu sınıfın kendisi Managed Heap'te (Gen 0) yaşar,
    // ama yönettiği kaynak (IntPtr) Unmanaged Memory'dedir.
    class NativeKaynakYonetici : IDisposable
    {
        // Unmanaged Adres (Pointer)
        private IntPtr _handle;
        private bool _disposed = false;

        public NativeKaynakYonetici(int boyut)
        {
            // Vahşi Batı'dan yer ayırıyoruz (Malloc)
            _handle = Marshal.AllocHGlobal(boyut);
            Console.WriteLine($"[Gen 0 -> Unmanaged] Wrapper doğdu. {boyut} byte RAM ayrıldı. Adres: {_handle}");
        }

        public void VeriYaz(string mesaj)
        {
            if (_disposed) throw new ObjectDisposedException("NativeKaynakYonetici");
            Console.WriteLine($"[İşlem] Unmanaged belleğe yazılıyor: {mesaj}");
        }

        // 1. MANUEL TEMİZLİK (Check-out)
        // Yazılımcı 'using' veya .Dispose() dediğinde çalışır.
        public void Dispose()
        {
            Temizle(true);
            // GC'ye not bırak: "Ben temizliğimi yaptım, Destructor'ı çalıştırma, beni direkt sil."
            GC.SuppressFinalize(this);
        }

        // 2. OTOMATİK EMNİYET SÜBAPI (Destructor)
        // Yazılımcı Dispose'u unutursa, GC nesneyi silmeden hemen önce burayı çağırır.
        ~NativeKaynakYonetici()
        {
            Console.WriteLine("[Destructor] Eyvah! Dispose unutulmuş. GC devreye girdi.");
            Temizle(false);
        }

        // Ortak Temizlik Mantığı
        protected virtual void Temizle(bool disposing)
        {
            if (!_disposed)
            {
                // A. Unmanaged Kaynakları Temizle (HER ZAMAN)
                if (_handle != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(_handle); // RAM'i iade et
                    _handle = IntPtr.Zero;
                    Console.WriteLine("[Temizlik] Unmanaged bellek işletim sistemine iade edildi.");
                }

                // B. Managed Kaynakları Temizle (SADECE DISPOSE İLE)
                if (disposing)
                {
                    Console.WriteLine("[Temizlik] Managed kaynaklar (varsa) temizlendi.");
                }

                _disposed = true;
            }
        }
    }
}
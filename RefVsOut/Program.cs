// ============================================================================
// ref ve out Anahtar Kelimeleri Arasındaki Fark
// ============================================================================
//
// REF (Reference):
// - Parametreyi referans ile geçer
// - Çağıran tarafından BAŞLANGIÇTA başlatılması ZORUNLUDUR
// - Metot içinde okunabilir ve yazılabilir
// - Orijinal değişkeni değiştirebilir
//
// OUT:
// - Parametreyi referans ile geçer
// - Çağıran tarafından başlatılması GEREKMEDİR
// - Metot içinde MUTLAKA atanması ZORUNLUDUR (başlatma yapmalısın)
// - Orijinal değişkeni değiştirebilir
// - Metottan dönen değer yerine kullanılabilir (örneğin: TryParse)
// ============================================================================

class Program
{
    static void Main()
    {
        Console.WriteLine("========== REF ÖRNEĞİ ==========\n");
        RefOrnegi();

        Console.WriteLine("\n========== OUT ÖRNEĞİ ==========\n");
        OutOrnegi();

        Console.WriteLine("\n========== ref vs out KARŞILAŞTIRMASI ==========\n");
        Karsilastirma();
    }

    // REF ÖRNEĞİ
    static void RefOrnegi()
    {
        int sayi = 10;
        Console.WriteLine($"Başlangıç değeri: {sayi}");

        // ref parametresi için değişken BAŞLATILMIŞ olmalı
        RefArtir(ref sayi);
        Console.WriteLine($"ref sonrası değer: {sayi}");

        // ref ile liste elemanları değiştirilebilir
        int[] dizi = { 1, 2, 3 };
        Console.WriteLine($"ref dizi[0] başlangıç: {dizi[0]}");
        RefArtir(ref dizi[0]);
        Console.WriteLine($"ref dizi[0] sonrası: {dizi[0]}");
    }

    static void RefArtir(ref int deger)
    {
        // ref parametresi halihazırda bir değere sahip olduğu için okuyabiliriz
        Console.WriteLine($"  Metot içinde gelen değer: {deger}");
        deger *= 2;
    }

    // OUT ÖRNEĞİ
    static void OutOrnegi()
    {
        // out parametresi için değişkenin başlatılması GEREKMEDİR
        int sonuc;
        Hesapla(5, 3, out sonuc);
        Console.WriteLine($"out sonrası değer: {sonuc}");

        // out ile string.TryParse örneği (gerçek dünya kullanımı)
        string metin = "42";
        if (int.TryParse(metin, out int parseLananDeger))
        {
            Console.WriteLine($"TryParse başarılı: {parseLananDeger}");
        }

        // Birden fazla out parametresi
        DivideByCal(10, 3, out int bolum, out int kalan);
        Console.WriteLine($"10 / 3 = {bolum}, kalan: {kalan}");
    }

    static void Hesapla(int a, int b, out int sonuc)
    {
        // out parametresi MUTLAKA atanması gerekli
        sonuc = a + b;
        Console.WriteLine($"  Hesaplama yapıldı: {a} + {b} = {sonuc}");
    }

    static void DivideByCal(int bolunecek, int bolen, out int bolum, out int kalan)
    {
        // Her iki out parametresi de atanmalı
        bolum = bolunecek / bolen;
        kalan = bolunecek % bolen;
    }

    // KARŞILAŞTIRMA
    static void Karsilastirma()
    {
        Console.WriteLine("1. Başlatma Gereksinimi:");
        Console.WriteLine("   ref  : Çağıran tarafından BAŞLATILMALI (ön koşul)");
        Console.WriteLine("   out  : Başlatılması GEREKMEDİR\n");

        Console.WriteLine("2. Atama Gereksinimi:");
        Console.WriteLine("   ref  : Atama isteğe bağlı (değer zaten var)");
        Console.WriteLine("   out  : Metot çıkmadan MUTLAKA atanmalı\n");

        Console.WriteLine("3. Kullanım Alanları:");
        Console.WriteLine("   ref  : Var olan değeri değiştirmek istediğinde");
        Console.WriteLine("   out  : Metottan birden fazla değer döndermek istediğinde\n");

        Console.WriteLine("4. Örnek Senaryolar:");
        Console.WriteLine("   ref  : Swap, Artırma/Azaltma işlemleri");
        Console.WriteLine("   out  : TryParse, Hesaplama sonuçları, Tuple alternatifi");
    }
}
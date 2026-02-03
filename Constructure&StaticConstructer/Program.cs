using System;

// ============================================================================
// Constructor vs Static Constructor Farkları
// ============================================================================

class Kullanici
{
    // Statik değişken
    public static int ToplamKullanici = 0;
    
    // Örnek değişkenler
    public string Ad { get; set; }
    public int ID { get; set; }

    // ========== STATIC CONSTRUCTOR ==========
    // Sınıf ilk kez kullanıldığında BİR KEZ çağrılır
    static Kullanici()
    {
        Console.WriteLine("❌ STATIC CONSTRUCTOR çağrıldı (Sınıf başlatılıyor)");
        ToplamKullanici = 0;
    }

    // ========== CONSTRUCTOR (Örnek Constructor) ==========
    // Her yeni örnek oluşturulduğunda çağrılır
    public Kullanici(string ad)
    {
        Console.WriteLine($"✅ CONSTRUCTOR çağrıldı (Yeni örnek oluşturuluyor)");
        Ad = ad;
        ToplamKullanici++;
        ID = ToplamKullanici;
        Console.WriteLine($"   Kullanıcı adı: {Ad}, ID: {ID}, Toplam: {ToplamKullanici}\n");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("========== FARK GÖSTEREN ÖRNEK ==========\n");

        // İlk örneği oluştur -> Static constructor + Örnek constructor çalışır
        Console.WriteLine("--- Kullanici1 oluşturuluyor ---");
        Kullanici k1 = new Kullanici("Ali");

        // İkinci örneği oluştur -> Sadece örnek constructor çalışır (static constructor tekrar çağrılmaz!)
        Console.WriteLine("--- Kullanici2 oluşturuluyor ---");
        Kullanici k2 = new Kullanici("Ayşe");

        // Üçüncü örneği oluştur -> Sadece örnek constructor çalışır
        Console.WriteLine("--- Kullanici3 oluşturuluyor ---");
        Kullanici k3 = new Kullanici("Mehmet");

        Console.WriteLine($"\n📊 Sonuç: Toplam {Kullanici.ToplamKullanici} kullanıcı oluşturuldu");
    }
}
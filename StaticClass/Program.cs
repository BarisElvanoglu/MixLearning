using System;

// ============================================================================
// STATIC CLASS ÖZELLİKLERİ
// ============================================================================
// 1. Instance oluşturulamaz (yeni Util() → HATA!)
// 2. Tüm üyeleri static olmak zorundadır
// 3. Parametresiz static constructor'ı olabilir (sınıf ilk kullanımında bir kez çağrılır)
// 4. Sealed gibi davranır: kalıtım alınamaz ve türetilmez
// 5. Interface implement edemez
// 6. Extension metodlar static class'da tanımlanmalıdır
// ============================================================================

// ❌ HATA: Static sınıf instance üye içeremez
// public static class HataliSinif
// {
//     public int DegerSahip; // ❌ ERROR: static sınıfta instance alan olamaz
//     public void MetodSahip() { } // ❌ ERROR: static sınıfta instance metod olamaz
// }

// ✅ DOĞRU: Static sınıf sadece static üyeler içerebilir
public static class HesapMakinesi
{
    // Statik değişkenler
    private static int _islemSayisi = 0;

    // Static constructor: sınıf ilk kez kullanıldığında bir kez çağrılır
    static HesapMakinesi()
    {
        Console.WriteLine("📊 HesapMakinesi: static constructor çalıştı (sınıf ilk kez kullanılıyor)");
        _islemSayisi = 0;
    }

    // Tüm üyeleri static olmalı
    public static int Topla(int a, int b)
    {
        _islemSayisi++;
        return a + b;
    }

    public static int Cıkar(int a, int b)
    {
        _islemSayisi++;
        return a - b;
    }

    public static int Carp(int a, int b)
    {
        _islemSayisi++;
        return a * b;
    }

    public static int IslemSayisi => _islemSayisi;
}

// ✅ Extension metodlar static sınıfta tanımlanmalı
public static class StringEksiksaydisi
{
    // Extension metod: "this" anahtar kelimesiyle tanımlanır
    public static int KelimeSayisi(this string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;
        return text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }

    public static string TerseCevir(this string text)
    {
        char[] karakterler = text.ToCharArray();
        Array.Reverse(karakterler);
        return new string(karakterler);
    }
}

// ✅ Utility/Helper sınıfı
public static class ZamanYardimcisi
{
    public static string SuAnkiTarih => DateTime.Now.ToString("dd/MM/yyyy");
    public static string SuAnkiSaat => DateTime.Now.ToString("HH:mm:ss");

    public static int GecenGun(DateTime tarih)
    {
        return (DateTime.Now - tarih).Days;
    }
}

// ✅ Logging utility
public static class Logger
{
    private static int _logSayisi = 0;

    static Logger()
    {
        Console.WriteLine("📝 Logger: static constructor çalıştı");
    }

    public static void Bilgi(string mesaj)
    {
        _logSayisi++;
        Console.WriteLine($"[INFO] {mesaj}");
    }

    public static void Hata(string mesaj)
    {
        _logSayisi++;
        Console.WriteLine($"[ERROR] {mesaj}");
    }

    public static void Uyari(string mesaj)
    {
        _logSayisi++;
        Console.WriteLine($"[WARNING] {mesaj}");
    }

    public static int ToplamLog => _logSayisi;
}

class Program
{
    static void Main()
    {
        Console.WriteLine("========== STATIC CLASS ÖRNEKLERİ ==========\n");

        // ==================== ÖRNEK 1: Static üyelere erişim ====================
        Console.WriteLine("--- 1. Static Üyelere Doğrudan Erişim ---");
        int sonuc1 = HesapMakinesi.Topla(10, 5);
        int sonuc2 = HesapMakinesi.Carp(6, 7);
        Console.WriteLine($"10 + 5 = {sonuc1}");
        Console.WriteLine($"6 × 7 = {sonuc2}");
        Console.WriteLine($"Toplam işlem: {HesapMakinesi.IslemSayisi}");
        Console.WriteLine();

        // ==================== ÖRNEK 2: Static Constructor ====================
        Console.WriteLine("--- 2. Static Constructor (ilk kullanımda bir kez çalışır) ---");
        Logger.Bilgi("Uygulama başlatıldı");
        Logger.Hata("Test hatası");
        Logger.Uyari("Test uyarısı");
        Console.WriteLine($"Toplam log: {Logger.ToplamLog}\n");

        // ==================== ÖRNEK 3: Extension Metodlar ====================
        Console.WriteLine("--- 3. Extension Metodlar (Static Sınıfta Tanımlanır) ---");
        string metin = "C Sharp Harika bir Dil";
        Console.WriteLine($"Metin: \"{metin}\"");
        Console.WriteLine($"Kelime sayısı: {metin.KelimeSayisi()}");
        Console.WriteLine($"Tersine çevrilmiş: \"{metin.TerseCevir()}\"");
        Console.WriteLine();

        // ==================== ÖRNEK 4: Utility Fonksiyonlar ====================
        Console.WriteLine("--- 4. Utility Sınıfı ---");
        Console.WriteLine($"Bugünün tarihi: {ZamanYardimcisi.SuAnkiTarih}");
        Console.WriteLine($"Şu anki saat: {ZamanYardimcisi.SuAnkiSaat}");
        DateTime dogumTarihi = new DateTime(2000, 5, 15);
        Console.WriteLine($"2000/05/15 tarihinden itibaren {ZamanYardimcisi.GecenGun(dogumTarihi)} gün geçti");
        Console.WriteLine();

        // ==================== ÖRNEK 5: Instance Oluşturma Denemesi ====================
        Console.WriteLine("--- 5. Instance Oluşturma Denemesi ---");
        // Aşağıdaki satır DERLEME HATASI verir:
        // var utility = new HesapMakinesi(); // ❌ CS0723: Cannot declare variable of static type 'HesapMakinesi'
        Console.WriteLine("❌ var utility = new HesapMakinesi(); → DERLEME HATASI");
        Console.WriteLine("   Static sınıftan instance oluşturulamaz!\n");

        // ==================== ÖRNEK 6: Kalıtım Denemesi ====================
        Console.WriteLine("--- 6. Kalıtım Denemesi ---");
        // Aşağıdaki satır DERLEME HATASI verir:
        // public class OzelHesapMakinesi : HesapMakinesi { } // ❌ CS0509: 'HesapMakinesi' is a static type and cannot be used as a base class
        Console.WriteLine("❌ public class OzelHesapMakinesi : HesapMakinesi { }");
        Console.WriteLine("   Static sınıftan kalıtım alınamaz!\n");

        // ==================== ÖZET TABLOSU ====================
        Console.WriteLine("========== STATIC CLASS - ÖZET TABLOSU ==========\n");
        Console.WriteLine("Özellik                      | Durum");
        Console.WriteLine("-".PadRight(55, '-'));
        Console.WriteLine("Instance oluşturulabilir     | ❌ Hayır");
        Console.WriteLine("Static üyelere erişim        | ✅ Doğrudan (ClassName.Member)");
        Console.WriteLine("Instance üyeleri içerebilir  | ❌ Hayır");
        Console.WriteLine("Static constructor           | ✅ Evet (parametresiz)");
        Console.WriteLine("Kalıtım alabilir/verebilir   | ❌ Hayır (sealed gibi)");
        Console.WriteLine("Interface implement edebilir | ❌ Hayır");
        Console.WriteLine("Extension metodlar içerebilir| ✅ Evet (bu amaçla kullanılır)");
        Console.WriteLine("Parametreli constructor      | ❌ Hayır");
        Console.WriteLine();

        // ==================== KULLANIM ALANLARI ====================
        Console.WriteLine("========== STATIC CLASS KULLANIM ALANLARI ==========\n");
        Console.WriteLine("✅ Utility/Helper metodlar (Math, Path, File)");
        Console.WriteLine("✅ Extension metodlar container'ı");
        Console.WriteLine("✅ Singleton pattern (ama Singleton sınıf öneriliyor)");
        Console.WriteLine("✅ Global fonksiyonlar (Logger, Configuration)");
        Console.WriteLine("✅ Matematik operasyonları (HesapMakinesi gibi)");
        Console.WriteLine("✅ .NET Framework'teki örnekler: Math, Convert, Environment");
    }
}

// ========== YAYGIN HATA VE DOĞRU KULLANIM ==========

// ❌ YANLIŞ: Normal sınıfta instance olmaması
// public class YanlisSinif
// {
//     public static void YaratMethod() { } // Static üyeleri var ama instance de oluşturabilir
// }

// ✅ DOĞRU: Sadece static üyeleri varsa static sınıf yap
// public static class DogruSinif
// {
//     public static void YaratMethod() { }
// }
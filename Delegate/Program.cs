using System;

// ============================================
// 1. DELEGATE - Temel Kullanım
// ============================================
// Delegate: Metodlara referans tutmak için kullanılan type-safe bir yoldur.
// Amacı: Metodları değişkenlere atamak, parametre olarak geçmek, geri dönüş değeri olarak kullanmak
delegate int HesapMakinesiDelegate(int ilkDeger, int ikinciDeger);

// ============================================
// 2. ACTION - Return değeri olmayan Delegate
// ============================================
// Action: Geri dönüş değeri olmayan (void) metodları tutmak için kullanılır.
// Avantajı: Delegate tanımlamaya gerek yok, hazır generic type

// ============================================
// 3. FUNC - Return değeri olan Delegate
// ============================================
// Func<TInput, TOutput>: Geri dönüş değeri olan metodları tutmak için kullanılır.
// Son type parameter dönüş değeridir.

// ============================================
// 4. EVENT - Olay tabanlı programlama
// ============================================
// Event: Belirli bir durumda (olay) abonelere (subscribers) bildirim gönderir.
// Publisher-Subscriber pattern implementasyonu
// Bağlantı kuran: += , Bağlantıyı koparan: -=

// ============================================
// 5. EVENT LEAK - Bellek sızıntısı
// ============================================
// Event Leak: Event'e subscribe olan nesneler, unsubscribe edilmezse bellek sızıntısına neden olur.
// Çözüm: Subscribe edilen eventlerden mutlaka unsubscribe etmek veya weak event pattern kullanmak

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== 1. DELEGATE ÖRNEĞI ===\n");
        DelegateOrnegi();

        Console.WriteLine("\n=== 2. ACTION ÖRNEĞI ===\n");
        ActionOrnegi();

        Console.WriteLine("\n=== 3. FUNC ÖRNEĞI ===\n");
        FuncOrnegi();

        Console.WriteLine("\n=== 4. EVENT ÖRNEĞI ===\n");
        EventOrnegi();

        Console.WriteLine("\n=== 5. EVENT LEAK ÖRNEĞI ===\n");
        EventLeakOrnegi();
    }

    // ====== 1. DELEGATE ÖRNEĞI ======
    static void DelegateOrnegi()
    {
        // Delegate tanımlandı: HesapMakinesiDelegate
        HesapMakinesiDelegate hesapMakinesi;

        // Delegate'e Toplama metodunu ata
        hesapMakinesi = Toplama;
        int sonuc1 = hesapMakinesi(5, 3);
        Console.WriteLine($"Delegate ile Toplama(5, 3) = {sonuc1}");

        // Delegate'e Carpma metodunu ata
        hesapMakinesi = Carpma;
        int sonuc2 = hesapMakinesi(5, 3);
        Console.WriteLine($"Delegate ile Carpma(5, 3) = {sonuc2}");

        // Multicast Delegate: Birden fazla metodu çağır
        hesapMakinesi = Toplama;
        hesapMakinesi += Carpma; // Son sonuç döner (Carpma sonucu)
        int sonuc3 = hesapMakinesi(5, 3);
        Console.WriteLine($"Delegate Multicast son sonuç = {sonuc3}");
    }

    // ====== 2. ACTION ÖRNEĞI ======
    static void ActionOrnegi()
    {
        // Action<T1, T2>: Parametresi olan ama return değeri olmayan metod
        Action<string> selamla = Selamla;
        selamla("Ahmet");

        // Lambda ile Action
        Action<int, int> topla = (a, b) => Console.WriteLine($"Toplama: {a + b}");
        topla(10, 5);

        // Multicast Action
        Action<string> islemler = Console.WriteLine;
        islemler += (msg) => Console.WriteLine($"İşlem tamamlandı: {msg}");
        islemler("Başarılı");
    }

    // ====== 3. FUNC ÖRNEĞI ======
    static void FuncOrnegi()
    {
        // Func<T1, T2, ..., TReturn>: Son type parameter return değeridir
        // Func<int, int, int> = iki int parametre, int return değeri
        Func<int, int, int> hesapla = Toplama;
        int sonuc1 = hesapla(10, 5);
        Console.WriteLine($"Func ile Toplama(10, 5) = {sonuc1}");

        // Lambda ile Func
        Func<int, int, int> carp = (a, b) => a * b;
        int sonuc2 = carp(10, 5);
        Console.WriteLine($"Func ile Carpma(10, 5) = {sonuc2}");

        // Func - String return
        Func<string, string> buyukHarf = (str) => str.ToUpper();
        Console.WriteLine($"Func ile ToUpper: {buyukHarf("merhaba")}");
    }

    // ====== 4. EVENT ÖRNEĞI ======
    static void EventOrnegi()
    {
        // Publisher: Olayı yayınlayan sınıf
        KargoSistemi kargoSistemi = new KargoSistemi();

        // Subscriber 1: Olaya abone ol
        kargoSistemi.KargoGonderildiEvent += (msg) => Console.WriteLine($"SMS: {msg}");

        // Subscriber 2: Aynı olaya başka bir abonelik
        kargoSistemi.KargoGonderildiEvent += (msg) => Console.WriteLine($"Email: {msg}");

        // Event'i tetikle (Publisher)
        kargoSistemi.KargoGonder("12345", "Ankara");
    }

    // ====== 5. EVENT LEAK ÖRNEĞI ======
    static void EventLeakOrnegi()
    {
        // EVENT LEAK PROBLEMI
        KargoSistemi kargoSistemi = new KargoSistemi();
        KargoTakip kargoTakip = new KargoTakip();

        // kargoTakip, KargoGonderildiEvent'e abone oldu
        kargoSistemi.KargoGonderildiEvent += kargoTakip.BilgiAl;

        Console.WriteLine("Kargo gönderildi:");
        kargoSistemi.KargoGonder("12345", "Istanbul");

        // EVENT LEAK: kargoTakip unsubscribe edilmezse bellek sızıntısı oluşur
        // Çözüm: kargoSistemi.KargoGonderildiEvent -= kargoTakip.BilgiAl;

        Console.WriteLine("\nSigileme başlangıcı (unsubscribe):");
        // Event Leak'i önlemek için unsubscribe et
        kargoSistemi.KargoGonderildiEvent -= kargoTakip.BilgiAl;

        Console.WriteLine("Kargo gönderildi (artık takip edilmiyor):");
        kargoSistemi.KargoGonder("67890", "Izmir");
    }

    // ====== YARDIMCI METOTLAR ======
    static int Toplama(int ilkDeger, int ikinciDeger)
    {
        return ilkDeger + ikinciDeger;
    }

    static int Bolme(int ilkDeger, int ikinciDeger)
    {
        return ilkDeger / ikinciDeger;
    }

    static int Cıkartma(int ilkDeger, int ikinciDeger)
    {
        return ilkDeger - ikinciDeger;
    }

    static int Carpma(int ilkDeger, int ikinciDeger)
    {
        return ilkDeger * ikinciDeger;
    }

    static void Selamla(string ad)
    {
        Console.WriteLine($"Merhaba {ad}!");
    }
}

// ====== PUBLISHER SINIFI ======
// Olayı yayınlayan sınıf: KargoSistemi
class KargoSistemi
{
    // Event delegate tanımı: Kargo gönderildiğinde ne yapılacağını belirtir
    public event Action<string> KargoGonderildiEvent;

    public void KargoGonder(string kargoNo, string sehir)
    {
        Console.WriteLine($"Kargo {kargoNo} {sehir}'e gönderiliyor...");

        // Event'i tetikle: Tüm abonelere bildir
        // null check yapılması önemli (abone yoksa null olabilir)
        KargoGonderildiEvent?.Invoke($"Kargo {kargoNo} {sehir}'e gönderildi!");
    }
}

// ====== SUBSCRIBER SINIFI ======
// Olaya abone olan sınıf: KargoTakip
class KargoTakip
{
    public void BilgiAl(string mesaj)
    {
        Console.WriteLine($"Takip Sistemi: {mesaj}");
    }
}
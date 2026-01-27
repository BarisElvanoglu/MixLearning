// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

static List<int> KlasikListeGetir()
{
    List<int> liste = new List<int>();
    for (int i = 1; i <= 10; i++)
    {
        Console.WriteLine($"   -> [Liste Metodu] Sayı {i} hazırlanıp listeye eklendi.");
        liste.Add(i);
    }
    return liste; // Tüm liste dolduktan sonra hepsi birden döner.
}

// KULLANIMI:
Console.WriteLine("--- KLASİK LİSTE BAŞLIYOR ---");
foreach (var sayi in KlasikListeGetir())
{
    Console.WriteLine($"Kullanıcı: {sayi} sayısını aldım.");
    if (sayi == 5) break; // Sadece ilk 2 sayıya ihtiyacım var.
}
static IEnumerable<int> YieldIleGetir()
{
    for (int i = 1; i <= 10; i++)
    {
        Console.WriteLine($"   -> [Yield Metodu] Sayı {i} hazırlandı ve teslim ediliyor.");
        yield return i; // Sayıyı ver ve BURADA DONUP BEKLE.
    }
}

Console.WriteLine("\n--- YIELD BAŞLIYOR ---");
foreach (var sayi in YieldIleGetir())
{
    Console.WriteLine($"Kullanıcı: {sayi} sayısını aldım.");
    if (sayi == 5) break; // İşim bitti, döngüden çıkıyorum.
}


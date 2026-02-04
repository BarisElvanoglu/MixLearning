using System;
using System.Linq;

public static class BenimGuzelUzantilarim
{
    // 'this string' ifadesi: Bu metodun string tipine ekleneceğini söyler.
    public static int KelimeSayisiBul(this string metin)
    {
        if (string.IsNullOrWhiteSpace(metin))
            return 0;

        return metin.Split(new[] { ' ', '.', '?' }, StringSplitOptions.RemoveEmptyEntries).Length;
    }
}

// KULLANIMI:
class Program
{
    static void Main()
    {
        string haber = "Bugün hava çok güzel, değil mi?";

        // Bak şimdi, sanki string'in kendi metoduymuş gibi çağırıyoruz:
        int sayi = haber.KelimeSayisiBul();

        Console.WriteLine($"Kelimeler sayıldı: {sayi}"); // Çıktı: 5
    }
}
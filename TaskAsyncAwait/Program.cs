using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
 
class MegaSimulasyon
{
    // Thread-safe koleksiyon: Çalınan paralar buraya güvenle atılır
    private static ConcurrentBag<string> _ganimet = new ConcurrentBag<string>();
    // Lock nesnesi: Tek kişilik dar koridor için
    private static readonly object _darKoridorKilidi = new object();
    // Semaphore: Kasaya aynı anda sadece 2 hırsız girebilir
    private static SemaphoreSlim _kasaOdasiSiniri = new SemaphoreSlim(2);
 
    static async Task Main(string[] args)
    {
        // 1. İptal mekanizması (Polis baskını simülasyonu)
        CancellationTokenSource polisBaskini = new CancellationTokenSource();
        Console.WriteLine("--- Soygun Başladı (Polis 5 saniye içinde gelebilir!) ---");
        Console.WriteLine("İptal etmek (Teslim olmak) için bir tuşa basın...\n");

        // Arka planda polis baskını süresini başlatan bir task
        //await koyduğumda tread i başlat boşa çıkar ama sonuç bekle
        // _ = koyduğumda tread i başlat boşa çıkar ama sonuç bekleme
        _ = Task.Run(async () =>
        {
            await Task.Delay(5000);
            polisBaskini.Cancel();
        });

        try
        {
            // 2. Paralel İşlemler (Task.Run ile 5 hırsız işe koyuluyor)
            List<Task> hirsizlar = new List<Task>();
            for (int i = 1; i <= 5; i++)
            {
                int hirsizId = i;
                hirsizlar.Add(SoygunYapAsync(hirsizId, polisBaskini.Token));
            }
 
            await Task.WhenAll(hirsizlar);
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\n[DİKKAT] POLİS GELDİ! Herkes kaçtı, soygun yarım kaldı!");
        }
 
        // Sonuçları göster
        Console.WriteLine("\n--- Soygun Sonu Raporu ---");
        Console.WriteLine($"Toplam çalınan parça sayısı: {_ganimet.Count}");
        foreach (var item in _ganimet) Console.WriteLine($"- {item}");
    }
 
    static async Task SoygunYapAsync(int id, CancellationToken iptalToken)
    {
        // A. LOCK KULLANIMI (Dar Koridor)
        // Aynı anda sadece 1 thread buraya girebilir
        lock (_darKoridorKilidi)
        {
            Console.WriteLine($"Hırsız {id}: Dar koridordan geçti.");
            Thread.Sleep(200); // Dar geçiş zaman alır
        }
 
        // B. SEMAPHORE KULLANIMI (Kasa Odası)
        // Aynı anda sadece 2 thread içeri girebilir
        await _kasaOdasiSiniri.WaitAsync(iptalToken);
        try
        {
            Console.WriteLine($"Hırsız {id}: Kasa odasına girdi, altınları topluyor...");
            // C. ASYNC/AWAIT (Altın toplama süresi)
            // Thread'i bloklamaz, sistem kaynaklarını serbest bırakır
            await Task.Delay(1500, iptalToken);
 
            // D. THREAD-SAFE COLLECTION
            _ganimet.Add($"Hırsız {id} tarafından çalınan altın");
            Console.WriteLine($"Hırsız {id}: Altını aldı ve odadan çıktı.");
        }
        finally
        {
            _kasaOdasiSiniri.Release(); // Diğer hırsıza yer aç
        }
    }
}
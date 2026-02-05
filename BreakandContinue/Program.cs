using System;

namespace LoopsExamples
{
    class Program
    {
        static void Main()
        {
            // ---------- 1) break örneği ----------
            // 1..10 arası dön, ilk i > 7 olduğunda döngüyü tamamen sonlandır
            Console.WriteLine("break örneği (i > 7 ise dur):");
            for (int i = 1; i <= 10; i++)
            {
                if (i > 7)
                {
                    Console.WriteLine($"i = {i} -> break ile döngü sonlandırılıyor");
                    break; // döngü bitiyor; 8,9,10 yazılmaz
                }
                Console.WriteLine(i);
            }

            Console.WriteLine();

            // ---------- 2) continue örneği ----------
            // 1..10 arası dön, çift sayıları atla (yazdırma) — sadece tekleri yazdır
            Console.WriteLine("continue örneği (çiftleri atla):");
            for (int i = 1; i <= 10; i++)
            {
                if (i % 2 == 0)
                {
                    // bu iterasyonun kalan kısmı atlanır, döngü bir sonraki i'ye geçer
                    continue;
                }
                Console.WriteLine(i); // sadece tek sayılar yazılır
            }

            Console.WriteLine();

            // ---------- 3) İç içe döngüler: break sadece bulunduğu döngüyü kırar ----------
            Console.WriteLine("nested loop - inner break sadece inner'ı sonlandırır:");
            for (int outer = 1; outer <= 3; outer++)
            {
                Console.WriteLine($"outer = {outer}");
                for (int inner = 1; inner <= 5; inner++)
                {
                    if (inner == 3)
                    {
                        Console.WriteLine("  inner == 3 -> break (sadece inner döngüsü kırılır)");
                        break; // yalnızca iç döngü burada sonlanır
                    }
                    Console.WriteLine($"  inner = {inner}");
                }
            }

            Console.WriteLine();

            // ---------- 4) Dış döngüyü de sonlandırmak (flag kullanımı) ----------
            Console.WriteLine("outer'ı da kırmak için flag kullanımı:");
            bool stop = false;
            for (int outer = 1; outer <= 3 && !stop; outer++)
            {
                Console.WriteLine($"outer = {outer}");
                for (int inner = 1; inner <= 5; inner++)
                {
                    if (inner == 3)
                    {
                        Console.WriteLine("  inner == 3 -> hem inner hem outer döngüsü sonlandırılıyor (flag)");
                        stop = true; // dış döngü koşulunu sağlayarak dış döngüyü de kıracağız
                        break;
                    }
                    Console.WriteLine($"  inner = {inner}");
                }
            }

            Console.WriteLine();

            // ---------- 5) Alternatif: goto ile dış döngüyü anında kırma (genelde önerilmez) ----------
            Console.WriteLine("goto ile dış döngüyü kırma (önerilmez, sadece gösterim):");
            for (int outer = 1; outer <= 3; outer++)
            {
                Console.WriteLine($"outer = {outer}");
                for (int inner = 1; inner <= 5; inner++)
                {
                    if (inner == 3)
                    {
                        Console.WriteLine("  inner == 3 -> goto ile tüm döngülerden çıkılıyor");
                        goto EndLoops; // etiketli yere atla, tüm döngüler biter
                    }
                    Console.WriteLine($"  inner = {inner}");
                }
            }

        EndLoops:
            Console.WriteLine("Döngüler sona erdi.");
        }
    }
}
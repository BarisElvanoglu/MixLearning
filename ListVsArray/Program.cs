using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ListVsArray
{
    public class Program
    {
        // Uygun entry point: static Main() - projede en az bir tane bulunmalı
        static void Main()
        {
            Console.WriteLine("========== LIST<T> vs ARRAY FARKLAR ==========\n");

            BoyutFarki();
            Console.WriteLine("\n--- 2. BELLEK KULLANIMI ---\n");
            BelleKKullanimi();
            Console.WriteLine("\n--- 3. FONKSİYONLAR (Metodlar) ---\n");
            FonksiyonlarFarki();
            Console.WriteLine("\n--- 4. ERIŞIM VERİMLİLİĞİ (Performance) ---\n");
            PerformansKarsilastirmasi();
            Console.WriteLine("\n--- 5. BAŞLATMA VE İLKLENDİRME ---\n");
            BaslatmaFarki();
            Console.WriteLine("\n--- 6. GERÇEK SENARYO ÖRNEĞİ ---\n");
            GercekSenaryoOrnegi();
        }

        // (İçerik aynı kaldı; kısaltılmadı)
        static void BoyutFarki()
        {
            int[] dizi = new int[5];
            Console.WriteLine($"Array boyutu: {dizi.Length} (sabit)");
            dizi[0] = 10; dizi[1] = 20;
            Console.WriteLine($"Array'a 2 eleman eklendi ama Length hala: {dizi.Length}");
            Array.Resize(ref dizi, 10);
            Console.WriteLine($"Array.Resize sonrası boyut: {dizi.Length}\n");

            List<int> liste = new List<int>();
            Console.WriteLine($"List başlangıç kapasitesi: {liste.Capacity}, eleman sayısı: {liste.Count}");
            for (int i = 0; i < 100; i++) liste.Add(i);
            Console.WriteLine($"100 eleman sonrası kapasitesi: {liste.Capacity}, eleman sayısı: {liste.Count}");
        }

        static void BelleKKullanimi()
        {
            int[] dizi = new int[1000];
            dizi[0] = 10; dizi[1] = 20;
            Console.WriteLine($"Array[1000]: {dizi.Length} eleman için bellek ayrıldı");
            Console.WriteLine($"Kullanılan: 2 eleman, Harcanan: {dizi.Length * sizeof(int)} byte\n");

            List<int> liste = new List<int>();
            liste.Add(10); liste.Add(20);
            Console.WriteLine($"List 2 eleman için:");
            Console.WriteLine($"  Capacity: {liste.Capacity}, Count: {liste.Count}");
            Console.WriteLine($"  Harcanan (yaklaşık): {liste.Capacity * sizeof(int)} byte");
        }

        static void FonksiyonlarFarki()
        {
            int[] dizi = { 5, 2, 8, 1, 9 };
            Console.WriteLine("Array Metodları:");
            Console.WriteLine($"  Length: {dizi.Length}");
            Array.Sort(dizi);
            Console.WriteLine($"  Sıralanmış: {string.Join(", ", dizi)}");
            Array.Reverse(dizi);
            Console.WriteLine($"  Ters çevrilmiş: {string.Join(", ", dizi)}\n");

            List<int> liste = new List<int> { 5, 2, 8, 1, 9 };
            Console.WriteLine("List<T> Metodları:");
            Console.WriteLine($"  Count: {liste.Count}");
            liste.Sort(); Console.WriteLine($"  Sıralanmış: {string.Join(", ", liste)}");
            liste.Reverse(); Console.WriteLine($"  Ters çevrilmiş: {string.Join(", ", liste)}");
            liste.Remove(2); Console.WriteLine($"  2 silindikten sonra: {string.Join(", ", liste)}");
            var bulundu = liste.Find(x => x > 5); Console.WriteLine($"  5'ten büyük ilk eleman: {bulundu}");
            var sayfa = liste.GetRange(0, 2); Console.WriteLine($"  İlk 2 eleman: {string.Join(", ", sayfa)}");
        }

        static void PerformansKarsilastirmasi()
        {
            const int N = 100_000;
            var sw = new Stopwatch();
            int[] dizi = new int[N];
            sw.Restart();
            for (int i = 0; i < N; i++) dizi[i] = i;
            sw.Stop(); Console.WriteLine($"Array yazma ({N} eleman): {sw.ElapsedMilliseconds} ms");

            sw.Restart(); int sum1 = 0;
            for (int i = 0; i < N; i++) sum1+= dizi[i];
            sw.Stop(); Console.WriteLine($"Array okuma ({N} eleman): {sw.ElapsedMilliseconds} ms");

            List<int> liste = new List<int>();
            sw.Restart();
            for (int i = 0; i < N; i++) liste.Add(i);
            sw.Stop(); Console.WriteLine($"List Add ({N} eleman): {sw.ElapsedMilliseconds} ms");

            sw.Restart(); int sum = 0;
            for (int i = 0; i < N; i++) sum += liste[i];
            sw.Stop(); Console.WriteLine($"List okuma ({N} eleman): {sw.ElapsedMilliseconds} ms");
        }

        static void BaslatmaFarki()
        {
            int[] dizi = new int[3];
            Console.WriteLine($"Array[3] başlangıç değerleri: {string.Join(", ", dizi)} (hepsi 0)");
            string[] metinDizi = new string[3];
            Console.WriteLine($"String Array[3]: {string.Join(", ", metinDizi.Select(x => x ?? "null"))} (hepsi null)\n");
            List<int> liste = new List<int>();
            Console.WriteLine($"List başlangıç Count: {liste.Count} (boş)");
            Console.WriteLine($"List başlangıç Capacity: {liste.Capacity}");
            List<int> liste2 = new List<int> { 1, 2, 3 };
            Console.WriteLine($"List {{ 1, 2, 3 }} Count: {liste2.Count}, Capacity: {liste2.Capacity}");
        }

        static void GercekSenaryoOrnegi()
        {
            Console.WriteLine("Senaryo: Dosyadan okunan satırları saklama\n");
            Console.WriteLine("1. Array Yaklaşımı:");
            string[] lines = new string[3];
            lines[0] = "Satır 1"; lines[1] = "Satır 2"; lines[2] = "Satır 3";
            Console.WriteLine($"Array: {string.Join(", ", lines)}\n");

            Console.WriteLine("2. List Yaklaşımı:");
            List<string> dinamikSatirlar = new List<string>();
            string[] dosyaSatirlari = { "Satır 1", "Satır 2", "Satır 3", "Satır 4", "Satır 5" };
            foreach (var satir in dosyaSatirlari) dinamikSatirlar.Add(satir);
            Console.WriteLine($"List: {string.Join(", ", dinamikSatirlar)}");
            Console.WriteLine($"Kaç satır okundu: {dinamikSatirlar.Count}\n");
            Console.WriteLine("✅ Sonuç: Boyut önceden biliniyorsa Array, bilinmiyorsa List kullan!");
        }
    }
}
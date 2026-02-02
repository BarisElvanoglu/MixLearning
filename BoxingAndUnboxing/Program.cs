
//•	Boxing: Değer tipi(int, struct vb.) bir reference tipe (object veya bir interface) kopyalanıp heap'e konulduğunda oluşan işlem.
//* Bu işlem yeni bir nesne oluşturur ve kopyalama maliyeti taşır.
//•	Unboxing: Daha önce boxed edilmiş bir object içindeki değeri tekrar değer tipine dönüştürme işlemi. Tür doğrulaması ve değerin stack'e kopyalanmasını içerir.
//•	Performans etkisi: Boxing bellek tahsisi (heap allocation) ve kopyalama üretir → CPU + GC maliyeti. Çok sık gerçekleşirse performans ve bellek baskısı yaratır.



using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

class Program
{
    static void Main()
    {
        // BOXING örneği: ArrayList (non-generic) kullanımı -> int değerleri box'lanır
        ArrayList arr = new ArrayList();
        for (int i = 0; i < 5; i++)
        {
            arr.Add(i); // boxing: int -> object
        }

        // UNBOXING örneği: object içinden int'e dönüşüm
        for (int i = 0; i < arr.Count; i++)
        {
            int value = (int)arr[i]; // unboxing: object -> int
            Console.WriteLine($"ArrayList item {i} = {value}");
        }

        Console.WriteLine();

        // GENERICS ile boxing önlenir: List<int> doğrudan değerleri saklar (no boxing)
        List<int> gen = new List<int>();
        for (int i = 0; i < 5; i++)
        {
            gen.Add(i); // no boxing
        }

        for (int i = 0; i < gen.Count; i++)
        {
            int value = gen[i]; // no unboxing
            Console.WriteLine($"List<int> item {i} = {value}");
        }

        Console.WriteLine();

        // Basit zaman ölçümü: ArrayList vs List<int> ekleme
        const int N = 100_000;
        var sw = new Stopwatch();

        sw.Restart();
        ArrayList aList = new ArrayList();
        for (int i = 0; i < N; i++)
            aList.Add(i); // boxing
        sw.Stop();
        Console.WriteLine($"ArrayList add ({N}) time: {sw.ElapsedMilliseconds} ms");

        sw.Restart();
        List<int> lList = new List<int>();
        for (int i = 0; i < N; i++)
            lList.Add(i); // no boxing
        sw.Stop();
        Console.WriteLine($"List<int> add ({N}) time: {sw.ElapsedMilliseconds} ms");

        // Not: mikrobenchmarklar ortamınıza göre değişir; gerçek ölçümler için BenchmarkDotNet kullanın.
    }
}


//•	Sık kullanılan yollar: generics(List<T>, Dictionary<TKey, TValue>) kullanın — boxing'in ana kaynağını ortadan kaldırır.
//•	API tasarımında değer tiplerini object veya non-generic koleksiyonlara göndermekten kaçının.
//•	Interface üzerinden çağrı yaparken value type boxing'e yol açabilir; gerekiyorsa generic veya constrained generics kullanın.
//•	Çok hassas performans gerektiren senaryolarda BenchmarkDotNet ile ölçüm yapın; .NET sürümlerinde JIT/GC geliştirmeleri olsa da boxing hâlâ kaçınılması gereken maliyetlidir.
//Kısa özet:
//•	Boxing = değer tipinden reference tipe kopyalama + heap allocation.
//•	Unboxing = reference içindeki değeri değer tipine geri kopyalama + tür kontrolü.
//•	Performans etkisi: CPU + bellek + GC yükü; çözüm: generics, yapı değişiklikleri, uygun API tasarımı.
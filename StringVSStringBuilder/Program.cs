using System;
using System.Text;

public class Program
{
    public static void Main()
    {
        Console.WriteLine("--- C# String Bellek Yönetimi ve Eşitlik Testleri ---\n");

        // 1. SENARYO: String Interning (Aynı referans)
        // Derleme zamanında bilinen aynı metinler Intern Pool'da tek adreste tutulur.
        string s1 = "Merhaba";//AX0005
        string s2 = "Merhaba";//AX0005
        Console.WriteLine("Senaryo 1 (Literal):");
        Console.WriteLine($"Eşit mi (==): {s1 == s2}"); // True
        Console.WriteLine($"Referans aynı mı: {object.ReferenceEquals(s1, s2)}"); // True
        Console.WriteLine("--------------------------------------------------\n");


        // 2. SENARYO: 'new' Anahtar Kelimesi (Farklı referans)
        // İçerik aynı olsa bile 'new' zorla Heap'te yeni bir adres açar.
        string s3 = new string("Merhaba".ToCharArray());

        Console.WriteLine("Senaryo 2 (New String):");
        Console.WriteLine($"Eşit mi (==): {s1 == s3}"); // True (String sınıfı içeriğe bakar)
        Console.WriteLine($"Referans aynı mı: {object.ReferenceEquals(s1, s3)}"); // False (Biri İntern Pool, biri Heap)
        Console.WriteLine("--------------------------------------------------\n");


        // 3. SENARYO: StringBuilder (Dinamik oluşturma)
        // StringBuilder arka planda char dizisi yönetir, ToString() her zaman yeni adres döner.
        var sb = new StringBuilder();
        sb.Append("Mer");
        sb.Append("haba");
        string s4 = sb.ToString();

        Console.WriteLine("Senaryo 3 (StringBuilder):");
        Console.WriteLine($"Eşit mi (==): {s1 == s4}"); // True (İçerik aynı)
        Console.WriteLine($"Referans aynı mı: {object.ReferenceEquals(s1, s4)}"); // False
        Console.WriteLine("--------------------------------------------------\n");


        // 4. SENARYO: Manuel Interning (Zorla Havuza Sokma)
        // s4 (Heap'teydi) manuel olarak Intern Pool'a sokuluyor.
        string s4Interned = string.Intern(s4);

        Console.WriteLine("Senaryo 4 (Manual Interning):");
        Console.WriteLine($"Referans s1 ile aynı mı: {object.ReferenceEquals(s1, s4Interned)}"); // True!
        Console.WriteLine("--------------------------------------------------\n");


        // 5. SENARYO: 'object' Tuzağı (Compile-time vs Runtime)
        // Değişkenler object tipine cast edilirse == operatörü referansa bakmaya zorlanır.
        object obj1 = s1; // Intern Pool adresi
        object obj3 = s3; // Heap adresi

        Console.WriteLine("Senaryo 5 (Object Casting - En Tehlikelisi):");
        Console.WriteLine($"== ile sonuç: {obj1 == obj3}"); // FALSE (Çünkü derleyici artık referans bakıyor)
        Console.WriteLine($"== ile sonuç: {obj1.Equals(obj3)}"); // TRUE 

        Console.WriteLine($"Equals() ile sonuç: {obj1.Equals(obj3)}"); // TRUE (Çünkü runtime içeriğe bakıyor)
    }
}

//equals runtime da çalıştığı için objelerin türünün stiirng olduğunu anlar stringlerin kıyası içeriğe bakma şeklindedir .
//ve buyüzden equals true döner ikisinin içide merhabadır

//string builder kullanmamızın sebebi gereksiz string interpoolda kullanmayacağımız textleri saklamak istememek

//eğer kullandığımız stringbuilderin içini yada new ile oluşturduğumuz stringin
//içini ilerde kullanırız diye string.intern ile stringpool a atarız .Burda bizim atacağımız 
//text önceden varsa direk o referansa bakmaya başlar
#nullable enable
using System;

namespace NullableTypes
{
    class Program
    {
        static void Main()
        {
            // ---------- 1) Nullable value type (değer tipi nullable) ----------
            // int? ve Nullable<int> aynı şeydir.
            int? maybeInt = null;                 // null atanabilir
            Console.WriteLine($"maybeInt.HasValue: {maybeInt.HasValue}"); // False
            Console.WriteLine($"maybeInt.GetValueOrDefault(): {maybeInt.GetValueOrDefault()}"); // 0 (default int)

            maybeInt = 42;
            Console.WriteLine($"maybeInt.HasValue: {maybeInt.HasValue}"); // True
            Console.WriteLine($"maybeInt.Value: {maybeInt.Value}"); // 42

            // null-coalescing (??) — null ise sağdaki değeri kullan
            int definite = maybeInt ?? -1;
            Console.WriteLine($"definite: {definite}"); // 42

            // null-coalescing assignment (??=) — sadece null ise atama yapar
            int? maybeA = null;
            maybeA ??= 10; // maybeA = maybeA ?? 10;
            Console.WriteLine($"maybeA after ??=: {maybeA}"); // 10

            // ---------- 2) Nullable<T> explicit kullanımı ----------
            Nullable<double> maybeDouble = new Nullable<double>(3.14);
            Console.WriteLine($"maybeDouble.HasValue: {maybeDouble.HasValue}, Value: {maybeDouble.Value}");

            // ---------- 3) Nullable reference types (C# 8+): string? ----------
            // #nullable enable ile derleyici nullable referans tiplerini izler.
            string? maybeName = null; // derleyici bu atamayı izler (uyarı verebilir)
            Console.WriteLine($"maybeName is null: {maybeName is null}");

            // Null-conditional operator (?.) — null ise sağ taraf değerlendirilmez, null döner
            Console.WriteLine($"maybeName?.Length: {maybeName?.Length}"); // boş/blank (null)

            // Null-coalescing ile güvenli okuma
            Console.WriteLine($"Length or -1: {maybeName?.Length ?? -1}");

            // Null-forgiving operator (!) — derleyicinin null uyarısını bastırır (runtime NullReferenceException riskine dikkat)
            string? nullableSurname = null;
            // Console.WriteLine(nullableSurname!.Length); // Eğer uncomment edilirse çalışmada NullReferenceException fırlatır.

            // Güvenli kullanım: pattern matching veya kontrol yap
            if (nullableSurname is null)
            {
                Console.WriteLine("nullableSurname null, işlem yapılmadı.");
            }
            else
            {
                Console.WriteLine($"Surname length: {nullableSurname.Length}");
            }

            // ---------- 4) Kullanım örneği: nullable parametreli metod ----------
            PrintLength(null);
            PrintLength("Elvan");

            // ---------- 5) Özet kısa notlar ----------
            // - Değer tipleri için: T?  (ör. int?) => Nullable<T> (struct) kullanılır, HasValue/Value/ GetValueOrDefault() metotları var.
            // - Referans tipleri için: T? (ör. string?) => derleyici düzeyinde null uyarıları sağlar (runtime davranış aynı).
            // - ?. , ?? ve ??= operatörleri null ile çalışmayı kolaylaştırır.
            // - '!' (null-forgiving) yalnızca derleyici uyarısını kapatır; null kontrolü yapmaz.
        }

        // Nullable referans/ değer dönüştürme ve güvenli okuma örneği
        static void PrintLength(string? s)
        {
            // s?.Length -> int? ; ?? ile fallback kullanıyoruz
            int lengthOrMinusOne = s?.Length ?? -1;
            Console.WriteLine($"PrintLength: length = {lengthOrMinusOne}");
        }
    }
}
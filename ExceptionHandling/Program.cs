using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("CASE 1 - throw (stack trace korunur)\n");
        try
        {
            CaseThrow();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); // Orijinal stack trace görünür. Stack Trace in uğradığı her bir satır belirtilir ve console a yazdırılır.
        }

        Console.WriteLine("\n-----------------------------\n");

        Console.WriteLine("CASE 2 - throw ex (stack trace yenilenir)\n");
        try
        {
            CaseThrowEx();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); // Stack trace CaseThrowEx içinden başlar — orijinal konum kaybolur.
        }

        Console.WriteLine("\n-----------------------------\n");

        Console.WriteLine("CASE 3 - wrap (inner exception korunur)\n");
        try
        {
            CaseWrap();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex); // Yeni exception görünür, ancak InnerException içinde orijinal stack trace vardır
            if (ex.InnerException != null)
            {
                Console.WriteLine("\n--- Inner Exception ---");
                Console.WriteLine(ex.InnerException);
            }
        }
    }

    // CASE 1: catch sonra 'throw;' kullanımı -> stack trace korunur
    static void CaseThrow()
    {
        try
        {
            LevelA();
        }
        catch
        {
            // Orijinal istisna yeniden atılıyor; stack trace korunur
            throw;
        }
    }

    // CASE 2: catch sonra 'throw ex;' kullanımı -> stack trace resetlenir
    static void CaseThrowEx()
    {
        try
        {
            LevelA();
        }
        catch (Exception ex)
        {
            // Uyarı: bu kullanım orijinal hata yerini kaybettirir
            throw ex;
        }
    }

    // CASE 3: yeni istisna oluşturup inner olarak verme -> context eklenir, orijinal inner içinde korunur
    static void CaseWrap()
    {
        try
        {
            LevelA();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Ek bağlam: LevelA çalışırken hata oluştu.", ex);
        }
    }

    static void LevelA() => LevelB();

    static void LevelB() => LevelC();

    static void LevelC()
    {
        // Orijinal hata buradan geliyor
        throw new ArgumentNullException("parametreX", "Parametre null olamaz (örnek hata).");
    }
}
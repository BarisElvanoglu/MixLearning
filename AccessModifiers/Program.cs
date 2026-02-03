using System;

// ============================================================================
// ACCESS MODIFIERS (Erişim Belirleyicileri)
// ============================================================================
// Hangi kodun sınıf, metod ve değişkenlere erişebileceğini kontrol eden anahtar kelimeler
// ============================================================================

// 1. PUBLIC - Herkese açık
// - Sınıf, metod, özellik, alan herkesten erişilebilir
// - Proje içinde ve dışında erişim mümkün
public class PublicOrnegi
{
    public string Mesaj = "Ben herkese açık";
    
    public void PublicMetod()
    {
        Console.WriteLine("Public metod çağrıldı");
    }
}

// 2. PRIVATE - Sadece sınıfın içinde
// - Sadece tanımlandığı sınıftan erişilebilir
// - Varsayılan erişim seviyesidir (yazılmazsa private sayılır)
// - Sınıf dışından erişim mümkün değil
public class PrivateOrnegi
{
    private string SifreBilgisi = "Bu gizli";
    private int _iç_Sayac = 0;
    
    private void GizliMetod()
    {
        Console.WriteLine("Bu metod sadece sınıf içinden çağrılabilir");
    }
    
    public void DıştanÇağrılabilir()
    {
        // Sınıf içinde private üyelere erişebiliriz
        GizliMetod();
        Console.WriteLine(SifreBilgisi);
        _iç_Sayac++;
    }
}

// 3. PROTECTED - Sadece sınıf ve türetilen sınıflar
// - Tanımlandığı sınıf + türetilen sınıflar erişebilir
// - Sınıf dışındaki diğer sınıflardan erişim mümkün değil
public class BaseClass
{
    protected string KorunanBilgi = "Sadece türetilen sınıflar beni okuyabilir";
    
    protected void KorunanMetod()
    {
        Console.WriteLine("Protected metod");
    }
}

public class TuretilenClass : BaseClass
{
    public void TuretilenMetoddan()
    {
        // Türetilen sınıftan protected üyelere erişebiliriz
        Console.WriteLine(KorunanBilgi);
        KorunanMetod();
    }
}

// 4. INTERNAL - Sadece aynı assembly içinde
// - Aynı proje/assembly içindeki tüm sınıflardan erişilebilir
// - Farklı projedeki assembly'den erişim mümkün değil
internal class InternalOrnegi
{
    internal string AssemblyBilgisi = "Sadece bu assembly içinde erişilir";
    
    internal void InternalMetod()
    {
        Console.WriteLine("Internal metod");
    }
}

// 5. PROTECTED INTERNAL - Protected VEYA Internal
// - Aynı assembly içinde: herkesten erişilebilir
// - Farklı assembly: sadece türetilen sınıflardan erişilebilir
public class ProtectedInternalOrnegi
{
    protected internal string MixBilgi = "Assembly içinde herkese, dışında türetilenlere açık";
    
    protected internal void MixMetod()
    {
        Console.WriteLine("Protected Internal metod");
    }
}

// 6. PRIVATE PROTECTED - Sadece türetilen sınıflar (aynı assembly)
// - Türetilen sınıflar: SADECE aynı assembly içinde erişebilir
// - Diğer assembly'deki türetilen sınıflar: erişim yok
public class PrivateProtectedOrnegi
{
    private protected string StrikBilgi = "Sadece bu assembly'deki türetilen sınıflar";
    
    private protected void StriktMetod()
    {
        Console.WriteLine("Private Protected metod");
    }
}

class Program
{
    static void Main()
    {
        Console.WriteLine("========== ACCESS MODIFIERS ÖRNEKLERI ==========\n");

        // 1. PUBLIC ÖRNEĞI
        Console.WriteLine("--- 1. PUBLIC ---");
        PublicOrnegi p = new PublicOrnegi();
        Console.WriteLine(p.Mesaj); // ✅ Erişilebilir
        p.PublicMetod(); // ✅ Erişilebilir
        Console.WriteLine();

        // 2. PRIVATE ÖRNEĞI
        Console.WriteLine("--- 2. PRIVATE ---");
        PrivateOrnegi pr = new PrivateOrnegi();
        // Console.WriteLine(pr.SifreBilgisi); // ❌ Hata! Private erişim yok
        // pr.GizliMetod(); // ❌ Hata! Private erişim yok
        pr.DıştanÇağrılabilir(); // ✅ Public metod çalışır ve private üyeleri kullanır
        Console.WriteLine();

        // 3. PROTECTED ÖRNEĞI
        Console.WriteLine("--- 3. PROTECTED ---");
        // BaseClass b = new BaseClass();
        // Console.WriteLine(b.KorunanBilgi); // ❌ Hata! Protected erişim yok
        TuretilenClass t = new TuretilenClass();
        t.TuretilenMetoddan(); // ✅ Türetilen sınıf protected üyelere erişebilir
        Console.WriteLine();

        // 4. INTERNAL ÖRNEĞI
        Console.WriteLine("--- 4. INTERNAL ---");
        InternalOrnegi i = new InternalOrnegi();
        Console.WriteLine(i.AssemblyBilgisi); // ✅ Aynı assembly içinde erişilebilir
        i.InternalMetod();
        Console.WriteLine();

        // 5. PROTECTED INTERNAL ÖRNEĞI
        Console.WriteLine("--- 5. PROTECTED INTERNAL ---");
        ProtectedInternalOrnegi pi = new ProtectedInternalOrnegi();
        Console.WriteLine(pi.MixBilgi); // ✅ Aynı assembly'de erişilebilir
        pi.MixMetod();
        Console.WriteLine();

        // 6. PRIVATE PROTECTED ÖRNEĞI
        Console.WriteLine("--- 6. PRIVATE PROTECTED ---");
        // Console.WriteLine dizinine erişemeyiz, sadece türetilen sınıflar (aynı assembly) erişebilir
        Console.WriteLine("Private Protected: Sadece türetilen sınıflar erişebilir\n");

        Console.WriteLine("========== ÖZET TABLOSU ==========\n");
        Console.WriteLine("Access Modifier       | Sınıf İçi | Aynı Proje | Farklı Proje | Türetilen");
        Console.WriteLine("----------------------|-----------|-----------|------------|----------");
        Console.WriteLine("public                |    ✅     |    ✅     |     ✅      |    ✅");
        Console.WriteLine("private               |    ✅     |    ❌     |     ❌      |    ❌");
        Console.WriteLine("protected             |    ✅     |    ❌     |     ❌      |    ✅");
        Console.WriteLine("internal              |    ✅     |    ✅     |     ❌      |    ❌");
        Console.WriteLine("protected internal    |    ✅     |    ✅     |     ✅*     |    ✅");
        Console.WriteLine("private protected     |    ✅     |    ✅*    |     ❌      |    ❌");
        Console.WriteLine("\n* = Sadece türetilen sınıflar");
    }
}
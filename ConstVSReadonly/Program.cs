using System;

public class BankSettings
{
    // 1. CONST (Compile-time Constant)
    // Değeri kod yazılırken verilir, uygulama derlendiği an "hard-coded" olarak gömülür.
    public const string BankName = "Global Central Bank";
    // 2. READONLY (Runtime Constant)
    // Değeri uygulama çalışırken verilir (örneğin DB'den veya Config'den gelir).
    // Sadece tanımlandığı an veya CONSTRUCTOR içinde atanabilir.
    public readonly string BranchCity;
    public readonly DateTime ConnectionTime;

    public BankSettings(string city)
    {
        // Readonly değişkenler constructor içinde dinamik olarak set edilebilir.
        BranchCity = city;
        ConnectionTime = DateTime.Now;
    }

    public void UpdateSettings()
    {
        // BankName = "New Bank"; // DERLEME HATASI! Değiştirilemez.
        // BranchCity = "Istanbul"; // DERLEME HATASI! Constructor dışında set edilemez.
    }
}


//1. "Versioning" Problemi(DLL Hell)
//Diyelim ki bir kütüphanede (DLL) public const int MaxLoanLimit = 5000; tanımladın.Bu kütüphaneyi
//kullanan diğer projeler derlendiğinde, 5000 değeri o projelerin içine doğrudan gömülür.
//Eğer kütüphaneyi güncelleyip değeri 10000 yaparsan ama diğer projeleri yeniden derlemezsen,
//o projeler hala 5000 kullanmaya devam eder.
//Çözüm: Eğer değerin değişme ihtimali varsa mutlaka readonly veya static readonly kullanmalısın.
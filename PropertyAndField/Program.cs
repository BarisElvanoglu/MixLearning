using System;

namespace PropertiesVsFields
{
    // Arayüz örneği: yalnızca property ile uygulanabilir (field ile değil)
    interface IPerson
    {
        string Name { get; }
    }

    // 1) Field (Alan) örneği
    // - Doğrudan sınıfın belleğindeki veri depolama birimi.
    // - Genelde private olmalı; public field kullanımı önerilmez (encapsulation ihlali).
    class PersonWithField
    {
        // public field: doğrudan erişim sağlar (okunması/ayarlanması serbest)
        public string name; // kötü uygulama: doğrulama, değiştirme kontrolü yok
    }

    // 2) Property (Özellik) örneği
    // - Kullanıcıya alan gibi görünür ama arka planda get/set metotları vardır.
    // - Getter/setter içinde mantık (doğrulama, dönüşüm, lazy init) çalıştırılabilir.
    // - Interface'leri implement etmek, erişim belirleyicileri ayırmak için kullanılır.
    class PersonWithProperty : IPerson
    {
        // backing field (gizli alan)
        private string _nameBacking;

        // Tam özellik: setter içinde doğrulama var
        public string Name
        {
            get => _nameBacking;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Name boş olamaz.", nameof(value));
                _nameBacking = value.Trim();
            }
        }

        // Auto-property: basit saklama, derleyici arka alan yaratır
        public int Age { get; set; }

        // Read-only computed property (hesaplanmış, depolama yok)
        public bool IsAdult => Age >= 18;

        // init-only property (C# 9+): sadece nesne ilklenirken atanabilir
        public string? Nickname { get; init; }
    }

    class Program
    {
        static void Main()
        {
            // Field kullanımının sakıncası
            var pf = new PersonWithField();
            pf.name = ""; // hiçbir doğrulama yok — yanlış durum oluşabilir
            Console.WriteLine($"PersonWithField.name: '{pf.name}'");

            // Property kullanımının avantajı
            var pp = new PersonWithProperty();
            try
            {
                pp.Name = "  Alice  "; // setter trim ve doğrulama yapıyor
                pp.Age = 25;
                Console.WriteLine($"PersonWithProperty.Name: '{pp.Name}'");
                Console.WriteLine($"IsAdult: {pp.IsAdult}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Hata: {ex.Message}");
            }

            // init-only örneği
            var pp2 = new PersonWithProperty { Age = 17, Nickname = "Al" }; // Nickname init esnasında atanabilir
            Console.WriteLine($"pp2 Nickname: {pp2.Nickname}, IsAdult: {pp2.IsAdult}");

            // Interface implementasyonu: sadece property ile yapılabilir
            IPerson ip = pp;
            Console.WriteLine($"IPerson.Name via interface: {ip.Name}");

            // Özet:
            // - Field: doğrudan veri; genelde private yapılır ve property üzerinden expose edilir.
            // - Property: get/set ile davranış eklenebilir, interface uyumluluğu, farklı erişim düzeyleri sağlar.
        }
    }
}